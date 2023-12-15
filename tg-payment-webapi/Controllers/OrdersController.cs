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

        return Ok(orders);
    }

    [HttpPut("Update/{id}")]
    public async Task<IActionResult> UpdateOrder(Guid id, [FromBody] OfferingDto offeringDto)
    {
        offeringDto.OfferingId = id;

        // Find order
        var order = await _orderService.GetOrder(id);

        if (order == null)
        {
            return NotFound($"Order not found: {id}");
        }


        try
        {
            await _orderService.UpdateOrder(offeringDto);
        }
        catch (Exception e)
        {
            return NotFound(e.Message);
        }

        return Ok();
    }
}

