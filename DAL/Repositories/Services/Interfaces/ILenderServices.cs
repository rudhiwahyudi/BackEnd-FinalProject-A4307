using DAL.DTO.Req;
using DAL.DTO.Res;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Services.Interfaces
{
	public interface ILenderServices
	{
		Task<decimal> GetBalanceAsync (string lenderId);
		Task<ResUpdateBalanceDto> UpdateGetBalanceAsync(ReqUpdateBalanceRequestDto reqUpdateBalanceRequestDto, string lenderId);
		Task<List<ResListLoanDto>> GetLoansAsync(string status);
		Task UpdateLoanStatusAsync(string loanId, ReqLoanUpdateDto reqLoanUpdateDto, string lenderId);
		Task<List<ResFundingHistoryDto>> GetLoanHistoryByLenderIdAsync(string lenderId);
	}
}
