using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentValidation;
using WDACC.Models.Entities;
using WDACC.Models.ViewModel.Member;

namespace WDACC.Validator
{

    //public class ScoreValidator : AbstractValidator<Memsrcs>
    public class ScoreValidator : BaseValidator<Memsrcs>
    {
        public ScoreValidator()
        {
            RuleFor(x => x.tuclass)
                .NotEmpty().WithMessage("開課單位為必要項");

            RuleFor(x => x.tuname)
                .NotEmpty().WithMessage("開課單位名稱為必要項")
                .MaximumLength(100).WithMessage("開課單位名稱長度超過100字元");

            RuleFor(x => x.tcclass)
                .NotEmpty().WithMessage("授課單位為必要項");

            RuleFor(x => x.tcname)
                .NotEmpty().WithMessage("課程名稱為必要項")
                .MaximumLength(100).WithMessage("課程名稱長度超過100字元");

            RuleFor(x => x.hours)
                .NotEmpty().WithMessage("授課時數為必要項,請填寫合理範圍的阿拉伯數字")
                .GreaterThan((byte)0).WithMessage("授課時數,必須大於 0。");

        }
    }

    /// <summary>
    /// 積分課程登錄
    /// </summary>
    public class ScoreFormValidator : BaseValidator<ScoreFormModel>
    {
        public ScoreFormValidator()
        {

            RuleFor(x => x.Score).NotNull().WithMessage("未輸入資料");

            When(x => x.Score != null, () =>
            {
                RuleFor(x => x.Score.tuclass)
                    .NotEmpty().WithMessage("開課單位為必要項");

                RuleFor(x => x.Score.tuname)
                    .NotEmpty().WithMessage("開課單位名稱為必要項")
                    .MaximumLength(100).WithMessage("開課單位名稱長度超過100字元");

                RuleFor(x => x.Score.tcclass)
                    .NotEmpty().WithMessage("授課單位為必要項");

                RuleFor(x => x.Score.tcname)
                    .NotEmpty().WithMessage("課程名稱為必要項")
                    .MaximumLength(100).WithMessage("課程名稱長度超過100字元");

                RuleFor(x => x.Score.hours)
                    .NotEmpty().WithMessage("授課時數為必要項,請填寫合理範圍的阿拉伯數字")
                    .GreaterThan((byte)0).WithMessage("授課時數,必須大於 0。");
            });

            When(x => x.EditMode == "CREATE", () =>
            {
                RuleFor(x => x.File)
                    .NotNull().WithMessage("佐證資料為必要項");
            });

            When(x => x.File != null, () =>
            {
                RuleFor(x => x.File.ContentLength)
                    .LessThanOrEqualTo(this.GetMaxFileSizeLimit()).WithMessage("上傳檔案容量過大");

                RuleFor(x => x.File.ContentType)
                    .Must(x => this.IsFileExtLegal(x)).WithMessage("不合法的檔案類型");
            });
        }

    }

}
