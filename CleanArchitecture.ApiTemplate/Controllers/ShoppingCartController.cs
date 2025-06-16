using CleanArchitecture.DataAccess.IRepository;
using CleanArchitecture.DataAccess.Models;
using CleanArchitecture.DataAccess.Repsitory;
using CleanArchitecture.DataAccess.IUnitOfWorks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleanArchitecture.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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

        [HttpGet("user/{userId}")]
        public IActionResult GetCart(int userId)
        {
            var items = _repository.GetAll(i => i.UserId == userId.ToString(), "Product");
            return Ok(items);
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart([FromBody] ShoppingCartItem item)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            _repository.Add(item);
            await _unitOfWork.Complete();
            return Ok(item);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCartItem(int id, [FromBody] ShoppingCartItem item)
        {
            if (id != item.Id) return BadRequest();
            var existing = _repository.Get(i => i.Id == id);
            if (existing == null) return NotFound();
            _repository.Update(item);
            await _unitOfWork.Complete();
            return Ok(item);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveFromCart(int id)
        {
            var item = _repository.Get(i => i.Id == id);
            if (item == null) return NotFound();
            _repository.Delete(item);
            await _unitOfWork.Complete();
            return Ok();
        }

        [HttpPost("checkout/{userId}")]
        public async Task<IActionResult> Checkout(string userId)
        {
            var cartItems = _repository.GetAll(i => i.UserId == userId.ToString(), "Product").ToList();
            if (!cartItems.Any()) return BadRequest("Cart is empty");

            var order = new Order
            {
                UserId = userId.ToString(),
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
                    UnitPrice = cartItem.Product.Price
                };
                _orderItemRepository.Add(orderItem);
                order.OrderItems.Add(orderItem);
                _repository.Delete(cartItem);
            }

            await _unitOfWork.Complete();
            return Ok(order);
        }
    }
}
