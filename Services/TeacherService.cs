using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SimpleInjector;
using Turbo.MVC.Base3.DataLayers;
using WDACC.Commons;
using WDACC.DataLayers;
using WDACC.Models;
using WDACC.Models.Entities;
using WDACC.Models.StoreExt;
using WDACC.Models.ViewModel;
using Omu.ValueInjecter;
using WDACC.Models.ViewModel.Member;
using WDACC.Validator;
using FluentValidation.Results;
using FluentValidation;
using Turbo.MVC.Base3.Models;
using System.IO;
using Turbo.MVC.Base3.Commons;
using Turbo.DataLayer;
using log4net;

namespace WDACC.Services
{
    public class TeacherService
    {
        //using log4net;
        //protected static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        protected static readonly ILog LOG = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private MyBaseDAO dao;
        private Container container;
        private LogService log;
        private ResultMessage msg;
        private FileService fileService;
        private MyModelBinder modelBinder;

        public TeacherService(
            Container container,
            MyBaseDAO dao,
            LogService log,
            ResultMessage msg,
            FileService fileServcie,
            MyModelBinder modelBinder)
        {
            this.container = container;
            this.dao = dao;
            this.log = log;
            this.msg = msg;
            this.fileService = fileServcie;
            this.modelBinder = modelBinder;
        }

        /// <summary>
        /// 變更密碼
        /// </summary>
        public void ChangePassword(MemberViewModel model)
        {
            // todo
            this.msg.Message = "";

        }

        /// <summary>
        /// 師資最新消息
        /// </summary>
        /// <param name="model"></param>
        public void GetTeacherNews(MemberViewModel model)
        {
            Store param = new Store();
            param["class"] = "3";
            param["ishot"] = "Y";
            param["today"] = DateTime.Now.Date;
            param["showoff"] = 1;
            model.News.HotGrid = this.dao.QueryForListAll<Store>("News.GetNewsList", param);

            param["class"] = "3";
            param["ishot"] = "N";
            param["today"] = DateTime.Now.Date;
            param["showoff"] = 1;
            model.News.Grid = this.dao.QueryForListAll<Store>("News.GetNewsList", param);
        }

        /// <summary>
        /// 取得講師最新消息明細
        /// </summary>
        /// <param name="model"></param>
        public void GetTeacherNewsDetail(MemberViewModel model, long? newsId)
        {
            News res = this.dao.GetRow(new News { id = newsId });
            IList<NewsFile> files = this.dao.GetRowList(new NewsFile { news_id = newsId });
            if (res != null)
            {
                model.News.NewsItem = res;
                model.News.NewsFiles = files;
            }
        }

        /// <summary>
        /// 師資招募清單 
        /// </summary>
        /// <param name="model"></param>
        public void GetTeacherTcsvList(MemberViewModel model)
        {
            Store param = new Store();
            param["today"] = DateTime.Now.Date;
            model.TcsvGrid = dao.QueryForListAll<Store>("Teacher.GetTeacherTcsvList", param);
        }

        /// <summary>
        /// 師資招募清單 
        /// </summary>
        /// <param name="model"></param>
        public void GetTeacherTcsvDetail(MemberViewModel model, long? id)
        {
            Store param = new Store();
            param["id"] = id;
            model.TeaSurvey = dao.QueryForObject<Store>("Teacher.GetTeacherTcsv", param);
        }

        public void SaveTeacherSurveyClickInclement(long? id)
        {
            if (!id.HasValue) { return; }

            TeaSurvey ts = this.dao.GetRow(new TeaSurvey { id = id });
            if (ts == null) { return; }

            this.dao.BeginTransaction();
            try
            {
                this.dao.Update(new TeaSurvey { clicks = ts.clicks + 1 }, new TeaSurvey { id = id });
                this.dao.CommitTransaction();
            }
            catch (Exception ex)
            {
                LOG.Error(ex.Message, ex);
                this.dao.RollBackTransaction();
                throw ex;
            }
        }

        /// <summary>
        /// 師資料基本資料維護
        /// </summary>
        /// <param name="model"></param>
        public void GetTeacherDetail(MemberViewModel model, long? mid)
        {
            Store res = dao.QueryForObject<Store>("Teacher.GetTeacherInfo", mid);
            if (res != null)
            {
                model.TeacherInfoForm = res.ConvertTo<BaseInfoModel>();
            }
        }

