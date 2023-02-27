using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TransactionalDomain.Dtos;
using TransactionalDomain.Entities;
using TransactionalDomain.Interfaces;

namespace TransactionalApi.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService accountService;
        public AccountController(IAccountService _accountService)
        {
            accountService = _accountService;
        }

        [HttpGet("byclient/{id}")]
        public async Task<IEnumerable<Account>> GetAccounts(int id)
        {
            return await accountService.GetClientAccounts(id);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Account>> GetAccount(int id)
        {
            var account = await accountService.GetAccountById(id);

            if (account == null) return NotFound();

            return account;
        }

        [HttpPost]
        public async Task<ActionResult> PostAccount([FromBody] AccountDto account)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var accountExists = await accountService.AccountExists(account.AccountNumber);

            if (accountExists) return Conflict();

            await accountService.AddAccount(account);

            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> PutAccount(int id, [FromBody] AccountDto account)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var AccountExists = await accountService.AccountExists(account.AccountNumber);

            if (!AccountExists)
            {
                await accountService.AddAccount(account);
                return Ok();
            }

            var index = accountService.UpdateAccount(id, account, true);

            return NoContent();
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult<Account>> PatchAccount(int id, [FromBody] AccountDto account)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var AccountExists = await accountService.AccountExists(account.AccountNumber);

            if (!AccountExists) return NotFound();

            var result = await accountService.UpdateAccount(id, account, false);

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAccount(int id)
        {
            var account = await accountService.GetAccountById(id);

            if (account == null) return NotFound();

            await accountService.RemoveAccount(id);

            return NoContent();
        }
    }
}
