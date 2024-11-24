using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MotorcycleRepairShop.Application.Interfaces;
using MotorcycleRepairShop.Application.Interfaces.Services;
using MotorcycleRepairShop.Application.Model;
using MotorcycleRepairShop.Domain.Entities;
using MotorcycleRepairShop.Domain.Enums;
using MotorcycleRepairShop.Share.Exceptions;
using MotorcycleRepairShop.Share.Extensions;
using MySqlX.XDevAPI.Common;
using Serilog;
using Image = MotorcycleRepairShop.Domain.Entities.Image;

namespace MotorcycleRepairShop.Infrastructure.Services
{
    public class ServiceRequestService : BaseService, IServiceRequestService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICloudinaryService<ServiceRequest> _cloudinaryService;
        private readonly IEmailService _emailService;
        public ServiceRequestService(ILogger logger, IUnitOfWork unitOfWork, IMapper mapper, ICloudinaryService<ServiceRequest> cloudinaryService, IEmailService emailService) : base(logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cloudinaryService = cloudinaryService;
            _emailService = emailService;
        }

        public async Task<TableResponse<ServiceRequestTable>> GetServiceRequestPagination(TableRequest request)
        {
            var (result, total) = await _unitOfWork.ServiceRequestRepository
                .GetPanigationAsync(request.PageIndex, request.PageSize, request.Keyword ?? "");

            var datas = _mapper.Map<IEnumerable<ServiceRequestTable>>(result);

            return new TableResponse<ServiceRequestTable>
            {
                PageSize = request.PageSize,
                Datas = datas,
                Total = total
            };
        }

        public async Task GetServiceRequestByUsername(string username)
            => await _unitOfWork.ServiceRequestRepository
            .GetByUsername(username);

        public async Task<ServiceRequestInfoDto> GetServiceRequestById(int id)
        {
            var serviceRequest = await _unitOfWork.ServiceRequestRepository
                .GetQueryable()
                .Include(sr => sr.Services)
                .ThenInclude(s => s.Service)
                .Include(sr => sr.Parts)
                .ThenInclude(p => p.Part)
                .Include(sr => sr.Problems)
                .ThenInclude(p => p.Problem)
                .AsSplitQuery()
                .FirstOrDefaultAsync(sr => sr.Id == id)
                ?? throw new NotFoundException(nameof(ServiceRequest), id);

            var result = _mapper.Map<ServiceRequestInfoDto>(serviceRequest);

            // Get part info
            result.Parts = _mapper.Map<List<ServiceRequestPartInfoDto>>(serviceRequest.Parts);
            // Get problem info
            result.Problems = _mapper.Map<List<string>>(serviceRequest.Problems);
            // Get service info
            result.Services = _mapper.Map<List<ServiceRequestItemInfo>>(serviceRequest.Services);
            return result;
        }

        public async Task<int> CreateDeirecServiceRequest(CreateServiceRequestDto serviceRequestDto)
            => await CreateServiceRequest(serviceRequestDto, ServiceType.Direct);

        public async Task<int> CreateRemoteServiceRequest(CreateServiceRequestDto serviceRequestDto)
            => await CreateServiceRequest(serviceRequestDto, ServiceType.Remote);

        public async Task<int> CreateRescueServiceRequest(CreateServiceRequestDto serviceRequestDto)
            => await CreateServiceRequest(serviceRequestDto, ServiceType.Rescue);

        public async Task UpdateServiceRequestUserInfoById(int serviceRequestId, ServiceRequestUserInfoDto serviceRequestUserInfoDto)
        {
            LogStart(serviceRequestId);
            var serviceRequest = await _unitOfWork.ServiceRequestRepository
                .GetSigleAsync(sr => sr.Id.Equals(serviceRequestId))
                ?? throw new NotFoundException(nameof(ServiceRequest), serviceRequestId);

            var updateServiceRequest = _mapper.Map(serviceRequestUserInfoDto, serviceRequest);
            _unitOfWork.ServiceRequestRepository.Update(updateServiceRequest);
            await _unitOfWork.SaveChangeAsync();
            LogEnd(serviceRequestId);
        }