        /// <summary>
        /// 讀取訊息頁
        /// </summary>
        /// <param name="model"></param>
        public void GetMessage(MemberViewModel model, string type)
        {
            switch (type)
            {
                case "Score":
                    model.ScoreHome = this.dao.GetRow(new Intro { id = 11 });
                    break;
                case "Share":
                    model.ShareHome = this.dao.GetRow(new Intro { id = 12 });
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 積分課程登錄頁, 師資績分資料
        /// </summary>
        public void ScoreRegister(MemberViewModel model)
        {
            // TODO: 取得空白積分登入表單 
            // 統計該講師目前的積分狀況
            model.ScoreForm = container.GetInstance<ScoreFormModel>();
            model.ScoreForm.PrepareAfterLoad();

            SessionModel sess = SessionModel.Get();
            long? mid = sess.UserInfo.User.user_id;

            Store param = new Store();
            param["mid"] = mid;
            model.ScoreForm.AuditList = this.dao.QueryForListAll<Store>("Course.GetScoreList", param);

        }

        /// <summary>
        /// 取得教材分享初始資料
        /// </summary>
        /// <param name="model"></param>
        public void PrepareSharePage(MemberViewModel model)
        {
            Store param = new Store();
            param["mid"] = this.GetTeacherMid();

            this.QueryShareWithMeFileList(model);   // 檔案分享清單
            model.MyShareFileList = this.dao.QueryForListAll<Store>("ShareFile.GetShareFileList", param);   // 我的檔案清單
        }

        /// <summary>
        /// 查詢檔案分享清單
        /// </summary>
        /// <param name="model"></param>
        public void QueryShareWithMeFileList(MemberViewModel model)
        {
            Store param = new Store();
            param.Collection(model.ShareWithMeParam);
            param["checked"] = "2";
            model.ShareWithMeList = this.dao.QueryForListAll<Store>("ShareFile.GetShareFileList", param);
        }

        /// <summary>
        /// 積分表
        /// </summary>
        /// <param name="model"></param>
        public void ScoreSummary(MemberViewModel model)
        {
            ScoreViewModel svm = model.ScoreView;
            Store param = new Store();
            long? mid = this.GetTeacherMid();
            param["mid"] = mid;
            param["year"] = svm.Year;
            if (svm.Year == 0) { svm.Year = DateTime.Now.Year; }
            bool isNewTeacher = false;  // 是否為近2年加入

            try
            {
                var memdtl = this.dao.GetRow(new MemDetail { mid = mid });
                // 近2年
                if (memdtl != null) { isNewTeacher = (DateTime.Now.Year - memdtl.regyear - 1) <= 2; }

                var scoreList = this.dao.GetRowList(new Memsrcs { mid = mid, ckeckd = 2, })
                    .Where(x => x.sdate.Value.Year == svm.Year);

                Store fsparam = new Store();
                fsparam["mid"] = mid;
                fsparam["checked"] = 2;
                fsparam["year"] = svm.Year;
                IList<Store> shareList = this.dao.QueryForListAll<Store>("ShareFile.GetShareFileList", fsparam);

                var meetingList = this.dao.QueryForListAll<Store>("Meeting.GetMemberMeetScoreList", param);

                svm.TeachHourClass1 = scoreList.Where(x => x.tuclass == 1).Sum(x => x.hours).GetValueOrDefault(0);
                svm.TeachHourClass2 = scoreList.Where(x => x.tuclass == 2).Sum(x => x.hours).GetValueOrDefault(0);
                svm.TeachHourClass3 = scoreList.Where(x => x.tuclass == 3).Sum(x => x.hours).GetValueOrDefault(0);
                svm.TeachHourClass4 = scoreList.Where(x => x.tuclass == 4).Sum(x => x.hours).GetValueOrDefault(0);
                svm.TeachHourClass5 = scoreList.Where(x => x.tuclass == 5).Sum(x => x.hours).GetValueOrDefault(0);

                svm.TeachScoreClass1 = svm.TeachHourClass1 * 100; // 每小時100分
                svm.TeachScoreClass2 = svm.TeachHourClass2 * 100;
                svm.TeachScoreClass3 = svm.TeachHourClass3 * 100;
                svm.TeachScoreClass4 = svm.TeachHourClass4 * 100;
                svm.TeachScoreClass5 = svm.TeachHourClass5 * 100;

                // 總分
                int classTotal = (
                    svm.TeachScoreClass1 +
                    svm.TeachScoreClass2 +
                    svm.TeachScoreClass3 +
                    svm.TeachScoreClass4 +
                    svm.TeachScoreClass5
                );

                // 社群分享講師
                var communitySpeakerList = meetingList.Where(x => new long?[] { 1, 2 }.Contains(x.Get("mclass").AsInt64()) && x.Get("mcont").AsInt64() == 2);
                svm.TeachScoreCommunitySpeaker = communitySpeakerList.Sum(x => x.Get("score").AsInt64().GetValueOrDefault(0));

                // 發展暑會議分享講師
                var sessionSpeakerList = meetingList.Where(x => new long?[] { 3 }.Contains(x.Get("mclass").AsInt64()) && x.Get("mcont").AsInt64() == 2);
                svm.TeachScoreSessionSpeaker = sessionSpeakerList.Sum(x => x.Get("score").AsInt64().GetValueOrDefault(0));

                // 分享講師積分小計
                long speakerTotal = svm.TeachScoreCommunitySpeaker + svm.TeachScoreSessionSpeaker;
                speakerTotal = speakerTotal > 900 ? 900 : speakerTotal; // 會議分享最多至900分

                // 授課積分加總
                svm.TeachTotalScore = (classTotal + speakerTotal);

                // 參與會議
                svm.CommunityScoreList = meetingList.Where(x => x.Get("mcont").AsInt64() == 1).ToList();
                svm.CommunityScoreTotal = svm.CommunityScoreList.Sum(x => x.Get("score").AsInt64().GetValueOrDefault(0));

                // 分享教材清單
                svm.ShareCount = shareList.Count();
                svm.ShareScore = svm.ShareCount * 300;  // 一份教材300分

                // 下載
                svm.DownloadCount = shareList.Select(x => x.Get("downloadcount").AsInt64()).Sum(x => x.HasValue ? x.Value : 0);
                svm.DownloadScore = svm.DownloadCount * 50; // 下載一次50分

                // 是否達到門檻
                if (!isNewTeacher)
                {
                    svm.IsTeachReach = (svm.TeachTotalScore >= 4800);   // 授課推廣 + 社群分享 + 發展署會議須 >= 4800分 
                }
                else
                {
                    svm.IsTeachReach = true;    // 近2年加入講師不計算授課推廣積分, 直接判為通過
                }
                svm.IsCommunityReach = (svm.CommunityScoreTotal >= 100);    // 社群會議參與須大於(或等於)100分
                svm.IsShareReach = (svm.ShareScore >= 300);                 // 教材分享須 >= 300
                svm.IsInWatch = !(svm.IsTeachReach && svm.IsCommunityReach && svm.IsShareReach);  // 一項未達門檻則列入觀察清單

                // 年度總積分
                svm.TotalScore = svm.TeachTotalScore + svm.CommunityScoreTotal + svm.ShareScore;

            }
            catch (Exception ex)
            {
                LOG.Warn(ex.Message, ex);
                throw ex;
            }
        }

        /// <summary>
        /// 授課花絮
        /// </summary>
        /// <param name="model"></param>
        public void GetTeachShow(MemberViewModel model)
        {
            // TODO
        }


        public void GetAllTeachShow(MemberViewModel model)
        {
            // TODO
        }

        /// <summary>
        /// 取得講師基本資料
        /// </summary>
        /// <param name="model"></param>
        public void GetTeacherInfo(MemberViewModel model)
        {
            long? mid = this.GetTeacherMid();

            model.TeacherInfoForm.MemberDetail = this.dao.GetRow(new MemDetail { mid = mid });
            model.TeacherInfoForm.IndusList = new List<MemIndusClass>();
            var indList = this.dao.GetRowList(new MemIndusClass { mid = mid });
            var regList = this.dao.GetRowList(new MemTechReg { mid = mid });

            for (int i = 0; i < 10; i++)
            {
                var item = indList.Where(x => x.order == i + 1).FirstOrDefault();
                if (item == null)
                {
                    model.TeacherInfoForm.IndusList.Add(new MemIndusClass { mid = mid, order = System.Convert.ToByte(i + 1) });
                }
                else
                {
                    model.TeacherInfoForm.IndusList.Add(item);
                }
            }

            model.TeacherInfoForm.TechRegList = regList.Select(x => x.cityid).ToList();

            model.TeacherInfoForm.PrepareAfterLoad();
        }

        /// <summary>
        /// 儲存師資基本資料
        /// </summary>
        /// <param name="model"></param>
        public void SaveTeacherInfo(MemberViewModel model)
        {
            model.TeacherInfoForm.PrepareBeforeSave();

            //Validate-儲存師資基本資料
            ValidationResult result = new TeacherInfoFormValidator().Validate(model.TeacherInfoForm);
            if (!result.IsValid)
            {
                throw new ValidationException(result.Errors);
            }

            MemDetail detail = model.TeacherInfoForm.MemberDetail;
            var reg = model.TeacherInfoForm.TechRegList;
            var inds = model.TeacherInfoForm.IndusList;
            var mid = this.GetTeacherMid();
            MemDetail memdtl = this.dao.GetRow(new MemDetail { mid = mid });
            detail.degree = memdtl.degree;//最高學歷
            detail.school = memdtl.school;//畢業學校
            detail.major = memdtl.major;//主修科目

            this.dao.BeginTransaction();
            try
            {
                foreach (var i in Enumerable.Range(0, 3))
                {
                    this.dao.Delete(new MemIndusClass { mid = mid, id = inds[i].id, order = (byte)(i + 1) });
                }

                foreach (var i in Enumerable.Range(0, 3))
                {
                    if (inds[i].indscid.HasValue)
                    {
                        inds[i].order = (byte)(i + 1);
                        this.dao.Insert(inds[i]);
                    }
                }

                this.dao.Delete(new MemTechReg { mid = mid });

                foreach (var item in reg)
                {
                    this.dao.Insert(new MemTechReg { mid = mid, cityid = item.Value });
                }

                //若無新值，就清空？
                ClearFieldMap cfm = new ClearFieldMap();
                cfm.Add("nickname");
                cfm.Add("gender");
                cfm.Add("birthdat");
                cfm.Add("zipcode");
                cfm.Add("address");
                cfm.Add("phone");
                cfm.Add("mobile");
                cfm.Add("fax");
                cfm.Add("website");
                cfm.Add("email");
                cfm.Add("jemail");
                cfm.Add("facebook");

                this.dao.Update(detail, new MemDetail { mid = mid }, cfm);

                this.dao.CommitTransaction();
            }
            catch (Exception ex)
            {
                LOG.Warn(ex.Message, ex);
                this.dao.RollBackTransaction();
                throw ex;
            }

        }

        /// <summary>
        /// 儲存講師可授課產業
        /// </summary>
        /// <param name="model"></param>
        public void SaveTeacherIndClass(MemberViewModel model)
        {
            var inds = model.TeacherInfoForm.IndusList;
            var mid = this.GetTeacherMid();

            this.dao.BeginTransaction();
            try
            {

                foreach (var i in Enumerable.Range(3, 7))
                {
                    this.dao.Delete(new MemIndusClass { mid = mid, id = inds[i].id, order = (byte)(i + 1) });
                }

                foreach (var i in Enumerable.Range(3, 7))
                {
                    if (inds[i].indscid.HasValue)
                    {
                        inds[i].mid = mid;
                        inds[i].order = (byte)(i + 1);
                        this.dao.Insert(inds[i]);
                    }
                }

                this.dao.CommitTransaction();
            }
            catch (Exception ex)
            {
                LOG.Warn(ex.Message, ex);
                this.dao.RollBackTransaction();
                throw ex;
            }

        }

        /// <summary>
        /// 取得積分課程
        /// </summary>
        /// <param name="model"></param>
        public void ScoreCourseDetail(MemberViewModel model)
        {

        }

        /// <summary>
        /// 積分課程登錄
        /// </summary>
        /// <param name="model"></param>
        public void SaveScore(MemberViewModel model)
        {
            // 檢核 資料 // 檢核 上傳檔案
            model.ScoreForm.PrepareBeforeSave();
            var validResult = new ScoreFormValidator().Validate(model.ScoreForm);
            if (!validResult.IsValid)
            {
                throw new ValidationException(validResult.Errors);
            }

            Tuple<string, string> uploadTuple = null;
            try
            {
                // 處理上傳檔案
                if (model.ScoreForm.File != null)
                {
                    uploadTuple = fileService.UploadScoreFile(model.ScoreForm.File, "1");
                }
            }
            catch (Exception ex)
            {
                LOG.Warn(ex.Message, ex);
                throw ex;
            }

            this.dao.BeginTransaction();
            try
            {

                if (model.ScoreForm.Score.id.HasValue)
                {
                    if (uploadTuple != null)
                    {
                        long? ckid = model.ScoreForm.CkFile.id;
                        CKFile ck = this.dao.GetRow(new CKFile { id = ckid });
                        if (ck != null)
                        {
                            ck.id = null;
                            ck.filname = uploadTuple.Item1;
                            this.dao.Update(ck, new CKFile { id = ckid });
                        }
                        else
                        {
                            var saveck = new CKFile
                            {
                                mid = this.GetTeacherMid(),
                                filname = uploadTuple.Item1,
                                mmsid = model.ScoreForm.Score.id
                            };
                            this.dao.Insert(saveck);
                        }
                    }

                    // update
                    model.ScoreForm.Score.ckeckd = 1;   // 送審
                    this.dao.Update(model.ScoreForm.Score, new Memsrcs { id = model.ScoreForm.Score.id });
                    // add func log
                }
                else
                {
                    var mid = this.GetTeacherMid();
                    // Insert
                    Memsrcs srcs = model.ScoreForm.Score;
                    srcs.mid = mid;
                    srcs.days = 0;
                    srcs.ckeckd = 1;
                    srcs.comment = "";
                    long? newSeq = this.dao.Insert(srcs);

                    CKFile ck = new CKFile
                    {
                        mmsid = newSeq,
                        mid = mid,
                        filname = uploadTuple.Item1,
                    };
                    this.dao.Insert(ck);

                }

                this.dao.CommitTransaction();
            }
            catch (Exception ex)
            {
                LOG.Warn(ex.Message, ex);
                this.dao.RollBackTransaction();
                throw ex;
            }
        }

        /// <summary>
        /// 儲存前台顯示狀況
        /// </summary>
        /// <param name="model"></param>
        public void SaveScoreShowoff(MemberViewModel model)
        {
            this.dao.BeginTransaction();
            try
            {
                foreach (var item in model.ScoreGrid)
                {
                    // 註: 顯示狀態不須審核
                    this.dao.Update(new Memsrcs { showoff = item.showoff }, new Memsrcs { id = item.id });
                }
                this.dao.CommitTransaction();
            }
            catch (Exception ex)
            {
                LOG.Warn(ex.Message, ex);
                this.dao.RollBackTransaction();
                throw ex;
            }
        }

        /// <summary>
        /// 取得積分課程清單
        /// </summary>
        /// <param name="model"></param>
        public void GetScoreCourseList(MemberViewModel model)
        {
            SessionModel sess = SessionModel.Get();
            long? mid = sess.UserInfo.User.user_id;

            Store param = new Store();
            param.Collection(model.ScoreGridParam);
            param["mid"] = mid;
            IList<Store> list = this.dao.QueryForListAll<Store>("Course.GetScoreList", param);
            model.ScoreGrid = new List<Memsrcs>();

            foreach (var item in list)
            {
                Memsrcs m = item.ConvertTo<Memsrcs>();
                model.ScoreGrid.Add(m);
            }
        }

        /// <summary>
        /// 取得積分單筆課程明細
        /// </summary>
        /// <param name="model"></param>
        /// <param name="lid"></param>
        public void GetScoreCourseDetail(MemberViewModel model, long? lid)
        {
            if (lid.HasValue)
            {
                model.ScoreForm.Score = this.dao.GetRow(new Memsrcs { id = lid });
                model.ScoreForm.CkFile = this.dao.GetRow(new CKFile { mmsid = lid });
                model.ScoreForm.EditMode = "UPDATE";
                model.ScoreForm.PrepareAfterLoad();
            }
        }

        /// <summary>
        /// 取得所有/該名師資料分享教材
        /// </summary>
        /// <param name="model"></param>
        public void GetShareFileList(MemberViewModel model)
        {
            model.Grid = dao.QueryForListAll<Store>("Teacher.GetShareFileList", model);
        }

        /// <summary>
        /// 教材分享明細
        /// </summary>
        /// <param name="model"></param>
        public void GetShareFileDetail(MemberViewModel model, long? sfid)
        {
            model.ShareFileForm.MemShare = this.dao.GetRow(new Memshare { id = sfid });
        }

        /// <summary>
        /// 教材分享 儲存
        /// </summary>
        /// <param name="model"></param>
        public void SaveShareFileDetail(MemberViewModel model)
        {
            ValidationResult result = new ShareFileModelValidator(model.ShareFileForm.EditMode).Validate(model.ShareFileForm);
            if (!result.IsValid) { throw new ValidationException(result.Errors); }

            ActionName.EditMode editMode = model.ShareFileForm.EditMode;
            long? mid = this.GetTeacherMid();

            Tuple<string, string> sres = null;
            Tuple<string, string> ires = null;
            Tuple<string, string> ares = null;

            bool flag_NG = false;

            if (editMode == ActionName.EditMode.CREATE)
            {
                try
                {
                    sres = this.fileService.UploadShareFile(model.ShareFileForm.SFile, "");
                    ires = this.fileService.UploadShareFile(model.ShareFileForm.IFile, "-1");
                    ares = this.fileService.UploadShareFile(model.ShareFileForm.AFile, "-2");
                }
                catch (Exception ex)
                {
                    LOG.Warn(ex.Message, ex);
                    flag_NG = true; //throw ex;
                }

                if (!flag_NG)
                {
                    this.dao.BeginTransaction();
                    try
                    {
                        var saveModel = model.ShareFileForm.MemShare;
                        saveModel.mid = mid;
                        saveModel.filename = sres.Item1;
                        saveModel.ifilename = ires.Item1;
                        saveModel.afilename = ares.Item1;
                        saveModel.@checked = 1;
                        saveModel.date = DateTime.Now;
                        saveModel.dlok = 0;
                        saveModel.reason = "";
                        this.dao.Insert(saveModel);

                        this.dao.CommitTransaction();
                    }
                    catch (Exception ex)
                    {
                        LOG.Warn(ex.Message, ex);
                        this.dao.RollBackTransaction();
                        flag_NG = true; //throw ex;
                    }
                }
            }
            else if (editMode == ActionName.EditMode.UPDATE)
            {
                var current = model.ShareFileForm;
                var saveModel = model.ShareFileForm.MemShare;

                Tuple<string, string> stup = null;
                Tuple<string, string> itup = null;
                Tuple<string, string> atup = null;

                try
                {
                    if (current.SFile != null)
                    {
                        stup = this.fileService.UploadShareFile(current.SFile, "");
                        if (stup != null) { saveModel.filename = stup.Item1; }
                    }

                    if (current.IFile != null)
                    {
                        itup = this.fileService.UploadShareFile(current.IFile, "-1");
                        if (itup == null) { saveModel.ifilename = itup.Item1; }
                    }

                    if (current.AFile != null)
                    {
                        atup = this.fileService.UploadShareFile(current.AFile, "-2");
                        if (atup == null) { saveModel.afilename = atup.Item1; }
                    }
                }
                catch (Exception ex)
                {
                    LOG.Warn(ex.Message, ex);
                    flag_NG = true; //throw ex;
                }

                if (!flag_NG && saveModel.id.HasValue)
                {
                    this.dao.BeginTransaction();
                    try
                    {
                        saveModel.@checked = 1;  // 送審
                        this.dao.Update(model.ShareFileForm.MemShare, new Memshare { id = saveModel.id });
                        this.dao.CommitTransaction();
                    }
                    catch (Exception ex)
                    {
                        LOG.Warn(ex.Message, ex);
                        this.dao.RollBackTransaction();
                        flag_NG = true; //throw ex;
                    }
                }
            }

            if (flag_NG)
            {
                //LOG.Warn(ex.Message, ex);
                //this.dao.RollBackTransaction();
                //this.dao.BeginTransaction();
                //this.log.AddFileLog();
                //this.dao.CommitTransaction();

                // 儲存失敗，刪除上傳檔案
                if (editMode == ActionName.EditMode.CREATE)
                {
                    var server = this.modelBinder.controllerContext.HttpContext.Server;
                    if (sres != null)
                    {
                        File.Delete(server.MapPath(sres.Item2));
                    }
                    if (ires != null)
                    {
                        File.Delete(server.MapPath(ires.Item2));
                    }
                    if (ares != null)
                    {
                        File.Delete(server.MapPath(ares.Item2));
                    }
                }
                LOG.Error("#SaveShareFileDetail - 儲存失敗，刪除上傳檔案!");
                throw new Exception("儲存失敗，刪除上傳檔案!");
            }
        }

        /// <summary>
        /// 意見回饋儲存 
        /// </summary>
        /// <param name="model"></param>
        public void SaveFeedback(MemberViewModel model)
        {
            // 輸入檢核
            ValidationResult vdr = new FeedbackSaveValidator().Validate(model.Feedback);
            if (!vdr.IsValid)
            {
                throw new ValidationException(vdr.Errors);
            }

            var saveModel = model.Feedback.Opinion;
            saveModel.date = DateTime.Now;
            saveModel.mid = this.GetTeacherMid();
            saveModel.answer = 1;  // 1: 未回覆， 2:已回覆
            saveModel.rdate = DateTime.Now;

            this.dao.BeginTransaction();
            try
            {
                this.dao.Insert(saveModel);
                this.dao.CommitTransaction();
            }
            catch (Exception ex)
            {
                LOG.Warn(ex.Message, ex);
                this.dao.RollBackTransaction();
                throw ex;
            }
        }

        /// <summary>
        /// 講師重新設定密碼
        /// </summary>
        /// <param name="model"></param>
        public void SavePassword(MemberViewModel model)
        {
            long? mid = this.GetTeacherMid();
            ValidationResult vdr = new PasswordResetValidator(this.dao, mid).Validate(model.PasswordForm);
            if (!vdr.IsValid)
            {
                throw new ValidationException(vdr.Errors);
            }

            Member oldMem = this.dao.GetRow(new Member { id = mid });
            if (oldMem == null) { return; }

            DateTime now = DateTime.Now;
            var member = new Member
            {
                password = MyCommonUtil.ComputeHash(model.PasswordForm.Password),  // 寫入新設定的加密密碼 
                pwdupdate = now
            };

            PasswordHistory ph = new PasswordHistory
            {
                mid = mid,
                password = oldMem.password,  // 記錄目前使用的密碼
                modifydate = now
            };

            this.dao.BeginTransaction();
            try
            {
                this.dao.Update(member, new Member { id = mid });
                this.dao.Insert(ph);

                this.dao.CommitTransaction();
            }
            catch (Exception ex)
            {
                LOG.Warn(ex.Message, ex);
                this.dao.RollBackTransaction();
                throw ex;
            }
        }

        /// <summary>
        /// 刪除積分課程
        /// </summary>
        /// <param name="model"></param>
        public void RemoveScoreCourse(MemberViewModel model, long? id)
        {
            if (!id.HasValue)
            {
                throw new ArgumentException("未提供積分課程序號");
            }

            this.dao.BeginTransaction();
            try
            {
                this.dao.Delete(new Memsrcs { id = id });
                this.dao.Delete(new CKFile { mmsid = id });
                this.dao.CommitTransaction();
            }
            catch (Exception ex)
            {
                LOG.Error(ex.Message, ex);
                this.dao.RollBackTransaction();
                throw ex;
            }
        }

        /// <summary>
        /// 取得師資明細 
        /// </summary>
        /// <param name="detail"></param>
        public void GetTeacherDetail(TeacherDetail detail, long? mid)
        {
            if (mid.HasValue)
            {
                MemDetail res = this.dao.GetRow(new MemDetail { mid = detail.MemDetail.mid });
                if (res != null)
                {
                    detail.MemDetail.InjectFrom(res);
                }
            }
        }

        /// <summary>
        /// 取得師資授課花絮維護資料
        /// </summary>
        public void GetTeacherShowDetail(MemberViewModel model)
        {
            long? mid = this.GetTeacherMid();
            var list = this.dao.GetRowList(new TeacherShow { mid = mid });
            var imgList = list.Where(x => x.@class == 1).ToList();
            var videoList = list.Where(x => x.@class == 2).ToList();

            if (imgList != null)
            {
                for (int i = 0; i < imgList.Count; i++)
                {
                    model.TeachShow.ImageGrid[i].InjectFrom(imgList[i]);
                }
            }

            if (videoList != null)
            {
                for (int i = 0; i < videoList.Count; i++)
                {
                    model.TeachShow.VideoGrid[i].InjectFrom(videoList[i]);
                }
            }
        }

        /// <summary>
        /// 儲存師資授課花絮
        /// </summary>
        /// <param name="model"></param>
        public void SaveTeacherShowDetail(MemberViewModel model)
        {
            long? mid = this.GetTeacherMid().GetValueOrDefault(-1);
            var ts = model.TeachShow;

            this.dao.BeginTransaction();
            try
            {
                switch (ts.Kind)
                {
                    case "IMAGE":
                        IList<TeacherShow> oldTeachShowList = this.dao.GetRowList(new TeacherShow { mid = mid, @class = 1 });

                        for (int i = 0; i < ts.ImageGrid.Count; i++)
                        {
                            var item = ts.ImageGrid[i];
                            var file = ts.ImageFiles[i];

                            if (!string.IsNullOrEmpty(item.title))
                            {
                                item.mid = mid;
                                if (file != null)
                                {
                                    Tuple<string, string> saveFile = this.fileService.UploadTeachShowFile(file, (i + 1).ToString());
                                    if (saveFile != null && !string.IsNullOrEmpty(saveFile.Item1))
                                    {
                                        item.content = saveFile.Item1;
                                    }

                                    if (!item.id.HasValue)
                                    {
                                        item.@class = 1;
                                        item.info = "";
                                        this.dao.Insert(item);
                                    }
                                    else
                                    {
                                        item.@class = 1;
                                        this.dao.Update(item, new TeacherShow { id = item.id });
                                    }
                                }
                                else
                                {
                                    if (item.id.HasValue)
                                    {
                                        item.content = null;
                                        this.dao.Update(item, new TeacherShow { id = item.id });
                                        oldTeachShowList = oldTeachShowList.Where(x => x.id != item.id).ToList();
                                    }
                                }
                            }
                            else
                            {
                                if (item.id.HasValue)
                                {
                                    this.dao.Delete(new TeacherShow { id = item.id });
                                }
                            }
                        }

                        this.dao.CommitTransaction();

                        foreach (var item in oldTeachShowList)
                        {
                            this.fileService.RemoveTeachShowFile(item.content);
                        }
                        break;

                    case "VIDEO":
                        this.dao.Delete(new TeacherShow { mid = mid, @class = 2 });
                        foreach (var item in ts.VideoGrid)
                        {
                            if (item.content != null && item.title != null && item.info != null)
                            {
                                item.@class = 2;
                                item.mid = mid;
                                this.dao.Insert(item);
                            }
                        }
                        break;
                }
                this.dao.CommitTransaction();
            }
            catch (Exception ex)
            {
                LOG.Error(ex.Message, ex);
                this.dao.RollBackTransaction();
                throw ex;
            }
        }

        /// <summary>
        /// 取得師資授課花絮
        /// </summary>
        /// <param name="detail"></param>
        public void GetTeacherShow(TeacherDetail detail, long? mid)
        {
            if (mid.HasValue)
            {
                IList<TeacherShow> list = this.dao.GetRowList(new TeacherShow { mid = mid });
                detail.TeacherShow = list;
            }
        }

        /// <summary>
        /// 取得師資授課區域
        /// </summary>
        /// <param name="detail"></param>
        /// <param name="mid"></param>
        public void GetTeacherTechRegion(TeacherDetail detail, long? mid)
        {
            if (mid.HasValue)
            {
                detail.MemTechReg = this.dao.QueryForListAll<Store>("Teacher.GetTechRegion", mid);
            }
        }

        /// <summary>
        /// 取得師資授課產業類別
        /// </summary>
        /// <param name="detail"></param>
        /// <param name="mid"></param>
        public void GetTeacherTechIndClass(TeacherDetail detail, long? mid)
        {
            if (mid.HasValue)
            {
                detail.IndClass = this.dao.QueryForListAll<Store>("Teacher.GetTechIndClass", mid);
            }
        }

        /// <summary>
        /// 取得師資授課時數
        /// </summary>
        /// <param name="detail"></param>
        /// <param name="mid"></param>
        public void GetTeacherCourseHours(TeacherDetail detail, long? mid)
        {
            if (mid.HasValue)
            {
                detail.CourseHours = this.dao.QueryForObject<Store>("Teacher.GetTeacherCourseHours", mid);
            }
        }

        /// <summary>
        /// 取得曾授課課程
        /// </summary>
        /// <param name="detail"></param>
        /// <param name="mid"></param>
        public void GetTeacherCourseList(TeacherDetail detail, long? mid)
        {
            if (mid.HasValue)
            {
                detail.CourseList = this.dao.QueryForList<Store>("Teacher.GetTeacherCourseList", mid);
            }
        }

        public IList<Store> GetZipCode()
        {
            return this.dao.QueryForListAll<Store>("KeyMap.CityRegionZip", null);
        }

        public long? GetTeacherMid()
        {
            long? result = null;
            try
            {
                SessionModel sess = SessionModel.Get();
                result = sess.UserInfo.User.user_id;
            }
            catch (Exception ex)
            {
                LOG.Error(ex.Message, ex);
                throw new LoginExceptions("登入已過期");
            }
            return result;
        }

    }
}

