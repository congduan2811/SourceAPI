using DocProAPI.Customs;
using DocProAPI.Models.QLKH;
using DocProModel.Models;
using DocProUtil;
using DocProUtil.Cf;
using DocProUtil.Customs.Entities;
using DocProUtil.Customs.Perms;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;
using static DocProAPI.Customs.Enum.Enumtype;

namespace DocProAPI.Controllers
{
    public class BaseController : ApiController
    {
        /// <summary>
        ///     Current user
        /// </summary>
        private User _cuser;
        protected User CUser
        {
            get
            {
                if (_cuser == null)
                {
                    _cuser = AclConfig.CurrentUserToken;
                }
                return _cuser;
            }
        }
        private List<UserTeam> _cuserteam;
        protected List<UserTeam> CUserTeam
        {
            get
            {
                if (Utils.IsEmpty<UserTeam>(_cuserteam))
                {
                    _cuserteam = DocProModel.Repository.UserTeamRepository.GetByUser(CUser.ID);
                }
                return _cuserteam;
            }
        }

        /// <summary>
        ///     Pagination
        /// </summary>
        private Pagination _paging;
        protected Pagination Paging
        {
            get
            {
                if (Utils.IsEmpty(_paging))
                {
                    _paging = new Pagination();
                    _paging.PageSize = Utils.GetInt(DATA, "Take");
                    _paging.PageIndex = Utils.GetInt(DATA, "Skip") == 0 ? 1 : Utils.GetInt(DATA, "Skip");
                    if (_paging.PageSize <= 0)
                        _paging.PageSize = 20;
                    if (_paging.PageIndex <= 0)
                        _paging.PageIndex = 1;
                }
                return _paging;
            }
        }

        #region Form Data
        private Hashtable _data;
        protected Hashtable DATA
        {
            get
            {
                if (Equals(_data, null))
                    _data = Utils.GetDataPost();

                return _data;
            }
            set
            {
                _data = value;
            }
        }
        #endregion

        #region Perms

        private bool _isQuanLyKhachHang;
        private bool _isQuanLyKhachHangChecked;
        private bool _isQuanLyTLCN;
        private bool _isQuanLyTLCNChecked;
        private bool _isQuanLyKho;
        private bool _isQuanLyKhoChecked;
        private bool _isSuDungKho;
        private bool _isSuDungKhoChecked;
        private bool _isQuanLyDanhMuc;
        private bool _isQuanLyDanhMucChecked;
        private bool _isQuanLyLoaiTaiLieu;
        private bool _isQuanLyLoaiTaiLieuChecked;
        private bool _isQuanLySoHoSo;
        private bool _isQuanLySoHoSoChecked;
        private bool _isBienTapTuLieu;
        private bool _isBienTapTuLieuChecked;
        private bool _isKiemDuyetTuLieu;
        private bool _isKiemDuyetTuLieuChecked;

        private bool _isBienTapTinTuc;
        private bool _isBienTapTinTucChecked;
        private bool _isKiemDuyetTinTuc;
        private bool _isKiemDuyetTinTucChecked;
        private bool _isXuatBanTinTuc;
        private bool _isXuatBanTinTucChecked;

        private bool _isBienTapQuyTrinh;
        private bool _isBienTapQuyTrinhChecked;
        private bool _isKiemDuyetQuyTrinh;
        private bool _isKiemDuyetQuyTrinhChecked;
        private bool _isXuatBanQuyTrinh;
        private bool _isXuatBanQuyTrinhChecked;

        private bool _isBienTapThuTuc;
        private bool _isBienTapThuTucChecked;
        private bool _isKiemDuyetThuTuc;
        private bool _isKiemDuyetThuTucChecked;
        private bool _isXuatBanThuTuc;
        private bool _isXuatBanThuTucChecked;
        private bool _isDangKyThuTuc;
        private bool _isDangKyThuTucChecked;
        private bool _isGiaiQuyetThuTuc;
        private bool _isGiaiQuyetThuTucChecked;

