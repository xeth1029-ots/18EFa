using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WDACC.Models.Entities;
using WDACC.Models.ViewModel.Admin;
using FluentValidation;

namespace WDACC.Validator
{
    public class MeetValidator : BaseValidator<Meeting>
    {
        public MeetValidator()
        {
            RuleFor(x => x.date)
                .NotNull()
                .WithMessage("會議日期為必要項");

            RuleFor(x => x.metname)
                .NotNull()
                .WithMessage("會議名稱為必要項");

            RuleFor(x => x.mclass)
                .NotNull()
                .WithMessage("會議類型為必要項");

            RuleFor(x => x.metloc)
                .NotNull()
                .WithMessage("會議地點為必要項");
        }
    }

}
