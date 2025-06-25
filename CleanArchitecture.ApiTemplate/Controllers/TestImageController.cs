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

      
        [HttpPost("multi-image")]
        public async Task<IActionResult> UploadMultipleImages(List<IFormFile> images)
        {
            if (images == null || images.Count == 0)
                return BadRequest("Please upload one or more valid images.");

            List<string> imageUrls = new();

            foreach (var image in images)
            {
                var url = await ImageHelper.SaveImageAsync(image);
                imageUrls.Add(url);
            }

            return Ok(new { imageUrls });
        }

        [HttpPost("add-multiple-images")]
        public async Task<IActionResult> AddImagesToProduct([FromBody] ProductImagesDto model)
        {
            if (model == null || model.ImageUrls == null || !model.ImageUrls.Any())
                return BadRequest("Product ID and image URLs are required.");

            // Create a list of photo entities
            var photos = model.ImageUrls.Select(url => new ProductPhoto
            {
                ProductId = model.ProductId,
                Url = url
            }).ToList();

            // Save photos to the database
            foreach (var photo in photos)
            {
                _photoRepository.Add(photo); // Make sure this supports adding one by one or use AddRange if available
            }

            await _unitOfWork.Complete();

            // Return success response
            return Ok(new
            {
                Message = "Images added successfully.",
                PhotoCount = photos.Count,
                ProductId = model.ProductId,
                Images = photos.Select(p => p.Url).ToList()
            });
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