        private bool _isQuanLyCongViec;
        private bool _isQuanLyCongViecChecked;
        private bool _isThucHienCongViec;
        private bool _isThucHienCongViecChecked;
        protected bool QuanLyTLCN
        {
            get
            {
                if (!_isQuanLyTLCNChecked)
                {
                    _isQuanLyTLCNChecked = true;
                    _isQuanLyTLCN = new ModulePerm
                    {
                        IsCreate = true,
                        Module = (int)IModule.QuanLyTLCN
                    }.Enabled(CUser);
                }
                return _isQuanLyTLCN;
            }
        }
        protected bool QuanLyKho
        {
            get
            {
                if (!_isQuanLyKhoChecked)
                {
                    _isQuanLyKhoChecked = true;
                    _isQuanLyKho = new ModulePerm
                    {
                        IsCreate = true,
                        Module = (int)IModule.QuanLyKho
                    }.Enabled(CUser);
                }
                return _isQuanLyKho;
            }
        }
        protected bool SuDungKho
        {
            get
            {
                if (_isQuanLyKho)
                {
                    _isSuDungKho = true;
                }
                else if (!_isSuDungKhoChecked)
                {
                    _isSuDungKhoChecked = true;
                    _isSuDungKho = new ModulePerm
                    {
                        IsCreate = true,
                        Module = (int)IModule.SuDungKho
                    }.Enabled(CUser);
                }
                return _isSuDungKho;
            }
        }
        protected bool QuanLyDanhMuc
        {
            get
            {
                if (!_isQuanLyDanhMucChecked)
                {
                    _isQuanLyDanhMucChecked = true;
                    _isQuanLyDanhMuc = new ModulePerm
                    {
                        IsCreate = true,
                        Module = (int)IModule.QuanLyDanhMuc
                    }.Enabled(CUser);
                }
                return _isQuanLyDanhMuc;
            }
        }
        protected bool QuanLyLoaiTaiLieu
        {
            get
            {
                if (!_isQuanLyLoaiTaiLieuChecked)
                {
                    _isQuanLyLoaiTaiLieuChecked = true;
                    _isQuanLyLoaiTaiLieu = new ModulePerm
                    {
                        IsCreate = true,
                        Module = (int)IModule.QuanLyLTL
                    }.Enabled(CUser);
                }
                return _isQuanLyLoaiTaiLieu;
            }
        }
        protected bool QuanLySoHoSo
        {
            get
            {
                if (!_isQuanLySoHoSoChecked)
                {
                    _isQuanLySoHoSoChecked = true;
                    _isQuanLySoHoSo = new ModulePerm
                    {
                        IsCreate = true,
                        Module = (int)IModule.QuanLySoHoSo
                    }.Enabled(CUser);
                }
                return _isQuanLySoHoSo;
            }
        }
        protected bool BienTapTuLieu
        {
            get
            {
                if (_isKiemDuyetTuLieu)
                {
                    _isBienTapTuLieu = true;
                }
                else if (!_isBienTapTuLieuChecked)
                {
                    _isBienTapTuLieuChecked = true;
                    _isBienTapTuLieu = new ModulePerm
                    {
                        IsCreate = true,
                        Module = (int)IModule.BienTapTuLieu
                    }.Enabled(CUser);
                }
                return _isBienTapTuLieu;
            }
        }
        protected bool KiemDuyetTuLieu
        {
            get
            {
                if (!_isKiemDuyetTuLieuChecked)
                {
                    _isKiemDuyetTuLieuChecked = true;
                    _isKiemDuyetTuLieu = new ModulePerm
                    {
                        IsCreate = true,
                        Module = (int)IModule.KiemDuyetTuLieu
                    }.Enabled(CUser);
                }
                return _isKiemDuyetTuLieu;
            }
        }
        protected bool BienTapTinTuc
        {
            get
            {
                if (!_isBienTapTinTucChecked)
                {
                    _isBienTapTinTucChecked = true;
                    _isBienTapTinTuc = new ModulePerm
                    {
                        IsCreate = true,
                        Module = (int)IModule.BienTapTinTuc
                    }.Enabled(CUser);
                }
                return _isBienTapTinTuc;
            }
        }
        protected bool KiemDuyetTinTuc
        {
            get
            {
                if (!_isKiemDuyetTinTucChecked)
                {
                    _isKiemDuyetTinTucChecked = true;
                    _isKiemDuyetTinTuc = new ModulePerm
                    {
                        IsCreate = true,
                        Module = (int)IModule.KiemDuyetTinTuc
                    }.Enabled(CUser);
                }
                return _isKiemDuyetTinTuc;
            }
        }
        protected bool XuatBanTinTuc
        {
            get
            {
                if (!_isXuatBanTinTucChecked)
                {
                    _isXuatBanTinTucChecked = true;
                    _isXuatBanTinTuc = new ModulePerm
                    {
                        IsCreate = true,
                        Module = (int)IModule.XuatBanTinTuc
                    }.Enabled(CUser);
                }
                return _isXuatBanTinTuc;
            }
        }
        protected bool BienTapQuyTrinh
        {
            get
            {
                if (!_isBienTapQuyTrinhChecked)
                {
                    _isBienTapQuyTrinhChecked = true;
                    _isBienTapQuyTrinh = new ModulePerm
                    {
                        IsCreate = true,
                        Module = (int)IModule.BienTapQuyTrinh
                    }.Enabled(CUser);
                }
                return _isBienTapQuyTrinh;
            }
        }
        protected bool KiemDuyetQuyTrinh
        {
            get
            {
                if (!_isKiemDuyetQuyTrinhChecked)
                {
                    _isKiemDuyetQuyTrinhChecked = true;
                    _isKiemDuyetQuyTrinh = new ModulePerm
                    {
                        IsCreate = true,
                        Module = (int)IModule.KiemDuyetQuyTrinh
                    }.Enabled(CUser);
                }
                return _isKiemDuyetQuyTrinh;
            }
        }
        protected bool XuatBanQuyTrinh
        {
            get
            {
                if (!_isXuatBanQuyTrinhChecked)
                {
                    _isXuatBanQuyTrinhChecked = true;
                    _isXuatBanQuyTrinh = new ModulePerm
                    {
                        IsCreate = true,
                        Module = (int)IModule.XuatBanQuyTrinh
                    }.Enabled(CUser);
                }
                return _isXuatBanQuyTrinh;
            }
        }
        protected bool BienTapThuTuc
        {
            get
            {
                if (!_isBienTapThuTucChecked)
                {
                    _isBienTapThuTucChecked = true;
                    _isBienTapThuTuc = new ModulePerm
                    {
                        IsCreate = true,
                        Module = (int)IModule.BienTapThuTuc
                    }.Enabled(CUser);
                }
                return _isBienTapThuTuc;
            }
        }
        protected bool KiemDuyetThuTuc
        {
            get
            {
                if (!_isKiemDuyetThuTucChecked)
                {
                    _isKiemDuyetThuTucChecked = true;
                    _isKiemDuyetThuTuc = new ModulePerm
                    {
                        IsCreate = true,
                        Module = (int)IModule.KiemDuyetThuTuc
                    }.Enabled(CUser);
                }
                return _isKiemDuyetThuTuc;
            }
        }
        protected bool XuatBanThuTuc
        {
            get
            {
                if (!_isXuatBanThuTucChecked)
                {
                    _isXuatBanThuTucChecked = true;
                    _isXuatBanThuTuc = new ModulePerm
                    {
                        IsCreate = true,
                        Module = (int)IModule.XuatBanThuTuc
                    }.Enabled(CUser);
                }
                return _isXuatBanThuTuc;
            }
        }
        protected bool IsAdminChannel
        {
            get
            {
                return CUser.IsAdmin && CUser.IDChannel == 0;
            }
        }
        protected bool IsViewMuonTra
        {
            get
            {
                var idModules = LicenseConfig.GetLicenseModules();
                var idMuonTra = (int)IModule.KiemDuyetHoSoMuonTra;

                return idModules.Any(x => x == idMuonTra);
            }
        }
        protected bool QuanLyKhachHang
        {
            get
            {
                if (!_isQuanLyKhachHangChecked)
                {
                    _isQuanLyKhachHangChecked = true;
                    _isQuanLyKhachHang = new ModulePerm
                    {
                        IsCreate = true,
                        Module = (int)IModule.QuanLyKhachHang
                    }.Enabled(CUser);
                }
                return _isQuanLyKhachHang;
            }
        }
        protected bool QuanLyCongViec
        {
            get
            {
                if (!_isQuanLyCongViecChecked)
                {
                    _isQuanLyCongViecChecked = true;
                    _isQuanLyCongViec = new ModulePerm
                    {
                        IsCreate = true,
                        Module = (int)IModule.QuanLyCongViec
                    }.Enabled(CUser);
                }
                return _isQuanLyCongViec;
            }
        }
        protected bool ThucHienCongViec
        {
            get
            {
                if (_isQuanLyCongViec)
                {
                    _isThucHienCongViec = true;
                }
                else if (!_isThucHienCongViecChecked)
                {
                    _isThucHienCongViecChecked = true;
                    _isThucHienCongViec = new ModulePerm
                    {
                        IsCreate = true,
                        Module = (int)IModule.ThucHienCongViec
                    }.Enabled(CUser);
                }
                return _isThucHienCongViec;
            }
        }
        protected bool DangKyThuTuc
        {
            get
            {
                if (!_isDangKyThuTucChecked)
                {
                    _isDangKyThuTucChecked = true;
                    _isDangKyThuTuc = new ModulePerm
                    {
                        IsCreate = true,
                        Module = (int)IModule.DangKyThuTuc
                    }.Enabled(CUser);
                }
                return _isDangKyThuTuc;
            }
        }
        protected bool GiaiQuyetThuTuc
        {
            get
            {
                if (!_isGiaiQuyetThuTucChecked)
                {
                    _isGiaiQuyetThuTucChecked = true;
                    _isGiaiQuyetThuTuc = new ModulePerm
                    {
                        IsCreate = true,
                        Module = (int)IModule.GiaiQuyetThuTuc
                    }.Enabled(CUser);
                }
                return _isGiaiQuyetThuTuc;
            }
        }
        #endregion

