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

        [HttpGet (ApiRoutes.Order.GetAll)]
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
                    UnitPrice = oi.UnitPrice,
                    CartCode = oi.CartCode
                }).ToList() ?? new List<OrderItemDto>()
            }).ToList();
            return Ok(orderDtos);
        }

        [HttpGet(ApiRoutes.Order.GetByID)]
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
                    UnitPrice = oi.UnitPrice,
                    CartCode = oi.CartCode
                }).ToList() ?? new List<OrderItemDto>()
            };
            return Ok(dto);
        }

        [HttpGet(ApiRoutes.Order.GetByUser)]
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
                    UnitPrice = oi.UnitPrice,
                    CartCode = oi.CartCode
                }).ToList() ?? new List<OrderItemDto>()
            }).ToList();
            return Ok(orderDtos);
        }

        [HttpPost (ApiRoutes.Order.Create)]
        public async Task<IActionResult> Create([FromBody] OrderCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var order = new Order
            {
                UserId = dto.UserId,
                OrderDate = DateTime.UtcNow,
                TotalAmount = dto.Items.Sum(i => i.UnitPrice * i.Quantity),
                OrderItems = dto.Items.Select(i => new OrderItem
                {
                    ProductId = i.ProductId,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice
                }).ToList()
            };
            _repository.Add(order);
            await _unitOfWork.Complete();
            var resultDto = new OrderDto
            {
                Id = order.Id,
                OrderDate = order.OrderDate,
                TotalAmount = order.TotalAmount,
                Items = order.OrderItems?.Select(oi => new OrderItemDto
                {
                    ProductId = oi.ProductId,
                    ProductName = oi.Product?.Name,
                    Quantity = oi.Quantity,
                    UnitPrice = oi.UnitPrice,
                    CartCode = oi.CartCode
                }).ToList() ?? new List<OrderItemDto>()
            };
            return Ok(resultDto);
        }

        [HttpPut(ApiRoutes.Order.Update)]
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
                    UnitPrice = oi.UnitPrice,
                    CartCode = oi.CartCode
                }).ToList() ?? new List<OrderItemDto>()
            };
            return Ok(dto);
        }

        [HttpDelete(ApiRoutes.Order.Delete)]
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
