using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Task7.Contracts.Requests;
using Task7.Contracts.Responses;
using Task7.Services.Interfaces;

namespace Task7.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientsController : ControllerBase
    {
        private readonly IClientService _clientService;

        public ClientsController(IClientService clientService)
            => _clientService = clientService;

        // POST /api/clients
        [HttpPost]
        public async Task<ActionResult<int>> Create([FromBody] CreateClientRequest request)
        {
            var newId = await _clientService.CreateClientAsync(request);
            return CreatedAtAction(nameof(GetTrips), new { id = newId }, newId);
        }

        // GET /api/clients/{id}/trips
        [HttpGet("{id:int}/trips")]
        public async Task<ActionResult<IEnumerable<GetAllClientTripsResponse>>> GetTrips(int id)
        {
            var trips = await _clientService.GetAllClientTripsAsync(id);
            return Ok(trips);
        }

        // PUT /api/clients/{id}/trips/{tripId}
        [HttpPut("{id:int}/trips/{tripId:int}")]
        public async Task<IActionResult> Register(int id, int tripId)
        {
            await _clientService.RegisterClientToTripAsync(id, tripId);
            return NoContent();
        }

        // DELETE /api/clients/{id}/trips/{tripId}
        [HttpDelete("{id:int}/trips/{tripId:int}")]
        public async Task<IActionResult> Unregister(int id, int tripId)
        {
            await _clientService.RemoveClientFromTripAsync(id, tripId);
            return NoContent();
        }
    }
}