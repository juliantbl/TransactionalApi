using Microsoft.AspNetCore.Mvc;
using TransactionalBll.Services;
using TransactionalDomain.Dtos;
using TransactionalDomain.Entities;
using TransactionalDomain.Interfaces;

namespace TransactionalApi.Controllers
{
    [Route("api/client")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly IClientService clientService;
        public ClientController(IClientService _clientService)
        {
            clientService=_clientService;
        }

        [HttpGet]
        public async Task<IEnumerable<Client>> GetClients()
        {
            return await clientService.GetClients(); 
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Client>> GetClient(int id)
        {
            var client = await clientService.GetClientById(id);

            if (client == null) return NotFound();
            
            return client;
        }

        [HttpPost]
        public async Task<ActionResult> PostClient([FromBody] ClientDto client)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var clientExists = await clientService.ClientExists(client.Identification);

            if (clientExists) return Conflict();

            await clientService.AddUserClient(client);

            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> PutClient(int id, [FromBody] ClientDto client)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var clientExists = await clientService.ClientExists(client.Identification);

            if (!clientExists) 
            { 
                 await clientService.AddUserClient(client);
                return Ok();
            }
            
            var index = clientService.UpdateClient(id, client,true);

            return NoContent();
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult<Client>> PatchClient(int id, [FromBody] ClientDto client)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var clientExists = await clientService.ClientExists(client.Identification);

            if (!clientExists) return NotFound();

            var result = await clientService.UpdateClient(id, client, false);

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClient(int id)
        {
            var client =await  clientService.GetClientById(id);

            if (client==null) return NotFound();

            await clientService.RemoveClient(client);

            return NoContent();
        }
    }
}