        public async Task UpdateServiceRequestStatus(int serviceRequestId, StatusEnum status)
        {
            var serviceRequest = await _unitOfWork.ServiceRequestRepository
                .GetSigleAsync(sr => sr.Id.Equals(serviceRequestId))
                ?? throw new NotFoundException(nameof(ServiceRequest), serviceRequestId);

            if (serviceRequest.StatusId == (int)StatusEnum.Completed
                || serviceRequest.StatusId == (int)StatusEnum.Canceled)
            {
                throw new UpdateStatusFailedException(
                    Enum.GetName(typeof(StatusEnum), serviceRequest.StatusId) ?? "Status",
                    status.GetDisplayName());
            }
            try
            {
                await _unitOfWork.BeginTransaction();
                LogStart(serviceRequestId);
                if (status == StatusEnum.Completed)
                {
                    // Comsume Inventory
                    var usedParts = await _unitOfWork.PartRepository
                        .GetByServiceRequestId(serviceRequestId);
                    foreach (var part in usedParts)
                    {
                        var item = await _unitOfWork.ServiceRequestPartRepository
                            .GetByServiceRequestIdAndPartId(serviceRequestId, part.Id);
                        var usedQuantity = item?.Quantity ?? 0;
                        var inventories = await _unitOfWork.PartInventoryRepository
                            .GetAllAsync(i => i.PartId.Equals(part.Id) && i.QuantityInStock > 0);
                        inventories = inventories.OrderBy(i => i.EntryDate).ToList();
                        if (!inventories.Any() || inventories.Sum(i => i.QuantityInStock) < usedQuantity)
                            throw new ArgumentNullException("Out of Stock");
                        while (usedQuantity > 0)
                        {
                            var inventory = inventories.First(i => i.QuantityInStock > 0);
                            if (inventory.QuantityInStock >= usedQuantity)
                            {
                                inventory.QuantityInStock -= usedQuantity;
                                usedQuantity = 0;
                            }
                            else
                            {
                                usedQuantity -= inventory.QuantityInStock;
                                inventory.QuantityInStock = 0;
                            }
                            _unitOfWork.PartInventoryRepository.Update(inventory);
                            await _unitOfWork.SaveChangeAsync();
                        }
                    }
                    serviceRequest.CompletionDate = DateTime.UtcNow;
                }
                serviceRequest.StatusId = (int)status;
                _unitOfWork.ServiceRequestRepository
                    .Update(serviceRequest);
                await _unitOfWork.SaveChangeAsync();

                if (!string.IsNullOrEmpty(serviceRequest.Email))
                    await SendEmail(serviceRequestId, serviceRequest.Email, status);

                _logger.Information($"UpdateServiceRequestStatus - Id: {serviceRequestId} - Status: {Enum.GetName(status)}");
                await _unitOfWork.CommitTransaction();
                LogEnd(serviceRequestId);
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollBackTransaction();
                _logger.Information("Error while UpdateServiceRequestStatus: ", ex.Message);
                throw new ArgumentException("Error while update service request status", ex.Message);
            }
        }

