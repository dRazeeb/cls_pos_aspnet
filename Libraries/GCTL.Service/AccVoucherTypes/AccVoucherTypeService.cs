using GCTL.Core.Data;
using GCTL.Core.ViewModels.Common;
using GCTL.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCTL.Service.AccVoucherTypes
{
    public class AccVoucherTypeService : AppService<AccVoucherType>, IAccVoucherTypeService
    {
        private readonly IRepository<AccVoucherType> AccVoucherTypeRepository;
        private readonly IRepository<CoreAccessCode> accessCodeRepository;

        public AccVoucherTypeService(IRepository<AccVoucherType> AccVoucherTypeRepository,      
            IRepository<CoreAccessCode> accessCodeRepository)
            : base(AccVoucherTypeRepository)
        {
            this.AccVoucherTypeRepository = AccVoucherTypeRepository;
            this.accessCodeRepository = accessCodeRepository;
        }

       

        public IEnumerable<CommonSelectModel> DropSelection()
        {
            return AccVoucherTypeRepository.All()
                .Select(x => new CommonSelectModel
                {
                    Code = x.VoucherTypeCode,
                    Name = x.Description
                });
        }

        public AccVoucherType GetInfo(string code)
        {
            throw new NotImplementedException();
        }

        public List<AccVoucherType> GetInfoList()
        {
            throw new NotImplementedException();
        }

        public bool IsExist(string name)
        {
            throw new NotImplementedException();
        }

        public bool IsExist(string name, string typeCode)
        {
            throw new NotImplementedException();
        }

        public bool IsExistByCode(string code)
        {
            throw new NotImplementedException();
        }

        public AccSubSubsidiaryLedger Save(AccSubSubsidiaryLedger entity)
        {
            throw new NotImplementedException();
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
