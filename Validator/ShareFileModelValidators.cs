using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentValidation;
using WDACC.Commons;
using WDACC.Models.ViewModel.Admin;
using WDACC.Models.ViewModel.Member;

namespace WDACC.Validator
{
    public class ShareFileModelValidator : BaseValidator<ShareFileModel>
    {
        /// <summary>
        /// action: INSERT | UPDATE
        /// </summary>
        /// <param name="action"></param>
        public ShareFileModelValidator(ActionName.EditMode action)
        {
            When(x => x.MemShare != null, () =>
            {
                RuleFor(x => x.MemShare.classid)
                    .NotNull()
                    .WithMessage("職能類別為必要項");

                RuleFor(x => x.MemShare.title)
                    .NotNull()
                    .WithMessage("教材名稱為必要項")
                    .MaximumLength(30)
                    .WithMessage("教材名稱長度超過30字元");

                RuleFor(x => x.MemShare.content)
                    .NotNull()
                    .WithMessage("檔案說明為必要項");
            });

            When(x => x != null && x.EditMode == ActionName.EditMode.CREATE, () =>
            {
                string s_SFile_type = "教材檔案";
                RuleFor(x => x.SFile)
                    .NotNull().WithMessage(string.Format("{0}為必要項", s_SFile_type))
                    .ChildRules(x =>
                        x.RuleFor(y => y.ContentType).Must(y => this.IsFileExtLegal(y)).WithMessage(string.Format("{0}為不合法的檔案類型", s_SFile_type)))
                    .ChildRules(x =>
                        x.RuleFor(y => y.ContentLength).LessThanOrEqualTo(this.GetMaxFileSizeLimit()).WithMessage(string.Format("{0}大小超過系統限制", s_SFile_type)));

                string s_IFile_type = "說明書檔案";
                RuleFor(x => x.IFile)
                    .NotNull().WithMessage(string.Format("{0}為必要項", s_IFile_type))
                    .ChildRules(x =>
                        x.RuleFor(y => y.ContentType).Must(y => this.IsFileExtLegal(y)).WithMessage(string.Format("{0}為不合法的檔案類型", s_IFile_type)))
                    .ChildRules(x =>
                        x.RuleFor(y => y.ContentLength).LessThanOrEqualTo(this.GetMaxFileSizeLimit()).WithMessage(string.Format("{0}大小超過系統限制", s_IFile_type)));

                string s_AFile_type = "授權書檔案";
                RuleFor(x => x.AFile)
                    .NotNull().WithMessage(string.Format("{0}為必要項", s_AFile_type))
                    .ChildRules(x =>
                        x.RuleFor(y => y.ContentType).Must(y => this.IsFileExtLegal(y)).WithMessage(string.Format("{0}為不合法的檔案類型", s_AFile_type)))
                    .ChildRules(x =>
                        x.RuleFor(y => y.ContentLength).LessThanOrEqualTo(this.GetMaxFileSizeLimit()).WithMessage(string.Format("{0}大小超過系統限制", s_AFile_type)));

            });

        }

    }

    public class FileUplaodModelValidator2 : BaseValidator<AdminFileUploadModel> {

