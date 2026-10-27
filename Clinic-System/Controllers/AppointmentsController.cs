using Business_Layer.DTOs.Appointments;
using Business_Layer.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Clinic_System.Controllers
{
    [Route("api/Appointments")]
    [ApiController]
    public class AppointmentsController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;
        public AppointmentsController(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }


        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<AppointmentReadDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<AppointmentReadDto>>> GetAllAppointments()
        {
            var appointments = await _appointmentService.GetAllAsync();

            return Ok(appointments);
        }


        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(AppointmentReadDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<AppointmentReadDto>> GetAppointmentById(int id)
        {
            if (id <= 0)
                return BadRequest("Appointment ID must be greater than zero.");

            var appointment = await _appointmentService.GetByIdAsync(id);

            if (appointment is null)
                return NotFound($"Appointment with ID {id} was not found.");

            return Ok(appointment);
        }


        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> CreateAppointment([FromBody] AppointmentCreateDto appointmentDto)
        {
            try
            {
                await _appointmentService.AddAsync(appointmentDto);

                return Ok(new
                {
                    message = "Appointment created successfully."
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
        public async Task<ActionResult> UpdateAppointment(int id, [FromBody] AppointmentUpdateDto appointmentDto)
        {
            if (id <= 0)
                return BadRequest("Appointment ID must be greater than zero.");

            var exists = await _appointmentService.ExistsAsync(id);

            if (!exists)
                return NotFound($"Appointment with ID {id} was not found.");

            try
            {
                await _appointmentService.UpdateAsync(id, appointmentDto);

                return Ok(new
                {
                    message = "Appointment updated successfully."
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


        [HttpPost("{id:int}/cancel")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> CancelAppointment(int id)
        {
            if (id <= 0)
                return BadRequest("Appointment ID must be greater than zero.");

            var exists = await _appointmentService.ExistsAsync(id);

            if (!exists)
                return NotFound($"Appointment with ID {id} was not found.");

            try
            {
                await _appointmentService.CancelAsync(id);

                return Ok(new
                {
                    message = "Appointment canceled successfully."
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