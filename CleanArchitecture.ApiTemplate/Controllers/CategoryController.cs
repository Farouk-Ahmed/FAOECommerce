using CleanArchitecture.DataAccess.IRepository;
using CleanArchitecture.DataAccess.Models;
using CleanArchitecture.DataAccess.IUnitOfWorks;
using CleanArchitecture.Services.DTOs.Categories;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Mapster;
using System.Collections.Generic;

[ApiController]
[Route("api/[controller]")]
public class CategoryController : ControllerBase
{
    private readonly IRepository<Category> _repository;
    private readonly IUnitOfWork _unitOfWork;

    public CategoryController(IRepository<Category> repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    [HttpPost (ApiRoutes.Category.Create)]
    public async Task<IActionResult> Create([FromBody] CategoryDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var category = dto.Adapt<Category>();
        _repository.Add(category);
        await _unitOfWork.Complete();
        var resultDto = category.Adapt<CategoryDto>();
        resultDto.ProductCount = 0;
        return Ok(resultDto);
    }

    [HttpPut(ApiRoutes.Category.Update)]
    public async Task<IActionResult> Update(int id, [FromBody] CategoryDto dto)
    {
        if (id != dto.Id) return BadRequest();
        var existing = _repository.Get(c => c.Id == id);
        if (existing == null) return NotFound();
        dto.Adapt(existing);
        _repository.Update(existing);
        await _unitOfWork.Complete();
        var resultDto = existing.Adapt<CategoryDto>();
        resultDto.ProductCount = existing.Products?.Count ?? 0;
        return Ok(resultDto);
    }

    [HttpGet (ApiRoutes.Category.GetAll)]
    public IActionResult GetAll()
    {
        var categories = _repository.GetAll(null, "Products");
        var categoryDtos = categories.Adapt<List<CategoryDto>>();
        foreach (var (dto, entity) in categoryDtos.Zip(categories, (dto, entity) => (dto, entity)))
        {
            dto.ProductCount = entity.Products?.Count ?? 0;
        }
        return Ok(categoryDtos);
    }

    [HttpGet(ApiRoutes.Category.GetById)]
    public IActionResult Get(int id)
    {
        var category = _repository.Get(c => c.Id == id, "Products");
        if (category == null) return NotFound();
        var dto = category.Adapt<CategoryDto>();
        dto.ProductCount = category.Products?.Count ?? 0;
        return Ok(dto);
    }

    [HttpDelete(ApiRoutes.Category.Delete)]
    public async Task<IActionResult> Delete(int id)
    {
        var category = _repository.Get(c => c.Id == id);
        if (category == null) return NotFound();
        _repository.Delete(category);
        await _unitOfWork.Complete();
        return Ok();
    }
}
