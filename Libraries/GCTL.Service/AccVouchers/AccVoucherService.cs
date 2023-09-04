using GCTL.Core.Data;
using GCTL.Core.ViewModels.AccVouchers;
using GCTL.Core.ViewModels.Common;
using GCTL.Data.Models;
using GCTL.Core.ViewModels.Accounts;
using GCTL.Core.Helpers;
using Dapper;
using GCTL.Data.Extensions;
using Microsoft.Extensions.Configuration;

namespace GCTL.Service.AccVouchers
{
    public class AccVoucherService : AppService<AccVoucherEntry>, IAccVoucherService
    {
        private readonly IRepository<AccVoucherEntry> AccVoucherEntryRepository;     
        private readonly IRepository<AccVoucherEntryDetails> AccVoucherEntryDetailsRepository;
        private readonly IRepository<AccVoucherType> AVTRepository;
        private readonly IRepository<AccSubsidiaryLedger> AccSubsidiaryLedgerRepository;        
        private readonly IRepository<CoreAccessCode> accessCodeRepository;
        private readonly IConfiguration configuration;

        public AccVoucherService(IRepository<AccVoucherEntry> AccVoucherEntryRepository,
             IRepository<AccVoucherEntryDetails> AccVoucherEntryDetailsRepository,
             IRepository<AccVoucherType> AVTRepository,
            IRepository<AccSubsidiaryLedger> AccSubsidiaryLedgerRepository,   
                    IRepository<CoreAccessCode> accessCodeRepository,
                      IConfiguration configuration)
                    : base(AccVoucherEntryRepository)
        {
            this.AccVoucherEntryRepository = AccVoucherEntryRepository;
            this.AccVoucherEntryDetailsRepository = AccVoucherEntryDetailsRepository;
            this.AVTRepository = AVTRepository;
            this.AccSubsidiaryLedgerRepository = AccSubsidiaryLedgerRepository;         
            this.accessCodeRepository = accessCodeRepository;
            this.configuration = configuration;
        }

       
        public AccVoucherSetupViewModel GetInfo(string code)
        {
            var data = (from gl in AccVoucherEntryRepository.All()
                        where (gl.VoucherNo == code)
                        select new AccVoucherSetupViewModel
                        {
                            Main_CompanyCode = gl.MainCompanyCode,
                            VoucherType_Code = gl.VoucherTypeCode,
                            VoucherNo = gl.VoucherNo,
                            VoucherDate = gl.VoucherDate.ToDateFormat(),
                            Narration = gl.Narration
                        }).FirstOrDefault();


            return data;
            
        }
        public List<AccVoucherDetailsSetupViewModel> GetInfoDetails(string VoucherNo)
        {
            var queryParameters = new DynamicParameters();
            queryParameters.Add("@VoucherNo", VoucherNo);
            var data = QueryExtensionsHelpers.Query<AccVoucherDetailsSetupViewModel>(configuration.GetConnectionString("ApplicationDbConnection"),
                "Prc_Frm_TempAccountDetails", queryParameters, System.Data.CommandType.StoredProcedure).ToList();
            return data;
        }
        public List<VoucherEntryReportModel> GetInfoForReport(string VoucherNo)
        {
            var queryParameters = new DynamicParameters();
            queryParameters.Add("@VoucherNo", VoucherNo);
            var data = QueryExtensionsHelpers.Query<VoucherEntryReportModel>(configuration.GetConnectionString("ApplicationDbConnection"),
                "Rpt_proc_BankReceiptVoucher", queryParameters, System.Data.CommandType.StoredProcedure).ToList();
            return data;
        }
        public List<AccVoucherSetupViewModel> GetInfoList(string FromDate, string ToDate)
        {
            string NewFromDate = FromDate + " 00:00:00";
            string NewToDate = ToDate + " 23:59:59";

            DateTime startDate = DateTime.MinValue;
            DateTime endDate = DateTime.MinValue;



            if (!string.IsNullOrWhiteSpace(FromDate))
                startDate = DateTime.ParseExact(NewFromDate, "dd/MM/yyyy HH:mm:ss", null);
            if (!string.IsNullOrWhiteSpace(ToDate))
                endDate = DateTime.ParseExact(NewToDate, "dd/MM/yyyy HH:mm:ss", null);


            var data = (from acv in AccVoucherEntryRepository.All()
                        join vt in AVTRepository.All() on acv.VoucherTypeCode equals vt.VoucherTypeCode


                        where
                           (startDate == DateTime.MinValue || acv.VoucherDate >= startDate)
                          && (endDate == DateTime.MinValue || acv.VoucherDate <= endDate)
                        select new AccVoucherSetupViewModel
                        {
                            VoucherNo = acv.VoucherNo,
                            VoucherDate = acv.VoucherDate.ToDateFormat(),
                            VoucherType_Code = vt.VoucherTypeName,
                            Narration = acv.Narration,
                            InvoiceNo = acv.InvoiceNo,
                            TotalAmount = (AccVoucherEntryDetailsRepository.All().Where(p => p.VoucherEntryAutoId == acv.VoucherEntryAutoId)
                            .Select(p => p.DebitAmount)).Sum()


                        }).ToList();
            return data;

        }
        public bool IsASSLexistInVE(string code)
        {
            return AccVoucherEntryDetailsRepository.All().Any(x => x.AccCode == code);
        }
        public bool IsExistByCode(string code)
        {
            throw new NotImplementedException();
        }
        public bool Delete(string code)
        {
            var result = AccVoucherEntryRepository.All().Where(x => x.VoucherNo == code).FirstOrDefault();
            if (result != null)
            {
                var sdDe = AccVoucherEntryDetailsRepository.All().Where(x => x.VoucherEntryAutoId == result.VoucherEntryAutoId).ToList();
                if (sdDe != null)
                {
                    AccVoucherEntryDetailsRepository.Delete(sdDe);
                }
                AccVoucherEntryRepository.Delete(result);
            }
            return true;
        }
        public bool DeleteByInvoiceNo(string InvoiceNo)
        {
            var queryParameters = new DynamicParameters();
            queryParameters.Add("@InvoiceNo", InvoiceNo);
            var data = QueryExtensionsHelpers.Query<DBNull>(configuration.GetConnectionString("ApplicationDbConnection"),
                "Proc_DeleteVoucherByInvoice", queryParameters, System.Data.CommandType.StoredProcedure);
            return true;
        }
        public bool Save(AccVoucherSetupViewModel model, UserInfoViewModel loginInfo)
        {
          
            AccVoucherEntry ve = new AccVoucherEntry();
            ve.MainCompanyCode = model.Main_CompanyCode;
            ve.FinancialYear = "";
            ve.CostCenterCodeNo = "";
            ve.PeriodCodeNo = "";
            ve.VoucherTypeCode = model.VoucherType_Code;
            ve.VoucherDate = model.VoucherDate.ToDate();
            ve.VoucherNo = model.VoucherNo;
            ve.Narration = model.Narration;
            ve.Luser = loginInfo.EmployeeId;
            ve.UserInfoEmployeeId = loginInfo.EmployeeId;
            ve.Ldate = DateTime.Now;
            ve.CompanyCode = "001";
            ve.IsApproved = "";
            Add(ve);
            foreach (var i in model.voucherDetails)
            {
                AccVoucherEntryDetails k = new AccVoucherEntryDetails();
                k.VoucherEntryAutoId = ve.VoucherEntryAutoId;
               
                k.AccCode = i.AccCode;
                k.TrType = i.TrType;
                k.Description = i.Description;
                k.DebitAmount = i.DebitAmount;
                k.CreditAmount = i.CreditAmount;
                k.ChequeNo = i.ChequeNo;
                if (!string.IsNullOrEmpty(i.ChequeDate))
                {
                    k.ChequeDate = i.ChequeDate.ToDate();
                }
                k.Luser = loginInfo.EmployeeId;
                AccVoucherEntryDetailsRepository.Add(k);
            }
            return true;


        }
        public bool Update(AccVoucherSetupViewModel entity, UserInfoViewModel loginInfo)
        {
            var result = AccVoucherEntryRepository.All().Where(x=>x.VoucherNo==entity.VoucherNo).FirstOrDefault();
            if (result != null)
            {
                result.MainCompanyCode = entity.Main_CompanyCode;
                result.VoucherTypeCode = entity.VoucherType_Code;
                result.VoucherDate = entity.VoucherDate.ToDate();
                result.Narration = entity.Narration;

                result.Luser = loginInfo.EmployeeId;
                result.UserInfoEmployeeId = loginInfo.EmployeeId;
                var sdDe = AccVoucherEntryDetailsRepository.All().Where(x => x.VoucherEntryAutoId == result.VoucherEntryAutoId).ToList();
                if (sdDe != null)
                {
                    AccVoucherEntryDetailsRepository.Delete(sdDe);                   
                }
                foreach (var i in entity.voucherDetails)
                {
                    AccVoucherEntryDetails k = new AccVoucherEntryDetails();
                    k.VoucherEntryAutoId = result.VoucherEntryAutoId;

                    k.AccCode = i.AccCode;
                    k.TrType = i.TrType;
                    k.Description = i.Description;
                    k.DebitAmount = i.DebitAmount;
                    k.CreditAmount = i.CreditAmount;
                    k.ChequeNo = i.ChequeNo;
                    if (!string.IsNullOrEmpty(i.ChequeDate))
                    {
                        k.ChequeDate = i.ChequeDate.ToDate();
                    }
                    k.Luser = loginInfo.EmployeeId;
                    AccVoucherEntryDetailsRepository.Add(k);
                }
            }
            AccVoucherEntryRepository.Update(result);
            return true;
        }
        public bool SavePermission(string accessCode)
        {
            return accessCodeRepository.All().Any(x => x.AccessCodeId == accessCode && x.Title == "Voucher Entry" && x.CheckAdd);
        }
        public bool UpdatePermission(string accessCode)
        {
            return accessCodeRepository.All().Any(x => x.AccessCodeId == accessCode && x.Title == "Voucher Entry" && x.CheckEdit);
        }
        public bool DeletePermission(string accessCode)
        {
            return accessCodeRepository.All().Any(x => x.AccessCodeId == accessCode && x.Title == "Voucher Entry" && x.CheckDelete);
        }
    }
}
