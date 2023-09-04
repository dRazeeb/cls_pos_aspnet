using GCTL.Core.Data;
using GCTL.Data;
using GCTL.Data.Models;
using GCTL.Service.Common;
using GCTL.Service.Companies;
using GCTL.Service.Employees;
using GCTL.Service.UnitTypes;
using GCTL.Service.Users;
using Microsoft.EntityFrameworkCore;
using GCTL.Service.PaymentModes;
using GCTL.Service.Religions;
using GCTL.Service.Designations;
using GCTL.Service.Departments;
using GCTL.Service.Reports;
using GCTL.Core.Configurations;
using Microsoft.Extensions.Options;
using GCTL.Service.Navigations;
using GCTL.Service.LogsLoggers;
using GCTL.Service.Loggers;
using GCTL.Service.Banks;
using GCTL.Service.BankBranches;
using GCTL.Service.BankAccounts;
using Microsoft.AspNetCore.Http.Features;
using GCTL.Service.AccControlLedgers;
using GCTL.Service.AccSubControlLedgers;
using GCTL.Service.AccGeneralLedgers;
using GCTL.Service.AccSubsidiaryLedgers;
using GCTL.Service.AccSubSubsidiaryLedgers;
using GCTL.Service.AccVouchers;
using GCTL.Service.AccVoucherTypes;
using GCTL.Service.ExpenseEntry;
using GCTL.Service.AccountReport;



namespace GCTL.UI.Core.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void ConfigureContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(x => 
            x.UseSqlServer(configuration.GetConnectionString("ApplicationDbConnection"),
            x => x.UseDateOnlyTimeOnly()));
        }

        public static void ConfigureServices(this IServiceCollection services)
        {
            services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IReligionService, ReligionService>();
            services.AddScoped<IDesignationService, DesignationService>();
            services.AddScoped<IDepartmentService, DepartmentService>();
            services.AddScoped<ICommonService, CommonService>();
            services.AddScoped<ICompanyService, CompanyService>();
            services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAccessCodeService, AccessCodeService>();
            services.AddScoped<IEncoderService, EncoderService>();
            services.AddScoped<IUnitTypeService, UnitTypeService>();
            services.AddScoped<IPaymentModeService, PaymentModeService>();
            //services.AddScoped<IPaymentTypeService, PaymentTypeService>();


            services.AddScoped<IBankService, BankService>();
            services.AddScoped<IBankBranchService, BankBranchService>();
            services.AddScoped<IBankAccountService, BankAccountService>();


            
            services.AddScoped<IAccControlLedgerService, AccControlLedgerService>();
            services.AddScoped<IAccSubControlLedgerService, AccSubControlLedgerService>();
            services.AddScoped<IAccGeneralLedgerService, AccGeneralLedgerService>();
            services.AddScoped<IAccSubsidiaryLedgerService, AccSubsidiaryLedgerService>();
            services.AddScoped<IAccSubSubsidiaryLedgerService, AccSubSubsidiaryLedgerService>();

            services.AddScoped<IAccVoucherService, AccVoucherService>();
            services.AddScoped<IAccVoucherTypeService, AccVoucherTypeService>();
            services.AddScoped<IExpenseEntryService, ExpenseEntryService>();
            services.AddScoped<IAccountReportService, AccountReportService>();

            services.AddScoped<IReportService, ReportService>();
            services.AddScoped<INavigationService, NavigationService>();
  

            services.AddScoped<ILogService, LogService>();
        }

        public static void ConfigureRequestFormSizeLimit(this IServiceCollection services)
        {
            services.Configure<FormOptions>(x => x.ValueCountLimit = 10000);
        }

        public static void ConfigureMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(Program));
        }

        public static void ConfigureSession(this IServiceCollection services)
        {
            services.AddDistributedMemoryCache();

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(20);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
        }

        public static void ReadConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<ApplicationSettings>(configuration.GetSection("ApplicationSettings"));
            services.AddSingleton(resolver => resolver.GetRequiredService<IOptions<ApplicationSettings>>().Value);

            services.Configure<SMSSetting>(configuration.GetSection("SMSSetting"));
            services.AddSingleton(resolver => resolver.GetRequiredService<IOptions<SMSSetting>>().Value);
        }
    }
}
