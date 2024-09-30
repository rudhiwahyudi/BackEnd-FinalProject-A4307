using DAL.DTO.Req;
using DAL.DTO.Res;
using DAL.Repositories.Services;
using DAL.Repositories.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Win32;
using System.Text;

namespace BEPeer.Controllers
{
    [Route("rest/V1/Loan/[action]")]
    [ApiController]
    public class LoanController : ControllerBase
    {
        public readonly ILoansServices _loansServices;

        public LoanController(ILoansServices loansServices)
        {
            _loansServices = loansServices;
        }

        [HttpPost]
        public async Task<IActionResult> NewLoan(ReqLoanDto loan)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState
                        .Where(x => x.Value.Errors.Any())
                        .Select(x => new
                        {
                            Field = x.Key,
                            Messages = x.Value.Errors.Select(e => e.ErrorMessage).ToList()
                        }).ToList();

                    var errorMesage = new StringBuilder("Validation eror occurde!");

                    return BadRequest(new ResBaseDto<object>
                    {
                        Success = false,
                        Message = errorMesage.ToString(),
                        Data = errors
                    });
                }

                var res = await _loansServices.CreateLoan(loan);

                return Ok(new ResBaseDto<string>
                {
                    Success = true,
                    Message = "Success and Loan data",
                    Data = res
                });

            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, new ResBaseDto<string>
                {
                    Success = false,
                    Message = ex.Message,
                    Data = null
                });

            }
        }

        [HttpPut]

        public async Task<IActionResult> UpdateLoan([FromQuery] string id, ReqLoanUpdateDto loan)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState
                        .Where(x => x.Value.Errors.Any())
                        .Select(x => new
                        {
                            Field = x.Key,
                            Messages = x.Value.Errors.Select(e => e.ErrorMessage).ToList()
                        }).ToList();

                    var errorMesage = new StringBuilder("Validation eror occurde!");

                    return BadRequest(new ResBaseDto<object>
                    {
                        Success = false,
                        Message = errorMesage.ToString(),
                        Data = errors
                    });
                }

                var res = await _loansServices.UpdateLoan(loan, id);

                return Ok(new ResBaseDto<string>
                {
                    Success = true,
                    Message = "Success and Loan data",
                    Data = "Success"
                });

            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, new ResBaseDto<string>
                {
                    Success = false,
                    Message = ex.Message,
                    Data = null
                });

            }
        }

        [HttpGet]

        public async Task<IActionResult> LoanList([FromQuery] string status)
        {
            try
            {
                var res = await _loansServices.LoanList(status);

                return Ok(new ResBaseDto<object>
                {
                    Success = true,
                    Message = "Success and Loan data",
                    Data = res
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResBaseDto<string>
                {
                    Success = false,
                    Message = ex.Message,
                    Data = null
                });
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] string id)
        {
            try
            {
                var response = await _loansServices.Delete(id);
                return Ok(new ResBaseDto<object>
                {
                    Success = true,
                    Message = "User berhasil di delete",
                    Data = response
                });
            }
            catch (Exception ex)
            {
                if (ex.Message == "User not found")
                {
                    return BadRequest(new ResBaseDto<string>
                    {
                        Success = false,
                        Message = ex.Message,
                        Data = null
                    });
                }

                return StatusCode(StatusCodes.Status500InternalServerError, new ResBaseDto<string>
                {
                    Success = false,
                    Message = ex.Message,
                    Data = null
                });

            }

        }
    }
}
