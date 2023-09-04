using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCTL.Core.ViewModels.AccIncomeStatement
{
    public class PLTotalDirectExpense
    {
        public string SubControlLedgerName { get; set; }
        public string GeneralLedgerName { get; set; }
        public decimal Amount { get; set; }
        public decimal TDE { get; set; }
    }
}
