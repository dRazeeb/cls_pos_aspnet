
using GCTL.Core.Data;
using GCTL.Core.ViewModels.AccSubSubsidiaryLedgers;
using GCTL.Core.ViewModels.Common;
using GCTL.Data.Extensions;
using GCTL.Data.Models;
using GCTL.Core.Configurations;
using Microsoft.Extensions.Configuration;
using Dapper;

namespace GCTL.Service.AccSubSubsidiaryLedgers
{
    public class AccSubSubsidiaryLedgerService : AppService<AccSubSubsidiaryLedger>, IAccSubSubsidiaryLedgerService
    {
        private readonly IRepository<AccSubSubsidiaryLedger> AccSubSubsidiaryLedgerRepository;
        private readonly IRepository<AccSubsidiaryLedger> AccSubsidiaryLedgerRepository;
        private readonly IRepository<AccGeneralLedger> accGeneralLedgerRepository;
        private readonly IRepository<AccSubControlLedger> accSubControlLedgerRepository;
        private readonly IRepository<AccControlLedger> accControlLedgerRepository;

        private readonly IConfiguration configuration;
        private readonly IRepository<CoreAccessCode> accessCodeRepository;
        public AccSubSubsidiaryLedgerService(IRepository<AccSubSubsidiaryLedger> AccSubSubsidiaryLedgerRepository,
            IRepository<AccSubsidiaryLedger> AccSubsidiaryLedgerRepository,
            IRepository<AccGeneralLedger> accGeneralLedgerRepository,
            IRepository<AccSubControlLedger> accSubControlLedgerRepository,
                    IRepository<AccControlLedger> accControlLedgerRepository,
                          IConfiguration configuration,
        IRepository<CoreAccessCode> accessCodeRepository)
                    : base(AccSubSubsidiaryLedgerRepository)
        {
            this.AccSubSubsidiaryLedgerRepository = AccSubSubsidiaryLedgerRepository;
            this.AccSubsidiaryLedgerRepository = AccSubsidiaryLedgerRepository;
            this.accGeneralLedgerRepository = accGeneralLedgerRepository;
            this.accSubControlLedgerRepository = accSubControlLedgerRepository;
            this.accControlLedgerRepository = accControlLedgerRepository;
            this.configuration = configuration;
            this.accessCodeRepository = accessCodeRepository;
        }

        public bool DeletePermission(string accessCode)
        {
            return accessCodeRepository.All().Any(x => x.AccessCodeId == accessCode && x.Title == "Chart Of Account" && x.CheckDelete);
        }

        public IEnumerable<CommonSelectModel> DropSelection()
        {
           //List< AccSubSubsidiaryLedger > Assl= new List<AccSubSubsidiaryLedger>();

            var data = QueryExtensionsHelpers.Query<AccSubSubsidiaryLedger>(configuration.GetConnectionString("ApplicationDbConnection"),
                "Prc_Load_AcountHead",  null,System.Data.CommandType.StoredProcedure).ToList();

            return data.Select(x => new CommonSelectModel
            {
                Code = x.SubSusidiaryLedgerCodeNo,
                Name = x.SubSubsidiaryLedgerName
          });
        }
        public IEnumerable<CommonSelectModel> getBkashkDropSelection()
        {
            return AccSubSubsidiaryLedgerRepository.All().Where(x=>x.SubsidiaryLedgerCodeNo== "1020070001")
           .Select(x => new CommonSelectModel
           {
               Code = x.SubSusidiaryLedgerCodeNo,
               Name = x.SubSubsidiaryLedgerName
           });
        }
        public IEnumerable<CommonSelectModel> getExpenseDropSelection()
        {
            return AccSubSubsidiaryLedgerRepository.All().Where(x => x.SubsidiaryLedgerCodeNo == "3020010001")
           .Select(x => new CommonSelectModel
           {
               Code = x.SubSusidiaryLedgerCodeNo,
               Name = x.SubSubsidiaryLedgerName
           });
        }
        public List<AccSubSubsidiaryLedgerSetupViewModel> GetAccHeadGrid(string SubSusidiaryLedgerCodeNo)
        {
            //List< AccSubSubsidiaryLedger > Assl= new List<AccSubSubsidiaryLedger>();
            var queryParameters = new DynamicParameters();
            queryParameters.Add("@SubSusidiaryLedgerCodeNo", SubSusidiaryLedgerCodeNo);
            var data = QueryExtensionsHelpers.Query<AccSubSubsidiaryLedgerSetupViewModel>(configuration.GetConnectionString("ApplicationDbConnection"),
                "proc_Select_CL_SCL_GGL_GLAndSLAllInfo", queryParameters, System.Data.CommandType.StoredProcedure).ToList();
            return data;
           

        }
        public AccSubSubsidiaryLedger GetInfo(string code)
        {
            return AccSubSubsidiaryLedgerRepository.GetById(code);
        }

        public IEnumerable<CommonSelectModel> GetInfoBYParent(string SubsidiaryLedgerCodeNo)
        {

            return AccSubSubsidiaryLedgerRepository.All().
                Where(x => x.SubsidiaryLedgerCodeNo == SubsidiaryLedgerCodeNo)
        .Select(x => new CommonSelectModel
        {
            Code = x.SubSusidiaryLedgerCodeNo,
            Name = x.SubSubsidiaryLedgerName
        });
        }

