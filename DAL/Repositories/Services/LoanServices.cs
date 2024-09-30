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
    public class LoanServices : ILoansServices
    {
        private readonly PeerlandingContext _peerlandingContext;
        public LoanServices(PeerlandingContext peerlandingContext)
        {
            _peerlandingContext = peerlandingContext;
        }
        public async Task<string> CreateLoan(ReqLoanDto loan)
        {
            var newLoan = new MstLoans
            {
                BorrowerId = loan.BorrowerId,
                Amount = loan.Amount,
                InterestRate = loan.InterestRate,
                Duration = loan.Duration,
            };
            
            await _peerlandingContext.AddAsync(newLoan);
            await _peerlandingContext.SaveChangesAsync();

            return newLoan.BorrowerId;
        }

        public async Task<string> Delete(string id)
        {
            var user = _peerlandingContext.MstLoans.SingleOrDefault(e => e.Id == id);

            if (user == null)
            {
                throw new Exception("User not found");
            }


            _peerlandingContext.MstLoans.Remove(user);
            _peerlandingContext.SaveChanges();


            return id;
        }

        public async Task<List<ResListLoanDto>> LoanList()
        {
            var loans = await _peerlandingContext.MstLoans
                .Include(l => l.User)
                .Select(loan => new ResListLoanDto
                {
                    LoanId = loan.Id,
                    BorrowerName = loan.User.Name,
                    Amount = loan.Amount, 
                    InterestRate = loan.InterestRate,
                    Duration = loan.Duration,
                    Status = loan.Status,
                    CreatedAt = loan.CreatedAt,
                    UpdatedAt = loan.UpdatedAt,

                }).ToListAsync();
            return loans;

        }

        public async Task<List<ResListLoanDto>> LoanList(string status)
        {
            var loans = await _peerlandingContext.MstLoans
                .Include(l => l.User)
                .Select(loan => new ResListLoanDto
                {
                    LoanId = loan.Id,
                    BorrowerName = loan.User.Name,
                    Amount = loan.Amount,
                    InterestRate = loan.InterestRate,
                    Duration = loan.Duration,
                    Status = loan.Status,
                    CreatedAt = loan.CreatedAt,
                    UpdatedAt = loan.UpdatedAt,

                }).OrderBy(x => x.CreatedAt)
                .Where(x => string.IsNullOrEmpty(status) || x.Status == status)
                .ToListAsync();

            return loans;
            
        }

        public async Task<ResUpdateDto> UpdateLoan(ReqLoanUpdateDto loan, string id)
        {
            var loans = _peerlandingContext.MstLoans.SingleOrDefault(x => x.Id == id);
            if (loans == null)
            {
                throw new Exception("Loans Id not found!");
            }

            loans.Status = loan.Status;
            loans.CreatedAt = DateTime.UtcNow;
            
            var newUser = _peerlandingContext.MstLoans.Update(loans).Entity;
            _peerlandingContext.SaveChanges();

            var updateRes = new ResUpdateDto
            {
                nama = newUser.Id,
            };

            return updateRes;

        }
    }
}
