using AutoMapper;
using MotorcycleRepairShop.Application.Interfaces;
using MotorcycleRepairShop.Application.Interfaces.Services;
using MotorcycleRepairShop.Application.Model;
using MotorcycleRepairShop.Domain.Entities;
using MotorcycleRepairShop.Share.Exceptions;
using Serilog;

namespace MotorcycleRepairShop.Infrastructure.Services
{
    public class ProblemService : BaseService, IProblemService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProblemService(ILogger logger, IMapper mapper, IUnitOfWork unitOfWork) : base(logger)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<TableResponse<ProblemTableDto>> GetProblemPagination(TableRequest request)
        {
            LogStart();
            var (result, total) = await _unitOfWork.ProblemRepository
                .GetPanigationAsync(request.PageIndex, request.PageSize, request.Keyword ?? "");
            List<ProblemTableDto> datas = [];

            foreach (var item in result)
            {
                datas.Add(_mapper.Map<ProblemTableDto>(item));
            }

            LogEnd();
            return new TableResponse<ProblemTableDto>
            {
                PageSize = request.PageSize,
                Datas = datas,
                Total = total
            };
        }
        public async Task<ProblemDto> GetProblemById(int problemId)
        {
            LogStart(problemId);
            var problem = await _unitOfWork.ProblemRepository
                .GetSigleAsync(p => p.Id.Equals(problemId))
                ?? throw new NotFoundException(nameof(Problem), problemId);
            var result = _mapper.Map<ProblemDto>(problem);
            LogEnd(problemId);
            return result;
        }

        public async Task<int> CreateProblem(ProblemDto problemDto)
        {
            LogStart();
            var createProblem = _mapper.Map<Problem>(problemDto);
            await _unitOfWork.ProblemRepository.CreateAsync(createProblem);
            await _unitOfWork.SaveChangeAsync();

            var result = createProblem.Id;
            LogEnd(result);
            return result;
        }

        public async Task<ProblemDto> UpdateProblem(int id, ProblemDto problemDto)
        {
            LogStart(id);
            var existProblem = await _unitOfWork.ProblemRepository
                .GetSigleAsync(p => p.Id.Equals(id))
                ?? throw new NotFoundException(nameof(Problem), id);
            var updateProblem = _mapper.Map(problemDto, existProblem);
            _unitOfWork.ProblemRepository.Update(updateProblem);
            await _unitOfWork.SaveChangeAsync();

            var result = _mapper.Map<ProblemDto>(updateProblem);
            LogEnd(id);
            return result;
        }

        public async Task DeleteProblem(int id)
        {
            LogStart(id);
            var existProblem = await _unitOfWork.ProblemRepository
                .GetSigleAsync(p => p.Id.Equals(id))
                ?? throw new NotFoundException(nameof(Problem), id);
            existProblem.IsActive = false;
            _unitOfWork.ProblemRepository.Update(existProblem);
            await _unitOfWork.SaveChangeAsync();
            LogEnd(id);
        }
    }
}
