using Microsoft.AspNetCore.Mvc;
using Task9.Application.DTOs;
using Task9.Application.Exceptions;
using Task9.Application.Services.Interfaces;
using Task9.Core.Models;

namespace Task9.Presentation.Controllers;

[ApiController]
[Route("api/trips")]
public class TripController(
    ITripService tripService,
    IClientTripService clientTripService) 
    : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(PaginatedResult<GetTripDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IEnumerable<GetTripDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTrips(
        [FromQuery(Name = "page")] int? page,
        [FromQuery(Name = "pageSize")] int? pageSize,
        CancellationToken cancellationToken = default)
    {
        if (page is null && pageSize is null)
        {
            var trips = await tripService.GetAllTripsAsync();
            return Ok(trips);
        }

        var paginatedTrips = await tripService.GetPaginatedTripsAsync(page ?? 1, pageSize ?? 10);
        return Ok(paginatedTrips);
    }
    
    [HttpPost("{idTrip:int}/clients")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> RegisterClientToTrip(
        int idTrip,
        [FromBody] RegisterClientDto dto,
        CancellationToken cancellationToken = default)
    {
        try
        {
            await clientTripService.RegisterClientToTripAsync(idTrip, dto);
            return StatusCode(StatusCodes.Status201Created);
        }
        catch (ClientTripExceptions.TripNotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (ClientTripExceptions.ClientAlreadyRegisteredException e)
        {
            return BadRequest(e.Message);
        }
        catch (ClientTripExceptions.TripAlreadyStartedException e)
        {
            return BadRequest(e.Message);
        }
        catch
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error.");
        }
    }
}