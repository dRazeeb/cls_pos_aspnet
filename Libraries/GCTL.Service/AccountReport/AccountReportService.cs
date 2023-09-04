using GCTL.Core.Data;
using GCTL.Core.ViewModels.AccVouchers;
using GCTL.Core.ViewModels.Common;
using GCTL.Data.Models;
using GCTL.Core.ViewModels.Accounts;
using GCTL.Core.Helpers;
using Dapper;
using GCTL.Data.Extensions;
using Microsoft.Extensions.Configuration;
using GCTL.Core.ViewModels.AccIncomeStatement;
using GCTL.Core.ViewModels.AccBalancesheet;

namespace GCTL.Service.AccountReport
{
    public class AccountReportService : AppService<AccVoucherEntry>, IAccountReportService
    {
        private readonly IRepository<AccVoucherEntry> AccVoucherEntryRepository;
        private readonly IRepository<CoreAccessCode> accessCodeRepository;
        private readonly IConfiguration configuration;

        public AccountReportService(IRepository<AccVoucherEntry> AccVoucherEntryRepository,        
                 IRepository<CoreAccessCode> accessCodeRepository,
                   IConfiguration configuration)
                 : base(AccVoucherEntryRepository)
        {
            this.AccVoucherEntryRepository = AccVoucherEntryRepository;
         
            this.accessCodeRepository = accessCodeRepository;
            this.configuration = configuration;
        }
        public List<PLTotalSales> IncServiceRevenue(IncomeStatementReportRequest request)
        {
            var ProcName = "Rpt_proc_PLTotalSales";
            var queryParameters = new DynamicParameters();
            queryParameters.Add("@FromDate", string.IsNullOrWhiteSpace(request.FromDate) ? "" : request.FromDate.ToDate());
            queryParameters.Add("@ToDate", string.IsNullOrWhiteSpace(request.ToDate) ? "" : request.ToDate.ToFullDate());

            var data = QueryExtensionsHelpers.Query<PLTotalSales>(configuration.GetConnectionString("ApplicationDbConnection"),
               ProcName, queryParameters, System.Data.CommandType.StoredProcedure).ToList();

            return data;
        }

        public List<PLTotalOtherIncome> IncOtherIncome(IncomeStatementReportRequest request)
        {
            var ProcName = "Rpt_proc_PLTotalOtherIncome";
            var queryParameters = new DynamicParameters();
            queryParameters.Add("@FromDate", string.IsNullOrWhiteSpace(request.FromDate) ? "" : request.FromDate.ToDate());
            queryParameters.Add("@ToDate", string.IsNullOrWhiteSpace(request.ToDate) ? "" : request.ToDate.ToFullDate());

            var data = QueryExtensionsHelpers.Query<PLTotalOtherIncome>(configuration.GetConnectionString("ApplicationDbConnection"),
               ProcName, queryParameters, System.Data.CommandType.StoredProcedure).ToList();


            return data;
        }

    
        public List<PLTotalDirectExpense> PLTotalDirectExpense(IncomeStatementReportRequest request)
        {
            var ProcName = "Rpt_proc_PLTotalDirectExpense";
            var queryParameters = new DynamicParameters();
            queryParameters.Add("@FromDate", string.IsNullOrWhiteSpace(request.FromDate) ? "" : request.FromDate.ToDate());
            queryParameters.Add("@ToDate", string.IsNullOrWhiteSpace(request.ToDate) ? "" : request.ToDate.ToFullDate());

            var data = QueryExtensionsHelpers.Query<PLTotalDirectExpense>(configuration.GetConnectionString("ApplicationDbConnection"),
               ProcName, queryParameters, System.Data.CommandType.StoredProcedure).ToList();


            return data;
        }

        public List<PLTotalInDirectExpense> PLTotalInDirectExpense(IncomeStatementReportRequest request)
        {
            var ProcName = "Rpt_proc_PLTotalInDirectExpense";
            var queryParameters = new DynamicParameters();
            queryParameters.Add("@FromDate", string.IsNullOrWhiteSpace(request.FromDate) ? "" : request.FromDate.ToDate());
            queryParameters.Add("@ToDate", string.IsNullOrWhiteSpace(request.ToDate) ? "" : request.ToDate.ToFullDate());

            var data = QueryExtensionsHelpers.Query<PLTotalInDirectExpense>(configuration.GetConnectionString("ApplicationDbConnection"),
               ProcName, queryParameters, System.Data.CommandType.StoredProcedure).ToList();


            return data;
        }

