using Business_Layer.DTOs.MedicalRecords;
using Business_Layer.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Clinic_System.Controllers
{
    [Route("api/MedicalRecords")]
    [ApiController]
    public class MedicalRecordsController : ControllerBase
    {
        private readonly IMedicalRecordService _medicalRecordService;
        public MedicalRecordsController(IMedicalRecordService medicalRecordService)
        {
            _medicalRecordService = medicalRecordService;
        }


        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<MedicalRecordReadDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<MedicalRecordReadDto>>> GetAllMedicalRecords()
        {
            var medicalRecords =
                await _medicalRecordService.GetAllAsync();

            return Ok(medicalRecords);
        }


        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(MedicalRecordReadDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<MedicalRecordReadDto>> GetMedicalRecordById(int id)
        {
            if (id <= 0)
                return BadRequest("Medical record ID must be greater than zero.");

            var medicalRecord =
                await _medicalRecordService.GetByIdAsync(id);

            if (medicalRecord is null)
                return NotFound($"Medical record with ID {id} was not found.");

            return Ok(medicalRecord);
        }


        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateMedicalRecord([FromBody] MedicalRecordCreateDto medicalRecordDto)
        {
            try
            {
                await _medicalRecordService.AddAsync(medicalRecordDto);

                return Ok(new
                {
                    message = "Medical record created successfully."
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }


        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateMedicalRecord(int id,[FromBody] MedicalRecordUpdateDto medicalRecordDto)
        {
            if (id <= 0)
                return BadRequest("Medical record ID must be greater than zero.");

            try
            {
                await _medicalRecordService.UpdateAsync(id, medicalRecordDto);

                return Ok(new
                {
                    message = "Medical record updated successfully."
                });
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }


        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteMedicalRecord(int id)
        {
            if (id <= 0)
                return BadRequest("Medical record ID must be greater than zero.");

            try
            {
                await _medicalRecordService.DeleteAsync(id);

                return Ok(new
                {
                    message = "Medical record deleted successfully."
                });
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }


        [HttpGet("exists/{id:int}")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<bool>> MedicalRecordExists(int id)
        {
            if (id <= 0)
                return BadRequest("Medical record ID must be greater than zero.");

            var exists = await _medicalRecordService.ExistsAsync(id);

            return Ok(exists);
        }
    }
}