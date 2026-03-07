using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentValidation;
using WDACC.Models.Entities;
using WDACC.Models.ViewModel.Member;

namespace WDACC.Validator
{
    /// <summary>
    /// 新增檢核
    /// </summary>
    public class FeedbackSaveValidator : BaseValidator<FeedbackModel>
    {
        /// <summary>
        /// 意見回饋新增檢核
        /// </summary>
        public FeedbackSaveValidator()
        {
            RuleFor(x => x.Opinion).NotNull().WithMessage("必填欄位未填");
            When(x => x != null, () =>
            {
                RuleFor(x => x.Opinion.email).NotEmpty().WithMessage("聯絡信箱為必要項");
                RuleFor(x => x.Opinion.mobile).NotEmpty().WithMessage("聯絡電話為必要項");
                RuleFor(x => x.Opinion.content).NotEmpty().WithMessage("諮詢/意見內容為必要項");
                RuleFor(x => x.IsAgree).Must(x => x == true).WithMessage("請勾選同意產業人才投資方案計畫專案辦公室使用個人資料之選項");
            });
        }
    }

    /// <summary>
    /// 管理者回覆檢核
    /// </summary>
    public class FeedbackAnswerValidator : BaseValidator<Opinion>
    {
        public FeedbackAnswerValidator()
        {

        }
    }

}