        public AccSubSubsidiaryLedgerSetupViewModel GetInfoForView(string code)
        {
            var data = (from assl in AccSubSubsidiaryLedgerRepository.All()
                        join asl in AccSubsidiaryLedgerRepository.All()
                        on assl.SubsidiaryLedgerCodeNo equals asl.SusidiaryLedgerCodeNo
                        join gl in accGeneralLedgerRepository.All()
                        on asl.GeneralLedgerCodeNo equals gl.GeneralLedgerCodeNo

                        join scl in accSubControlLedgerRepository.All()
                        on gl.SubControlLedgerCodeNo equals scl.SubControlLedgerCodeNo

                        join cl in accControlLedgerRepository.All()
                        on scl.ControlLedgerCodeNo equals cl.ControlLedgerCodeNo

                        where (assl.SubSusidiaryLedgerCodeNo == code)
                        select new AccSubSubsidiaryLedgerSetupViewModel
                        {
                            SubsidiaryLedgerCodeNo = assl.SubsidiaryLedgerCodeNo,
                            SubSusidiaryLedgerCodeNo = assl.SubSusidiaryLedgerCodeNo,
                            SubSubsidiaryLedgerName = assl.SubSubsidiaryLedgerName,
                            ShortName = assl.ShortName,
                            OpeningBalance=assl.OpeningBalance,
                            SubsidiaryLedgerName = asl.SubsidiaryLedgerName,

                            GeneralLedgerCodeNo = gl.GeneralLedgerCodeNo,
                            GeneralLedgerName = gl.GeneralLedgerName,
                            SubControlLedgerCodeNo = gl.SubControlLedgerCodeNo,
                            Ldate = assl.Ldate,
                            ModifyDate = assl.ModifyDate,
                            SubControlLedgerName = scl.SubControlLedgerName,
                            ControlLedgerCodeNo = scl.ControlLedgerCodeNo,
                            ControlLedgerName = cl.ControlLedgerName
                        }).FirstOrDefault();


            return data;
        }

        public List<AccSubSubsidiaryLedgerSetupViewModel> GetInfoList(string ControlLedgerCodeNo,
            string SubControlLedgerCodeNo, string GeneralLedgerCodeNo,string SubsidiaryLedgerCodeNo)
        {
            var data = (from assl in AccSubSubsidiaryLedgerRepository.All()
                        join asl in AccSubsidiaryLedgerRepository.All()
                        on assl.SubsidiaryLedgerCodeNo equals asl.SusidiaryLedgerCodeNo

                        join gl in accGeneralLedgerRepository.All()
                        on asl.GeneralLedgerCodeNo equals gl.GeneralLedgerCodeNo

                        join scl in accSubControlLedgerRepository.All()
                        on gl.SubControlLedgerCodeNo equals scl.SubControlLedgerCodeNo
                        join cl in accControlLedgerRepository.All()
                        on scl.ControlLedgerCodeNo equals cl.ControlLedgerCodeNo

                        where (ControlLedgerCodeNo == null || scl.ControlLedgerCodeNo == ControlLedgerCodeNo)
                     && (SubControlLedgerCodeNo == null || gl.SubControlLedgerCodeNo == SubControlLedgerCodeNo)
                      && (GeneralLedgerCodeNo == null || asl.GeneralLedgerCodeNo == GeneralLedgerCodeNo)
                      && (SubsidiaryLedgerCodeNo == null || assl.SubsidiaryLedgerCodeNo == SubsidiaryLedgerCodeNo)
                        select new AccSubSubsidiaryLedgerSetupViewModel
                        {
                            SubsidiaryLedgerCodeNo=assl.SubsidiaryLedgerCodeNo,
                            SubSusidiaryLedgerCodeNo=assl.SubSusidiaryLedgerCodeNo,
                            SubSubsidiaryLedgerName=assl.SubSubsidiaryLedgerName,
                            
                            SubsidiaryLedgerName = asl.SubsidiaryLedgerName,
                            GeneralLedgerCodeNo = gl.GeneralLedgerCodeNo,
                            GeneralLedgerName = gl.GeneralLedgerName,
                            SubControlLedgerName = scl.SubControlLedgerName,
                            ControlLedgerName = cl.ControlLedgerName
                        }).ToList();
            return data;
        }

        public bool IsExist(string SubsidiaryLedgerCodeNo, string name)
        {
            return AccSubSubsidiaryLedgerRepository.All().Any(x => x.SubSubsidiaryLedgerName == name && x.SubsidiaryLedgerCodeNo == SubsidiaryLedgerCodeNo);
        }

        public bool IsExist(string SubsidiaryLedgerCodeNo, string name, string typeCode)
        {
            return AccSubSubsidiaryLedgerRepository.All().Any(x => x.SubSubsidiaryLedgerName == name && x.SubsidiaryLedgerCodeNo == SubsidiaryLedgerCodeNo && x.SubSusidiaryLedgerCodeNo != typeCode);
        }

        public bool IsExistByCode(string code)
        {
            return AccSubSubsidiaryLedgerRepository.All().Any(x => x.SubSusidiaryLedgerCodeNo == code);
        }

        public bool IsExistBySL(string code)
        {
            return AccSubSubsidiaryLedgerRepository.All().Any(x => x.SubsidiaryLedgerCodeNo == code);
        }

        public AccSubSubsidiaryLedger Save(AccSubSubsidiaryLedger entity)
        {
            if (IsExistByCode(entity.SubSusidiaryLedgerCodeNo))
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
