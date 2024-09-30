using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTO.Req
{
    public class ReqLoanUpdateDto
    {
        //[Required(ErrorMessage = "BorrowerId is Required")]
        //public string Id { get; set; }


        [Required(ErrorMessage = "Status is Required")]
        public string Status { get; set; }
    }
}
