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
	public class LenderService : ILenderServices
	{
		private readonly PeerlandingContext _context;

		public LenderService(PeerlandingContext context)
		{
			_context = context;
		}

		public async Task<decimal> GetBalanceAsync(string lenderId)
		{
			var lender = await _context.MstUsers.FindAsync(lenderId);
			return lender?.Balance ?? 0;
		}

	
		public async Task<List<ResListLoanDto>> GetLoansAsync(string status)
		{
			// Mengambil daftar pinjaman berdasarkan status
			return await _context.TrnFundings
				.Include(funding => funding.Loans) // Sertakan informasi loan
				.Include(funding => funding.User) // Sertakan informasi lender
				.Where(funding => funding.Loans.Status == status)
				.Select(funding => new ResListLoanDto
				{
					LoanId = funding.Loans.Id,
					BorrowerName = _context.MstUsers
						.Where(user => user.Id == funding.Loans.BorrowerId) // Ambil BorrowerName berdasarkan BorrowerId
						.Select(user => user.Name) // Ganti 'Name' dengan nama properti yang sesuai
						.FirstOrDefault(),
					Amount = funding.Loans.Amount,
					InterestRate = funding.Loans.InterestRate,
					Duration = funding.Loans.Duration,
					Status = funding.Loans.Status,
					CreatedAt = funding.Loans.CreatedAt,
					UpdatedAt = funding.Loans.UpdatedAt
				})
				.ToListAsync();
		}

		public async Task<ResUpdateBalanceDto> UpdateGetBalanceAsync(ReqUpdateBalanceRequestDto reqUpdateBalanceRequest, string lenderId)
		{
			try
			{
				// Cari user berdasarkan email atau Id dari token JWT
				var user = await _context.MstUsers.SingleOrDefaultAsync(e => e.Id == lenderId);
				if (user == null)
				{
					throw new Exception("User not found");
				}

				user.Balance += reqUpdateBalanceRequest.balance;

				_context.MstUsers.Update(user);
				await _context.SaveChangesAsync();

				return new ResUpdateBalanceDto
				{
					message = "Update balance successfully"
				};
			}
			catch (Exception ex)
			{
				throw new Exception("An error occurred while updating the user profile: " + ex.Message);
			}
		}

		public async Task<List<ResFundingHistoryDto>> GetLoanHistoryByLenderIdAsync(string lenderId)
		{
			// Mengambil semua funding yang terkait dengan lender tertentu
			var fundingHistories = await _context.TrnFundings
				.Include(f => f.Loans) // Sertakan informasi Loans
				.Where(funding => funding.LenderId == lenderId)
				.Select(funding => new ResFundingHistoryDto
				{
					LoanId = funding.LoanId,
					LenderId = funding.LenderId,
					Amount = funding.Amount,
					FundedAt = funding.FundedAt,
					LoanStatus = funding.Loans.Status // Menyertakan status loan
				})
				.OrderByDescending(funding => funding.FundedAt) // Urutkan berdasarkan waktu pendanaan
				.ToListAsync();

			// Mengembalikan hasil sebagai list DTO
			return fundingHistories;
		}

		public async Task UpdateLoanStatusAsync(string loanId, ReqLoanUpdateDto reqLoanUpdateDto, string lenderId)
		{
			// Cari funding berdasarkan LoanId dan LenderId
			var funding = await _context.TrnFundings
				.Include(f => f.Loans) // Sertakan informasi Loans
				.FirstOrDefaultAsync(f => f.LoanId == loanId && f.LenderId == lenderId);

			if (funding == null)
			{
				throw new Exception("Funding not found for the specified loan and lender.");
			}

			// Catat status sebelumnya
			var previousStatus = funding.Loans.Status;

			// Update status loan jika dari "requested" ke "funded"
			if (previousStatus.Equals("requested", StringComparison.InvariantCultureIgnoreCase) &&
				reqLoanUpdateDto.Status.Equals("funded", StringComparison.InvariantCultureIgnoreCase))
			{
				funding.Loans.Status = reqLoanUpdateDto.Status;
				funding.Loans.UpdatedAt = DateTime.UtcNow;

				// Simpan perubahan ke database
				await _context.SaveChangesAsync();

				// Mencatat transaksi funding
				var newFunding = new TrnFunding
				{
					LoanId = funding.Loans.Id,
					LenderId = lenderId,
					Amount = funding.Amount, // Ambil dari funding yang sama
					FundedAt = DateTime.UtcNow
				};

				_context.TrnFundings.Add(newFunding);
				await _context.SaveChangesAsync();
			}
			else
			{
				throw new Exception("Status can only be changed from 'requested' to 'funded'.");
			}
		}
	}
}