        private List<object> _messages = new List<object>();
        protected void SetMessage(object message)
        {
            _messages.Add(message);
        }
        protected void SetMessages(List<object> messages)
        {
            _messages.AddRange(messages);
        }

        protected JsonResult<APIEntity> SuccessResult(object data = null, object message = null)
        {
            if (Utils.IsNotEmpty(message))
            {
                _messages.Add(message);
            }
            return Json(new APIEntity
            {
                Status = 1,
                Data = data ?? new object(),
                Messages = _messages
            });
        }
        protected JsonResult<APIEntity> ErrorResult(object message = null, object data = null)
        {
            if (Utils.IsNotEmpty(message))
            {
                _messages.Add(message);
            }
            return Json(new APIEntity
            {
                Status = 0,
                Data = data ?? new object(),
                Messages = _messages
            });
        }
        protected bool InvalidRequest(dynamic obj)
        {
            string[] keys = HttpContext.Current.Request.Form.AllKeys;
            string[] keysFile = HttpContext.Current.Request.Files.AllKeys;
            PropertyInfo[] properties = obj.GetType().GetProperties();
            var keyRequest = 0;
            for (int j = 0; j < (int)properties.Length; j++)
            {
                var property = properties[j];
                if (keys.Contains(property.Name) || keysFile.Contains(property.Name))
                {
                    keyRequest++;
                    if (keyRequest >= 1)
                        return false;
                }
            }
            return true;
        }
        protected object RenderMyDoc(MyDoc myDoc, Dictionary<long, isPin> dicPin, List<User> users, List<MyDoc> folders, List<UserRecent> userrecents = null)
        {
            return new
            {
                ID = myDoc.ID,
                Path = GlobalConfig.StgSrcPath(myDoc.Path),
                Size = Utils.SizeConvert(myDoc.Size),
                Name = myDoc.Name,
                Created = CUtils.DateTimeToLong(myDoc.Created),
                CreateBy = (users.FirstOrDefault(t => t.ID == myDoc.CreatedBy) ?? new User()).Name ?? "",
                Updated = userrecents == null ? CUtils.DateTimeToLong(myDoc.Updated) : CUtils.DateTimeToLong(userrecents.FirstOrDefault(t => t.IDDoc == myDoc.ID).Created),
                UpdateBy = (users.FirstOrDefault(t => t.ID == myDoc.UpdatedBy) ?? new User()).Name ?? "",
                IsFolder = myDoc.IsFolder,
                Icon = GlobalConfig.StgSrcPath((myDoc.Extension ?? "").Replace(".", "") + ".png"),
                IsView = true,
                IsUpdate = true,
                IsDelete = true,
                IsCreate = true,
                IsCopy = true,
                IsDownload = true,
                IsMove = true,
                IsShare = true,
                IsNote = true,
                IsPassword = !string.IsNullOrEmpty(myDoc.Password),
                IsSetPassword = CUser.ID == myDoc.CreatedBy,
                IsChangePassword = CUser.ID == myDoc.CreatedBy,
                TypeExtension = CUtils.GetTypebyExtension(myDoc.Extension),
                Parent = myDoc.Parent,
                IsPin = (int)dicPin[myDoc.ID],
                PasswordParentId = GetPasswordParent(folders, myDoc)
            };
        }
        protected object RenderShareDoc(MyDoc myDoc, Dictionary<long, isPin> dicPin, List<User> users, MyDocPerm myDocPerm, bool isQcn, List<MyDoc> folders, List<UserRecent> userrecents = null)
        {
            return new
            {
                ID = myDoc.ID,
                Path = GlobalConfig.StgSrcPath(myDoc.Path),
                Size = Utils.SizeConvert(myDoc.Size),
                Name = myDoc.Name,
                Created = CUtils.DateTimeToLong(myDoc.Created),
                CreateBy = (users.FirstOrDefault(t => t.ID == myDoc.CreatedBy) ?? new User()).Name ?? "",
                Updated = userrecents == null ? CUtils.DateTimeToLong(myDoc.Updated) : CUtils.DateTimeToLong(userrecents.FirstOrDefault(t => t.IDDoc == myDoc.ID).Created),
                UpdateBy = (users.FirstOrDefault(t => t.ID == myDoc.UpdatedBy) ?? new User()).Name ?? "",
                IsFolder = myDoc.IsFolder,
                Icon = GlobalConfig.StgSrcPath((myDoc.Extension ?? "").Replace(".", "") + ".png"),
                IsView = myDocPerm.IsView(myDoc),
                IsUpdate = myDocPerm.IsUpdate(myDoc),
                IsDelete = myDocPerm.IsDelete(myDoc),
                IsCreate = myDocPerm.IsCreate(myDoc),
                IsCopy = myDocPerm.IsCopy(myDoc),
                IsDownload = myDocPerm.IsDownload(myDoc),
                IsMove = myDocPerm.IsMove(myDoc),
                IsShare = myDocPerm.IsShare(myDoc),
                IsNote = myDocPerm.IsNote(myDoc),
                IsPassword = !string.IsNullOrEmpty(myDoc.Password),
                IsSetPassword = CUser.ID == myDoc.CreatedBy,
                IsChangePassword = CUser.ID == myDoc.CreatedBy,
                TypeExtension = CUtils.GetTypebyExtension(myDoc.Extension),
                Parent = myDoc.Parent,
                IsPin = (int)dicPin[myDoc.ID],
                PasswordParentId = GetPasswordParent(folders, myDoc)
            };
        }
        protected object RenderStgDoc(StgDoc stgDoc, Dictionary<long, isPin> dicPin, List<User> users, StgDocPerm stgDocPerm, bool isQlk, List<StgDoc> folders, List<UserRecent> userrecents = null)
        {
            return new
            {
                ID = stgDoc.ID,
                Path = GlobalConfig.StgSrcPath(stgDoc.Path),
                Size = Utils.SizeConvert(stgDoc.Size),
                Name = stgDoc.Name,
                Created = CUtils.DateTimeToLong(stgDoc.Created),
                CreateBy = (users.FirstOrDefault(t => t.ID == stgDoc.CreatedBy) ?? new User()).Name ?? "",
                Updated = userrecents == null ? CUtils.DateTimeToLong(stgDoc.Updated) : CUtils.DateTimeToLong(userrecents.FirstOrDefault(t => t.IDDoc == stgDoc.ID).Created),
                UpdateBy = (users.FirstOrDefault(t => t.ID == stgDoc.UpdatedBy) ?? new User()).Name ?? "",
                IsFolder = stgDoc.IsFolder,
                Icon = GlobalConfig.StgSrcPath((stgDoc.Extension ?? "").Replace(".", "") + ".png"),
                IsView = isQlk || stgDocPerm.IsView(stgDoc),
                IsUpdate = isQlk || stgDocPerm.IsUpdate(stgDoc),
                IsDelete = isQlk || stgDocPerm.IsDelete(stgDoc),
                IsCreate = isQlk || stgDocPerm.IsCreate(stgDoc),
                IsCopy = isQlk || stgDocPerm.IsCopy(stgDoc),
                IsDownload = isQlk || stgDocPerm.IsDownload(stgDoc),
                IsMove = isQlk || stgDocPerm.IsMove(stgDoc),
                IsShare = isQlk || stgDocPerm.IsShare(stgDoc),
                IsNote = isQlk || stgDocPerm.IsNote(stgDoc),
                IsPassword = !string.IsNullOrEmpty(stgDoc.Password),
                IsSetPassword = isQlk,
                IsChangePassword = isQlk,
                TypeExtension = CUtils.GetTypebyExtension(stgDoc.Extension),
                Parent = stgDoc.Parent,
                IsPin = (int)dicPin[stgDoc.ID],
                PasswordParentId = GetPasswordParent(folders, stgDoc)
            };
        }
        private long GetPasswordParent(List<StgDoc> stgDocs, StgDoc stgDoc)
        {
            var parent = stgDocs.FirstOrDefault(t => t.ID == stgDoc.Parent);
            if (Utils.IsEmpty(parent) || parent.ID == 0)
                return 0;

            return !string.IsNullOrEmpty(parent.Password)
                ? parent.ID
                : GetPasswordParent(stgDocs, parent);
        }
        private long GetPasswordParent(List<MyDoc> myDocs, MyDoc myDoc)
        {
            var parent = myDocs.FirstOrDefault(t => t.ID == myDoc.Parent);
            if (Utils.IsEmpty(parent) || parent.ID == 0)
                return 0;

            return !string.IsNullOrEmpty(parent.Password)
                ? parent.ID
                : GetPasswordParent(myDocs, parent);
        }
        protected Dictionary<long, isPin> GetDicPinDocs(List<StgDoc> stgdocs, StgDoc stgdoc = null)
        {
            var items = new List<StgDoc>();
            if (Utils.IsNotEmpty(stgdocs))
            {
                items.AddRange(stgdocs);
            }
            if (Utils.IsNotEmpty(stgdoc))
            {
                items.Add(stgdoc);
            }

            var dicResult = new Dictionary<long, isPin>();
            var userFavorites = items.Count > 0 ? Repository.UserFavoriteRepository.GetList(CUser.IDChannel, CUser.ID, FavoriteType.Stgfile, items.Select(x => x.ID).ToArray()) : new List<UserFavorite>();
            foreach (var item in items)
            {
                dicResult[item.ID] = userFavorites.Any(x => x.IDDoc == item.ID) ? isPin.Co : isPin.Khong;
            }
            return dicResult;
        }
        protected Dictionary<long, isPin> GetDicPinDoc(StgDoc stgdoc)
        {
            return GetDicPinDocs(null, stgdoc);
        }
        protected Dictionary<long, isPin> GetDicPinDocs(List<MyDoc> mydocs, MyDoc mydoc = null)
        {
            var items = new List<MyDoc>();
            if (Utils.IsNotEmpty(mydocs))
            {
                items.AddRange(mydocs);
            }
            if (Utils.IsNotEmpty(mydoc))
            {
                items.Add(mydoc);
            }

            var dicResult = new Dictionary<long, isPin>();
            var userFavorites = items.Count > 0 ? Repository.UserFavoriteRepository.GetList(CUser.IDChannel, CUser.ID, FavoriteType.Myfile, items.Select(x => x.ID).ToArray()) : new List<UserFavorite>();
            foreach (var item in items)
            {
                dicResult[item.ID] = userFavorites.Any(x => x.IDDoc == item.ID) ? isPin.Co : isPin.Khong;
            }
            return dicResult;
        }
        protected Dictionary<long, isPin> GetDicPinDoc(MyDoc mydoc)
        {
            return GetDicPinDocs(null, mydoc);
        }
        protected void GetDataBeforSearch(Hashtable data)
        {
            var typeDateCreate = Utils.GetInts(data, "TpDateCreate");
            var typeExtension = Utils.GetInts(data, "TypeExtension");
            var idDoctypes = Utils.GetInts(data, "IDDoctype");
            if (!string.IsNullOrEmpty(Utils.GetString(data, "TpDateCreate")) && Utils.IsNotEmpty<int>(typeDateCreate))
            {
                var startDate = DateTime.MinValue;
                var endDate = DateTime.MaxValue;
                CUtils.GetDateFromTypeCreate(typeDateCreate, out startDate, out endDate);
                data.Add("StartDate", Utils.DateToString(startDate, "dd-MM-yyyy"));
                data.Add("EndDate", Utils.DateToString(endDate, "dd-MM-yyyy"));
            }
            if (!string.IsNullOrEmpty(Utils.GetString(data, "TypeExtension")) && Utils.IsNotEmpty<int>(typeExtension))
                data.Add("Extension", CUtils.GetExtensionByType(typeExtension));
            if (!string.IsNullOrEmpty(Utils.GetString(data, "IDDoctype")) && Utils.IsNotEmpty<int>(idDoctypes))
                data["IDDoctype"] = idDoctypes;
            else if (data.ContainsKey("IDDoctype"))
                data.Remove("IDDoctype");
        }

