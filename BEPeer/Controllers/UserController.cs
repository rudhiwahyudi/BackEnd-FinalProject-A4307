using DAL.DTO.Req;
using DAL.DTO.Res;
using DAL.Repositories.Services;
using DAL.Repositories.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace BEPeer.Controllers
{
    [Route("rest/v1/user/[action]")]
    //[Authorize(Roles = "admin")]
    [ApiController]
    public class UserController : ControllerBase
    {   
        private readonly IUserServices _userServices;
        public UserController(IUserServices userServices) 
        {
            _userServices = userServices;
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> AddUser(ReqRegisterUserDto register)
        {
            var currentUser = HttpContext.User;
            if (!currentUser.IsInRole("admin"))
            {
                return Forbid("You do not have permission to add users.");
            }
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

                var res = await _userServices.Register(register);

                return Ok(new ResBaseDto<string>
                {
                    Success = true,
                    Message = "user registred",
                    Data = res
                });
            }

            catch (Exception ex)
            {
                if (ex.Message == "Emial already used")
                {
                    return BadRequest(new ResBaseDto<object>
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

        [HttpPost]
        public async Task<IActionResult> Register(ReqRegisterUserDto register)
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
                            Message = x.Value.Errors.Select(equals => equals.ErrorMessage).ToList()
                        }).ToList();

                    var errorMessage = new StringBuilder("Validation errors occured!");

                    return BadRequest(new ResBaseDto<object>
                    {
                        Success = false,
                        Message = errorMessage.ToString(),
                        Data = errors
                    });
                }

                var res = await _userServices.Register(register);

                return Ok(new ResBaseDto<String>
                {
                    Success = true,
                    Message = "user registered!",
                    Data = res
                });
            }
            catch (Exception ex)
            {
                if (ex.Message == "Email already used")
                {
                    return BadRequest(new ResBaseDto<object>
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

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var users = await _userServices.GetAllUsers();
                return Ok(new ResBaseDto<List<ResUserDto>>
                {
                    Success = true,
                    Message = "Success gathered All user data!",
                    Data = users
                });
            }
            catch (Exception ex)
            {
                if (ex.Message == "Email or Password is Wrong!")
                {
                    return BadRequest(new ResBaseDto<object>
                    {
                        Success = false,
                        Message = ex.Message,
                        Data = null,
                    });
                }
                return StatusCode(StatusCodes.Status500InternalServerError, new ResBaseDto<string>
                {
                    Success = false,
                    Message = ex.Message,
                    Data = null,
                });
            }
        }


        [HttpGet]
        public async Task<IActionResult> GetUserId([FromQuery] string id)
        {
            try
            {
                var users = await _userServices.GetUserId(id);

                return Ok(new ResBaseDto<ResGetUserDto>
                {
                    Success = true,
                    Message = "List of Users",
                    Data = users
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResBaseDto<ResGetUserDto>
                {
                    Success = false,
                    Message = ex.Message,
                    Data = null
                });

            }
        }


        [HttpPost] 
        public async Task<IActionResult> Login(ReqLoginDto loginDto)
        {
            try
            {
                var response = await _userServices.Login(loginDto);
                return Ok(new ResBaseDto<object>
                {
                    Success = true,
                    Message = "User login Success",
                    Data = response
                });

            }catch (Exception ex)
            {
                if (ex.Message == "Invalid email or Password")
                {
                    return BadRequest(new ResBaseDto<object>
                    {
                        Success = false, 
                        Message= ex.Message,
                        Data  = null
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

        [HttpPut]
        //[Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdatebyAdmin([FromQuery] string id, ReqUpdateAdminDto reqUpdate)
        {
            try
            {
                var res = await _userServices.UpdateUserbyAdmin(reqUpdate, id);

                return Ok(new ResBaseDto<object>
                {
                    Success = true,
                    Message = "User Updated!",
                    Data = res,
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResBaseDto<string>
                {
                    Success = false,
                    Message = ex.Message,
                    Data = null,
                });
            }
        }

        [HttpDelete]
        //[Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete([FromQuery] string id)
        {
            try
            {
                var response = await _userServices.Delete(id);
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