        public List<BSNonCurrentAsset> GetBSFixedAsset(BalancesheetReportRequest request)
        {
            var ProcName = "Rpt_proc_BSNonCurrentAsset";
            var queryParameters = new DynamicParameters();
            queryParameters.Add("@FromDate", string.IsNullOrWhiteSpace(request.FromDate) ? "" : request.FromDate.ToDate());
            queryParameters.Add("@ToDate", string.IsNullOrWhiteSpace(request.ToDate) ? "" : request.ToDate.ToFullDate());

            var data = QueryExtensionsHelpers.Query<BSNonCurrentAsset>(configuration.GetConnectionString("ApplicationDbConnection"),
               ProcName, queryParameters, System.Data.CommandType.StoredProcedure).ToList();


            return data;
        }
        public List<BSTotalCurrentAsset> GetBSCurrentAsset(BalancesheetReportRequest request)
        {
            var ProcName = "Rpt_proc_BSTotalCurrentAsset";
            var queryParameters = new DynamicParameters();
            queryParameters.Add("@FromDate", string.IsNullOrWhiteSpace(request.FromDate) ? "" : request.FromDate.ToDate());
            queryParameters.Add("@ToDate", string.IsNullOrWhiteSpace(request.ToDate) ? "" : request.ToDate.ToFullDate());

            var data = QueryExtensionsHelpers.Query<BSTotalCurrentAsset>(configuration.GetConnectionString("ApplicationDbConnection"),
               ProcName, queryParameters, System.Data.CommandType.StoredProcedure).ToList();


            return data;
        }
        public List<BSTotalLongLiablities> GetBSNonCurrentLibilities(BalancesheetReportRequest request)
        {
            var ProcName = "Rpt_proc_BSTotalLongLiablities";
            var queryParameters = new DynamicParameters();
            queryParameters.Add("@FromDate", string.IsNullOrWhiteSpace(request.FromDate) ? "" : request.FromDate.ToDate());
            queryParameters.Add("@ToDate", string.IsNullOrWhiteSpace(request.ToDate) ? "" : request.ToDate.ToFullDate());

            var data = QueryExtensionsHelpers.Query<BSTotalLongLiablities>(configuration.GetConnectionString("ApplicationDbConnection"),
               ProcName, queryParameters, System.Data.CommandType.StoredProcedure).ToList();


            return data;
        }
        public List<BSTotalCurrentLiablities> GetBSCurrentLibilities(BalancesheetReportRequest request)
        {
            var ProcName = "Rpt_proc_BSTotalCurrentLiablities";
            var queryParameters = new DynamicParameters();
            queryParameters.Add("@FromDate", string.IsNullOrWhiteSpace(request.FromDate) ? "" : request.FromDate.ToDate());
            queryParameters.Add("@ToDate", string.IsNullOrWhiteSpace(request.ToDate) ? "" : request.ToDate.ToFullDate());

            var data = QueryExtensionsHelpers.Query<BSTotalCurrentLiablities>(configuration.GetConnectionString("ApplicationDbConnection"),
               ProcName, queryParameters, System.Data.CommandType.StoredProcedure).ToList();


            return data;
        }
        public List<BSTotalCapital> GetBSCapital(BalancesheetReportRequest request)
        {
            var ProcName = "Rpt_proc_BSTotalCapital";
            var queryParameters = new DynamicParameters();
            queryParameters.Add("@FromDate", string.IsNullOrWhiteSpace(request.FromDate) ? "" : request.FromDate.ToDate());
            queryParameters.Add("@ToDate", string.IsNullOrWhiteSpace(request.ToDate) ? "" : request.ToDate.ToFullDate());

            var data = QueryExtensionsHelpers.Query<BSTotalCapital>(configuration.GetConnectionString("ApplicationDbConnection"),
               ProcName, queryParameters, System.Data.CommandType.StoredProcedure).ToList();


            return data;
        }

    }
}
