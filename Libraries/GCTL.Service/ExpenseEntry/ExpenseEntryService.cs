using GCTL.Core.Data;
using GCTL.Core.ViewModels.ExpenseEntry;
using GCTL.Core.ViewModels.Common;
using GCTL.Data.Extensions;
using GCTL.Data.Models;
using GCTL.Core.Configurations;
using Microsoft.Extensions.Configuration;
using Dapper;
using GCTL.Core.Helpers;
using GCTL.Core.ViewModels.Accounts;

namespace GCTL.Service.ExpenseEntry
{
    public class ExpenseEntryService : AppService<HmsExpenseEntry>, IExpenseEntryService
    {
        private readonly IRepository<HmsExpenseEntry> ExpenseEntryRepository;
        private readonly IRepository<AccSubSubsidiaryLedger> AccSubSubsidiaryLedgerRepository;
      
        private readonly IConfiguration configuration;
        private readonly IRepository<CoreAccessCode> accessCodeRepository;
        public ExpenseEntryService(IRepository<HmsExpenseEntry> ExpenseEntryRepository,
            IRepository<AccSubSubsidiaryLedger> AccSubSubsidiaryLedgerRepository,
           
                      IConfiguration configuration,
    IRepository<CoreAccessCode> accessCodeRepository)
                : base(ExpenseEntryRepository)
        {
            this.ExpenseEntryRepository = ExpenseEntryRepository;
            this.AccSubSubsidiaryLedgerRepository = AccSubSubsidiaryLedgerRepository;
          
            this.configuration = configuration;
            this.accessCodeRepository = accessCodeRepository;
        }
        public bool DeletePermission(string accessCode)
        {
            return accessCodeRepository.All().Any(x => x.AccessCodeId == accessCode && x.Title == "Expense Entry" && x.CheckDelete);
        }

        public ExpenseEntrySetupViewModel GetInfo(string code)
        {
            var data = (from gl in ExpenseEntryRepository.All()
                        where (gl.ExpenseCode == code)
                        select new ExpenseEntrySetupViewModel
                        {
                            ExpenseCode = gl.ExpenseCode,
                            ExpenseDate = gl.ExpenseDate.ToDateFormat(),
                            SubSusidiaryLedgerCodeNo = gl.SubSusidiaryLedgerCodeNo,
                            Amount = gl.Amount,
                            PaymentMode = gl.PaymentMode,
                            ChequeNo=gl.ChequeNo,
                            ChequeDate=gl.ChequeDate==null ? "": Convert.ToDateTime(gl.ChequeDate).ToDateFormat(),
                            BankAccountNo=gl.BankAccountNo,
                            TransferBankFrom=gl.TransferBankFrom,
                            TransferBankTo=gl.TransferBankTo,
                            BkashNo=gl.BkashNo,
                            Remarks=gl.Remarks
                        }).FirstOrDefault();

           // var result = ExpenseEntryRepository.All().Where(x => x.ExpenseCode == code).FirstOrDefault();
            return data;
        }

        public List<ExpenseEntrySetupViewModel> GetInfoList(string FromDate, string ToDate)
        {
            string NewFromDate = FromDate + " 00:00:00";
            string NewToDate = ToDate + " 23:59:59";

            DateTime startDate = DateTime.MinValue;
            DateTime endDate = DateTime.MinValue;



            if (!string.IsNullOrWhiteSpace(FromDate))
                startDate = DateTime.ParseExact(NewFromDate, "dd/MM/yyyy HH:mm:ss", null);
            if (!string.IsNullOrWhiteSpace(ToDate))
                endDate = DateTime.ParseExact(NewToDate, "dd/MM/yyyy HH:mm:ss", null);

            var data = (from ei in ExpenseEntryRepository.All()
                        join eh in AccSubSubsidiaryLedgerRepository.All()
                        on ei.SubSusidiaryLedgerCodeNo equals eh.SubSusidiaryLedgerCodeNo
                       
                        where
        (startDate == DateTime.MinValue || ei.ExpenseDate >= startDate)
       && (endDate == DateTime.MinValue || ei.ExpenseDate <= endDate)
                        select new ExpenseEntrySetupViewModel
                        {
                            ExpenseCode = ei.ExpenseCode,
                            ExpenseDate = ei.ExpenseDate.ToDateFormat(),
                            SubSusidiaryLedgerCodeNo = eh.SubSubsidiaryLedgerName,
                            Amount = ei.Amount,
                            
                        }).ToList();


            return data;

        }

