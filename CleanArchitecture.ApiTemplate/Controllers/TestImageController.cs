namespace CleanArchitecture.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestImageController : ControllerBase
    {
        private readonly IRepository<ProductPhoto> _photoRepository;
        private readonly IRepository<Product> _productRepository;
        private readonly IUnitOfWork _unitOfWork;

        public TestImageController(
            IRepository<ProductPhoto> photoRepository,
            IRepository<Product> productRepository,
            IUnitOfWork unitOfWork)
        {
            _photoRepository = photoRepository;
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
        }

        [HttpPost("upload-to-product")]
        public async Task<IActionResult> UploadAndAssignImages(
            [FromForm] int productId,
            [FromForm] List<IFormFile> images,
            [FromForm] int? mainIndex)
        {
            if (images == null || images.Count == 0)
                return BadRequest("Please upload at least one image.");

            // Replace the 'Any' call with a proper query
            var productExists = _productRepository.GetAllQuery(p => p.Id == productId).Any();
            if (!productExists)
                return NotFound("Product not found.");

            List<ProductPhoto> photos = new();

            for (int i = 0; i < images.Count; i++)
            {
                var image = images[i];

                try
                {
                    var url = await ImageHelper.SaveImageAsync(image);
                    photos.Add(new ProductPhoto
                    {

                        ProductId = productId,
                        Url = url,
                        IsMain = (mainIndex.HasValue && mainIndex.Value == i),
                    });
                }
                catch (Exception ex)
                {
                    return StatusCode(500, $"Error saving image: {ex.Message}");
                }
            }

            try
            {
                // Unset previous main photo if needed
                if (mainIndex.HasValue)
                {
                    var existingMain = _photoRepository.GetAll(p => p.ProductId == productId && p.IsMain).ToList();
                    foreach (var item in existingMain)
                        item.IsMain = false;
                }

                foreach (var photo in photos)
                {
                    _photoRepository.Add(photo);
                }

                await _unitOfWork.Complete();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Database error: {ex.Message}");
            }

            return Ok(new
            {
                Message = "Images uploaded and linked successfully.",
                ProductId = productId,
                Photos = photos.Select(p => new { p.Url, p.IsMain })
            });
        }

        [HttpGet("product-images/{productId}")]
        public IActionResult GetProductImages(int productId)
        {
            var photos = _photoRepository
                .GetQuery()
                .Where(p => p.ProductId == productId)
                .Include(p => p.Product)
                .Select(p => new ProductPhotoDto
                {
                    Id = p.Id,
                    Url = p.Url,
                    IsMain = p.IsMain,
                    ProductName = p.Product.Name
                })
                .ToList();

            return Ok(photos);
        }

        [HttpDelete("remove-photo/{photoId}")]
        public async Task<IActionResult> RemoveImage(int photoId)
        {
            var photo = _photoRepository.Get(p => p.Id == photoId);
            if (photo == null) return NotFound("Photo not found.");

            ImageHelper.DeleteImage(photo.Url);
            _photoRepository.Delete(photo);
            await _unitOfWork.Complete();

            return Ok("Image deleted.");
        }

        [HttpPut("replace-photo")]
        public async Task<IActionResult> ReplaceImage(IFormFile newImage, [FromQuery] string? oldImageUrl)
        {
            try
            {
                if (newImage == null || newImage.Length == 0)
                    return BadRequest("Please provide a valid new image.");

                var newImageUrl = await ImageHelper.ReplaceImageAsync(newImage, oldImageUrl);
                return Ok(new { newImageUrl });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error replacing image: {ex.Message}");
            }
        }

        [HttpDelete("delete-by-url")]
        public IActionResult DeleteImageByUrl([FromQuery] string imageUrl)
        {
            try
            {
                bool deleted = ImageHelper.DeleteImage(imageUrl);
                return deleted ? Ok("Image deleted successfully.") : NotFound("Image not found.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error deleting image: {ex.Message}");
            }
        }
    }
}