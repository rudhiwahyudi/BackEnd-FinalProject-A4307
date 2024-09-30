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
    public class RepaymentServices : IRepaymentServices
    {
        private readonly PeerlandingContext _peerlandingContext;
        public RepaymentServices(PeerlandingContext peerlandingContext)
        {
            _peerlandingContext = peerlandingContext;
        }

        public async Task<string> CreateRepayment(ReqAddRepaymentDto reqRepaymentDto)
        {
            // Convert decimals to doubles for power calculation
            double interestRateDouble = (double)reqRepaymentDto.interest_rate;
            double amountDouble = (double)reqRepaymentDto.amount;
            double durationDouble = (int)reqRepaymentDto.duration;

            // Calculate installment (angsuran) using double values
            double angsuranDouble = (interestRateDouble / 100 * amountDouble) / (1 - (1 / Math.Pow(1 + interestRateDouble / 100, durationDouble)));
            // Convert result back to decimal
            decimal angsuran = Math.Round((decimal)angsuranDouble, 2);
            decimal totalBayar = Math.Round(angsuran * (decimal)durationDouble, 2);

            var newRepayment = new TrnRepayment
            {
                LoanId = reqRepaymentDto.loan_id,
                Amount = totalBayar,
                RepaidAmount = 0,
                BalanceAmount = totalBayar,
                RepaidStatus = ""
            };

            await _peerlandingContext.AddAsync(newRepayment);
            await _peerlandingContext.SaveChangesAsync();

            return newRepayment.LoanId;
        }


        public async Task<List<ResListRepaymentDto>> RepaymentListByBorrowerId(string? borrower_id = null)
        {
            var repaymentsQuery = _peerlandingContext.TrnRepayments
                .Include(repayment => repayment.Loans) // Join ke MstLoans
                .Where(repayment => borrower_id == null || repayment.Loans.BorrowerId == borrower_id) // Gunakan 'repayment'
                .Select(repayment => new ResListRepaymentDto
                {
                    Id = repayment.Id,
                    LoanId = repayment.LoanId,
                    Amount = repayment.Amount,
                    RepaidAmount = repayment.RepaidAmount,
                    BalanceAmount = repayment.BalanceAmount,
                    InterestRate = repayment.Loans.InterestRate,
                    Duration = repayment.Loans.Duration,
                    RepaidStatus = repayment.RepaidStatus,
                    PaidAt = repayment.PaidAt
                })
                .OrderByDescending(repayment => repayment.PaidAt);

            return await repaymentsQuery.ToListAsync();
        }


        public async Task<string> UpdateRepayment(ReqUpdateRepaymentDto reqRepaymentDto, string id)
        {

            try
            {
                // Cari user berdasarkan email atau Id dari token JWT
                var repayment = await _peerlandingContext.TrnRepayments.SingleOrDefaultAsync(repayment => repayment.Id == id);
                if (repayment == null)
                {
                    throw new Exception("Repayment not found");
                }

                repayment.RepaidAmount = repayment.RepaidAmount + reqRepaymentDto.pay;
                repayment.BalanceAmount = repayment.BalanceAmount - reqRepaymentDto.pay;
                if (repayment.BalanceAmount < repayment.Amount / 12)
                {
                    repayment.RepaidStatus = "done";
                }
                repayment.PaidAt = DateTime.UtcNow;

                await _peerlandingContext.SaveChangesAsync();

                return $"Repayment with id={repayment.Id} has been updated successfully.";
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating the repayment: " + ex.Message);
            }
        }
    }
}
