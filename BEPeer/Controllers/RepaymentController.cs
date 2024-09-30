using DAL.DTO.Req;
using DAL.Repositories.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BEPeer.Controllers
{
    [Route("api/v1/repayment/[action]")]
    [ApiController]
    public class RepaymentController : ControllerBase
    {
        private readonly IRepaymentServices _repaymentServices;

        public RepaymentController(IRepaymentServices repaymentServices)
        {
            _repaymentServices = repaymentServices;
        }


        // Action untuk membuat repayment baru
        [HttpPost]
        public async Task<IActionResult> CreateRepayment([FromBody] ReqAddRepaymentDto reqRepaymentDto)
        {
            try
            {
                var loanId = await _repaymentServices.CreateRepayment(reqRepaymentDto);
                return Ok(new
                {
                    Success = true,
                    Message = "Repayment created successfully",
                    LoanId = loanId
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Success = false,
                    Message = ex.Message
                });
            }
        }

        // Action untuk mendapatkan daftar repayment berdasarkan borrower_id
        [HttpGet]
        public async Task<IActionResult> GetRepaymentsByBorrowerId([FromQuery] string? borrower_id = null)
        {
            try
            {
                var repayments = await _repaymentServices.RepaymentListByBorrowerId(borrower_id);
                return Ok(new
                {
                    Success = true,
                    Message = "Repayments retrieved successfully",
                    Data = repayments
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Success = false,
                    Message = ex.Message
                });
            }
        }

        // Action untuk memperbarui repayment
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRepayment(string id, [FromBody] ReqUpdateRepaymentDto reqRepaymentDto)
        {
            try
            {
                var result = await _repaymentServices.UpdateRepayment(reqRepaymentDto, id);
                return Ok(new
                {
                    Success = true,
                    Message = result
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Success = false,
                    Message = ex.Message
                });
            }
        }
    }
}
