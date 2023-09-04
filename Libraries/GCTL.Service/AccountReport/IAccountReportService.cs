using GCTL.Core.ViewModels.AccIncomeStatement;
using GCTL.Core.ViewModels.AccBalancesheet;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCTL.Service.AccountReport
{
    public interface IAccountReportService
    {
        List<PLTotalSales> IncServiceRevenue(IncomeStatementReportRequest request);
        List<PLTotalOtherIncome> IncOtherIncome(IncomeStatementReportRequest request);
        List<PLTotalDirectExpense> PLTotalDirectExpense(IncomeStatementReportRequest request);
        List<PLTotalInDirectExpense> PLTotalInDirectExpense(IncomeStatementReportRequest request);

        List<BSNonCurrentAsset> GetBSFixedAsset(BalancesheetReportRequest request);
        List<BSTotalCurrentAsset> GetBSCurrentAsset(BalancesheetReportRequest request);
        List<BSTotalLongLiablities> GetBSNonCurrentLibilities(BalancesheetReportRequest request);
        List<BSTotalCurrentLiablities> GetBSCurrentLibilities(BalancesheetReportRequest request);
        List<BSTotalCapital> GetBSCapital(BalancesheetReportRequest request);
    }
}
