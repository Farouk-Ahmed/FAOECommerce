using CleanArchitecture.DataAccess.IRepository;
using CleanArchitecture.DataAccess.Models;
using CleanArchitecture.DataAccess.IUnitOfWorks;
using CleanArchitecture.Services.DTOs.Orders;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;

namespace CleanArchitecture.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IRepository<Order> _repository;
        private readonly IUnitOfWork _unitOfWork;
        public OrderController(IOrderRepository orderRepository, IRepository<Order> repository, IUnitOfWork unitOfWork)
        {
            _orderRepository = orderRepository;
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var orders = _repository.GetAll(null, "OrderItems,OrderItems.Product");
            var orderDtos = orders.Select(order => new OrderDto
            {
                Id = order.Id,
                OrderDate = order.OrderDate,
                TotalAmount = order.TotalAmount,
                Items = order.OrderItems?.Select(oi => new OrderItemDto
                {
                    ProductId = oi.ProductId,
                    ProductName = oi.Product?.Name,
                    Quantity = oi.Quantity,
                    UnitPrice = oi.UnitPrice
                }).ToList() ?? new List<OrderItemDto>()
            }).ToList();
            return Ok(orderDtos);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var order = _repository.Get(o => o.Id == id, "OrderItems,OrderItems.Product");
            if (order == null) return NotFound();
            var dto = new OrderDto
            {
                Id = order.Id,
                OrderDate = order.OrderDate,
                TotalAmount = order.TotalAmount,
                Items = order.OrderItems?.Select(oi => new OrderItemDto
                {
                    ProductId = oi.ProductId,
                    ProductName = oi.Product?.Name,
                    Quantity = oi.Quantity,
                    UnitPrice = oi.UnitPrice
                }).ToList() ?? new List<OrderItemDto>()
            };
            return Ok(dto);
        }

        [HttpGet("by-user/{userId}")]
        public async Task<IActionResult> GetByUser(int userId)
        {
            var orders = await _orderRepository.GetOrdersByUserIdAsync(userId);
            var orderDtos = orders.Select(order => new OrderDto
            {
                Id = order.Id,
                OrderDate = order.OrderDate,
                TotalAmount = order.TotalAmount,
                Items = order.OrderItems?.Select(oi => new OrderItemDto
                {
                    ProductId = oi.ProductId,
                    ProductName = oi.Product?.Name,
                    Quantity = oi.Quantity,
                    UnitPrice = oi.UnitPrice
                }).ToList() ?? new List<OrderItemDto>()
            }).ToList();
            return Ok(orderDtos);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Order order)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            _repository.Add(order);
            await _unitOfWork.Complete();
            var dto = new OrderDto
            {
                Id = order.Id,
                OrderDate = order.OrderDate,
                TotalAmount = order.TotalAmount,
                Items = order.OrderItems?.Select(oi => new OrderItemDto
                {
                    ProductId = oi.ProductId,
                    ProductName = oi.Product?.Name,
                    Quantity = oi.Quantity,
                    UnitPrice = oi.UnitPrice
                }).ToList() ?? new List<OrderItemDto>()
            };
            return Ok(dto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Order order)
        {
            if (id != order.Id) return BadRequest();
            var existing = _repository.Get(o => o.Id == id);
            if (existing == null) return NotFound();
            _repository.Update(order);
            await _unitOfWork.Complete();
            var dto = new OrderDto
            {
                Id = order.Id,
                OrderDate = order.OrderDate,
                TotalAmount = order.TotalAmount,
                Items = order.OrderItems?.Select(oi => new OrderItemDto
                {
                    ProductId = oi.ProductId,
                    ProductName = oi.Product?.Name,
                    Quantity = oi.Quantity,
                    UnitPrice = oi.UnitPrice
                }).ToList() ?? new List<OrderItemDto>()
            };
            return Ok(dto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var order = _repository.Get(o => o.Id == id);
            if (order == null) return NotFound();
            _repository.Delete(order);
            await _unitOfWork.Complete();
            return Ok();
        }
    }
}
