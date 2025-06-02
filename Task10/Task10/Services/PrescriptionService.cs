using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Task10.Controllers;
using Task10.Data;
using Task10.DTOs;
using Task10.Models;

namespace Task10.Services;

public class PrescriptionService(PrescriptionDbContext context) : IPrescriptionService
{
    public async Task<IActionResult> AddPrescriptionAsync(PrescriptionRequestDTO request)
        {
            if (request.Medicaments.Count > 10)
                return new BadRequestObjectResult("A prescription can have a maximum of 10 medications.");

            if (request.DueDate < request.Date)
                return new BadRequestObjectResult("DueDate must be later than or equal to Date.");

            var doctor = await context.Doctors.FindAsync(request.DoctorId);
            if (doctor == null) return new NotFoundObjectResult("Doctor not found.");

            var patient = await context.Patients
                .FirstOrDefaultAsync(p =>
                    p.FirstName == request.Patient.FirstName &&
                    p.LastName == request.Patient.LastName &&
                    p.BirthDate == request.Patient.BirthDate);

            if (patient == null)
            {
                patient = new Patient
                {
                    FirstName = request.Patient.FirstName,
                    LastName = request.Patient.LastName,
                    BirthDate = request.Patient.BirthDate
                };
                context.Patients.Add(patient);
                await context.SaveChangesAsync();
            }

            var allMedIds = request.Medicaments.Select(m => m.IdMedicament).ToList();
            var existingMedIds = await context.Medicaments
                .Where(m => allMedIds.Contains(m.IdMedicament))
                .Select(m => m.IdMedicament)
                .ToListAsync();

            if (existingMedIds.Count != allMedIds.Count)
                return new NotFoundObjectResult("One or more medicaments do not exist.");

            var prescription = new Prescription
            {
                Date = request.Date,
                DueDate = request.DueDate,
                IdPatient = patient.IdPatient,
                IdDoctor = doctor.IdDoctor,
                PrescriptionMedicaments = request.Medicaments.Select(m => new PrescriptionMedicament
                {
                    IdMedicament = m.IdMedicament,
                    Dose = m.Dose,
                    Details = m.Details
                }).ToList()
            };

            context.Prescriptions.Add(prescription);
            await context.SaveChangesAsync();

            return new CreatedAtActionResult(
                nameof(PrescriptionController.GetPatientData),
                "Prescription",
                new { id = patient.IdPatient },
                null
            );
        }

        public async Task<object?> GetPatientDataAsync(int id)
        {
            var patient = await context.Patients
                .Include(p => p.Prescriptions)
                    .ThenInclude(p => p.Doctor)
                .Include(p => p.Prescriptions)
                    .ThenInclude(p => p.PrescriptionMedicaments)
                        .ThenInclude(pm => pm.Medicament)
                .FirstOrDefaultAsync(p => p.IdPatient == id);

            if (patient == null) return null;

            var result = new
            {
                patient.IdPatient,
                patient.FirstName,
                patient.LastName,
                patient.BirthDate,
                Prescriptions = patient.Prescriptions
                    .OrderBy(p => p.DueDate)
                    .Select(p => new
                    {
                        p.IdPrescription,
                        p.Date,
                        p.DueDate,
                        Doctor = new { p.Doctor.IdDoctor, p.Doctor.FirstName, p.Doctor.LastName },
                        Medicaments = p.PrescriptionMedicaments.Select(pm => new
                        {
                            pm.IdMedicament,
                            pm.Medicament.Name,
                            pm.Dose,
                            pm.Details
                        })
                    })
            };

            return result;
        }
    }