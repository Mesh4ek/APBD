using Microsoft.AspNetCore.Mvc;
using Task10.DTOs;
using Task10.Services;

namespace Task10.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PrescriptionController(IPrescriptionService prescriptionService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> AddPrescription(PrescriptionRequestDTO request)
    {
        return await prescriptionService.AddPrescriptionAsync(request);
    }

    [HttpGet("patient/{id}")]
    public async Task<IActionResult> GetPatientData(int id)
    {
        var result = await prescriptionService.GetPatientDataAsync(id);
        if (result == null) return NotFound();
        return Ok(result);
    }
}