using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTO.Req
{
    public class ReqRegisterUserDto
    {
        [Required(ErrorMessage = "name is required")]
        [MaxLength(30, ErrorMessage = "name cannot exceed 30 character")]

        public string Name { get; set; }

        [Required(ErrorMessage = "email is required")]
        [MaxLength(50, ErrorMessage = "email cannot exceed 50 character")]

        public string Email { get; set; }

        [Required(ErrorMessage = "password is required")]
        [MinLength(8, ErrorMessage = "password must be at lest 8 characters long")]
        [MaxLength(50, ErrorMessage = "password cannot exceed 50 character")]

        public string Password { get; set; }

        [Required(ErrorMessage = "role is required")]
        [MaxLength(30, ErrorMessage = "role cannot exced 30 character")]

        public string Role { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "balance must be a positif")]

        public decimal? Belance { get; set; }
    }
}
