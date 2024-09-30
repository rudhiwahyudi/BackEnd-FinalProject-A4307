using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTO.Res
{
	public class ResLenderDto
	{
		public int Id { get; set; }
		public string Name { get; set; } // Nama lender
		public decimal Balance { get; set; } // Saldo lender
	}
}
