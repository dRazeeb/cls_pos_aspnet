
using GCTL.Core.Data;
using GCTL.Core.ViewModels.AccSubsidiaryLedgers;
using GCTL.Core.ViewModels.Common;
using GCTL.Data.Models;

namespace GCTL.Service.AccSubsidiaryLedgers
{
    public class AccSubsidiaryLedgerService : AppService<AccSubsidiaryLedger>, IAccSubsidiaryLedgerService
    {
        private readonly IRepository<AccSubsidiaryLedger> AccSubsidiaryLedgerRepository;
        private readonly IRepository<AccGeneralLedger> accGeneralLedgerRepository;
        private readonly IRepository<AccSubControlLedger> accSubControlLedgerRepository;
        private readonly IRepository<AccControlLedger> accControlLedgerRepository;
        private readonly IRepository<CoreAccessCode> accessCodeRepository;
        public AccSubsidiaryLedgerService(IRepository<AccSubsidiaryLedger> AccSubsidiaryLedgerRepository,
            IRepository<AccGeneralLedger> accGeneralLedgerRepository,
            IRepository<AccSubControlLedger> accSubControlLedgerRepository,
                    IRepository<AccControlLedger> accControlLedgerRepository,
                    IRepository<CoreAccessCode> accessCodeRepository)
                    : base(AccSubsidiaryLedgerRepository)
        {
            this.AccSubsidiaryLedgerRepository = AccSubsidiaryLedgerRepository;
            this.accGeneralLedgerRepository = accGeneralLedgerRepository;
            this.accSubControlLedgerRepository = accSubControlLedgerRepository;
            this.accControlLedgerRepository = accControlLedgerRepository;
            this.accessCodeRepository = accessCodeRepository;
        }

        public bool DeletePermission(string accessCode)
        {
            return accessCodeRepository.All().Any(x => x.AccessCodeId == accessCode && x.Title == "Chart Of Account" && x.CheckDelete);
        }

        public IEnumerable<CommonSelectModel> DropSelection()
        {
            return AccSubsidiaryLedgerRepository.All()
           .Select(x => new CommonSelectModel
           {
               Code = x.SusidiaryLedgerCodeNo,
               Name = x.SubsidiaryLedgerName
           });
        }
        public IEnumerable<CommonSelectModel> GetInfoBYParenet(string GeneralLedgerCodeNo)
        {
            return AccSubsidiaryLedgerRepository.All().
                Where(x => x.GeneralLedgerCodeNo == GeneralLedgerCodeNo)
        .Select(x => new CommonSelectModel
        {
            Code = x.SusidiaryLedgerCodeNo,
            Name = x.SubsidiaryLedgerName
        });
        }
        public AccSubsidiaryLedger GetInfo(string code)
        {
            return AccSubsidiaryLedgerRepository.GetById(code);
        }

        public AccSubsidiaryLedgerSetupViewModel GetInfoForView(string code)
        {
            var data = (from asl in AccSubsidiaryLedgerRepository.All()

                        join gl in accGeneralLedgerRepository.All()
                        on asl.GeneralLedgerCodeNo equals gl.GeneralLedgerCodeNo

                        join scl in accSubControlLedgerRepository.All()
                        on gl.SubControlLedgerCodeNo equals scl.SubControlLedgerCodeNo
                       
                        join cl in accControlLedgerRepository.All()
                        on scl.ControlLedgerCodeNo equals cl.ControlLedgerCodeNo

                        where (asl.SusidiaryLedgerCodeNo == code)
                        select new AccSubsidiaryLedgerSetupViewModel
                        {
                            SusidiaryLedgerCodeNo = asl.SusidiaryLedgerCodeNo,
                            SubsidiaryLedgerName=asl.SubsidiaryLedgerName,
                            ShortName = asl.ShortName,

                            GeneralLedgerCodeNo = gl.GeneralLedgerCodeNo,
                            GeneralLedgerName = gl.GeneralLedgerName,
                            SubControlLedgerCodeNo = gl.SubControlLedgerCodeNo,
                            Ldate = asl.Ldate,
                            ModifyDate = asl.ModifyDate,
                            SubControlLedgerName = scl.SubControlLedgerName,
                            ControlLedgerCodeNo = scl.ControlLedgerCodeNo,
                            ControlLedgerName = cl.ControlLedgerName
                        }).FirstOrDefault();


            return data;
        }

        public List<AccSubsidiaryLedgerSetupViewModel> GetInfoList(string ControlLedgerCodeNo, string SubControlLedgerCodeNo, string GeneralLedgerCodeNo)
        {
            var data = (from asl in AccSubsidiaryLedgerRepository.All()
                         join gl in accGeneralLedgerRepository.All()
                         on asl.GeneralLedgerCodeNo equals gl.GeneralLedgerCodeNo
                        join scl in accSubControlLedgerRepository.All()
                        on gl.SubControlLedgerCodeNo equals scl.SubControlLedgerCodeNo
                        join cl in accControlLedgerRepository.All()
                        on scl.ControlLedgerCodeNo equals cl.ControlLedgerCodeNo

                        where (ControlLedgerCodeNo == null || scl.ControlLedgerCodeNo == ControlLedgerCodeNo)
                     && (SubControlLedgerCodeNo == null || gl.SubControlLedgerCodeNo == SubControlLedgerCodeNo)
                      && (GeneralLedgerCodeNo == null || asl.GeneralLedgerCodeNo == GeneralLedgerCodeNo)
                        select new AccSubsidiaryLedgerSetupViewModel
                        {
                            SusidiaryLedgerCodeNo = asl.SusidiaryLedgerCodeNo,
                            SubsidiaryLedgerName=asl.SubsidiaryLedgerName,
                            GeneralLedgerCodeNo = gl.GeneralLedgerCodeNo,
                            GeneralLedgerName = gl.GeneralLedgerName,
                            SubControlLedgerName = scl.SubControlLedgerName,
                            ControlLedgerName = cl.ControlLedgerName
                        }).ToList();
            return data;
        }

        public bool IsExist(string GeneralLedgerCodeNo, string name)
        {
            return AccSubsidiaryLedgerRepository.All().Any(x => x.SubsidiaryLedgerName == name  && x.GeneralLedgerCodeNo == GeneralLedgerCodeNo);
        }

        public bool IsExist(string GeneralLedgerCodeNo, string name, string typeCode)
        {
            return AccSubsidiaryLedgerRepository.All().Any(x => x.SubsidiaryLedgerName == name && x.GeneralLedgerCodeNo == GeneralLedgerCodeNo && x.SusidiaryLedgerCodeNo != typeCode);
        }

        public bool IsExistByCode(string code)
        {
            return AccSubsidiaryLedgerRepository.All().Any(x => x.SusidiaryLedgerCodeNo == code);
        }

        public bool IsExistByGL(string code)
        {
            return AccSubsidiaryLedgerRepository.All().Any(x => x.GeneralLedgerCodeNo == code);
        }

        public AccSubsidiaryLedger Save(AccSubsidiaryLedger entity)
        {
            if (IsExistByCode(entity.SusidiaryLedgerCodeNo))
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
