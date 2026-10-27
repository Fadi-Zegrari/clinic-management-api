using Business_Layer.DTOs.PaymentMethods;
using Business_Layer.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Clinic_System.Controllers
{
    [Route("api/PaymentMethods")]
    [ApiController]
    public class PaymentMethodsController : ControllerBase
    {
        private readonly IPaymentMethodService _paymentMethodService;
        public PaymentMethodsController(
            IPaymentMethodService paymentMethodService)
        {
            _paymentMethodService = paymentMethodService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllPaymentMethods()
        {
            var paymentMethods =
                await _paymentMethodService.GetAllAsync();

            return Ok(paymentMethods);
        }


        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetPaymentMethodById(int id)
        {
            if (id <= 0)
                return BadRequest("Payment method ID must be greater than zero.");

            var paymentMethod =
                await _paymentMethodService.GetByIdAsync(id);

            if (paymentMethod is null)
                return NotFound($"Payment method with ID {id} was not found.");

            return Ok(paymentMethod);
        }


        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreatePaymentMethod([FromBody] PaymentMethodCreateDto paymentMethodDto)
        {
            try
            {
                await _paymentMethodService.AddAsync(paymentMethodDto);

                return Ok(new
                {
                    message = "Payment method created successfully."
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


        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdatePaymentMethod(int id,[FromBody] PaymentMethodUpdateDto paymentMethodDto)
        {
            if (id <= 0)
                return BadRequest("Payment method ID must be greater than zero.");

            if (!await _paymentMethodService.ExistsAsync(id))
                return NotFound($"Payment method with ID {id} was not found.");

            try
            {
                await _paymentMethodService.UpdateAsync(id, paymentMethodDto);

                return Ok(new
                {
                    message = "Payment method updated successfully."
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


        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeletePaymentMethod(int id)
        {
            if (id <= 0)
                return BadRequest("Payment method ID must be greater than zero.");

            if (!await _paymentMethodService.ExistsAsync(id))
                return NotFound($"Payment method with ID {id} was not found.");

            try
            {
                await _paymentMethodService.DeleteAsync(id);

                return Ok(new
                {
                    message = "Payment method deleted successfully."
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
    }
}