using DAL.DTO.Req;
using DAL.DTO.Res;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Services.Interfaces
{
    public interface IUserServices
    {
        Task<string> Register(ReqRegisterUserDto register);

        //Task<string> AddUser(AddUser addUser);

        Task<List<ResUserDto>> GetAllUsers();
        Task<ResGetUserDto> GetUserId(string id);

        Task<ResLoginDto> Login(ReqLoginDto reqLogin);
        Task<ResUpdateDto> UpdateUserbyAdmin(ReqUpdateAdminDto reqUpdate, string id);
        Task<string> Delete(string id);
        Task<string> UpdateUserProfile(ReqUpdateUserProfileDto updateUserProfileDto, string email, string id, string role);
    }
}

