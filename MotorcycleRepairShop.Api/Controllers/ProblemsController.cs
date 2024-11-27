using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MotorcycleRepairShop.Application.Interfaces.Services;
using MotorcycleRepairShop.Application.Model;
using MotorcycleRepairShop.Application.Model.Problem;

namespace MotorcycleRepairShop.Api.Controllers
{
    [Authorize(Roles = "Admin,Manager")]
    [Route("api/[controller]")]
    [ApiController]
    public class ProblemsController : ControllerBase
    {
        private readonly IProblemService _problemService;

        public ProblemsController(IProblemService problemService)
        {
            _problemService = problemService;
        }

        [HttpGet("dropdown")]
        public async Task<ActionResult<IEnumerable<ProblemForDropdownDto>>> GetForDropDownList()
            => Ok(await _problemService.GetProblemsForDropDownList());

        [HttpGet("pagination")]
        public async Task<ActionResult<TableResponse<ProblemTableDto>>> GetPagination(
            [FromQuery] int pageIndex = 0,
            [FromQuery] int pageSize = 5,
            [FromQuery] string keyword = "")
            => Ok(await _problemService.GetProblemPagination(new TableRequest
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                Keyword = keyword
            }));

        [HttpGet("{id}")]
        public async Task<ActionResult<ProblemDto>> GetProblemById(int id)
            => Ok(await _problemService.GetProblemById(id));

        [HttpPost]
        public async Task<ActionResult<int>> CreateProblem(ProblemDto problem)
            => CreatedAtAction(nameof(CreateProblem), await _problemService.CreateProblem(problem));

        [HttpPut("{id}")]
        public async Task<ActionResult<ProblemDto>> UpdateProblem(int id,  ProblemDto problem) 
            => Ok(await _problemService.UpdateProblem(id, problem));

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProblem(int id)
        {
            await _problemService.DeleteProblem(id);
            return NoContent();
        }
    }
}
