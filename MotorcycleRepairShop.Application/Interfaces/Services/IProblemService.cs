using MotorcycleRepairShop.Application.Model;

namespace MotorcycleRepairShop.Application.Interfaces.Services
{
    public interface IProblemService
    {
        Task<int> CreateProblem(ProblemDto problemDto);
        Task DeleteProblem(int id);
        Task<ProblemDto> GetProblemById(int problemId);
        Task<TableResponse<ProblemTableDto>> GetProblemPagination(TableRequest request);
        Task<ProblemDto> UpdateProblem(int id, ProblemDto problemDto);
    }
}
