using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Task7.Contracts.Responses;
using Task7.Services.Interfaces;

namespace Task7.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TripsController : ControllerBase
    {
        private readonly ITripService _tripService;

        public TripsController(ITripService tripService)
            => _tripService = tripService;

        // GET /api/trips
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetAllTripsResponse>>> GetAll()
        {
            var trips = await _tripService.GetAllTripsAsync();
            return Ok(trips);
        }

        // GET /api/trips/{id}
        [HttpGet("{id:int}")]
        public async Task<ActionResult<GetAllTripsResponse>> GetById(int id)
        {
            var trip = await _tripService.GetTripByIdAsync(id);
            if (trip == null) return NotFound();
            return Ok(trip);
        }
    }
}