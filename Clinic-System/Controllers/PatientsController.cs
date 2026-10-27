using Business_Layer.DTOs.Patients;
using Business_Layer.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace App_Layer.Controllers
{
    [Route("api/Patients")]
    [ApiController]
    public class PatientsController : ControllerBase
    {
        private readonly IPatientService _patientService;
        public PatientsController(IPatientService patientService)
        {
            _patientService = patientService;
        }


        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<PatientReadDto>),StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<PatientReadDto>>> GetAllPatients()
        {
            var patients = await _patientService.GetAllAsync();

            return Ok(patients);
        }


        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(PatientReadDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PatientReadDto>> GetPatientById(int id)
        {
            if (id <= 0)
                return BadRequest("Patient ID must be greater than zero.");

            var patient = await _patientService.GetByIdAsync(id);

            if (patient is null)
                return NotFound($"Patient with ID {id} was not found.");

            return Ok(patient);
        }


        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> CreatePatient([FromBody] PatientCreateDto patientDto)
        {
            await _patientService.AddAsync(patientDto);

            return Ok(new
            {
                message = "Patient created successfully."
            });
        }


        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Update(int id,[FromBody] PatientUpdateDto patientDto)
        {
            if (id <= 0)
                return BadRequest(
                    "Patient ID must be greater than zero.");

            var exists = await _patientService.ExistsAsync(id);

            if (!exists)
                return NotFound(
                    $"Patient with ID {id} was not found.");

            await _patientService.UpdateAsync(id, patientDto);

            return Ok(new
            {
                message = "Patient updated successfully."
            });
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Delete(int id)
        {
            if (id <= 0)
                return BadRequest(
                    "Patient ID must be greater than zero.");

            var exists = await _patientService.ExistsAsync(id);

            if (!exists)
                return NotFound(
                    $"Patient with ID {id} was not found.");

            try
            {
                await _patientService.DeleteAsync(id);

                return Ok(new
                {
                    message = "Patient deleted successfully."
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