        /// <summary>
        /// action: INSERT | UPDATE
        /// </summary>
        /// <param name="action"></param>
        public FileUplaodModelValidator2()
        {
            string s_SFile_type = "教材檔案";
            RuleFor(x => x.SFile)
                .NotNull().WithMessage(string.Format("{0}為必要項", s_SFile_type))
                .ChildRules(x =>
                    x.RuleFor(y => y.ContentType).Must(y => this.IsFileExtLegal(y)).WithMessage(string.Format("{0}為不合法的檔案類型", s_SFile_type)))
                .ChildRules(x =>
                    x.RuleFor(y => y.ContentLength).LessThanOrEqualTo(this.GetMaxFileSizeLimit()).WithMessage(string.Format("{0}大小超過系統限制", s_SFile_type)));

            string s_IFile_type = "說明書檔案";
            RuleFor(x => x.IFile)
                .NotNull().WithMessage(string.Format("{0}為必要項", s_IFile_type))
                .ChildRules(x =>
                    x.RuleFor(y => y.ContentType).Must(y => this.IsFileExtLegal(y)).WithMessage(string.Format("{0}為不合法的檔案類型", s_IFile_type)))
                .ChildRules(x =>
                    x.RuleFor(y => y.ContentLength).LessThanOrEqualTo(this.GetMaxFileSizeLimit()).WithMessage(string.Format("{0}大小超過系統限制", s_IFile_type)));

            string s_AFile_type = "授權書檔案";
            RuleFor(x => x.AFile)
                .NotNull().WithMessage(string.Format("{0}為必要項", s_AFile_type))
                .ChildRules(x =>
                    x.RuleFor(y => y.ContentType).Must(y => this.IsFileExtLegal(y)).WithMessage(string.Format("{0}為不合法的檔案類型", s_AFile_type)))
                .ChildRules(x =>
                    x.RuleFor(y => y.ContentLength).LessThanOrEqualTo(this.GetMaxFileSizeLimit()).WithMessage(string.Format("{0}大小超過系統限制", s_AFile_type)));

        }

    }

    public class FileUplaodModelValidator : BaseValidator<AdminFileUploadModel>
    {
        /// <summary>
        /// action: INSERT | UPDATE
        /// </summary>
        /// <param name="action"></param>
        public FileUplaodModelValidator()
        {
            When(x => x.Detail != null, () =>
            {
                RuleFor(x => x.Detail.mid)
                    .NotNull()
                    .WithMessage("講師姓名為必要項");

                RuleFor(x => x.Detail.date)
                    .NotNull()
                    .WithMessage("上傳日期為必要項");

                RuleFor(x => x.Detail.classid)
                    .NotNull()
                    .WithMessage("職能類別為必要項");

                RuleFor(x => x.Detail.title)
                    .NotNull()
                    .WithMessage("教材名稱為必要項")
                    .MaximumLength(30)
                    .WithMessage("教材名稱長度超過30字元");

                RuleFor(x => x.Detail.content)
                    .NotNull()
                    .WithMessage("檔案說明為必要項");
            });

            string s_SFile_type = "教材檔案";
            RuleFor(x => x.SFile)
                .NotNull().WithMessage(string.Format("{0}為必要項", s_SFile_type))
                .ChildRules(x =>
                    x.RuleFor(y => y.ContentType).Must(y => this.IsFileExtLegal(y)).WithMessage(string.Format("{0}為不合法的檔案類型", s_SFile_type)))
                .ChildRules(x =>
                    x.RuleFor(y => y.ContentLength).LessThanOrEqualTo(this.GetMaxFileSizeLimit()).WithMessage(string.Format("{0}大小超過系統限制", s_SFile_type)));

            string s_IFile_type = "說明書檔案";
            RuleFor(x => x.IFile)
                .NotNull().WithMessage(string.Format("{0}為必要項", s_IFile_type))
                .ChildRules(x =>
                    x.RuleFor(y => y.ContentType).Must(y => this.IsFileExtLegal(y)).WithMessage(string.Format("{0}為不合法的檔案類型", s_IFile_type)))
                .ChildRules(x =>
                    x.RuleFor(y => y.ContentLength).LessThanOrEqualTo(this.GetMaxFileSizeLimit()).WithMessage(string.Format("{0}大小超過系統限制", s_IFile_type)));

            string s_AFile_type = "授權書檔案";
            RuleFor(x => x.AFile)
                .NotNull().WithMessage(string.Format("{0}為必要項", s_AFile_type))
                .ChildRules(x =>
                    x.RuleFor(y => y.ContentType).Must(y => this.IsFileExtLegal(y)).WithMessage(string.Format("{0}為不合法的檔案類型", s_AFile_type)))
                .ChildRules(x =>
                    x.RuleFor(y => y.ContentLength).LessThanOrEqualTo(this.GetMaxFileSizeLimit()).WithMessage(string.Format("{0}大小超過系統限制", s_AFile_type)));


        }

    }

}