        #region ServiceRequestItem: UpSert - Delete
        public async Task<ServiceRequestItemDto> UpSertServiceItemToServiceRequest(int serviceRequestId, UpSertServiceRequestItemDto serviceItemRequestDto)
        {
            LogStart(serviceRequestId);
            var serviceid = serviceItemRequestDto.ServiceId;
            var logData = $"- ServiceRequestId: {serviceRequestId} - ServiceId: {serviceid}";

            await CheckExitsServiceRequest(serviceRequestId);

            var existService = await _unitOfWork.ServiceRepository
                .GetSigleAsync(s => s.Id.Equals(serviceid))
                ?? throw new NotFoundException(nameof(Service), serviceid);

            ServiceRequestItem? existServiceItem = await _unitOfWork.ServiceRequestItemRepository
                .GetByServiceRequestIdAndServiceId(serviceRequestId, serviceid);

            if (existServiceItem == null)
            {
                LogStart($"Create ServiceRequestItem {logData}");
                existServiceItem = new()
                {
                    ServiceId = serviceid,
                    ServiceRequestId = serviceRequestId,
                    Price = existService.Price,
                    Quantity = serviceItemRequestDto.Quantity,
                };
                await _unitOfWork.ServiceRequestItemRepository.CreateAsync(existServiceItem);
                LogEnd($"Create ServiceRequestItem {logData}");
            }
            else
            {
                LogStart($"Update ServiceRequestItem {logData}");
                _mapper.Map(serviceItemRequestDto, existServiceItem);
                _unitOfWork.ServiceRequestItemRepository
                    .Update(existServiceItem);
                LogEnd($"Update ServiceRequestItem {logData}");
            }
            await _unitOfWork.SaveChangeAsync();

            var result = _mapper.Map<ServiceRequestItemDto>(existServiceItem);
            LogEnd(serviceRequestId);
            return result;
        }
        public async Task DeleteServiceItemToServiceRequest(int serviceRequestId, int serviceId)
        {
            var logData = $"- ServiceRequestId: {serviceRequestId} - ServiceId: {serviceId}";
            LogStart($"Delete ServiceRequestItem {logData}");

            var deleteServiceItem = await _unitOfWork.ServiceRequestItemRepository
                .GetByServiceRequestIdAndServiceId(serviceRequestId, serviceId)
                ?? throw new NotFoundException($"{nameof(ServiceRequestItem)}: " +
                $"{nameof(ServiceRequest)} with ID {serviceRequestId} and {nameof(Service)}", serviceId);
            _unitOfWork.ServiceRequestItemRepository.Delete(deleteServiceItem);
            await _unitOfWork.SaveChangeAsync();

            LogEnd($"Delete ServiceRequestItem {logData}");
        }
        #endregion

        # region ServiceRequestPart: UpSert - Delete

        public async Task<ServiceRequestPartDto> UpSertServicePartToServiceRequest(int serviceRequestId, UpSertServiceRequestPartDto servicePartRequestDto)
        {
            LogStart(serviceRequestId);
            var partId = servicePartRequestDto.PartId;
            var logData = $"- ServiceRequestId: {serviceRequestId} - PartId: {partId}";

            await CheckExitsServiceRequest(serviceRequestId);

            var existPart = await _unitOfWork.PartRepository
                .GetSigleAsync(s => s.Id.Equals(partId))
                ?? throw new NotFoundException(nameof(Part), partId);

            ServiceRequestPart? existPartItem = await _unitOfWork.ServiceRequestPartRepository
                .GetByServiceRequestIdAndPartId(serviceRequestId, partId);

            if (existPartItem == null)
            {
                LogStart($"Create ServiceRequestPart {logData}");
                existPartItem = new()
                {
                    PartId = partId,
                    ServiceRequestId = serviceRequestId,
                    Price = existPart.Price,
                    Quantity = servicePartRequestDto.Quantity,
                    WarrantyTo = existPart.WarrantyPeriod != 0 ? DateTime.UtcNow.AddMonths(existPart.WarrantyPeriod) : null,
                };
                await _unitOfWork.ServiceRequestPartRepository.CreateAsync(existPartItem);
                LogEnd($"Create ServiceRequestPart {logData}");
            }
            else
            {
                LogStart($"Update ServiceRequestPart {logData}");
                _mapper.Map(servicePartRequestDto, existPartItem);
                _unitOfWork.ServiceRequestPartRepository
                    .Update(existPartItem);
                LogEnd($"Update ServiceRequestPart {logData}");
            }
            await _unitOfWork.SaveChangeAsync();

            var result = _mapper.Map<ServiceRequestPartDto>(existPartItem);
            LogEnd(serviceRequestId);
            return result;
        }

        public async Task DeleteServicePartInServiceRequest(int serviceRequestId, int partId)
        {
            var logData = $"- ServiceRequestId: {serviceRequestId} - PartId: {partId}";
            LogStart($"Delete ServiceRequestPart {logData}");

            var deletePartItem = await _unitOfWork.ServiceRequestPartRepository
                .GetByServiceRequestIdAndPartId(serviceRequestId, partId)
                ?? throw new NotFoundException($"{nameof(ServiceRequestPart)}: " +
                $"{nameof(ServiceRequest)} with ID {serviceRequestId} and {nameof(Part)}", partId);
            _unitOfWork.ServiceRequestPartRepository.Delete(deletePartItem);
            await _unitOfWork.SaveChangeAsync();

            LogEnd($"Delete ServiceRequestPart {logData}");
        }
        #endregion

