using Microsoft.AspNetCore.Mvc;
using Utilites;
using CleanArchitecture.DataAccess.Models;
using CleanArchitecture.DataAccess.IRepository;
using CleanArchitecture.DataAccess.IUnitOfWorks;
using System.Threading.Tasks;

namespace CleanArchitecture.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestImageController : ControllerBase
    {
        private readonly IRepository<ProductPhoto> _photoRepository;
        private readonly IUnitOfWork _unitOfWork;
        public TestImageController(IRepository<ProductPhoto> photoRepository, IUnitOfWork unitOfWork)
        {
            _photoRepository = photoRepository;
            _unitOfWork = unitOfWork;
        }

        [HttpPost("image")]
        public async Task<IActionResult> UploadImage(IFormFile image)
        {
            try
            {
                if (image == null || image.Length == 0)
                    return BadRequest("Please upload a valid image.");

                var imageUrl = await ImageHelper.SaveImageAsync(image);

                return Ok(new { imageUrl });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error uploading image: {ex.Message}");
            }
        }

        [HttpPost("add-to-product")]
        public async Task<IActionResult> AddImageToProduct([FromQuery] int productId, [FromQuery] string imageUrl)
        {
            var photo = new ProductPhoto { ProductId = productId, Url = imageUrl };
            _photoRepository.Add(photo);
            await _unitOfWork.Complete();
            return Ok(photo);
        }

        [HttpDelete("remove-from-product/{photoId}")]
        public async Task<IActionResult> RemoveImageFromProduct(int photoId)
        {
            var photo = _photoRepository.Get(p => p.Id == photoId);
            if (photo == null) return NotFound();
            ImageHelper.DeleteImage(photo.Url);
            _photoRepository.Delete(photo);
            await _unitOfWork.Complete();
            return Ok();
        }

        [HttpGet("product-images/{productId}")]
        public IActionResult GetProductImages(int productId)
        {
            var photos = _photoRepository.GetAll(p => p.ProductId == productId);
            return Ok(photos);
        }

        [HttpDelete("image")]
        public IActionResult DeleteImage([FromQuery] string imageUrl)
        {
            try
            {
                bool deleted = ImageHelper.DeleteImage(imageUrl);
                return deleted
                    ? Ok("Image deleted successfully.")
                    : NotFound("Image not found.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error deleting image: {ex.Message}");
            }
        }

        [HttpPut("image")]
        public async Task<IActionResult> ReplaceImage(IFormFile newImage, [FromQuery] string? oldImageUrl)
        {
            try
            {
                if (newImage == null || newImage.Length == 0)
                    return BadRequest("Please upload a valid new image.");

                var newImageUrl = await ImageHelper.ReplaceImageAsync(newImage, oldImageUrl);

                return Ok(new { newImageUrl });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error replacing image: {ex.Message}");
            }
        }
    }
}
