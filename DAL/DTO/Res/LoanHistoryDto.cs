using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTO.Res
{
	public class LoanHistoryDto
	{
		public int Id { get; set; }
		public int LoanId { get; set; } // ID pinjaman yang terkait
		public int LenderId { get; set; } // ID lender yang terkait
		public string Status { get; set; } // Status pinjaman (requested, funded, repaid)
		public DateTime UpdatedAt { get; set; } = DateTime.UtcNow; // Tanggal perubahan status
	}
}
	