        #region ServiceRequestProblem: Upsert - Delete

        public async Task<ServiceRequestProblemDto> AddProblemToServiceRequest(int serviceRequestId, int problemId)
        {
            await CheckExitsServiceRequest(serviceRequestId);

            var existProblem = await _unitOfWork.ProblemRepository
                .GetSigleAsync(s => s.Id.Equals(problemId))
                ?? throw new NotFoundException(nameof(Problem), problemId);

            ServiceRequestProblem? existServicePart = await _unitOfWork.ServiceRequestProblemRepository
                .GetByServiceRequestIdAndProblemId(serviceRequestId, problemId);

            if (existServicePart == null)
            {
                existServicePart = new()
                {
                    ServiceRequestId = serviceRequestId,
                    ProblemId = problemId,
                    ReportedDate = DateTime.Now,
                };
                await _unitOfWork.ServiceRequestProblemRepository.CreateAsync(existServicePart);
            }
            await _unitOfWork.SaveChangeAsync();
            LogSuccess($"- ServiceRequestId: {serviceRequestId} - ProblemId: {problemId}", "Add Problem");

            var result = _mapper.Map<ServiceRequestProblemDto>(existServicePart);
            result.Name = existProblem.Name;
            return result;
        }

        public async Task DeleteProblemInServiceRequest(int serviceRequestId, int problemId)
        {
            var deleteProblem = await _unitOfWork.ServiceRequestProblemRepository
                .GetByServiceRequestIdAndProblemId(serviceRequestId, problemId)
                ?? throw new NotFoundException($"{nameof(ServiceRequestProblem)}: " +
                $"{nameof(ServiceRequest)} with ID {serviceRequestId} and {nameof(Problem)}", problemId);
            _unitOfWork.ServiceRequestProblemRepository.Delete(deleteProblem);
            await _unitOfWork.SaveChangeAsync();

            LogSuccess($"- ServiceRequestId: {serviceRequestId} - ProblemId: {problemId}", "Add Problem");
        }

        #endregion

        #region Media

        public async Task<IEnumerable<string>> AddMediaToServiceRequest(int serviceRequestId, IEnumerable<IFormFile> mediaData, MediaType type)
        {
            bool serviceRequestExists = await _unitOfWork.ServiceRequestRepository
                .AnyAsync(serviceRequestId);
            if (!serviceRequestExists)
            {
                throw new NotFoundException(nameof(ServiceRequest), serviceRequestId);
            }

            LogStart(serviceRequestId);
            List<string> result = [];
            if (type == MediaType.Image)
            {
                foreach (var image in mediaData)
                {
                    _logger.Information("Adding image...");
                    var mediaItem = new Image()
                    {
                        ServiceRequestId = serviceRequestId,
                        Name = _cloudinaryService.UploadPhoto(image)
                    };
                    await _unitOfWork.ImageRepository.CreateAsync(mediaItem);
                    result.Add(mediaItem.Name);
                }
            }
            else
            {
                foreach (var video in mediaData)
                {
                    _logger.Information("Adding video...");
                    var mediaItem = new Video()
                    {
                        ServiceRequestId = serviceRequestId,
                        Name = _cloudinaryService.UploadVideo(video)
                    };
                    await _unitOfWork.VideoRepository.CreateAsync(mediaItem);
                    result.Add(mediaItem.Name);
                }
            }

            await _unitOfWork.SaveChangeAsync();
            LogEnd(serviceRequestId);
            return result;
        }

        public async Task DeleteMediaInServiceRequest(int serviceRequestId, IEnumerable<string> mediaUrls, MediaType type)
        {

            if (type == MediaType.Image)
            {
                var exitsImages = await _unitOfWork.ImageRepository
                .GetAllAsync(i => mediaUrls.Contains(i.Name) && i.ServiceRequestId.Equals(serviceRequestId));
                if (exitsImages.Any())
                {
                    foreach (var image in exitsImages)
                    {
                        await _cloudinaryService.DeletePhotoAsync(image.Name);
                        _unitOfWork.ImageRepository.Delete(image);
                    }
                    await _unitOfWork.SaveChangeAsync();
                    _logger.Information("Deleting image...");
                }
            }
            else
            {
                var exitsVideos = await _unitOfWork.VideoRepository
                .GetAllAsync(i => mediaUrls.Contains(i.Name) && i.ServiceRequestId.Equals(serviceRequestId));
                if (exitsVideos.Any())
                {
                    foreach (var video in exitsVideos)
                    {
                        await _cloudinaryService.DeleteVideoAsync(video.Name);
                        _unitOfWork.VideoRepository.Delete(video);
                    }
                    await _unitOfWork.SaveChangeAsync();
                    _logger.Information("Deleting video...");
                }
            }
        }

