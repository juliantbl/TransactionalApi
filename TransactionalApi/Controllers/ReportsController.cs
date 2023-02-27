using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TransactionalBll.Services;
using TransactionalDomain.Dtos;
using TransactionalDomain.Entities;
using TransactionalDomain.Interfaces;

namespace TransactionalApi.Controllers
{
    [Route("api/reports")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        public IReportService reportService { get; set; }
        public ReportsController(IReportService _reportService)
        {
            reportService = _reportService;
        }
        [HttpPost("statement")]
        public async Task<ActionResult<IEnumerable<Account>>> GenerateStatement([FromBody] StatementDto statementDto)
        {         
            if (!ModelState.IsValid) return BadRequest(ModelState);

            return Ok(await reportService.GenerateStatement(statementDto));
        }
    }
}
