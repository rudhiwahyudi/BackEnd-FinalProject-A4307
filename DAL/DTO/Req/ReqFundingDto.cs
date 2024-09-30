using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTO.Req
{
    public class ReqFundingDto
    {
        public string loan_id { get; set; }
        public string lender_id { get; set; }
        public decimal amount { get; set; }

    }
}
