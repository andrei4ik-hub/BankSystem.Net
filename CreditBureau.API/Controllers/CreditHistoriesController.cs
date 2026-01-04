using Microsoft.AspNetCore.Mvc;
using CreditBureau.Core.Models;
using CreditBureau.Core.DTOs;
using CreditBureau.Services.Interfaces;

namespace CreditBureau.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CreditHistoriesController : ControllerBase
    {
        private readonly ICreditHistoryService _creditHistoryService;

        public CreditHistoriesController(ICreditHistoryService creditHistoryService)
        {
            _creditHistoryService = creditHistoryService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CreditHistoryDto>>> GetCreditHistories()
        {
            var histories = await _creditHistoryService.GetAllCreditHistoriesAsync();
            return Ok(histories);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CreditHistoryDto>> GetCreditHistory(int id)
        {
            var history = await _creditHistoryService.GetCreditHistoryByIdAsync(id);
            if (history == null)
                return NotFound();
            return Ok(history);
        }

        [HttpGet("borrower/{borrowerId}")]
        public async Task<ActionResult<IEnumerable<CreditHistoryDto>>> GetByBorrower(int borrowerId)
        {
            var histories = await _creditHistoryService.GetCreditHistoriesByBorrowerAsync(borrowerId);
            return Ok(histories);
        }

        [HttpPost]
        public async Task<ActionResult<CreditHistory>> CreateCreditHistory(CreateCreditHistoryDto createDto)
        {
            try
            {
                var createdHistory = await _creditHistoryService.CreateCreditHistoryAsync(createDto);
                return CreatedAtAction(nameof(GetCreditHistory), new { id = createdHistory.Id }, createdHistory);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<CreditHistory>> UpdateCreditHistory(int id, CreditHistory creditHistory)
        {
            if (id != creditHistory.Id)
                return BadRequest("ID в пути не совпадает с ID в теле запроса");

            var updatedHistory = await _creditHistoryService.UpdateCreditHistoryAsync(id, creditHistory);
            if (updatedHistory == null)
                return NotFound();
            return Ok(updatedHistory);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCreditHistory(int id)
        {
            var result = await _creditHistoryService.DeleteCreditHistoryAsync(id);
            if (!result)
                return NotFound();
            return NoContent();
        }
    }
}