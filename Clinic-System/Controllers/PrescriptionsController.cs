using Business_Layer.DTOs.Prescriptions;
using Business_Layer.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Clinic_System.Controllers
{
    [Route("api/Prescriptions")]
    [ApiController]
    public class PrescriptionsController : ControllerBase
    {
        private readonly IPrescriptionService _prescriptionService;
        public PrescriptionsController(
            IPrescriptionService prescriptionService)
        {
            _prescriptionService = prescriptionService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<PrescriptionReadDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<PrescriptionReadDto>>> GetAllPrescriptions()
        {
            var prescriptions =
                await _prescriptionService.GetAllAsync();

            return Ok(prescriptions);
        }


        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(PrescriptionReadDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PrescriptionReadDto>> GetPrescriptionById(int id)
        {
            if (id <= 0)
                return BadRequest("Prescription ID must be greater than zero.");

            var prescription =
                await _prescriptionService.GetByIdAsync(id);

            if (prescription is null)
                return NotFound($"Prescription with ID {id} was not found.");

            return Ok(prescription);
        }


        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreatePrescription([FromBody] PrescriptionCreateDto prescriptionDto)
        {
            try
            {
                await _prescriptionService.AddAsync(prescriptionDto);

                return Ok(new
                {
                    message = "Prescription created successfully."
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }


        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdatePrescription(int id, [FromBody] PrescriptionUpdateDto prescriptionDto)
        {
            if (id <= 0)
                return BadRequest("Prescription ID must be greater than zero.");

            try
            {
                await _prescriptionService.UpdateAsync(id, prescriptionDto);

                return Ok(new
                {
                    message = "Prescription updated successfully."
                });
            }
            catch (ArgumentException ex)
            {
                return NotFound(new
                {
                    message = ex.Message
                });
            }
        }


        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeletePrescription(int id)
        {
            if (id <= 0)
                return BadRequest("Prescription ID must be greater than zero.");

            try
            {
                await _prescriptionService.DeleteAsync(id);

                return Ok(new
                {
                    message = "Prescription deleted successfully."
                });
            }
            catch (ArgumentException ex)
            {
                return NotFound(new
                {
                    message = ex.Message
                });
            }
        }


        [HttpGet("exists/{id:int}")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<bool>> PrescriptionExists(int id)
        {
            if (id <= 0)
                return BadRequest("Prescription ID must be greater than zero.");

            var exists = await _prescriptionService.ExistsAsync(id);

            return Ok(exists);
        }
    }
}