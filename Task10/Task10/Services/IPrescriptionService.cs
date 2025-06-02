using Microsoft.AspNetCore.Mvc;
using Task10.DTOs;

namespace Task10.Services;

public interface IPrescriptionService
{
    Task<IActionResult> AddPrescriptionAsync(PrescriptionRequestDTO request);
    Task<object?> GetPatientDataAsync(int patientId);
}