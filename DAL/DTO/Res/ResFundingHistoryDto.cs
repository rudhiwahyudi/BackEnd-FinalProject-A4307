using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTO.Res
{
	public class ResFundingHistoryDto
	{
		public string LoanId { get; set; }
		public string LenderId { get; set; }
		public decimal Amount { get; set; }
		public DateTime FundedAt { get; set; }
		public string LoanStatus { get; set; }
	}
}
