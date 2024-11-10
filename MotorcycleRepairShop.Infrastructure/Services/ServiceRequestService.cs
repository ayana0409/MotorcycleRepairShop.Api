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
using Serilog;
using Image = MotorcycleRepairShop.Domain.Entities.Image;

namespace MotorcycleRepairShop.Infrastructure.Services
{
    public class ServiceRequestService : BaseService, IServiceRequestService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICloudinaryService<ServiceRequest> _cloudinaryService;
        public ServiceRequestService(ILogger logger, IUnitOfWork unitOfWork, IMapper mapper, ICloudinaryService<ServiceRequest> cloudinaryService) : base(logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cloudinaryService = cloudinaryService;
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

        public async Task<ServiceRequestDto> GetServiceRequestById(int id)
        {
            LogStart(id);
            var service = await _unitOfWork.Table<ServiceRequest>()
                .Include(r => r.Images)
                .Include(r => r.Videos)
                .Include(r => r.Status)
                .AsSplitQuery()
                .FirstOrDefaultAsync(r => r.Id.Equals(id))
                ?? throw new NotFoundException(nameof(ServiceRequest), id);

            var result = _mapper.Map<ServiceRequestDto>(service);
            var problems = await _unitOfWork.ProblemRepository
                .GetByServideRequestId(id);
            result.Problems = problems.Select(p => p.Name).ToList();

            var services = await _unitOfWork.ServiceRepository
                .GetByServiceRequestId(id);
            result.Services = services.Select(p => p.Name).ToList();

            var parts = await _unitOfWork.PartRepository
                .GetByServiceRequestId(id);
            result.Parts = parts.Select(p => p.Name).ToList();

            LogEnd(id);
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

            serviceRequest.StatusId = (int)status;
            _unitOfWork.ServiceRequestRepository
                .Update(serviceRequest);
            await _unitOfWork.SaveChangeAsync();
            _logger.Information($"UpdateServiceRequestStatus - Id: {serviceRequestId} - Status: {Enum.GetName(status)}");
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
                    LogStart(serviceRequestId, "DeleteImagesInServiceRequest");
                    foreach (var image in exitsImages)
                    {
                        await _cloudinaryService.DeletePhotoAsync(image.Name);
                        _unitOfWork.ImageRepository.Delete(image);
                    }
                    await _unitOfWork.SaveChangeAsync();
                    LogEnd(serviceRequestId, "DeleteImagesInServiceRequest");
                }
            }
            else
            {
                var exitsVideos = await _unitOfWork.VideoRepository
                .GetAllAsync(i => mediaUrls.Contains(i.Name) && i.ServiceRequestId.Equals(serviceRequestId));
                if (exitsVideos.Any())
                {
                    LogStart(serviceRequestId, "DeleteVideosInServiceRequest");
                    foreach (var video in exitsVideos)
                    {
                        await _cloudinaryService.DeleteVideoAsync(video.Name);
                        _unitOfWork.VideoRepository.Delete(video);
                    }
                    await _unitOfWork.SaveChangeAsync();
                    LogEnd(serviceRequestId, "DeleteVideosInServiceRequest");
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
    }
}