        #endregion

        private async Task<int> CreateServiceRequest(CreateServiceRequestDto serviceRequestDto, ServiceType type)
        {
            try
            {
                LogStart($"CreateServiceRequest {Enum.GetName(type)}");
                await _unitOfWork.BeginTransaction();
                var service = _mapper.Map<ServiceRequest>(serviceRequestDto);
                service.ServiceType = type;

                var user = await _unitOfWork.Table<ApplicationUser>()
                        .FirstOrDefaultAsync(u => u.MobilePhone.Equals(serviceRequestDto.MobilePhone));
                if (user != null)
                    service.CustomerId = user.Id;

                await _unitOfWork.ServiceRequestRepository.CreateAsync(service);
                await _unitOfWork.SaveChangeAsync();
                var result = service.Id;

                // Add problems to service request
                if (serviceRequestDto.Problems.Count != 0)
                    await AddProblemsToServiceRequest(result, serviceRequestDto.Problems);
                // Add services to service request
                if (serviceRequestDto.Services.Count != 0)
                    await AddServicesToServiceRequest(result, serviceRequestDto.Services);
                // Add images
                if (serviceRequestDto.Images.Any())
                    await AddMediaToServiceRequest(result, serviceRequestDto.Images, MediaType.Image);

                // Add videos
                if (serviceRequestDto.Videos.Any())
                    await AddMediaToServiceRequest(result, serviceRequestDto.Videos, MediaType.Video);

                if (!string.IsNullOrEmpty(serviceRequestDto.Email))
                    await SendEmail(result, serviceRequestDto.Email, StatusEnum.New);

                LogEnd(result, $"CreateServiceRequest {Enum.GetName(type)}");
                await _unitOfWork.CommitTransaction();
                return result;
            }
            catch (NotFoundException)
            {
                await _unitOfWork.RollBackTransaction();
                throw;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollBackTransaction();
                LogEnd($"CreateServiceRequest Error - {ex.Message}");
                throw;
            }
        }

        private async Task AddProblemsToServiceRequest(int serviceRequestId, IEnumerable<int> problemIds)
        {
            List<ServiceRequestProblem> problems = [];
            foreach (var problemId in problemIds)
                problems.Add(new() { ServiceRequestId = serviceRequestId, ProblemId = problemId });
            await _unitOfWork.Table<ServiceRequestProblem>()
                .AddRangeAsync(problems);
            await _unitOfWork.SaveChangeAsync();
            LogSuccess(serviceRequestId);
        }

        private async Task AddServicesToServiceRequest(int serviceRequestId, IEnumerable<int> serviceIds)
        {
            List<ServiceRequestItem> services = [];
            var existService = await _unitOfWork.ServiceRepository
                .GetAllAsync(s => serviceIds.Contains(s.Id));
            foreach (var serviceid in serviceIds)
                services.Add(new()
                {
                    ServiceId = serviceid,
                    Quantity = 1,
                    ServiceRequestId = serviceRequestId,
                    Price = existService.FirstOrDefault(s => s.Id.Equals(serviceid))?.Price
                                ?? throw new NotFoundException(nameof(Service), serviceid)
                });
            await _unitOfWork.Table<ServiceRequestItem>()
                .AddRangeAsync(services);
            await _unitOfWork.SaveChangeAsync();
            LogSuccess(serviceRequestId);
        }

        private async Task CheckExitsServiceRequest(int serviceRequestId)
        {
            bool serviceRequestExists = await _unitOfWork.ServiceRequestRepository
                .AnyAsync(serviceRequestId);
            if (!serviceRequestExists)
            {
                throw new NotFoundException(nameof(ServiceRequest), serviceRequestId);
            }
        }

