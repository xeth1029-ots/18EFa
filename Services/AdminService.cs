using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Turbo.MVC.Base3.Commons;
using WDACC.DataLayers;
using WDACC.Models;
using WDACC.Models.Entities;
using WDACC.Models.StoreExt;
using WDACC.Models.ViewModel;
using WDACC.Models.ViewModel.Admin;
using WDACC.Validator;
using FluentValidation;
using FluentValidation.Results;
using static WDACC.Validator.AdminPasswordResetValidator;
using WDACC.Commons;
using Turbo.DataLayer;
using System.Text.RegularExpressions;
using log4net;
using System.Collections;
using System.Web.Mvc;

namespace WDACC.Services
{
    public class AdminService
    {
        private static readonly ILog LOG = LogManager.GetLogger(typeof(AdminService));
        private MyBaseDAO dao;
        private FileService fileService;

        public AdminService(MyBaseDAO dao, FileService fileService)
        {
            this.dao = dao;
            this.fileService = fileService;
        }

        /// <summary>
        /// 管理者首頁
        /// </summary>
        /// <param name="model"></param>
        public void HomeIndex(AdminViewModel model)
        {

        }

        /// <summary>
        /// 管理員 最新消息
        /// </summary>
        /// <param name="model"></param>
        public void GetAdminNewsList(AdminViewModel model)
        {
            this.GetAdminNewsTab1(model);
            this.dao.SetPageInfo(null, "1");  // 清除rid以重新查詢
            this.GetAdminNewsTab2(model);
            this.dao.SetPageInfo(null, "1");  // 清除rid以重新查詢
            this.GetAdminNewsTab3(model);
            this.dao.SetPageInfo(null, "1");  // 清除rid以重新查詢
            this.GetAdminNewsTab4(model);
            this.dao.SetPageInfo(null, "1");  // 清除rid以重新查詢
        }

        public void GetAdminNewsTab1(AdminViewModel model)
        {
            Store param = new Store();
            param["showoff"] = 1;
            param["class"] = 2;
            model.NewsModel.Grid1 = this.dao.QueryForList<Store>("Admin.GetNewsList", param);
            if (model.NewsModel.Grid1 == null)
            {
                model.NewsModel.Grid1 = new List<Store>();
            }
            model.NewsModel.TabPaging1.rid = this.dao.ResultID;
            model.NewsModel.TabPaging1.PagingInfo.Total = this.dao.TotalRecords;
        }
        public void GetAdminNewsTab2(AdminViewModel model)
        {
            Store param = new Store();
            param["showoff"] = 1;
            param["class"] = 3;
            model.NewsModel.Grid2 = this.dao.QueryForList<Store>("Admin.GetNewsList", param);
            if (model.NewsModel.Grid2 == null)
            {
                model.NewsModel.Grid2 = new List<Store>();
            }
            model.NewsModel.TabPaging2.rid = this.dao.ResultID;
            model.NewsModel.TabPaging2.PagingInfo.Total = this.dao.TotalRecords;
        }
        public void GetAdminNewsTab3(AdminViewModel model)
        {
            Store param = new Store();
            param["showoff"] = 1;
            param["class"] = 4;
            model.NewsModel.Grid3 = this.dao.QueryForList<Store>("Admin.GetNewsList", param);
            if (model.NewsModel.Grid3 == null)
            {
                model.NewsModel.Grid1 = new List<Store>();
            }
            model.NewsModel.TabPaging3.rid = this.dao.ResultID;
            model.NewsModel.TabPaging3.PagingInfo.Total = this.dao.TotalRecords;
        }

        public void GetAdminNewsTab4(AdminViewModel model)
        {
            Store param = new Store();
            param["showoff"] = 0;
            model.NewsModel.Grid4 = this.dao.QueryForList<Store>("Admin.GetNewsList", param);
            if (model.NewsModel.Grid4 == null)
            {
                model.NewsModel.Grid4 = new List<Store>();
            }
            model.NewsModel.TabPaging4.rid = this.dao.ResultID;
            model.NewsModel.TabPaging4.PagingInfo.Total = this.dao.TotalRecords;
        }

