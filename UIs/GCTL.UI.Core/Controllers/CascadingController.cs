using Microsoft.AspNetCore.Mvc;
using GCTL.Service.Departments;
using GCTL.Service.Designations;
using GCTL.Service.Employees;
using GCTL.Service.PaymentModes;
using GCTL.Service.Religions;
using GCTL.Service.Shifts;
using GCTL.Service.AccSubControlLedgers;
using GCTL.Service.AccGeneralLedgers;
using GCTL.Service.AccSubsidiaryLedgers;
using GCTL.Service.AccSubSubsidiaryLedgers;

namespace GCTL.UI.Core.Controllers
{
    public class CascadingController : Controller
    {
        private readonly IReligionService religionService;
        private readonly IDesignationService designationService;
        private readonly IDepartmentService departmentService;
        private readonly IShiftService shiftService;
        private readonly IEmployeeService employeeService;
        private readonly IPaymentModeService paymentModeService;
        
        private readonly IAccSubControlLedgerService SubControlLedgerService;
        private readonly IAccGeneralLedgerService GeneralLedgerService;
        private readonly IAccSubsidiaryLedgerService SubsidiaryLedgerService;
        private readonly IAccSubSubsidiaryLedgerService asslService;
        public CascadingController(
            IAccSubControlLedgerService SubControlLedgerService,
             IAccGeneralLedgerService GeneralLedgerService,
              IAccSubsidiaryLedgerService SubsidiaryLedgerService,
              IAccSubSubsidiaryLedgerService asslService,
                                   IReligionService religionService,
                                   IDesignationService designationService,
                                   IDepartmentService departmentService,
                                   IShiftService shiftService,
                                   IEmployeeService employeeService,
                                   IPaymentModeService paymentModeService)
        {
            this.SubsidiaryLedgerService = SubsidiaryLedgerService;
            this.GeneralLedgerService = GeneralLedgerService;
            this.SubControlLedgerService = SubControlLedgerService;
            this.asslService = asslService;
            this.religionService = religionService;
            this.designationService = designationService;
            this.departmentService = departmentService;
            this.shiftService = shiftService;
            this.employeeService = employeeService;
            this.paymentModeService = paymentModeService;
 
        }
        public IActionResult GetASSLExpenseHead()
        {
            return Json(asslService.getExpenseDropSelection());
        }
        public IActionResult GetASLedger(string GeneralLedgerCodeNo)
        {
            return Json(SubsidiaryLedgerService.GetInfoBYParenet(GeneralLedgerCodeNo));
        }
        public IActionResult GetGeneralLedger(string SubControlLedgerCodeNo)
        {
            return Json(GeneralLedgerService.GetInfoBYParent(SubControlLedgerCodeNo));
        }
        public IActionResult GetSubControlLedger(string ControlLedgerCodeNo)
        {
            return Json(SubControlLedgerService.SubControlLedgerSelectionBYCl(ControlLedgerCodeNo));
        }
        
        public IActionResult GetReligions()
        {
            return Json(religionService.ReligionSelection());
        }

        public IActionResult GetDesignations()
        {
            return Json(designationService.DesignationSelection());
        }

        public IActionResult GetDepartments()
        {
            return Json(departmentService.DepartmentSelection());
        }

        public IActionResult GetPaymentModes()
        {
            return Json(paymentModeService.PaymentModeSelection());
        }

        public IActionResult GetEmployeeSummary(string employeeId)
        {
            return Json(employeeService.GetEmployee(employeeId));
        }

    }
}
