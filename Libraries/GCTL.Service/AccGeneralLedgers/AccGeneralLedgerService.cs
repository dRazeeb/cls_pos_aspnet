using GCTL.Core.Data;
using GCTL.Core.ViewModels.AccGeneralLedgers;
using GCTL.Core.ViewModels.AccSubControlLedgers;
using GCTL.Core.ViewModels.Common;
using GCTL.Data.Models;
using GCTL.Service.AccSubControlLedgers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCTL.Service.AccGeneralLedgers
{
    public class AccGeneralLedgerService : AppService<AccGeneralLedger>, IAccGeneralLedgerService
    {
        private readonly IRepository<AccGeneralLedger> accGeneralLedgerRepository;
        private readonly IRepository<AccSubControlLedger> accSubControlLedgerRepository;
        private readonly IRepository<AccControlLedger> accControlLedgerRepository;
        private readonly IRepository<CoreAccessCode> accessCodeRepository;
        public AccGeneralLedgerService(IRepository<AccGeneralLedger> accGeneralLedgerRepository,
            IRepository<AccSubControlLedger> accSubControlLedgerRepository,
                    IRepository<AccControlLedger> accControlLedgerRepository,
                    IRepository<CoreAccessCode> accessCodeRepository)
                    : base(accGeneralLedgerRepository)
        {
            this.accGeneralLedgerRepository = accGeneralLedgerRepository;
            this.accSubControlLedgerRepository = accSubControlLedgerRepository;
            this.accControlLedgerRepository = accControlLedgerRepository;
            this.accessCodeRepository = accessCodeRepository;
        }

        public bool DeleteGeneralLedger(string id)
        {
            var entity = GetGeneralLedger(id);
            if (entity != null)
            {
                accGeneralLedgerRepository.Delete(entity);
                return true;
            }
            return false;
        }

        public bool DeletePermission(string accessCode)
        {
            return accessCodeRepository.All().Any(x => x.AccessCodeId == accessCode && x.Title == "Chart Of Account" && x.CheckDelete);
        }

        public IEnumerable<CommonSelectModel> GeneralLedgerSelection()
        {
            return accGeneralLedgerRepository.All()
           .Select(x => new CommonSelectModel
           {
               Code = x.GeneralLedgerCodeNo,
               Name = x.GeneralLedgerName
           });
        }
        public IEnumerable<CommonSelectModel> GetInfoBYParent(string SubControlLedgerCodeNo)
        {
            return accGeneralLedgerRepository.All().
                Where(x => x.SubControlLedgerCodeNo == SubControlLedgerCodeNo)
        .Select(x => new CommonSelectModel
        {
            Code = x.GeneralLedgerCodeNo,
            Name = x.GeneralLedgerName
        });
        }
        public AccGeneralLedger GetGeneralLedger(string code)
        {
            return accGeneralLedgerRepository.GetById(code);
        }
        public AccGeneralLedgerSetupViewModel GetGeneralLedgerForView(string code)
        {
            var data = (from gl in accGeneralLedgerRepository.All()
                        join scl in accSubControlLedgerRepository.All()
                        on gl.SubControlLedgerCodeNo equals scl.SubControlLedgerCodeNo
                        join cl in accControlLedgerRepository.All()
                        on scl.ControlLedgerCodeNo equals cl.ControlLedgerCodeNo

                        where (gl.GeneralLedgerCodeNo == code)
                        select new AccGeneralLedgerSetupViewModel
                        {
                            GeneralLedgerCodeNo = gl.GeneralLedgerCodeNo,
                            GeneralLedgerName = gl.GeneralLedgerName,
                            ShortName = gl.ShortName,
                            SubControlLedgerCodeNo = gl.SubControlLedgerCodeNo,
                            Ldate = gl.Ldate,
                            ModifyDate = gl.ModifyDate,
                            SubControlLedgerName = scl.SubControlLedgerName,
                            ControlLedgerCodeNo = scl.ControlLedgerCodeNo,
                            ControlLedgerName = cl.ControlLedgerName
                        }).FirstOrDefault();


            return data;
        }
        public List<AccGeneralLedgerSetupViewModel> GetGeneralLedgers(string ControlLedgerCodeNo, string SubControlLedgerCodeNo)
        {
            var data = (from gl in accGeneralLedgerRepository.All()
                        join scl in accSubControlLedgerRepository.All()
                        on gl.SubControlLedgerCodeNo equals scl.SubControlLedgerCodeNo
                        join cl in accControlLedgerRepository.All()
                        on scl.ControlLedgerCodeNo equals cl.ControlLedgerCodeNo

                        where (ControlLedgerCodeNo == null || scl.ControlLedgerCodeNo == ControlLedgerCodeNo)
                     && (SubControlLedgerCodeNo == null || gl.SubControlLedgerCodeNo == SubControlLedgerCodeNo)

                        select new AccGeneralLedgerSetupViewModel
                        {
                            GeneralLedgerCodeNo = gl.GeneralLedgerCodeNo,
                            GeneralLedgerName = gl.GeneralLedgerName,
                            SubControlLedgerName = scl.SubControlLedgerName,
                            ControlLedgerName = cl.ControlLedgerName
                        }).ToList();


            return data;
        }

        public bool IsGeneralLedgerExist(string SubControlLedgerCodeNo, string name)
        {
            return accGeneralLedgerRepository.All().Any(x => x.GeneralLedgerName == name && x.SubControlLedgerCodeNo == SubControlLedgerCodeNo);
        }

        public bool IsGeneralLedgerExist(string SubControlLedgerCodeNo, string name, string typeCode)
        {
            return accGeneralLedgerRepository.All().Any(x => x.GeneralLedgerName == name && x.SubControlLedgerCodeNo == SubControlLedgerCodeNo && x.GeneralLedgerCodeNo != typeCode);
        }

        public bool IsGeneralLedgerExistByCode(string code)
        {
            return accGeneralLedgerRepository.All().Any(x => x.GeneralLedgerCodeNo == code);
        }

        public bool IsGeneralLedgerExistBySCL(string code)
        {
            return accSubControlLedgerRepository.All().Any(x => x.SubControlLedgerCodeNo == code);
        }

        public AccGeneralLedger SaveGeneralLedger(AccGeneralLedger entity)
        {
            if (IsGeneralLedgerExistByCode(entity.GeneralLedgerCodeNo))
            {
                Update(entity);
            }
            else
            {
                Add(entity);
            }
            return entity;
        }

        public bool SavePermission(string accessCode)
        {
            return accessCodeRepository.All().Any(x => x.AccessCodeId == accessCode && x.Title == "Chart Of Account" && x.CheckAdd);
        }

        public bool UpdatePermission(string accessCode)
        {
            return accessCodeRepository.All().Any(x => x.AccessCodeId == accessCode && x.Title == "Chart Of Account" && x.CheckEdit);
        }
    }
}
