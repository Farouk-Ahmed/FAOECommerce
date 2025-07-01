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

        [HttpGet(ApiRoutes.Product.GetAll)]
        public async Task<IActionResult> GetAll()
        {
            var products = await _productRepository.GetProductsWithCategoriesAsync();
            var productDtos = products.Adapt<List<ProductDto>>();
            foreach (var (dto, entity) in productDtos.Zip(products, (dto, entity) => (dto, entity)))
            {
                dto.CategoryName = entity.Category?.Name;
                dto.PhotoUrls = entity.Photos?.Select(p => p.Url).ToList() ?? new List<string>();
            }
            return Ok(productDtos);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var product = _repository.Get(p => p.Id == id, "Category,Photos");
            if (product == null) return NotFound();
            var dto = product.Adapt<ProductDto>();
            dto.CategoryName = product.Category?.Name;
            dto.PhotoUrls = product.Photos?.Select(p => p.Url).ToList() ?? new List<string>();
            return Ok(dto);
        }

        [HttpPost(ApiRoutes.Product.Create)]
        public async Task<IActionResult> Create([FromBody] ProductCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var product = dto.Adapt<Product>();
            _repository.Add(product);
            await _unitOfWork.Complete();
            var createdProduct = await _repository
       .GetQuery()
       .Include(p => p.Category)
       .Include(p => p.Photos) 
       .FirstOrDefaultAsync(p => p.Id == product.Id);

            var resultDto = product.Adapt<ProductDto>();
            resultDto.CategoryName = product.Category?.Name;
            resultDto.PhotoUrls = product.Photos?.Select(p => p.Url).ToList() ?? new List<string>();
            return Ok(resultDto);
        }

        [HttpPut(ApiRoutes.Product.Update)]
        public async Task<IActionResult> Update(int id, [FromBody] ProductUpdateDto dto)
        {
            if (id != dto.Id) return BadRequest();
            var existing = _repository.Get(p => p.Id == id);
            if (existing == null) return NotFound();
            dto.Adapt(existing);
            _repository.Update(existing);
            await _unitOfWork.Complete();
            var resultDto = existing.Adapt<ProductDto>();
            resultDto.CategoryName = existing.Category?.Name;
            resultDto.PhotoUrls = existing.Photos?.Select(p => p.Url).ToList() ?? new List<string>();
            return Ok(resultDto);
        }

        [HttpDelete(ApiRoutes.Product.Delete)]
        public async Task<IActionResult> Delete(int id)
        {
            var product = _repository.Get(p => p.Id == id);
            if (product == null) return NotFound();
            _repository.Delete(product);
            await _unitOfWork.Complete();
            return Ok();
        }

        [HttpGet(ApiRoutes.Product.GetByCategory)]
        public async Task<IActionResult> GetByCategory(int categoryId)
        {
            var products = await _productRepository.GetProductsByCategoryAsync(categoryId);
            var productDtos = products.Adapt<List<ProductDto>>();
            foreach (var (dto, entity) in productDtos.Zip(products, (dto, entity) => (dto, entity)))
            {
                dto.CategoryName = entity.Category?.Name;
                dto.PhotoUrls = entity.Photos?.Select(p => p.Url).ToList() ?? new List<string>();
            }
            return Ok(productDtos);
        }
    }
}
