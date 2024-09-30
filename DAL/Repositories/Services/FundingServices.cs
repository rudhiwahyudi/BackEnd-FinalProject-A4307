using DAL.DTO.Req;
using DAL.DTO.Res;
using DAL.Models;
using DAL.Repositories.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Services
{
    public class FundingServices : IFundingServices
    {
        private readonly PeerlandingContext _peerlandingContext;
        public FundingServices(PeerlandingContext peerlandingContext)
        {
            _peerlandingContext = peerlandingContext;
        }
        public async Task<string> CreateFunding(ReqFundingDto funding)
        {
            var newFunding = new TrnFunding
            {
                LoanId = funding.loan_id,
                LenderId = funding.lender_id,
                Amount = funding.amount,
            };

            await _peerlandingContext.AddAsync(newFunding);
            await _peerlandingContext.SaveChangesAsync();

            return newFunding.LoanId;
        }

        public async Task<List<ResListFundingDto>> FundingList(string? lender_id = null)
        {
            var fundingsQuery = _peerlandingContext.TrnFundings
                .Include(f => f.Loans)
                .ThenInclude(l => l.User) // Join ke MstLoans dan MstUser untuk mengambil nama borrower
                .Where(funding => lender_id == null || funding.LenderId == lender_id)
                .Select(funding => new ResListFundingDto
                {
                    Id = funding.Id,
                    LoanId = funding.LoanId,
                    LenderId = funding.LenderId,
                    BorrowerName = funding.Loans.User.Name, // Nama peminjam dari MstUser
                    LoanAmount = funding.Loans.Amount, // Jumlah pinjaman dari MstLoans
                    InterestRate = funding.Loans.InterestRate, // Bunga pinjaman dari MstLoans
                    Duration = funding.Loans.Duration, // Durasi pinjaman dari MstLoans
                    Status = funding.Loans.Status, // Status pinjaman dari MstLoans
                    FundedAt = funding.FundedAt // Tanggal pendanaan dari TrnFunding
                })
                .OrderByDescending(funding => funding.FundedAt);

            return await fundingsQuery.ToListAsync();
        }
    }
}
