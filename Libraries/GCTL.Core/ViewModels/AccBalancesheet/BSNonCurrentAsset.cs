using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCTL.Core.ViewModels.AccBalancesheet
{
    public class BSNonCurrentAsset
    {
        public string SubControlLedgerName { get; set; }
        public string GeneralLedgerName { get; set; }
        public decimal Amount { get; set; }
        public decimal TFS { get; set; }
    }
}
