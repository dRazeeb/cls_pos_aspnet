using GCTL.Core.Data;
using GCTL.Core.ViewModels.Common;
using GCTL.Data.Models;

namespace GCTL.Service.AccControlLedgers
{
    public class AccControlLedgerService : AppService<AccControlLedger>, IAccControlLedgerService
    {
        private readonly IRepository<AccControlLedger> accControlLedgerRepository;
        private readonly IRepository<AccSubControlLedger> accSubControlLedgerRepository;

        private readonly IRepository<CoreAccessCode> accessCodeRepository;

        public AccControlLedgerService(IRepository<AccControlLedger> accControlLedgerRepository,
            IRepository<AccSubControlLedger> accSubControlLedgerRepository,
            IRepository<CoreAccessCode> accessCodeRepository)
            : base(accControlLedgerRepository)
        {
            this.accControlLedgerRepository = accControlLedgerRepository;
            this.accSubControlLedgerRepository = accSubControlLedgerRepository;

            this.accessCodeRepository = accessCodeRepository;
        }
        public IEnumerable<CommonSelectModel> ControlLedgerSelection()
        {
            return accControlLedgerRepository.All()
                 .Select(x => new CommonSelectModel
                 {
                     Code = x.ControlLedgerCodeNo,
                     Name = x.ControlLedgerName
                 });
        }

        public bool DeleteControlLedger(string id)
        {
            var entity = GetControlLedger(id);
            if (entity != null)
            {

                    accControlLedgerRepository.Delete(entity);
                    return true;
            }
            return false;
        }

       

        public AccControlLedger GetControlLedger(string code)
        {
            return accControlLedgerRepository.GetById(code);
        }

        public List<AccControlLedger> GetControlLedgers()
        {
            return GetAll();
        }

        public bool IsControlLedgerExist(string name)
        {
            return accControlLedgerRepository.All().Any(x => x.ControlLedgerName == name);
        }

        public bool IsControlLedgerExist(string name, string typeCode)
        {
            return accControlLedgerRepository.All().Any(x => x.ControlLedgerName == name && x.ControlLedgerCodeNo != typeCode);
        }

        public bool IsControlLedgerExistByCode(string code)
        {
            return accControlLedgerRepository.All().Any(x => x.ControlLedgerCodeNo == code);
        }

        public AccControlLedger SaveControlLedger(AccControlLedger entity)
        {
            if (IsControlLedgerExistByCode(entity.ControlLedgerCodeNo))
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
        public bool DeletePermission(string accessCode)
        {
            return accessCodeRepository.All().Any(x => x.AccessCodeId == accessCode && x.Title == "Chart Of Account" && x.CheckDelete);
        }
    }
}
