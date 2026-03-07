using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WDACC.Models.Entities;

namespace WDACC.Models.ViewModel.Member
{
    public class TeacherShowModel
    {
        public TeacherShowModel()
        {
            this.ImageGrid = new List<TeacherShow> {
                new TeacherShow(),
                new TeacherShow(),
                new TeacherShow(),
                new TeacherShow(),
                new TeacherShow()
            };

            this.VideoGrid = new List<TeacherShow> {
                new TeacherShow(),
                new TeacherShow(),
                new TeacherShow()
            };
        }

        /// <summary>
        /// IMAGE | VIDEO 
        /// </summary>
        public string Kind { get; set; }

        public IList<TeacherShow> ImageGrid { get; set; }

        public IList<TeacherShow> VideoGrid { get; set; }

        //public HttpFileCollectionBase ImageFiles { get; set; }
        public IList<HttpPostedFileBase> ImageFiles { get; set; }

    }
}
