using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTO.Res
{
    public class ResListFundingDto
    {
        public string Id { get; set; }
        public string LoanId { get; set; }
        public string LenderId { get; set; }
        public string BorrowerName { get; set; }
        public decimal LoanAmount { get; set; }
        public decimal InterestRate { get; set; }
        public int Duration { get; set; }
        public string Status { get; set; }
        public DateTime FundedAt { get; set; }
    }
}
