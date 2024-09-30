using DAL.DTO.Req;
using DAL.DTO.Res;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Services.Interfaces
{
    public interface IRepaymentServices
    {
        Task<string> CreateRepayment(ReqAddRepaymentDto reqRepaymentDto);
        Task<string> UpdateRepayment(ReqUpdateRepaymentDto reqRepaymentDto, string id);
        Task<List<ResListRepaymentDto>> RepaymentListByBorrowerId(string borrower_id);
    }
}
