using DAL.DTO.Req;
using DAL.DTO.Res;
using DAL.Repositories.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BEPeer.Controllers
{
	[ApiController]
	[Route("rest/V1/Lander/[action]")]
	public class LenderController : Controller
	{
		private readonly ILenderServices _lenderService;

		public LenderController(ILenderServices lenderService)
		{
			_lenderService = lenderService;
		}

		/// <summary>
		/// Get the balance of a lender by their ID.
		/// </summary>
		/// <param name="lenderId">The ID of the lender.</param>
		/// <returns>The balance of the lender.</returns>
		[HttpGet("{lenderId}/balance")]
		public async Task<ActionResult<decimal>> GetBalance(string lenderId)
		{
			var balance = await _lenderService.GetBalanceAsync(lenderId);
			return Ok(balance);
		}

		/// <summary>
		/// Update the balance of a lender.
		/// </summary>
		/// <param name="lenderId">The ID of the lender.</param>
		/// <param name="amount">The amount to change the balance by.</param>
		/// <returns>A response indicating success or failure.</returns>
		[HttpPost("{lenderId}/balance/update")]
		public async Task<IActionResult> UpdateBalance(string lenderId, [FromBody] ReqUpdateBalanceRequestDto reqUpdateBalanceRequest)
		{
			if (reqUpdateBalanceRequest == null || reqUpdateBalanceRequest.balance <= 0)
			{
				return BadRequest("Invalid request data.");
			}

			try
			{
				var result = await _lenderService.UpdateGetBalanceAsync(reqUpdateBalanceRequest, lenderId);
				return Ok(result); // Mengembalikan status 200 OK dengan hasil
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"Internal server error: {ex.Message}"); // Mengembalikan status 500 jika terjadi error
			}
		}

		/// <summary>
		/// Get a list of loans based on their status.
		/// </summary>
		/// <param name="status">The status of loans to retrieve.</param>
		/// <returns>A list of loans with borrower details.</returns>
		//[HttpGet("loans")]
		//public async Task<ActionResult<List<ResListLoanDto>>> GetLoans(string status)
		//{
		//	try
		//	{
		//		var loans = await _lenderService.GetLoansAsync(status);
		//		return Ok(loans);
		//	}
		//	catch (Exception ex)
		//	{

		//	}

		//}

		/// <summary>
		/// Get loan history for a specific lender.
		/// </summary>
		/// <param name="lenderId">The ID of the lender.</param>
		/// <returns>A list of loans that the lender has funded.</returns>

		[HttpGet("{lenderId}/history")]
		public async Task<IActionResult> GetLoanHistory(string lenderId)
		{
			if (string.IsNullOrWhiteSpace(lenderId))
			{
				return BadRequest("Lender ID cannot be empty.");
			}

			try
			{
				var history = await _lenderService.GetLoanHistoryByLenderIdAsync(lenderId);
				if (history == null || history.Count == 0)
				{
					return NotFound("No funding history found for this lender.");
				}

				return Ok(history); // Status 200 OK dengan hasil
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"Internal server error: {ex.Message}"); // Status 500 jika terjadi error
			}
		}

		// PUT: api/lender/{loanId}/update-status
		[HttpPut("{loanId}/update-status")]
		public async Task<IActionResult> UpdateLoanStatus(string loanId, [FromBody] ReqLoanUpdateDto reqLoanUpdateDto, [FromHeader] string lenderId)
		{
			if (string.IsNullOrWhiteSpace(loanId) || string.IsNullOrWhiteSpace(lenderId))
			{
				return BadRequest("Loan ID and Lender ID cannot be empty.");
			}

			if (reqLoanUpdateDto == null || string.IsNullOrWhiteSpace(reqLoanUpdateDto.Status))
			{
				return BadRequest("Invalid request data.");
			}

			try
			{
				await _lenderService.UpdateLoanStatusAsync(loanId, reqLoanUpdateDto, lenderId);
				return NoContent(); // Status 204 No Content
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"Internal server error: {ex.Message}"); // Status 500 jika terjadi error
			}
		}
	}
}
