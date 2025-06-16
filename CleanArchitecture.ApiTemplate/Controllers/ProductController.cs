using CleanArchitecture.DataAccess.IRepository;
using CleanArchitecture.DataAccess.Models;
using CleanArchitecture.DataAccess.IUnitOfWorks;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Linq;

namespace CleanArchitecture.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly IRepository<Product> _repository;
        private readonly IUnitOfWork _unitOfWork;
        public ProductController(IProductRepository productRepository, IRepository<Product> repository, IUnitOfWork unitOfWork)
        {
            _productRepository = productRepository;
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var products = await _productRepository.GetProductsWithCategoriesAsync();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var product = _repository.Get(p => p.Id == id, "Category");
            if (product == null) return NotFound();
            return Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Product product)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            _repository.Add(product);
            await _unitOfWork.Complete();
            return Ok(product);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Product product)
        {
            if (id != product.Id) return BadRequest();
            var existing = _repository.Get(p => p.Id == id);
            if (existing == null) return NotFound();
            _repository.Update(product);
            await _unitOfWork.Complete();
            return Ok(product);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var product = _repository.Get(p => p.Id == id);
            if (product == null) return NotFound();
            _repository.Delete(product);
            await _unitOfWork.Complete();
            return Ok();
        }

        [HttpGet("by-category/{categoryId}")]
        public async Task<IActionResult> GetByCategory(int categoryId)
        {
            var products = await _productRepository.GetProductsByCategoryAsync(categoryId);
            return Ok(products);
        }
    }
}
