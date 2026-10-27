using Business_Layer.DTOs.Specializations;
using Business_Layer.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Clinic_System.Controllers
{
    [Route("api/Specializations")]
    [ApiController]
    public class SpecializationsController : ControllerBase
    {
        private readonly ISpecializationService _specializationService;
        public SpecializationsController(ISpecializationService specializationService)
        {
            _specializationService = specializationService;
        }


        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<SpecializationReadDto>),StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<SpecializationReadDto>>>GetAllSpecializations()
        {
            var specializations =
                    await _specializationService.GetAllAsync();

            return Ok(specializations);
        }


        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(SpecializationReadDto),StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<SpecializationReadDto>>GetSpecializationById(int id)
        {
            if (id <= 0)
                return BadRequest(
                    "Specialization ID must be greater than zero.");

            var specialization =
                await _specializationService.GetByIdAsync(id);

            if (specialization is null)
                return NotFound(
                    $"Specialization with ID {id} was not found.");

            return Ok(specialization);
        }


        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> CreateSpecialization([FromBody] SpecializationCreateDto specializationDto)
        {
            await _specializationService.AddAsync(specializationDto);

            return Ok
            (new
            {
                message = "Specialization created successfully."
            });
        }


        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> UpdateSpecialization(int id,[FromBody] SpecializationUpdateDto specializationDto)
        {
            if (id <= 0)
                return BadRequest(
                    "Specialization ID must be greater than zero.");

            var exists =
                await _specializationService.ExistsAsync(id);

            if (!exists)
                return NotFound(
                    $"Specialization with ID {id} was not found.");

            await _specializationService.UpdateAsync(
                id, specializationDto);

            return Ok(new
            {
                message = "Specialization updated successfully."
            });
        }


        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeleteSpecialization(int id)
        {
            if (id <= 0)
                return BadRequest(
                    "Specialization ID must be greater than zero.");

            var exists =
                await _specializationService.ExistsAsync(id);

            if (!exists)
                return NotFound(
                    $"Specialization with ID {id} was not found.");

            try
            {
                await _specializationService.DeleteAsync(id);

                return Ok(new
                {
                    message = "Specialization deleted successfully."
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