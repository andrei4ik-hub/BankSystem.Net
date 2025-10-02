
using Microsoft.AspNetCore.Mvc;
using CreditBureau.Core.Models;
using CreditBureau.Services.Interfaces;

namespace CreditBureau.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BorrowersController : ControllerBase
    {
        private readonly IBorrowerService _borrowerService;

        public BorrowersController(IBorrowerService borrowerService)
        {
            _borrowerService = borrowerService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Borrower>>> GetBorrowers()
        {
            var borrowers = await _borrowerService.GetAllBorrowersAsync();

            return Ok(borrowers);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Borrower>> GetBorrower(int id)
        {
            var borrower = await _borrowerService.GetBorrowerByIdAsync(id);
            
            if (borrower == null)
                return NotFound();
            
            return Ok(borrower);
        }

        [HttpPost]
        public async Task<ActionResult<Borrower>> CreateBorrower(Borrower borrower)
        {
            try
            {
                var createdBorrower = await _borrowerService.CreateBorrowerAsync(borrower);
                return CreatedAtAction(nameof(GetBorrower), new { id = createdBorrower.Id }, createdBorrower);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Borrower>> UpdateBorrower(int id, Borrower borrower)
        {
            if (id != borrower.Id)
                return BadRequest("ID в пути не совпадает с ID в теле запроса");

            var updatedBorrower = await _borrowerService.UpdateBorrowerAsync(id, borrower);
            
            if (updatedBorrower == null)
                return NotFound();
            
            return Ok(updatedBorrower);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteBorrower(int id)
        {
            var result = await _borrowerService.DeleteBorrowerAsync(id);
            
            if (!result)
                return NotFound();
            
            return NoContent();
        }
    }
}