using Microsoft.AspNetCore.Mvc;
using CreditBureau.Core.Models;
using CreditBureau.Services.Interfaces;

namespace CreditBureau.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LendersController : ControllerBase
    {
        private readonly ILenderService _lenderService;

        public LendersController(ILenderService lenderService)
        {
            _lenderService = lenderService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Lender>>> GetLenders()
        {
            var lenders = await _lenderService.GetAllLendersAsync();
            return Ok(lenders);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Lender>> GetLender(int id)
        {
            var lender = await _lenderService.GetLenderByIdAsync(id);
            if (lender == null)
                return NotFound();
            return Ok(lender);
        }

        [HttpPost]
        public async Task<ActionResult<Lender>> CreateLender(Lender lender)
        {
            try
            {
                var createdLender = await _lenderService.CreateLenderAsync(lender);
                return CreatedAtAction(nameof(GetLender), new { id = createdLender.Id }, createdLender);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Lender>> UpdateLender(int id, Lender lender)
        {
            if (id != lender.Id)
                return BadRequest("ID в пути не совпадает с ID в теле запроса");

            var updatedLender = await _lenderService.UpdateLenderAsync(id, lender);
            if (updatedLender == null)
                return NotFound();
            return Ok(updatedLender);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteLender(int id)
        {
            var result = await _lenderService.DeleteLenderAsync(id);
            if (!result)
                return NotFound();
            return NoContent();
        }
    }
}