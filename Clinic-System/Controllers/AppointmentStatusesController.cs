using Business_Layer.DTOs.AppointmentStatuses;
using Business_Layer.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Clinic_System.Controllers
{
    [Route("api/AppointmentStatuses")]
    [ApiController]
    public class AppointmentStatusesController : ControllerBase
    {
        private readonly IAppointmentStatusService _appointmentStatusService;

        public AppointmentStatusesController(
            IAppointmentStatusService appointmentStatusService)
        {
            _appointmentStatusService = appointmentStatusService;
        }


        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<AppointmentStatusReadDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<AppointmentStatusReadDto>>>GetAllStatuses()
        {
            var statuses = await _appointmentStatusService.GetAllAsync();

            return Ok(statuses);
        }


        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(AppointmentStatusReadDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<AppointmentStatusReadDto>>GetStatusById(int id)
        {
            if (id <= 0)
                return BadRequest("Appointment status ID must be greater than zero.");

            var status = await _appointmentStatusService.GetByIdAsync(id);

            if (status is null)
                return NotFound($"Appointment status with ID {id} was not found.");

            return Ok(status);
        }


        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> CreateStatus([FromBody] AppointmentStatusCreateDto dto)
        {
            await _appointmentStatusService.AddAsync(dto);

            return Ok(new
            {
                message = "Appointment status created successfully."
            });
        }


        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> UpdateStatus(int id,[FromBody] AppointmentStatusUpdateDto dto)
        {
            if (id <= 0)
                return BadRequest("Appointment status ID must be greater than zero.");

            var exists = await _appointmentStatusService.ExistsAsync(id);

            if (!exists)
                return NotFound($"Appointment status with ID {id} was not found.");

            await _appointmentStatusService.UpdateAsync(id, dto);

            return Ok(new
            {
                message = "Appointment status updated successfully."
            });
        }


        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult> DeleteStatus(int id)
        {
            if (id <= 0)
                return BadRequest("Appointment status ID must be greater than zero.");

            var exists = await _appointmentStatusService.ExistsAsync(id);

            if (!exists)
                return NotFound($"Appointment status with ID {id} was not found.");

            try
            {
                await _appointmentStatusService.DeleteAsync(id);

                return Ok(new
                {
                    message = "Appointment status deleted successfully."
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