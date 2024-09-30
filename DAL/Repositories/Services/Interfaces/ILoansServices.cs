using DAL.DTO.Req;
using DAL.DTO.Res;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Services.Interfaces
{
    public interface ILoansServices
    {
        Task<string> CreateLoan(ReqLoanDto loans);

        Task<ResUpdateDto> UpdateLoan(ReqLoanUpdateDto loans, string id);

        Task<List<ResListLoanDto>> LoanList(string status);
        Task<string> Delete(string id);


    }
}
