using CleanArchitecture.DataAccess.IRepository;
using CleanArchitecture.DataAccess.IUnitOfWorks;
using CleanArchitecture.DataAccess.Models;
using CleanArchitecture.DataAccess.Repsitory;
using CleanArchitecture.Services.DTOs.Orders;
using CleanArchitecture.Services.DTOs.ShoppingCarts;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CleanArchitecture.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ShoppingCartController : ControllerBase
    {
        private readonly IShoppingCartRepository _cartRepository;
        private readonly IRepository<ShoppingCartItem> _repository;
        private readonly IRepository<Order> _orderRepository;
        private readonly IRepository<OrderItem> _orderItemRepository;
        private readonly IRepository<Product> _productRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ShoppingCartController(
            IShoppingCartRepository cartRepository,
            IRepository<ShoppingCartItem> repository,
            IRepository<Order> orderRepository,
            IRepository<OrderItem> orderItemRepository,
            IRepository<Product> productRepository,
            IUnitOfWork unitOfWork)
        {
            _cartRepository = cartRepository;
            _repository = repository;
            _orderRepository = orderRepository;
            _orderItemRepository = orderItemRepository;
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
        }

        private static string GenerateRandomCartCode(int length = 8)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        [HttpGet(ApiRoutes.ShoppingCart.GetCart)]
        public IActionResult GetCart(int userId)
        {
            var items = _repository.GetAll(i => i.UserId == userId.ToString(), "Product");
            var itemDtos = items.Select(i => new ShoppingCartItemDto
            {
                Id = i.Id,
                ProductId = i.ProductId,
                ProductName = i.Product?.Name,
                Quantity = i.Quantity,
                Price = i.Product?.Price ?? 0,
                TotalPrice = i.TotalPrice,
                CartCode = i.CartCode // Include CartCode in response
            }).ToList();
            return Ok(itemDtos);
        }

        [HttpPost(ApiRoutes.ShoppingCart.AddToCart)]
        public async Task<IActionResult> AddToCart([FromBody] List<ShoppingCartItemCreateDto> items)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = User?.Claims?.FirstOrDefault(c =>
                c.Type == "sub" || c.Type == "UserId" || c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var cartCode = GenerateRandomCartCode();

            var addedItems = new List<ShoppingCartItem>();

            foreach (var dto in items.Where(i => i.ProductId > 0 && i.Quantity > 0))
            {
                var product = _productRepository.Get(p => p.Id == dto.ProductId);
                if (product == null)
                    continue;

                var newItem = new ShoppingCartItem
                {
                    UserId = userId,
                    ProductId = dto.ProductId,
                    ProductName = product.Name,
                    Quantity = dto.Quantity,
                    Price = product.Price,
                    TotalPrice = product.Price * dto.Quantity,
                    CartCode = cartCode,
                    CreatedDate = DateTime.Now
                };

                _repository.Add(newItem);
                addedItems.Add(newItem);
            }

            await _unitOfWork.Complete();

            var result = new
            {
                cartCode = cartCode,
                items = addedItems.Select(item => new
                {
                    item.Id,
                    item.ProductId,
                    item.ProductName,
                    item.Quantity,
                    item.Price,
                    item.TotalPrice
                })
            };

            return Ok(result);
        }
        [HttpPut(ApiRoutes.ShoppingCart.UpdateCartItem)]
        public async Task<IActionResult> UpdateCartItem(int id, [FromBody] ShoppingCartItemUpdateDto dto)
        {
            if (id != dto.Id) return BadRequest();
            var existing = _repository.Get(i => i.Id == id);
            if (existing == null) return NotFound();
            dto.Adapt(existing);
            _repository.Update(existing);
            await _unitOfWork.Complete();
            var resultDto = new ShoppingCartItemDto
            {
                Id = existing.Id,
                ProductId = existing.ProductId,
                ProductName = existing.Product?.Name,
                Quantity = existing.Quantity,
                Price = existing.Product?.Price ?? 0
            };
            return Ok(resultDto);
        }

        [HttpDelete(ApiRoutes.ShoppingCart.RemoveFromCart)]
        public async Task<IActionResult> RemoveFromCart(int id)
        {
            var item = _repository.Get(i => i.Id == id);
            if (item == null) return NotFound();
            _repository.Delete(item);
            await _unitOfWork.Complete();
            return Ok();
        }

        [HttpPost(ApiRoutes.ShoppingCart.Checkout)]
        public async Task<IActionResult> Checkout(string userId, string cartCode)
        {
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(cartCode))
                return BadRequest("UserId and CartCode are required");

            var cartItems = _repository
                .GetAll(i => i.UserId == userId && i.CartCode == cartCode, "Product")
                .ToList();

            if (!cartItems.Any())
                return BadRequest("Cart is empty");

            // تحقق من توفّر الكمية لكل منتج
            foreach (var item in cartItems)
            {
                if (item.Product == null)
                    return BadRequest($"Product {item.ProductId} not found");

                if (item.Product.Quantity < item.Quantity)
                    return BadRequest($"Insufficient stock for {item.Product.Name}. Available: {item.Product.Quantity}, Requested: {item.Quantity}");
            }

            var order = new Order
            {
                UserId = userId,
                OrderDate = DateTime.UtcNow,
                TotalAmount = cartItems.Sum(i => i.Product.Price * i.Quantity),
                OrderItems = new List<OrderItem>()
            };
            _orderRepository.Add(order);

            foreach (var cartItem in cartItems)
            {
                var orderItem = new OrderItem
                {
                    Order = order,
                    ProductId = cartItem.ProductId,
                    Quantity = cartItem.Quantity,
                    UnitPrice = cartItem.Product.Price,
                    CartCode = cartItem.CartCode
                };
                _orderItemRepository.Add(orderItem);
                order.OrderItems.Add(orderItem);

                // خصم الكمية من المنتج
                cartItem.Product.Quantity -= cartItem.Quantity;

                // حذف العنصر من السلة
                _repository.Delete(cartItem);
            }

            await _unitOfWork.Complete();

            var orderDto = new CleanArchitecture.Services.DTOs.Orders.OrderDto
            {
                Id = order.Id,
                OrderDate = order.OrderDate,
                TotalAmount = order.TotalAmount,
                Items = cartItems.Select(ci => new CleanArchitecture.Services.DTOs.Orders.OrderItemDto
                {
                    ProductId = ci.ProductId,
                    ProductName = ci.Product?.Name,
                    Quantity = ci.Quantity,
                    UnitPrice = ci.Product?.Price ?? 0,
                    CartCode = ci.CartCode
                }).ToList()
            };

            return Ok(orderDto);
        }
    }
}
