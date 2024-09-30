using DAL.DTO.Req;
using DAL.DTO.Res;
using DAL.Repositories.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace BEPeer.Controllers
{
    [Route("api/v1/funding/[action]")]
    [ApiController]
    public class FundingController : ControllerBase
    {
        private readonly IFundingServices _fundingServices;

        public FundingController(IFundingServices fundingServices)
        {
            _fundingServices = fundingServices;
        }
        [HttpPost]
        public async Task<IActionResult> CreateFunding(ReqFundingDto fundingDto)
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
                    var errorMessage = new StringBuilder("Validation error occured!");

                    return BadRequest(new ResBaseDto<object>
                    {
                        Success = false,
                        Message = errorMessage.ToString(),
                        Data = errors
                    });
                }
                var res = await _fundingServices.CreateFunding(fundingDto);
                return Ok(new ResBaseDto<string>
                {
                    Success = true,
                    Message = "Success add funding data",
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

        [HttpGet]
        public async Task<IActionResult> FundingList([FromQuery] string? lender_id = null)
        {
            try
            {
                var res = await _fundingServices.FundingList(lender_id);
                return Ok(new ResBaseDto<object>
                {
                    Success = true,
                    Message = "Success getting funding list",
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
    }
}
