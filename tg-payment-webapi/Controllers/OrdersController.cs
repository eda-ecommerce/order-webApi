using Core.Models.DTOs.Order;

[Route("api/Orders")]
[ApiController]
public class OrdersController : ControllerBase
{

    private readonly ILogger<OrdersController> _logger;
    private readonly IOrderService _orderService;

    public OrdersController(ILogger<OrdersController> logger, IOrderService orderService)
    {
        _logger = logger;
        _orderService = orderService;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetOrders()
    {
        _logger.LogInformation($"Get orders request");

        List<OrderDto> orders = null;
        orders = await _orderService.GetOrders();
        
        if (orders == null)
        {
            return NotFound($"Orders not found");
        }

        return Ok(orders);
    }
    
    [HttpGet("{id}/Order")]
    public async Task<IActionResult> GetOrder(Guid id)
    {
        _logger.LogInformation($"Get orders request");
    
        OrderDto order = null;
        order = await _orderService.GetOrder(id);
        
        if (order == null)
        {
            return NotFound($"Order not found: {id}");
        }
    
        return Ok(order);
    }

    [HttpPut("Update/{id}")]
    public async Task<IActionResult> UpdateOrder(Guid id, [FromBody] OrderUpdateDto OrderUpdateDto)
    {
        // Find order
        var order = await _orderService.GetOrder(id);
    
        if (order == null)
        {
            return NotFound($"Order not found: {id}");
        }
    
        try
        {
            await _orderService.UpdateOrder(id, OrderUpdateDto);
        }
        catch (Exception e)
        {
            return NotFound(e.Message);
        }
    
        return Ok($"Order Updated Successfully: {id}");
    }
}