        public bool IsExistByCode(string code)
        {
            return ExpenseEntryRepository.All().Any(x => x.ExpenseCode == code);
        }

        public bool ExcuteInVoucherEntryPV(string ExpenseCode)
        {
            var queryParameters = new DynamicParameters();
            queryParameters.Add("@ExpenseCode", ExpenseCode);
            var data = QueryExtensionsHelpers.Query<DBNull>(configuration.GetConnectionString("ApplicationDbConnection"),
                "Proc_VoucherEntryFromExpensePaymentPV", queryParameters, System.Data.CommandType.StoredProcedure);
            return true;
        }
        public bool Update(ExpenseEntrySetupViewModel entity, UserInfoViewModel loginInfo)
        {
            var result = ExpenseEntryRepository.All().Where(x => x.ExpenseCode == entity.ExpenseCode).FirstOrDefault();
            if (result != null)
            {
                result.ExpenseDate = entity.ExpenseDate.ToDate();
                result.SubSusidiaryLedgerCodeNo = entity.SubSusidiaryLedgerCodeNo;
                result.Amount = Convert.ToDecimal(entity.Amount);
                result.PaymentMode = entity.PaymentMode;
                result.ChequeNo = entity.ChequeNo;
                if (!string.IsNullOrEmpty(entity.ChequeDate))
                {
                    result.ChequeDate = entity.ChequeDate.ToDate();
                }
                result.BankAccountNo = entity.BankAccountNo;
                result.TransferBankFrom = entity.TransferBankFrom;
                result.TransferBankTo = entity.TransferBankTo;
                result.BkashNo = entity.BkashNo;
                result.Remarks = entity.Remarks;
                result.ModifyDate = DateTime.Now;
                result.Luser = loginInfo.EmployeeId;
                ExpenseEntryRepository.Update(result);
            }
           

            return true;
        }
        public bool Save(ExpenseEntrySetupViewModel entity, UserInfoViewModel loginInfo)
        {
            
                HmsExpenseEntry ee = new HmsExpenseEntry();
                ee.ExpenseCode = entity.ExpenseCode;
                ee.ExpenseDate=entity.ExpenseDate.ToDate();
                ee.SubSusidiaryLedgerCodeNo = entity.SubSusidiaryLedgerCodeNo;
                ee.Amount = Convert.ToDecimal(entity.Amount);
                ee.PaymentMode = entity.PaymentMode;
                ee.ChequeNo = entity.ChequeNo;
                if (!string.IsNullOrEmpty(entity.ChequeDate))
                {
                    ee.ChequeDate = entity.ChequeDate.ToDate();
                }
                ee.BankAccountNo = entity.BankAccountNo;
                ee.TransferBankFrom = entity.TransferBankFrom;
                ee.TransferBankTo = entity.TransferBankTo;
                ee.BkashNo = entity.BkashNo;
                ee.Remarks = entity.Remarks;
            ee.Luser = loginInfo.EmployeeId;
            ee.Ldate = DateTime.Now;
            ee.CompanyCode = loginInfo.CompanyCode;
            ee.EmployeeId= loginInfo.EmployeeId;
            Add(ee);

            return true;
        }

        public bool SavePermission(string accessCode)
        {
            return accessCodeRepository.All().Any(x => x.AccessCodeId == accessCode && x.Title == "Expense Entry" && x.CheckAdd);
        }

        public bool UpdatePermission(string accessCode)
        {
            return accessCodeRepository.All().Any(x => x.AccessCodeId == accessCode && x.Title == "Expense Entry" && x.CheckEdit);
        }
    }
}