        /// <summary>
        /// 儲存系統設定資料
        /// </summary>
        /// <param name="model"></param>
        public void SaveIntro(AdminViewModel model)
        {
            Intro dtl = null;
            if (model.SystemModel.Detail.id.HasValue)
            {
                dtl = model.SystemModel.Detail;
                dtl.content = HttpUtility.HtmlDecode(dtl.content);
            }
            else
            {
                ArgumentException ex = new ArgumentException("無Intro.id");
                LOG.Error(ex.Message, ex);
                throw ex;
            }

            this.dao.BeginTransaction();
            try
            {
                this.dao.Update(dtl, new Intro { id = dtl.id });
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
        /// 講師資料設定
        /// </summary>
        /// <param name="model"></param>
        public void SaveTeacherInfo(AdminViewModel model)
        {
            model.TeacherModel.PrepareBeforeSave();

            ValidationResult result = new TeacherInfoValidator().Validate(model.TeacherModel);
            if (!result.IsValid)
            {
                ValidationException ex = new ValidationException(result.Errors);
                LOG.Error(ex.Message, ex);
                throw ex;
            }

            var detail = model.TeacherModel.Detail;
            var reg = model.TeacherModel.TechRegList; //授課區域
            var inds = model.TeacherModel.IndusList; //可授課產業別
            var orgatt = model.TeacherModel.OrgAttribList; //服務單位屬性
            var mid = detail.mid;

            this.dao.BeginTransaction();
            try
            {
                //可授課產業別
                foreach (var i in Enumerable.Range(0, 3))
                {
                    if (inds[i].indscid.HasValue)
                    {
                        this.dao.Delete(new MemIndusClass { mid = mid, id = inds[i].id, order = (byte)(i + 1) });
                    }
                }
                //可授課產業別
                if (inds != null && inds.Count > 0)
                {
                    foreach (var i in Enumerable.Range(0, 3))
                    {
                        if (inds[i].indscid.HasValue)
                        {
                            inds[i].mid = mid;
                            inds[i].order = (byte)(i + 1);
                            this.dao.Insert(inds[i]);
                        }
                    }
                }
                //授課區域
                this.dao.Delete(new MemTechReg { mid = mid });
                if (reg != null)
                {
                    foreach (var item in reg)
                    {
                        this.dao.Insert(new MemTechReg { mid = mid, cityid = item });
                    }
                }
                //服務單位屬性
                this.dao.Delete(new MemOrgAttrib { mid = mid });
                if (orgatt != null)
                {
                    foreach (var item in orgatt)
                    {
                        this.dao.Insert(new MemOrgAttrib { mid = mid, attrid = item });
                    }
                }

                ClearFieldMap cfm = new ClearFieldMap();
                cfm.Add("nickname");
                cfm.Add("regyear"); //加入年份
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
                cfm.Add("offreason"); //下線原因
                cfm.Add("offdate"); //下線日期

                this.dao.Update(detail, new MemDetail { mid = mid }, cfm);

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
        /// 儲存講師可授課產業別
        /// </summary>
        /// <param name="model"></param>
        public void SaveTeachIndClass(AdminViewModel model)
        {
            var inds = model.TeacherModel.IndusList;
            var mid = model.TeacherModel.Detail.mid;

            this.dao.BeginTransaction();
            try
            {
                foreach (var i in Enumerable.Range(3, 7))
                {
                    this.dao.Delete(new MemIndusClass { mid = mid, id = inds[i].id, order = (byte)(i + 1) });
                }

                foreach (var i in Enumerable.Range(3, 7))
                {
                    inds[i].mid = mid;
                    inds[i].order = (byte)(i + 1);
                    this.dao.Insert(inds[i]);
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
        /// 儲存講師登入密碼
        /// </summary>
        /// <param name="model"></param>
        public void SaveTeacherPassword(AdminViewModel model)
        {
            // 資料檢核
            var result = new AdminPasswordResetValidator
                (this.dao, model.TeacherModel.Detail.mid).Validate(model.TeacherModel);

            if (!result.IsValid)
            {
                FluentValidation.ValidationException ex = new FluentValidation.ValidationException(result.Errors);
                LOG.Warn(ex.Message, ex);
                throw ex;
            }

            var teach = model.TeacherModel;
            MemDetail dtl = model.TeacherModel.Detail;
            Member oldMem = this.dao.GetRow(new Member { id = dtl.mid });

            this.dao.BeginTransaction();
            try
            {
                if (oldMem != null)
                {
                    PasswordHistory ph = new PasswordHistory();
                    ph.mid = dtl.mid;
                    ph.password = oldMem.password;
                    ph.modifydate = oldMem.pwdupdate;
                    this.dao.Insert(ph);

                    Member mem = new Member();
                    mem.pwdupdate = DateTime.Now;
                    mem.password = MyCommonUtil.ComputeHash(teach.Password);
                    this.dao.Update(mem, new Member { id = dtl.mid });
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
        /// 取得系統設定資料
        /// </summary>
        /// <param name="model"></param>
        /// <param name="nid"></param>
        public void GetSystemDataDetail(AdminViewModel model, short? nid, bool isAllData)
        {
            if (nid.HasValue && !isAllData)
            {
                model.SystemModel.Detail = this.dao.GetRow(new Intro { id = nid });
                if (model.SystemModel.Detail != null)
                {
                    model.SystemModel.Detail.content = HttpUtility.HtmlDecode(model.SystemModel.Detail.content);
                }
            }
            else
            {
                if (isAllData)
                {
                    model.SystemModel.IntroList = this.dao.GetRowList(new Intro());
                }
            }
        }

        /// <summary>
        /// 取得系統最新消息明細
        /// </summary>
        /// <param name="model"></param>
        /// <param name="nid"></param>
        public void GetAdminNewsDetail(AdminViewModel model, long? nid)
        {
            model.NewsModel.NewsForm = this.dao.GetRow(new News { id = nid });
            var dtl = model.NewsModel.NewsForm;
            if (model.NewsModel.NewsForm != null)
            {
                dtl.content = HttpUtility.HtmlDecode(dtl.content);
            }
            model.NewsModel.NewsFiles = this.dao.GetRowList(new NewsFile { news_id = nid });
            model.NewsModel.PrepareAfterLoad();
        }

        /// <summary>
        /// 講師基本資料清單
        /// </summary>
        /// <param name="model"></param>
        public void GetTeacherInfoList(AdminViewModel model)
        {
            Store param = new Store();
            param["realname"] = model.TeacherModel.RealName;
            param["s_loginemail"] = model.TeacherModel.LoginEmail;
            param["s_email"] = model.TeacherModel.Email;
            param["s_pid"] = model.TeacherModel.PID;
            var list = this.dao.QueryForListAll<Store>("Admin.GetTeacherList", param);

            if (list != null)
            {
                model.TeacherModel.Grid = list;
            }
            else
            {
                model.TeacherModel.Grid = new List<Store>();
            }
        }

        /// <summary>
        /// 講師基本資料明細
        /// </summary>
        /// <param name="model"></param>
        public void GetTeacherInfoDetail(AdminViewModel model, long? mid)
        {
            model.TeacherModel.Mem = this.dao.GetRow(new Member { id = mid });
            model.TeacherModel.Detail = this.dao.GetRow(new MemDetail { mid = mid });
            model.TeacherModel.pid_mk = MyCommonUtil.GetMASKIDNO1((model.TeacherModel.Detail != null) ? model.TeacherModel.Detail.pid : "");
            var regList = this.dao.GetRowList(new MemTechReg { mid = mid });
            var orgattrList = this.dao.GetRowList(new MemOrgAttrib { mid = mid }); //服務單位屬性
            var indClassList = this.dao.GetRowList(new MemIndusClass { mid = mid }).OrderBy(x => x.order).ToList();

            model.TeacherModel.IndusList = new List<MemIndusClass>();
            for (int i = 0; i < 10; i++)
            {
                var item = indClassList.Where(x => x.order == i + 1).FirstOrDefault();
                model.TeacherModel.IndusList.Add(item ?? new MemIndusClass());
            }

            model.TeacherModel.TechRegList = regList.Select(x => x.cityid).ToList();

            //服務單位屬性
            model.TeacherModel.OrgAttribList = orgattrList.Select(x => x.attrid).ToList();

            model.TeacherModel.PrepareAfterLoad();
        }


        /// <summary>
        /// 儲侟師資招募狀態
        /// </summary>
        /// <param name="model"></param>
        public void SaveTeasurveyDetail(AdminViewModel model)
        {
            var dtl = model.RecruitModel.Teasurvey;
            this.dao.BeginTransaction();
            try
            {
                TeaSurvey saveModel = new TeaSurvey { confirm = dtl.confirm };
                this.dao.Update(saveModel, new TeaSurvey { id = dtl.id });
                this.dao.CommitTransaction();
            }
            catch (Exception ex)
            {
                LOG.Error(ex.Message, ex);
                this.dao.RollBackTransaction();
                throw ex;
            }
        }

        /// <summary> 管理者協助上傳檔案 </summary>
        /// <param name="model"></param>
        public void SaveFileUpload(AdminViewModel model)
        {
            var dtl = model.FileUpload;
            dtl.Detail.date = dtl.UploadDate.GetDate();
            dtl.Detail.@checked = 1;
            dtl.Detail.dlok = 0;
            dtl.Detail.reason = "";

            // 欄位檢核
            var result = new FileUplaodModelValidator().Validate(dtl);
            if (!result.IsValid)
            {
                ValidationException ex = new ValidationException(result.Errors);
                LOG.Warn(ex.Message, ex);
                throw ex;
            }

            Tuple<string, string> sfile = null;
            Tuple<string, string> ifile = null;
            Tuple<string, string> afile = null;
            try
            {
                sfile = this.fileService.UploadShareFile(dtl.SFile, "");
                ifile = this.fileService.UploadShareFile(dtl.IFile, "-1");
                afile = this.fileService.UploadShareFile(dtl.AFile, "-2");
            }
            catch (Exception ex)
            {
                LOG.Error(ex.Message, ex);
                throw ex;
            }

            if (sfile != null) { dtl.Detail.filename = sfile.Item1; }
            if (ifile != null) { dtl.Detail.ifilename = ifile.Item1; }
            if (afile != null) { dtl.Detail.afilename = afile.Item1; }

            this.dao.BeginTransaction();
            try
            {
                this.dao.Insert(dtl.Detail);    // 新增檔案
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
        /// 會議活動 頁面 
        /// </summary>
        /// <param name="model"></param>
        public void GetAdminMeeting(AdminViewModel model)
        {
            // TODO 取得空白會議表單 & 全部的會議清單
            if (!model.MeetingModel.Year.HasValue)
            {
                model.MeetingModel.Year = DateTime.Now.Year;
            }

            Store param = new Store();
            param["year"] = model.MeetingModel.Year;
            model.MeetingGrid = this.dao.QueryForListAll<Store>("Admin.GetMeetingList", param);

            //model.MeetingModel.SharedList = new List<AdminMeetMemberModel>();
            //for (int i = 0; i < 8; i++)
            //{
            //    model.MeetingModel.SharedList.Add(new AdminMeetMemberModel());
            //}

            model.MeetingModel.PrepareAfterLoad();
        }

        /// <summary>
        /// 會議活動明細
        /// </summary>
        /// <param name="model"></param>
        public void GetAdminMeetingDetail(AdminViewModel model, long? metid)
        {
            // 取得空白會議表單 & 全部的會議清單
            if (!metid.HasValue)
            {
                ArgumentException ex = new ArgumentException("未提供會議編號");
                LOG.Warn(ex.Message, ex);
                throw ex;
            }

            Store param = new Store();
            param["id"] = metid;
            Store res = this.dao.QueryForObject<Store>("Admin.GetMeetingList", param);

            if (res != null)
            {
                model.MeetingModel.Meet = res.ConvertTo<Meeting>();
                model.MeetingModel.Meet.time = new TimeSpan(res.Get("shour").AsInt32().Value, res.Get("sminute").AsInt32().Value, 00);
                model.MeetingModel.Meet.etime = new TimeSpan(res.Get("ehour").AsInt32().Value, res.Get("eminute").AsInt32().Value, 00);

                Store dparam = new Store();
                dparam["metid"] = metid;
                var detailList = this.dao.QueryForListAll<Store>("Admin.GetMeetingDetailList", dparam);

                if (detailList != null)
                {
                    var dList = (from a in detailList
                                 where a.Get("mcont").AsInt32() == 1
                                 select new AdminMeetMemberModel
                                 {
                                     Name = a.Get("realname").AsText(),
                                     Value = a.Get("mid").AsText(),
                                     Score = a.Get("score").AsText()
                                 }).ToList();

                    model.MeetingModel.MetDetailList = dList;

                    var sList = (from a in detailList
                                 where a.Get("mcont").AsInt32() == 2
                                 select new AdminMeetMemberModel
                                 {
                                     Name = a.Get("realname").AsText(),
                                     Value = a.Get("mid").AsText(),
                                     Score = a.Get("score").AsText()
                                 }).ToList();

                    model.MeetingModel.SharedList = sList;
                }

                model.MeetingModel.PrepareAfterLoad();
            }
        }

        public void GetMeetMemberList(AdminViewModel model, string name)
        {
            Store param = new Store();
            param["realname"] = name;
            var res = this.dao.QueryForListAll<Store>("Admin.GetTeacherList", param);

            var list = (from x in res
                        select new AdminMeetMemberModel
                        {
                            Name = x.Get("realname").AsText().ToString(),
                            Value = x.Get("mid").AsInt64().ToString()
                        }
            ).ToList();

            model.MeetingModel.MetDetailList = list;
        }

        /// <summary>
        /// 取得待審核統計資料
        /// </summary>
        /// <param name="model"></param>
        public void GetReviewCountData(AdminViewModel model)
        {
            if (!model.AuditParam.AuditYear.HasValue)
            {
                model.AuditParam.AuditYear = DateTime.Now.Year;
            }
            Store param = new Store();
            param.Collection(model.AuditParam);
            model.CourseAuditList = this.dao.QueryForListAll<Store>("Admin.CourseAuditList", param);
            model.FileAuditList = this.dao.QueryForListAll<Store>("Admin.FileAuditList", param);
            model.HomeFileAuditList = this.dao.QueryForListAll<Store>("Admin.HomeFileAuditList", param);
        }

        /// <summary>
        /// 取得課程審核明細資料
        /// </summary>
        /// <param name="model"></param>
        public void GetReviewDataCourse(AdminViewModel model)
        {
            var param = model.AuditParam;
            var member = this.dao.GetRow(new MemDetail { mid = param.Mid });
            if (member != null)
            {
                param.RealName = member.realname;
            }

            var srcList = this.dao.GetRowList(new Memsrcs { mid = param.Mid, ckeckd = param.Ckeckd });
            model.AuditCourseDetailList = srcList.Where(x => x.sdate.Value.Year == param.AuditYear).ToList();
        }

        /// <summary>
        /// 取得上傳檔案審核明細資料
        /// </summary>
        /// <param name="model"></param>
        public void GetReviewDataFile(AdminViewModel model)
        {
            var param = model.AuditParam;

            var member = this.dao.GetRow(new MemDetail { mid = param.Mid });
            if (member != null)
            {
                param.RealName = member.realname;
            }

            var shareList = this.dao.GetRowList(new Memshare { mid = param.Mid, @checked = param.Ckeckd });
            model.AuditShareDetailList = shareList.Where(x => x.date.Value.Year == param.AuditYear).ToList();
        }

        /// <summary>
        /// 查詢講師姓名
        /// </summary>
        /// <param name="keyword">查詢文字</param>
        /// <returns></returns>
        public IList<Store> GetMemberList(string keyword)
        {
            IList<Store> res = null;
            if (!string.IsNullOrEmpty(keyword))
            {
                Store param = new Store();
                param["keyword"] = keyword;
                res = this.dao.QueryForListAll<Store>("Admin.QueryMemberList", param);
            }
            return res;
        }

        /// <summary>
        /// 儲存新增帳號資料
        /// </summary>
        /// <param name="model"></param>
        public void SaveAccount(AdminViewModel model)
        {
            ValidationResult result = new AdminMemberValidator(this.dao).Validate(model.MemberModel);
            if (!result.IsValid)
            {
                ValidationException ex = new ValidationException(result.Errors);
                LOG.Warn(ex.Message, ex);
                throw ex;
            }

            var current = model.MemberModel;
            Member mem = current.Mem;
            MemDetail memdetail = current.Detail;

            this.dao.BeginTransaction();
            try
            {
                mem.username = mem.email;
                mem.password = MyCommonUtil.ComputeHash(mem.password);
                mem.pwdupdate = DateTime.Now;
                mem.ROPLE = 2; //2:教師可能-OWASP,角X色 0:非教師, 2:教師可能,3:（未使用）,4:（未使用）
                long newSeq = this.dao.Insert(mem);

                memdetail.mid = newSeq;
                memdetail.regyear = DateTime.Now.Year; //加入年份
                this.dao.Insert(memdetail);

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
        /// 儲存會議資料
        /// </summary>
        /// <param name="model"></param>
        public void SaveMeeting(AdminViewModel model)
        {
            model.MeetingModel.PrepareBeforeSave();
            var current = model.MeetingModel;
            var dtl = current.Meet;

            ValidationResult result = new MeetValidator().Validate(dtl);    // 欄位檢核 
            if (!result.IsValid)
            {
                ValidationException ex = new ValidationException(result.Errors);
                LOG.Warn(ex.Message, ex);
                throw ex;
            }

            this.dao.BeginTransaction();
            try
            {
                if (dtl.id.HasValue)
                {
                    // update
                    this.dao.Update(dtl, new Meeting { id = dtl.id });

                    this.dao.Delete(new Metdetail { metid = dtl.id });

                    if (current.SaveDetailList != null)
                    {
                        foreach (var item in current.SaveDetailList)
                        {
                            this.dao.Insert(new Metdetail
                            {
                                mcont = 1,
                                metid = dtl.id,
                                mid = System.Convert.ToInt64(item),
                                score = current.MeetScore
                            });
                        }
                    }

                    if (current.SaveShareList != null)
                    {
                        foreach (var item in current.SaveShareList)
                        {
                            this.dao.Insert(new Metdetail
                            {
                                mcont = 2,
                                metid = dtl.id,
                                mid = System.Convert.ToInt64(item),
                                score = current.SharedScore
                            });
                        }
                    }
                }
                else
                {
                    var newMetid = this.dao.Insert(dtl);

                    if (newMetid > 0)
                    {
                        if (current.SaveDetailList != null)
                        {
                            foreach (var item in current.SaveDetailList)
                            {
                                this.dao.Insert(new Metdetail
                                {
                                    mcont = 1,
                                    metid = newMetid,
                                    mid = System.Convert.ToInt64(item),
                                    score = current.MeetScore
                                });
                            }
                        }

                        if (current.SaveShareList != null)
                        {
                            foreach (var item in current.SaveShareList)
                            {
                                this.dao.Insert(new Metdetail
                                {
                                    mcont = 2,
                                    metid = newMetid,
                                    mid = System.Convert.ToInt64(item),
                                    score = current.SharedScore
                                });
                            }
                        }
                    }
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
        /// 儲存課程審核結果
        /// </summary>
        /// <param name="model"></param>
        public void SaveCourseAudit(AdminViewModel model)
        {
            this.dao.BeginTransaction();
            try
            {
                foreach (var item in model.AuditCourseDetailList)
                {
                    this.dao.Update(
                        new Memsrcs { ckeckd = item.ckeckd, comment = item.comment },
                        new Memsrcs { id = item.id });
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
        /// 儲存上傳教材檔案審核結果
        /// </summary>
        /// <param name="model"></param>
        public void SaveShareFileAudit(AdminViewModel model)
        {
            this.dao.BeginTransaction();
            try
            {
                foreach (var item in model.AuditShareDetailList)
                {
                    this.dao.Update(
                        new Memshare { @checked = item.@checked, reason = item.reason },
                        new Memshare { id = item.id });
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
        /// 下架/上架最新消息
        /// </summary>
        /// <param name="model"></param>
        /// <param name="nid"></param>
        public void SaveNewsCloseOpen(AdminViewModel model, long? nid)
        {
            var row = this.dao.GetRow(new News { id = nid });
            byte? newShowoff = 0;

            if (row != null)
            {
                newShowoff = (row.showoff == (byte)0 ? (byte)1 : (byte)0);
            }

            this.dao.BeginTransaction();
            try
            {
                this.dao.Update(new News { showoff = newShowoff }, new News { id = nid });
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
        /// 講師積分資料清單
        /// </summary>
        /// <param name="model"></param>
        public void GetScoreList(AdminViewModel model)
        {
            int year = 0;
            if (!model.ScoreModel.Year.HasValue)
            {
                year = DateTime.Now.Year;
                model.ScoreModel.Year = year;
            }
            else
            {
                year = model.ScoreModel.Year.Value;
            }

            Store param = new Store();
            param["year"] = year;
            string statementId = null;
            if (year >= 2015)
            {
                statementId = "Admin.GetScoreList2015";
            }
            else
            {
                statementId = "Admin.GetScoreList2014";
            }

            model.ScoreModel.Grid = this.dao.QueryForListAll<Store>(statementId, param);
        }

        /// <summary>
        /// 師資招募清單
        /// </summary>
        /// <param name="model"></param>
        public void GetTeacherSurveyList(AdminViewModel model, Int16? confirm)
        {
            Store param = new Store();
            if (confirm.HasValue)
            {
                param["confirm"] = confirm;
            }
            model.RecruitModel.Grid = this.dao.QueryForListAll<Store>("Admin.GetTeasurveyList", param);
        }

        /// <summary>
        /// 師資招募
        /// </summary>
        /// <param name="model"></param>
        public void GetTeacherSurveyDetail(AdminViewModel model, long? tsid)
        {
            model.RecruitModel.Teasurvey = this.dao.GetRow(new TeaSurvey { id = tsid });
        }

        /// <summary>
        /// 意見回饋清單
        /// </summary>
        /// <param name="model"></param>
        /// <param name="answer">回覆狀態</param>
        public void GetFeedbackList(AdminViewModel model, Int16? answer)
        {
            Store param = new Store();
            if (answer.HasValue)
            {
                param["answer"] = answer;
            }

            model.FeedbackModel.Grid = this.dao.QueryForListAll<Store>("Admin.GetFeedbackList", param);

            if (model.FeedbackModel.Grid == null)
            {
                model.FeedbackModel.Grid = new List<Store>();
            }
        }

        /// <summary>
        /// 管理者帳號清單
        /// </summary>
        /// <param name="model"></param>
        public void GetMgrAccountList(AdminViewModel model)
        {
            Store param = new Store();
            param["role"] = 0;

            var form = model.MgrModel.Form;
            if (form != null)
            {
                param["pid"] = form.pid;
                param["realname"] = form.realname;
                param["email"] = form.email;
            }

            model.MgrModel.Grid = this.dao.QueryForListAll<Store>("Admin.GetMemberList", param);
        }

        /// <summary>
        /// 管理者帳號明細
        /// </summary>
        /// <param name="model"></param>
        /// <param name="mid"></param>
        public void GetMgrAccountDetail(AdminViewModel model, long? mid)
        {
            if (mid.HasValue)
            {
                model.MgrModel.Form = this.dao.GetRow(new MemDetail { mid = mid });
                model.MgrModel.Mem = this.dao.GetRow(new Member { id = mid });
                if (model.MgrModel.Mem != null) { model.MgrModel.Mem.password = null; }
            }
            else
            {
                model.MgrModel.Form = new MemDetail();
                model.MgrModel.Mem = new Member();
            }
        }

        /// <summary>
        /// 儲存管理者帳號
        /// </summary>
        /// <param name="model"></param>
        public void SaveMgrAccount(AdminViewModel model)
        {
            var current = model.MgrModel;

            var result = new AdminMgrValidator(this.dao, current.Form.mid).Validate(current);
            if (!result.IsValid)
            {
                ValidationException ex = new ValidationException(result.Errors);
                LOG.Warn(ex.Message, ex);
                throw ex;
            }

            this.dao.BeginTransaction();
            try
            {
                if (!current.Form.mid.HasValue)
                {
                    Member mem = current.Mem;
                    mem.username = mem.email;
                    mem.password = MyCommonUtil.ComputeHash(mem.password);
                    mem.pwdupdate = DateTime.Now;
                    mem.ROPLE = 0; //非教師-OWASP,角X色 0:非教師, 2:教師可能,3:（未使用）,4:（未使用）
                    long? newSeq = this.dao.Insert(mem);

                    MemDetail dtl = current.Form;
                    dtl.email = mem.email;
                    dtl.mid = newSeq;
                    this.dao.Insert(dtl);
                }
                else
                {
                    Member mb = this.dao.GetRow(new Member { id = current.Form.mid });
                    if (mb != null)
                    {
                        PasswordHistory ph = new PasswordHistory();
                        ph.mid = current.Form.mid;
                        ph.modifydate = mb.pwdupdate;
                        ph.password = mb.password;
                        this.dao.Insert(ph);
                    }

                    Member mem = current.Mem;
                    mem.username = mem.email;
                    mem.password = MyCommonUtil.ComputeHash(mem.password);
                    mem.pwdupdate = DateTime.Now;
                    this.dao.Update(mem, new Member { id = current.Form.mid });

                    MemDetail dtl = current.Form;
                    dtl.email = mem.email;
                    this.dao.Update(dtl, new MemDetail { mid = current.Form.mid });

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
        ///  刪除管理者帳號
        /// </summary>
        /// <param name="mid"></param>
        public void RemoveMgrAccount(long? mid)
        {
            if (!mid.HasValue) { return; }

            this.dao.BeginTransaction();
            try
            {
                this.dao.Delete(new Member { id = mid });
                this.dao.Delete(new MemDetail { mid = mid });
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
        /// 意見回饋明細
        /// </summary>
        public void GetFeedbackDetail(AdminViewModel model, long? opid)
        {
            if (opid.HasValue)
            {
                Store param = new Store();
                param["id"] = opid;
                Store store = this.dao.QueryForObject<Store>("Admin.GetFeedbackList", param);
                if (store != null)
                {
                    var opinion = store.ConvertTo<Opinion>();
                    if (opinion != null)
                    {
                        model.FeedbackModel.OpinionItem = opinion;
                        var memdetail = this.dao.GetRow(new MemDetail { mid = opinion.mid });
                        if (memdetail != null)
                        {
                            model.FeedbackModel.RealName = memdetail.realname;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 儲存回覆意見
        /// </summary>
        /// <param name="model"></param>
        public void SaveFeedbackDetail(AdminViewModel model)
        {
            var dtl = model.FeedbackModel.OpinionItem;
            Opinion saveModel = new Opinion
            {
                answer = dtl.answer,
                response = dtl.response,
                rdate = DateTime.Now
            };

            this.dao.BeginTransaction();
            try
            {
                this.dao.Update(saveModel, new Opinion { id = dtl.id });
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
        /// 儲存最新消息明細
        /// </summary>
        /// <param name="model"></param>
        public void SaveNewsDetail(AdminViewModel model)
        {
            model.NewsModel.PrepareBeforeSave();

            var dtl = model.NewsModel.NewsForm;
            dtl.content = HttpUtility.HtmlDecode(dtl.content);

            this.dao.BeginTransaction();
            try
            {

                if (dtl.id.HasValue)
                {
                    this.dao.Update(dtl, new News { id = dtl.id });
                    if (model.NewsModel.NewsFiles != null)
                    {
                        for (int i = 0; i < model.NewsModel.NewsFiles.Count; i++)
                        {
                            var item = model.NewsModel.NewsFiles[i];

                            if (model.NewsModel.FileRemoveList[i] == true)
                            {
                                this.dao.Delete(new NewsFile { id = item.id });
                            }
                        }
                    }

                    if (model.NewsModel.Files != null)
                    {
                        IList<string> commentList = model.NewsModel.Comments;
                        for (int i = 0; i < model.NewsModel.Files.Count; i++)
                        {
                            var file = model.NewsModel.Files[i];
                            if (file != null)
                            {
                                Tuple<string, string> fileuploaded = this.fileService.UploadNewsFile(file, (i + 1).ToString());
                                NewsFile newsfile = new NewsFile
                                {
                                    news_id = dtl.id,
                                    comment = commentList[i],
                                    filename = fileuploaded.Item1,
                                    forder = (short)(i + 1)
                                };
                                this.dao.Insert(newsfile);
                            }
                        }
                    }

                }
                else
                {
                    dtl.showoff = 1;
                    int i_news_id = this.dao.Insert(dtl);
                    if (i_news_id > 0)
                    {
                        if (model.NewsModel.Files != null && model.NewsModel.Files.Count > 0)
                        {
                            for (int i = 0; i < model.NewsModel.Files.Count; i++)
                            {
                                var fi = model.NewsModel.Files[i];
                                if (fi != null)
                                {
                                    Tuple<string, string> filePath = fileService.UploadNewsFile(fi, (i + 1).ToString());
                                    if (filePath.Item1 != null)
                                    {
                                        NewsFile currentNewsFile = new NewsFile
                                        {
                                            news_id = i_news_id,
                                            filename = filePath.Item1,
                                            comment = model.NewsModel.Comments[i],
                                            forder = (short)(i + 1)
                                        };
                                        this.dao.Insert(currentNewsFile);
                                    }
                                }
                            }
                        }
                    }
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


        public void AdminAccountList(AdminViewModel model)
        {

        }

        /// <summary>
        /// 取得登入log紀錄
        /// </summary>
        /// <param name="model"></param>
        public void QueryLoginLogList(LogViewModel model, bool isPaging = false)
        {
            if (!isPaging)
            {
                var form = model.LoginLogForm;
                form.StartDate = form.StartDateTw.GetDate();
                form.EndDate = form.EndDateTw.GetDate();

                Store param = new Store();
                param.Collection(form);
                model.LoginGrid = this.dao.QueryForList<Store>("Log.QueryLoginLog", param);
            }
            else
            {
                model.LoginGrid = this.dao.QueryForList<Store>("Log.QueryLoginLog", null);
            }
        }

        /// <summary>
        /// 取得登入檔案上傳紀錄
        /// </summary>
        /// <param name="model"></param>
        public void QueryFileLogList(LogViewModel model, bool isPaging = false)
        {
            if (!isPaging)
            {
                var form = model.FileLogForm;
                form.StartDate = form.StartDateTw.GetDate();
                form.EndDate = form.EndDateTw.GetDate();

                Store param = new Store();
                param.Collection(form);
                this.dao.SetPageInfo(form.rid, form.p);

                model.FileGrid = this.dao.QueryForList<Store>("Log.QueryFileLog", param);
            }
            else
            {
                model.FileGrid = this.dao.QueryForList<Store>("Log.QueryFileLog", null);
            }
        }

        /// <summary>
        /// get checkboxlist orgattrib txt 
        /// </summary>
        /// <param name="i_mid"></param>
        /// <param name="orgattr"></param>
        /// <returns></returns>
        public string get_orgattriblist(long i_mid, IList<SelectListItem> orgattr)
        {
            string s_Rst = "";
            MemOrgAttrib param_o = new MemOrgAttrib();
            param_o.mid = i_mid;//item.mid;
            IList<MemOrgAttrib> resList_o = this.dao.GetRowList(param_o);
            if (resList_o != null && resList_o.Count > 0)
            {
                List<string> orgattriblist = new List<string>();
                foreach (var item_o in resList_o)
                {
                    Store store_o = new Store();
                    store_o.Collection(item_o);
                    orgattriblist.Add(store_o.Get("attrid").AsCodeText(orgattr));
                }
                s_Rst = string.Join(",", orgattriblist);
            }
            return s_Rst;
        }

        /// <summary>
        /// 取得登入檔案上傳紀錄
        /// </summary>
        /// <param name="model"></param>
        public void QueryFuncLogList(LogViewModel model, bool isPaging = false)
        {
            if (!isPaging)
            {
                var form = model.FuncLogForm;
                form.StartDate = form.StartDateTw.GetDate();
                form.EndDate = form.EndDateTw.GetDate();

                Store param = new Store();
                param.Collection(form);
                this.dao.SetPageInfo(form.rid, form.p);

                model.FuncGrid = this.dao.QueryForList<Store>("Log.QueryFuncLog", param);
            }
            else
            {
                model.FuncGrid = this.dao.QueryForList<Store>("Log.QueryFuncLog", null);
            }
        }

        /// <summary>
        /// 資料匯出
        /// </summary>
        /// <param name="model"></param>
        public void GetExportList(AdminViewModel model)
        {
            MemDetail param = new MemDetail();
            if (model.ExportModel.Active != null) { param.active = model.ExportModel.Active; }
            IList<MemDetail> resList = this.dao.GetRowList(param);

            if (resList != null && resList.Count > 0)
            {
                IList<Store> grid = new List<Store>();
                var gender = CommonOptions.Gender(); //gender
                var teachUnit = CommonOptions.TeachUnit();
                var group = CommonOptions.Group();
                var acadeType = CommonOptions.AcadeType();
                var degree = CommonOptions.Degree();
                var zip = CommonOptions.ZipWithRegion();
                IList<SelectListItem> orgattr = CommonOptions.OrgAttrib();

                foreach (var item in resList)
                {
                    Store store = new Store();
                    store.Collection(item);

                    //get checkboxlist orgattrib txt 
                    store["orgattrib"] = get_orgattriblist(item.mid.Value, orgattr);

                    //LOG.Debug(string.Format("store.Get(offdate).RawValue:{0}", store.Get("offdate").RawValue ?? "[Null]"));
                    //LOG.Debug(string.Format("store.Get(offdate).AsDateTimeText():{0}", store.Get("offdate").AsDateTimeText()));
                    //LOG.Debug(string.Format("store.Get(offdate).AsDateTimeText(yyyy-MM-dd):{0}", store.Get("offdate").AsDateTimeText("yyyy-MM-dd")));
                    //store["birthdat"] = store.Get("birthdat").RawValue != null ? store.Get("birthdat").AsDateTimeText("yyyy-MM-dd") : "";
                    //store["offdate"] = store.Get("offdate").RawValue != null ? store.Get("offdate").AsDateTimeText("yyyy-MM-dd") : "";
                    store["birthdat"] = store.Get("birthdat").AsDateTimeText("yyyy-MM-dd");
                    //store["offdate"] = store.Get("offdate").AsDateTimeText("yyyy-MM-dd");
                    store["offdate"] = store.Get("offdate").AsDateTimeTextTw();

                    store["gender"] = store.Get("gender").AsCodeText(gender);
                    store["degree"] = store.Get("degree").AsCodeText(degree);
                    store["group"] = store.Get("group").AsCodeText(group);
                    store["acadetype"] = store.Get("acadetype").AsCodeText(acadeType);

                    // 設定授課單元
                    string memteachunit = store.Get("teachunit").AsText();
                    List<string> teachList = new List<string>();
                    if (!string.IsNullOrEmpty(memteachunit))
                    {
                        if (memteachunit[0] == '1') { teachList.Add("DC"); }
                        if (memteachunit[1] == '1') { teachList.Add("BC"); }
                        if (memteachunit[2] == '1') { teachList.Add("KC"); }
                    }
                    store["teachunit"] = string.Join(",", teachList);

                    // 設定地址
                    var address = store.Get("address").AsText();
                    var zipName = store.Get("zipcode").AsCodeText(zip);
                    store["address"] = zipName + address;

                    // 轉碼
                    string pattern = "&(amp;)+";
                    string[] fields2 = new string[] { "skill", "history", "major", "jobtitle", "school", "jobcomp", "address" };
                    foreach (var s_field in fields2)
                    {
                        if (store.ContainsKey(s_field))
                            store[s_field] = Regex.Replace(Regex.Replace(store.Get(s_field).AsText().Replace("\\r\\n", "\r\n").Replace(" ", ""), @"\u00A0", ""), pattern, "＆");
                    }
                    //store["skill"] = Regex.Replace(Regex.Replace(store.Get("skill").AsText().Replace("\\r\\n", "\r\n").Replace(" ", ""), @"\u00A0", ""), pattern, "＆");
                    //store["history"] = Regex.Replace(Regex.Replace(store.Get("history").AsText().Replace("\\r\\n", "\r\n").Replace(" ", ""), @"\u00A0", ""), pattern, "＆");
                    //store["major"] = Regex.Replace(Regex.Replace(store.Get("major").AsText().Replace("\\r\\n", "\r\n").Replace(" ", ""), @"\u00A0", ""), pattern, "＆");
                    //store["jobtitle"] = Regex.Replace(Regex.Replace(store.Get("jobtitle").AsText().Replace("\\r\\n", "\r\n").Replace(" ", ""), @"\u00A0", ""), pattern, "＆");
                    //store["school"] = Regex.Replace(Regex.Replace(store.Get("school").AsText().Replace("\\r\\n", "\r\n").Replace(" ", ""), @"\u00A0", ""), pattern, "＆");
                    //store["jobcomp"] = Regex.Replace(Regex.Replace(store.Get("jobcomp").AsText().Replace("\\r\\n", "\r\n").Replace(" ", ""), @"\u00A0", ""), pattern, "＆");
                    //store["address"] = Regex.Replace(Regex.Replace(store.Get("address").AsText().Replace("\\r\\n", "\r\n").Replace(" ", ""), @"\u00A0", ""), pattern, "＆");
                    grid.Add(store);
                }

                model.ExportModel.Grid = grid;
            }
        }

        /// <summary>
        /// 會議類型列表
        /// </summary>
        /// <param name="model"></param>
        public void GetSessionExportList(AdminViewModel model)
        {
            var current = model.MeetExportModel;
            if (current != null)
            {
                Store param = new Store();
                param["year"] = current.Year.Value;
                param["mclass"] = current.MClass.Value;

                model.MeetExportModel.Grid = this.dao.QueryForListAll<Store>("Admin.GetMeetExportList", param);
            }
        }
    }
}
