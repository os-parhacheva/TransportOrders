using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using TransportOrders.API.DTO_Model;
using TransportOrders.API.DTO;
using TransportOrders.Domain.Model;
using TransportOrders.Infrasrtructure.Data;
using TransportOrders.Infrasrtructure.Repository;

namespace TransportOrders.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : Controller
    {
        private readonly OrderRepository _orderRepository;

        public OrderController(Context context)
        {
            _orderRepository = new OrderRepository(context);
        }

        //GET: api/Order
        [HttpGet("")]
        public async Task<ActionResult<IEnumerable<OrderDTO>>> GetOrders()
        {
            var orders = await _orderRepository.GetAllAsync();

            if (orders == null) 
            {
                return NotFound();
            }

            return ConversionDTO.ConvertOrdersToOrderDTOs(orders);
        }

        //Get api/Order/:orderId
        [HttpGet("{orderId}")]
        public async Task<ActionResult<OrderDTO>> GetOrder(Guid orderId)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null)
            {
                return NotFound();
            }
            return ConversionDTO.GeOrderDTOFromOrder(order);
        }

        // POST api/Order
        [HttpPost]
        public async Task<ActionResult<Order>> PostOrder(Order order)
        {
            var validationResults = new List<ValidationResult>();
            var context = new ValidationContext(order);

            if (!Validator.TryValidateObject(order, context, validationResults, true))
            {
                foreach (var error in validationResults)
                {
                    foreach (var member in error.MemberNames)
                    {
                        ModelState.AddModelError(member, error.ErrorMessage);
                    }
                }

                return ValidationProblem(ModelState);
            }
            await _orderRepository.AddAsync(order);
            return Ok();
        }

        // PUT api/User/:id
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(Guid id, [FromBody] Order order)
        {
            var validationResults = new List<ValidationResult>();
            var context = new ValidationContext(order);

            if (!Validator.TryValidateObject(order, context, validationResults, true))
            {
                foreach (var error in validationResults)
                {
                    foreach (var member in error.MemberNames)
                    {
                        ModelState.AddModelError(member, error.ErrorMessage);
                    }
                }

                return ValidationProblem(ModelState);
            }

            var _user = await _orderRepository.GetByIdAsync(id);
            if (id != order.Id || _user == null)
            {
                return BadRequest();
            }

            await _orderRepository.UpdateAsync(order);
            return NoContent();
        }

        // DELETE api/User/:id
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            Order order = await _orderRepository.GetByIdAsync(id);

            if (order == null)
            {
                return NotFound();
            }
            await _orderRepository.DeleteAsync(id);
            return NoContent();
        }
    }
}
