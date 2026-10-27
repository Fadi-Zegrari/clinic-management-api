using Business_Layer.DTOs.Payments;
using Business_Layer.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Clinic_System.Controllers
{
    [Route("api/Payments")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        public PaymentsController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<PaymentReadDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<PaymentReadDto>>> GetAllPayments()
        {
            var payments = await _paymentService.GetAllAsync();

            return Ok(payments);
        }


        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(PaymentReadDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PaymentReadDto>> GetPaymentById(int id)
        {
            if (id <= 0)
                return BadRequest("Payment ID must be greater than zero.");

            var payment = await _paymentService.GetByIdAsync(id);

            if (payment is null)
                return NotFound($"Payment with ID {id} was not found.");

            return Ok(payment);
        }


        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> CreatePayment([FromBody] PaymentCreateDto paymentDto)
        {
            try
            {
                await _paymentService.AddAsync(paymentDto);

                return Ok(new
                {
                    message = "Payment registered successfully."
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new
                {
                    message = ex.Message
                });
            }
        }


        [HttpGet("exists/{id:int}")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<bool>> PaymentExists(int id)
        {
            if (id <= 0)
                return BadRequest("Payment ID must be greater than zero.");

            var exists = await _paymentService.ExistsAsync(id);

            return Ok(exists);
        }
    }
}