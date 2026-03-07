using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WDACC.Commons;
using WDACC.DataLayers;
using WDACC.Models.ViewModel.Facade;
using WDACC.Models.ViewModel;
using WDACC.Services;
using WDACC.Models.ViewModel.Member;

namespace WDACC 
{
    public class ContainerConfig
    {
        public static void Register(Container container)
        { 
            container.Register<MyBaseDAO>(Lifestyle.Scoped);
            container.Register<LogCollection>(Lifestyle.Scoped);
            container.Register<MyModelBinder>(Lifestyle.Scoped);
            container.Register<ResultMessage>(Lifestyle.Scoped);

            // 註冊 ViewModels
            container.Register<MemberViewModel>(Lifestyle.Scoped);
            container.Register<ScoreFormModel>(Lifestyle.Scoped);

            //container.Register<NewsViewModel>(Lifestyle.Scoped);
            //container.Register<CourseViewModel>(Lifestyle.Scoped);
            //container.Register<LoginViewModel>(Lifestyle.Scoped);
            container.Register<FacadeViewModel>(Lifestyle.Scoped);

            // 註冊 Service
            container.Register<FacadeService>(Lifestyle.Scoped);
            container.Register<TeacherService>(Lifestyle.Scoped);
            container.Register<CourseService>(Lifestyle.Scoped);
            container.Register<AdminService>(Lifestyle.Scoped);
            container.Register<NewsService>(Lifestyle.Scoped);
            container.Register<AccountService>(Lifestyle.Scoped);
            container.Register<LogService>(Lifestyle.Scoped);
            container.Register<FileService>(Lifestyle.Scoped);

        }

    }
}
