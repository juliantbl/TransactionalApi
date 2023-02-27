using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TransactionalDomain.Dtos;
using TransactionalDomain.Entities;
using TransactionalDomain.Interfaces;

namespace TransactionalApi.Controllers
{
    [Route("api/transactions")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService transactionService;
        public TransactionController(ITransactionService _transactionService)
        {
            transactionService = _transactionService;
        }

        [HttpGet("byaccount/{id}")]
        public async Task<IEnumerable<Transaction>> GetTransactions(int id)
        {
            return await transactionService.GetAccountTransactions(id);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Transaction>> GetTransaction(int id)
        {
            var transaction = await transactionService.GetTransactionById(id);

            if (transaction == null) return NotFound();

            return transaction;
        }

        [HttpPost]
        public async Task<ActionResult> PostTransaction([FromBody] TransactionDto transaction)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var msg=await transactionService.AddTransaction(transaction);

            return Ok(msg);
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult<Transaction>> PatchTransaction(int id, [FromBody] TransactionDto transaction)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var transactionExists = await transactionService.TransactionExists(id);

            if (!transactionExists) return NotFound();

            var result = await transactionService.UpdateTransaction(id, transaction);

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTransaction(int id)
        {
            var transaction = await transactionService.GetTransactionById(id);

            if (transaction == null) return NotFound();

            await transactionService.RemoveTransaction(id);

            return NoContent();
        }
    }
}