        protected object GetDataAccessPass(Hashtable data)
        {
            data.Add("Url", HttpContext.Current.Request.Url);
            return data;
        }

        #region Render
        protected object RenderCustomers(List<Customer> customers, List<User> users)
        {
            return customers.Select(t => RenderCustomer(t, users));
        }
        protected object RenderCustomer(Customer customer, List<User> users)
        {
            return new
            {
                ID = customer.ID,
                Name = customer.Name,
                Email = customer.Email,
                Phone = customer.Phone,
                Describe = customer.Describe,
                CreatedBy = (users.FirstOrDefault(x => x.ID == customer.CreatedBy) ?? new User()).Name
            };
        }
        protected object RenderAppointments(List<Appointment> appointments, List<User> users, List<Customer> customers, List<Contact> contacts, List<AppointmentContact> appointmentcontacts)
        {
            return appointments.Select(t => RenderAppointment(t, users, customers, contacts, appointmentcontacts));
        }
        protected object RenderAppointment(Appointment appointment, List<User> users, List<Customer> customers, List<Contact> contacts, List<AppointmentContact> appointmentcontacts)
        {
            var appointmentcontact = appointmentcontacts.FirstOrDefault(t => t.IDAppointment == appointment.ID) ?? new AppointmentContact();
            var contact = contacts.FirstOrDefault(t => t.ID == appointmentcontact.IDContact) ?? new Contact();
            return new
            {
                ID = appointment.ID,
                Name = appointment.Name,
                Worked = CUtils.DateTimeToLong(appointment.Worked),
                IDCustomer = appointment.IDCustomer,
                Describe = appointment.Describe,
                Customer = (customers.FirstOrDefault(x => x.ID == appointment.IDCustomer) ?? new Customer()).Name,
                ResultSale = appointment.ResultSale,
                ResultSaleContent = appointment.ResultSaleContent,
                ResultBDH = appointment.ResultBDH,
                ResultBDHContent = appointment.ResultBDHContent,
                CreatedBy = (users.FirstOrDefault(x => x.ID == appointment.CreatedBy) ?? new User()).Name,
                ConactName = contact.Name,
                ContactEmail = contact.Email,
                ContactPhone = contact.Phone,
                ContactPosition = contact.Position,
                ContactAddress = contact.Address,
                IsBDH = GlobalConfig.IsAccess(CUser, IModule.QuanLyTatCaDanhBaKhachHang)
            };
        }
        protected object RenderContacts(List<Contact> contacts, List<User> users, List<Customer> customers)
        {
            return contacts.Select(t => RenderContact(t, users, customers));
        }
        protected object RenderContact(Contact contact, List<User> users, List<Customer> customers)
        {
            return new
            {
                ID = contact.ID,
                IDCustomer = contact.IDCustomer,
                Customer = (customers.FirstOrDefault(x => x.ID == contact.IDCustomer) ?? new Customer()).Name,
                Name = contact.Name,
                Email = contact.Email,
                Phone = contact.Phone,
                Describe = contact.Describe,
                Address = contact.Address,
                Position = contact.Position,
                CreatedBy = (users.FirstOrDefault(x => x.ID == contact.CreatedBy) ?? new User()).Name
            };
        }
        #endregion
    }
}
