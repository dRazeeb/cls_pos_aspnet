using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCTL.Core.ViewModels.BankAccounts
{
    public class BankAccountModel
    {
        public decimal AutoId { get; set; }
        public string AccInfoId { get; set; }
        public string AccountName { get; set; }
        public string AccountNo { get; set; }
        public string BankName { get; set; }
        public string BranchName { get; set; }
    }
}
