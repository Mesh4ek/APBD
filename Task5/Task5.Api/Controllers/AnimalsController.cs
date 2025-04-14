using Microsoft.AspNetCore.Mvc;
using Task5.Api.Contracts.Requests;
using Task5.Api.Models;
using Task5.Api.Data;

namespace Task5.Api.Controllers;

[ApiController]
[Route("api/animals")]
public class AnimalsController : ControllerBase
{
    private readonly List<Animal> _animals = AnimalsRepository.Animals;
    
    #region CRUD: Animals
    [HttpGet]
    public IActionResult GetAll() => Ok(_animals);

    [HttpGet("{id:int}")]
    public IActionResult Get(int id)
    {
        var animal = _animals.FirstOrDefault(x => x.Id == id);
        return animal is null ? NotFound() : Ok(animal);
    }

    [HttpPost]
    public IActionResult Create(CreateAnimalRequest request)
    {
        var id = _animals.Max(a => a.Id) + 1;
        var animal = new Animal
        {
            Id = id,
            Name = request.Name,
            Category = request.Category,
            Weight = request.Weight,
            FurColor = request.FurColor
        };
        _animals.Add(animal);
        return CreatedAtAction(nameof(Get), new { id }, animal);
    }

    [HttpPut("{id:int}")]
    public IActionResult Update(int id, UpdateAnimalRequest request)
    {
        var animal = _animals.FirstOrDefault(x => x.Id == id);
        if (animal is null) return NotFound();
        animal.Name = request.Name;
        animal.Category = request.Category;
        animal.Weight = request.Weight;
        animal.FurColor = request.FurColor;
        return Ok(animal);
    }

    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        var animal = _animals.FirstOrDefault(x => x.Id == id);
        if (animal is null) return NotFound();
        _animals.Remove(animal);
        return NoContent();
    }
    #endregion
    
    #region Animal Visit Endpoints
    [HttpGet("{id:int}/visits")]
    public IActionResult GetVisits(int id)
    {
        var animal = _animals.FirstOrDefault(x => x.Id == id);
        return animal is null ? NotFound() : Ok(animal.Visits);
    }

    [HttpPost("{id:int}/visits")]
    public IActionResult AddVisit(int id, [FromBody] Visit visit)
    {
        var animal = _animals.FirstOrDefault(x => x.Id == id);
        if (animal is null) return NotFound();
        visit.Id = animal.Visits.Count > 0 ? animal.Visits.Max(v => v.Id) + 1 : 1;
        animal.Visits.Add(visit);
        return CreatedAtAction(nameof(GetVisits), new { id }, visit);
    }
    #endregion
}