        private async Task SendEmail(int serviceRequestId, string email, StatusEnum status)
        {
            switch (status)
            {
                case StatusEnum.New:
                    await _emailService.SendEmailAsync(email, "Yêu cầu đã được tạo",
                        $"<h1>Yêu cầu đã được tạo với mã là: {serviceRequestId}</h1>" +
                        $"<h3>Chúng tôi sẽ liên hệ với bạn trong thời gian sớm nhất.</h3>" +
                        $"<p>Bạn có thể kiểm tra chi tiết yêu cầu bằng SỐ ĐIỆN THOẠI của mình hoặc dùng TÀI KHOẢN đã đăng ký" +
                        $" tại trang web chính thức của Motorcycle Repair Shop.</p>");
                    break;
                case StatusEnum.UnderInspection:
                    await _emailService.SendEmailAsync(email, $"Yêu cầu {serviceRequestId} đang được kiểm tra",
                        $"<h1>Yêu cầu với mã là {serviceRequestId} của bạn đang được chúng tôi kiểm tra.</h1>" +
                        $"<p>Bạn có thể kiểm tra chi tiết yêu cầu bằng SỐ ĐIỆN THOẠI của mình hoặc dùng TÀI KHOẢN đã đăng ký" +
                        $" tại trang web chính thức của Motorcycle Repair Shop.</p>");
                    break;
                case StatusEnum.AwaitingPayment:
                    await _emailService.SendEmailAsync(email, $"Yêu cầu {serviceRequestId} đang chờ thanh toán",
                        $"<h1>Yêu cầu với mã là {serviceRequestId} của bạn đang chờ bạn thanh toán.</h1>" +
                        $"<h3>Bạn có thể thanh toán online hoặc đến trực tiếp cửa hàng của chúng tôi.</h3>" +
                        $"<p>Bạn có thể kiểm tra chi tiết yêu cầu bằng SỐ ĐIỆN THOẠI của mình hoặc dùng TÀI KHOẢN đã đăng ký" +
                        $" tại trang web chính thức của Motorcycle Repair Shop.</p>");
                    break;
                case StatusEnum.Processing:
                    await _emailService.SendEmailAsync(email, $"Yêu cầu {serviceRequestId} đang được xử lý",
                        $"<h1>Yêu cầu với mã là {serviceRequestId} của bạn đang được xử lý.</h1>" +
                        $"<h3>Chúng tôi sẽ liên hệ với bạn ngay sau khi đã hoàn thành.</h3>" +
                        $"<p>Bạn có thể kiểm tra chi tiết yêu cầu bằng SỐ ĐIỆN THOẠI của mình hoặc dùng TÀI KHOẢN đã đăng ký" +
                        $" tại trang web chính thức của Motorcycle Repair Shop.</p>");
                    break;
                case StatusEnum.Canceled:
                    await _emailService.SendEmailAsync(email, $"Yêu cầu {serviceRequestId} đã bị từ chối",
                        $"<h1>Yêu cầu với mã là {serviceRequestId} của đã bị từ chối.</h1>" +
                        $"<h3>Xin cảm ơn bạn đã sử dụng dịch vụ của chúng tôi.</h3>" +
                        $"<p>Bạn có thể kiểm tra chi tiết yêu cầu bằng SỐ ĐIỆN THOẠI của mình hoặc dùng TÀI KHOẢN đã đăng ký" +
                        $" tại trang web chính thức của Motorcycle Repair Shop.</p>");
                    break;
                case StatusEnum.Completed:
                    await _emailService.SendEmailAsync(email, $"Yêu cầu {serviceRequestId} đã hoàn thành",
                        $"<h1>Yêu cầu với mã là {serviceRequestId} của đã hoàn thành.</h1>" +
                        $"<h3>Hãy nhận phương tiện của bạn. Xin cảm ơn bạn đã sử dụng dịch vụ của chúng tôi.</h3>" +
                        $"<p>Bạn có thể kiểm tra chi tiết yêu cầu bằng SỐ ĐIỆN THOẠI của mình hoặc dùng TÀI KHOẢN đã đăng ký" +
                        $" tại trang web chính thức của Motorcycle Repair Shop.</p>");
                    break;
            }
        }
    }
}
