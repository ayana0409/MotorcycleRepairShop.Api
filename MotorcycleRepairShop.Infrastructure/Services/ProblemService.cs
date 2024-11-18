using AutoMapper;
using MotorcycleRepairShop.Application.Interfaces;
using MotorcycleRepairShop.Application.Interfaces.Services;
using MotorcycleRepairShop.Application.Model;
using MotorcycleRepairShop.Application.Model.Problem;
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
        public async Task<IEnumerable<ProblemForDropdownDto>> GetProblemsForDropDownList()
        {
            var existList = await _unitOfWork.ProblemRepository.GetAllAsync();
            var result = _mapper.Map<IEnumerable<ProblemForDropdownDto>>(existList);
            return result;
        }

        public async Task<TableResponse<ProblemTableDto>> GetProblemPagination(TableRequest request)
        {
            var (result, total) = await _unitOfWork.ProblemRepository
                .GetPanigationAsync(request.PageIndex, request.PageSize, request.Keyword ?? "");
            
            var datas = _mapper.Map<IEnumerable<ProblemTableDto>>(result);
            return new TableResponse<ProblemTableDto>
            {
                PageSize = request.PageSize,
                Datas = datas,
                Total = total
            };
        }
        public async Task<ProblemDto> GetProblemById(int problemId)
        {
            var problem = await _unitOfWork.ProblemRepository
                .GetSigleAsync(p => p.Id.Equals(problemId))
                ?? throw new NotFoundException(nameof(Problem), problemId);
            var result = _mapper.Map<ProblemDto>(problem);
            return result;
        }

        public async Task<int> CreateProblem(ProblemDto problemDto)
        {
            var createProblem = _mapper.Map<Problem>(problemDto);
            await _unitOfWork.ProblemRepository.CreateAsync(createProblem);
            await _unitOfWork.SaveChangeAsync();

            var result = createProblem.Id;
            LogSuccess(result);
            return result;
        }

        public async Task<ProblemDto> UpdateProblem(int id, ProblemDto problemDto)
        {
            var existProblem = await _unitOfWork.ProblemRepository
                .GetSigleAsync(p => p.Id.Equals(id))
                ?? throw new NotFoundException(nameof(Problem), id);
            var updateProblem = _mapper.Map(problemDto, existProblem);
            _unitOfWork.ProblemRepository.Update(updateProblem);
            await _unitOfWork.SaveChangeAsync();

            var result = _mapper.Map<ProblemDto>(updateProblem);
            LogSuccess(id);
            return result;
        }

        public async Task DeleteProblem(int id)
        {
            var existProblem = await _unitOfWork.ProblemRepository
                .GetSigleAsync(p => p.Id.Equals(id))
                ?? throw new NotFoundException(nameof(Problem), id);
            existProblem.IsActive = false;
            _unitOfWork.ProblemRepository.Update(existProblem);
            await _unitOfWork.SaveChangeAsync();
            LogSuccess(id);
        }
    }
}
