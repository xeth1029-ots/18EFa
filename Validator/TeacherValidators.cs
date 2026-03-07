using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WDACC.Models.ViewModel.Admin;
using FluentValidation;
using WDACC.Models.Entities;
using WDACC.Models.ViewModel.Member;

namespace WDACC.Validator
{
    public class TeacherInfoValidator : BaseValidator<AdminTeacherModel>
    {
        public TeacherInfoValidator()
        {
            RuleFor(x => x.Detail.regyear).NotEmpty().WithMessage("加入年份 欄位為必要項");
            RuleFor(x => x.Detail.degree).NotEmpty().WithMessage("最高學歷 欄位為必要項");
            RuleFor(x => x.Detail.school).NotEmpty().WithMessage("畢業學校 欄位為必要項");
            RuleFor(x => x.Detail.major).NotEmpty().WithMessage("主修科目 欄位為必要項");
            RuleFor(x => x.Detail.skill).NotEmpty().WithMessage("專長/領域 欄位為必要項");
            RuleFor(x => x.Detail.jobcomp).NotEmpty().WithMessage("服務單位 欄位為必要項");
            RuleFor(x => x.Detail.jobtitle).NotEmpty().WithMessage("職稱 欄位為必要項");

            RuleFor(x => x.TechUnitList).Must(x => this.CheckTeachUnit(x)).WithMessage("授課單元 欄位為必要項");
            RuleFor(x => x.TechRegList).Must(x => this.CheckTeachReg(x)).WithMessage("授課區域 欄位為必要項");
            RuleFor(x => x.IndusList).Must(x => this.CheckIndus(x)).WithMessage("可授課產業別(1-3) 為必要項");

            RuleFor(x => x.Detail.group).NotEmpty().WithMessage("所屬社群 欄位為必要項");
            RuleFor(x => x.Detail.acadetype).NotEmpty().WithMessage("產學類別 欄位為必要項");
            RuleFor(x => x.Detail.history).NotEmpty().WithMessage("簡歷 欄位為必要項");
            //RuleFor(x => x.OrgAttribList).Must(x => this.CheckOrgAttrib(x)).WithMessage("服務單位屬性 欄位為必要項");
            RuleFor(x => x.Detail.active).NotEmpty().WithMessage("在線or下線 欄位為必要項");
            //TeacherModel //下線:0
            When(x => x.Detail.active == 0, () =>
            {
                RuleFor(x => x.Detail.offdate).NotNull().WithMessage("在線or下線 選擇下線 下線日期 欄位為必要項");
                RuleFor(x => x.Detail.offreason).NotNull().WithMessage("在線or下線 選擇下線 下線原因 欄位為必要項");
            });
            When(x => x.Detail.offdate.HasValue, () =>
            {
                RuleFor(x => x.Detail.active).Equal(0).WithMessage("欄位 下線日期有值 在線or下線 要選擇下線");
            });
            When(x => x.Detail.offreason != null, () =>
            {
                RuleFor(x => x.Detail.active).Equal(0).WithMessage("欄位 下線原因有值 在線or下線 要選擇下線");
            });
        }

        public bool CheckTeachUnit(IList<bool> list)
        {
            bool result = false;
            if (list != null)
            {
                var res = list.Where(x => x == true).ToList();
                if (res != null && res.Count > 0) { result = true; }
            }
            return result;
        }

        /// <summary>
        /// CheckOrgAttrib
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public bool CheckOrgAttrib(IList<long?> list)
        {
            bool result = false;
            if (list == null) { return result; }

            var res = list.Where(x => x.HasValue && x.Value > 0).ToList();
            if (res != null && res.Count > 0) { result = true; }
            return result;
        }

        public bool CheckTeachReg(IList<long?> list)
        {
            bool result = false;
            if (list != null)
            {
                var res = list.Where(x => x.HasValue && x.Value > 0).ToList();
                if (res != null && res.Count > 0) { result = true; }
            }
            return result;
        }

        public bool CheckIndus(IList<MemIndusClass> list)
        {
            bool result = false;
            if (list != null)
            {
                var rs3 = list.Where(x => x.indscid.HasValue && x.indscid.Value > 0).ToList();
                if (rs3 != null && rs3.Count == 3) { result = true; }
            }
            return result;
        }

    }


    public class TeacherInfoFormValidator : BaseValidator<BaseInfoModel>
    {
        /// <summary>Validate-儲存師資基本資料</summary>
        public TeacherInfoFormValidator()
        {
            //RuleFor(x => x.MemberDetail.degree).NotEmpty().WithMessage("最高學歷 欄位為必要項");
            //RuleFor(x => x.MemberDetail.school).NotEmpty().WithMessage("畢業學校 欄位為必要項");
            //RuleFor(x => x.MemberDetail.major).NotEmpty().WithMessage("主修科目 欄位為必要項");

            RuleFor(x => x.MemberDetail.skill).NotEmpty().WithMessage("專長/領域 欄位為必要項");
            RuleFor(x => x.MemberDetail.jobcomp).NotEmpty().WithMessage("服務單位 欄位為必要項");
            RuleFor(x => x.MemberDetail.jobtitle).NotEmpty().WithMessage("職稱 欄位為必要項");

            RuleFor(x => x.TechRegList).Must(x => this.CheckTeachReg(x)).WithMessage("授課區域 欄位為必要項");
            RuleFor(x => x.IndusList).Must(x => this.CheckIndus(x)).WithMessage("可授課產業別(1-3) 為必要項");

            RuleFor(x => x.MemberDetail.group).NotEmpty().WithMessage("所屬社群 欄位為必要項");
            RuleFor(x => x.MemberDetail.acadetype).NotEmpty().WithMessage("產學類別 欄位為必要項");
            RuleFor(x => x.MemberDetail.history).NotEmpty().WithMessage("簡歷 欄位為必要項");
        }

        public bool CheckTeachReg(IList<long?> list)
        {
            bool result = false;
            if (list != null)
            {
                var res = list.Where(x => x.HasValue && x.Value > 0).ToList();
                if (res != null && res.Count > 0) { result = true; }
            }
            return result;
        }

        public bool CheckIndus(IList<MemIndusClass> list)
        {
            bool result = false;
            if (list != null)
            {
                var rs3 = list.Where(x => x.indscid.HasValue && x.indscid.Value > 0).ToList();
                if (rs3 != null && rs3.Count == 3) { result = true; }
            }
            return result;
        }

    }

}
