using GCTL.Core.Data;
using GCTL.Core.ViewModels.AccSubControlLedgers;
using GCTL.Core.ViewModels.Common;
using GCTL.Data.Models;
using System.Numerics;
using System.Security.Cryptography;

namespace GCTL.Service.AccSubControlLedgers
{
    public class AccSubControlLedgerService : AppService<AccSubControlLedger>, IAccSubControlLedgerService
    {
        private readonly IRepository<AccSubControlLedger> accSubControlLedgerRepository;
        private readonly IRepository<AccControlLedger> accControlLedgerRepository;
        private readonly IRepository<CoreAccessCode> accessCodeRepository;

        public AccSubControlLedgerService(IRepository<AccSubControlLedger> accSubControlLedgerRepository,
            IRepository<AccControlLedger> accControlLedgerRepository,
            IRepository<CoreAccessCode> accessCodeRepository)
            : base(accSubControlLedgerRepository)
        {
            this.accSubControlLedgerRepository = accSubControlLedgerRepository;
            this.accControlLedgerRepository = accControlLedgerRepository;
            this.accessCodeRepository = accessCodeRepository;
        }

        public bool DeletePermission(string accessCode)
        {
            return accessCodeRepository.All().Any(x => x.AccessCodeId == accessCode && x.Title == "Chart Of Account" && x.CheckDelete);
        }

        public bool DeleteSubControlLedger(string id)
        {
            var entity = GetSubControlLedger(id);
            if (entity != null)
            {
                accSubControlLedgerRepository.Delete(entity);
                return true;
            }
            return false;
        }

        public List<AccSubControlLedgerSetupViewModel> GetSubControlLedgers(string ControlLedgerCodeNo)
        {

            //var data = (from doctor in doctorRepository.All()
            //            join department in departmentRepository.All()
            //            on doctor.DepartmentCode equals department.DepartmentCode into doctorDepartment
            //            select new DoctorViewModel
            //            {
            //                DoctorCode = doctor.DoctorCode,
            //                BanglaDoctorName = doctor.BanglaDoctorName
            //            }).ToList();
            var data = (from scl in accSubControlLedgerRepository.All()
                        join cl in accControlLedgerRepository.All()
                        on scl.ControlLedgerCodeNo equals cl.ControlLedgerCodeNo
                        where (ControlLedgerCodeNo == null || scl.ControlLedgerCodeNo == ControlLedgerCodeNo)

                        select new AccSubControlLedgerSetupViewModel
                        {
                            ControlLedgerCodeNo=scl.ControlLedgerCodeNo,
                            SubControlLedgerCodeNo=scl.SubControlLedgerCodeNo,
                            SubControlLedgerName=scl.SubControlLedgerName,
                            ControlLedgerName=cl.ControlLedgerName
                        }).ToList();


            return data;
        }

        public AccSubControlLedger GetSubControlLedger(string code)
        {
            return accSubControlLedgerRepository.GetById(code);
        }

        public bool IsSubControlLedgerExist(string ControlLedgerCode, string name)
        {
            return accSubControlLedgerRepository.All().Any(x => x.SubControlLedgerName == name && x.ControlLedgerCodeNo==ControlLedgerCode);
        }

        public bool IsSubControlLedgerExist(string ControlLedgerCode, string name, string typeCode)
        {
            return accSubControlLedgerRepository.All().Any(x => x.SubControlLedgerName == name && x.ControlLedgerCodeNo == ControlLedgerCode && x.SubControlLedgerCodeNo != typeCode);
        }

        public bool IsSubControlLedgerExistByCode(string code)
        {
            return accSubControlLedgerRepository.All().Any(x => x.SubControlLedgerCodeNo == code);
        }
        public bool IsSubControlLedgerExistByCL(string code)
        {
            return accSubControlLedgerRepository.All().Any(x => x.ControlLedgerCodeNo == code);
        }
        
        public bool SavePermission(string accessCode)
        {
            return accessCodeRepository.All().Any(x => x.AccessCodeId == accessCode && x.Title == "Chart Of Account" && x.CheckAdd);
        }

        public AccSubControlLedger SaveSubControlLedger(AccSubControlLedger entity)
        {
            if (IsSubControlLedgerExistByCode(entity.SubControlLedgerCodeNo))
            {
                Update(entity);
            }
            else
            {
                Add(entity);

            }

            return entity;
        }

        public IEnumerable<CommonSelectModel> SubControlLedgerSelection()
        {
            return accSubControlLedgerRepository.All()
       .Select(x => new CommonSelectModel
       {
           Code = x.SubControlLedgerCodeNo,
           Name = x.SubControlLedgerName
       });
          
        }
        public IEnumerable<CommonSelectModel> SubControlLedgerSelectionBYCl(string ControlLedgerCodeNo)
        {
            return accSubControlLedgerRepository.All().
                Where(x => x.ControlLedgerCodeNo == ControlLedgerCodeNo)
        .Select(x => new CommonSelectModel
        {
            Code = x.SubControlLedgerCodeNo,
            Name = x.SubControlLedgerName
        });
        }
        public bool UpdatePermission(string accessCode)
        {
            return accessCodeRepository.All().Any(x => x.AccessCodeId == accessCode && x.Title == "Chart Of Account" && x.CheckEdit);
        }
    }
}
