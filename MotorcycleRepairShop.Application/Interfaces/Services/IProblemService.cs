using MotorcycleRepairShop.Application.Model;
using MotorcycleRepairShop.Application.Model.Problem;

namespace MotorcycleRepairShop.Application.Interfaces.Services
{
    public interface IProblemService
    {
        Task<int> CreateProblem(ProblemDto problemDto);
        Task DeleteProblem(int id);
        Task<ProblemDto> GetProblemById(int problemId);
        Task<TableResponse<ProblemTableDto>> GetProblemPagination(TableRequest request);
        Task<IEnumerable<ProblemForDropdownDto>> GetProblemsForDropDownList();
        Task<ProblemDto> UpdateProblem(int id, ProblemDto problemDto);
    }
}
