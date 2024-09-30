using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTO.Req
{
    public class ReqAddRepaymentDto
    {
        public string loan_id { get; set; }
        public decimal interest_rate { get; set; }
        public decimal amount { get; set; }
        public decimal duration { get; set; }
    }
}
