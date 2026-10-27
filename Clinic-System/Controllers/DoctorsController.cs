using Business_Layer.DTOs.Doctors;
using Business_Layer.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Clinic_System.Controllers
{
    [Route("api/Doctors")]
    [ApiController]
    public class DoctorsController : ControllerBase
    {
        private readonly IDoctorService _doctorService;
        public DoctorsController(IDoctorService doctorService)
        {
            _doctorService = doctorService;
        }


        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<DoctorReadDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<DoctorReadDto>>> GetAllDoctors()
        {
            var doctors = await _doctorService.GetAllAsync();

            return Ok(doctors);
        }


        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(DoctorReadDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<DoctorReadDto>> GetDoctorById(int id)
        {
            if (id <= 0)
                return BadRequest("Doctor ID must be greater than zero.");

            var doctor = await _doctorService.GetByIdAsync(id);

            if (doctor is null)
                return NotFound($"Doctor with ID {id} was not found.");

            return Ok(doctor);
        }


        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> CreateDoctor([FromBody] DoctorCreateDto doctorDto)
        {
            await _doctorService.AddAsync(doctorDto);

            return Ok(new
            {
                message = "Doctor created successfully."
            });
        }


        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> UpdateDoctor(int id,[FromBody] DoctorUpdateDto doctorDto)
        {
            if (id <= 0)
                return BadRequest("Doctor ID must be greater than zero.");

            var exists = await _doctorService.ExistsAsync(id);

            if (!exists)
                return NotFound($"Doctor with ID {id} was not found.");

            await _doctorService.UpdateAsync(id, doctorDto);

            return Ok(new
            {
                message = "Doctor updated successfully."
            });
        }


        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeleteDoctor(int id)
        {
            if (id <= 0)
                return BadRequest("Doctor ID must be greater than zero.");

            var exists = await _doctorService.ExistsAsync(id);

            if (!exists)
                return NotFound($"Doctor with ID {id} was not found.");

            try
            {
                await _doctorService.DeleteAsync(id);

                return Ok(new
                {
                    message = "Doctor deleted successfully."
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