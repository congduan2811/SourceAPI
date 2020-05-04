using DocProUtil;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Data.SqlTypes;
using System.Net;
using System.IO;
using DocProAPI.Models;
using DocProModel.Models;
using System.Web.Configuration;
using DocProAPI.Repository;
using System.Text;
using System.Globalization;

namespace DocProAPI.Customs
{
    public static class CUtils
    {
        public static List<StgDoc> SortInStgDoc(List<StgDoc> data, long[] ids)
        {
            var dataSort = new List<StgDoc>();
            foreach (var id in ids)
            {
                var parent = data.Where(x => x.ID == id).SingleOrDefault();
                if (Utils.IsNotEmpty(parent))
                    dataSort.Add(parent);
            }
            return dataSort;
        }
        public static List<MyDoc> SortInMyDoc(List<MyDoc> data, long[] ids)
        {
            var dataSort = new List<MyDoc>();
            foreach (var id in ids)
            {
                var parent = data.Where(x => x.ID == id).SingleOrDefault();
                if (Utils.IsNotEmpty(parent))
                    dataSort.Add(parent);
            }
            return dataSort;
        }
        public static T BindRequestUpdate<T>(this T obj, Hashtable data)
        {
            if (object.Equals(data, null))
            {
                return obj;
            }
            PropertyInfo[] properties = obj.GetType().GetProperties();
            for (int i = 0; i < (int)properties.Length; i++)
            {
                var property = properties[i];
                try
                {
                    var propertyType = property.PropertyType;
                    if (data.ContainsKey(property.Name))
                    {
                        if (propertyType == typeof(string))
                        {
                            string value = Utils.GetString(data, property.Name, false);
                            property.SetValue(obj, value);
                        }
                        else if (propertyType == typeof(int))
                        {
                            int value = Utils.GetInt(data, property.Name);
                            property.SetValue(obj, value);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Loger.Log(ex, "BindRequestUpdate");
                }
            }
            return obj;
        }
        public static DateTime? ConvertLongToDateTime(string date)
        {
            try
            {
                var dateConvert = ConvertUnixEpochTime(Convert.ToInt64(date));
                if (ValidateDateTime(dateConvert))
                    return dateConvert;
                else
                    return null;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public static bool ValidateDateTime(DateTime date)
        {
            try
            {
                if (date > SqlDateTime.MinValue && date < SqlDateTime.MaxValue)
                    return true;
                else return false;
            }
            catch
            {
                return false;
            }
        }
        public static DateTime ConvertUnixEpochTime(long milliseconds)
        {
            DateTime Fecha = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return Fecha.ToLocalTime().AddMilliseconds(milliseconds);
        }
        public static long DateTimeToLong(DateTime? date)
        {
            try
            {
                if (Utils.IsNotEmpty(date))
                    return Convert.ToInt64(date.Value.ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds);
                else
                    return 0;
            }
            catch (Exception)
            {
                return 0;
            }

        }
        public static void LogRequest()
        {
            string[] keys = HttpContext.Current.Request.Form.AllKeys;
            for (int i = 0; i < keys.Length; i++)
            {
                Loger.Log(HttpContext.Current.Request.Form[keys[i]], keys[i]);
            }
            string[] keysFile = HttpContext.Current.Request.Files.AllKeys;
            for (int i = 0; i < keysFile.Length; i++)
            {
                Loger.Log(HttpContext.Current.Request.Files[keysFile[i]].FileName, HttpContext.Current.Request.Files[keysFile[i]].FileName);
            }
        }
        public static string SaveFileToStorage(HttpPostedFileBase file)
        {
            var url = WebConfigurationManager.AppSettings["DomainDocPro"] + "/uploader/upfile";
            int filelength = file.ContentLength;
            byte[] imagebytes = new byte[filelength];
            file.InputStream.Read(imagebytes, 0, filelength);
            var path =UploadFilesToRemoteUrl(url, imagebytes, "FileDocument", file.FileName, file.ContentType);
            return path;
        }
        public static string UploadFilesToRemoteUrl(string url, byte[] files, string paramName, string fileName, string contentType)
        {
            var pathFile = string.Empty;
            string boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");
            byte[] boundaryBytes = System.Text.Encoding.ASCII.GetBytes("--" + boundary + "\r\n");
            HttpWebRequest webRequest = (HttpWebRequest)HttpWebRequest.Create(url);
            webRequest.ContentType = "multipart/form-data; boundary=" + boundary;
            webRequest.Method = "POST";
            try
            {
                using (Stream requestStream = webRequest.GetRequestStream())
                {
                    requestStream.Write(boundaryBytes, 0, boundaryBytes.Length);
                    string headerTemplate = "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: {2}\r\n\r\n";
                    string header = string.Format(headerTemplate, paramName, fileName, contentType);
                    byte[] headerbytes = System.Text.Encoding.UTF8.GetBytes(header);
                    requestStream.Write(headerbytes, 0, headerbytes.Length);
                    using (MemoryStream memoryStream = new MemoryStream(files))
                    {
                        byte[] buffer = new byte[4096];
                        int bytesRead = 0;
                        while ((bytesRead = memoryStream.Read(buffer, 0, buffer.Length)) != 0)
                        {
                            requestStream.Write(buffer, 0, bytesRead);
                        }
                    }
                    byte[] trailerBytes = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");
                    requestStream.Write(trailerBytes, 0, trailerBytes.Length);
                }
                using (HttpWebResponse wr = (HttpWebResponse)webRequest.GetResponse())
                {
                    using (Stream response = wr.GetResponseStream())
                    {
                        using (StreamReader reader2 = new StreamReader(response))
                        {
                            var uploadFileModel = Utils.Deserialize<UploadFileModel>(reader2.ReadToEnd());
                            if (Utils.IsNotEmpty(uploadFileModel))
                                return uploadFileModel.FilePath;
                        }
                    }
                }
            }
            catch (Exception)
            {
                return pathFile;
            }

            return pathFile;
        }
        public static bool CompareObject<T>(T Object1, T object2)
        {
            Type type = typeof(T);
            if (Object1 == null || object2 == null)
                return false;
            foreach (System.Reflection.PropertyInfo property in type.GetProperties())
            {
                if (property.Name != "ExtensionData")
                {
                    string Object1Value = string.Empty;
                    string Object2Value = string.Empty;
                    if (type.GetProperty(property.Name).GetValue(Object1, null) != null)
                        Object1Value = type.GetProperty(property.Name).GetValue(Object1, null).ToString();
                    if (type.GetProperty(property.Name).GetValue(object2, null) != null)
                        Object2Value = type.GetProperty(property.Name).GetValue(object2, null).ToString();
                    if (Object1Value.Trim() != Object2Value.Trim())
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        public static int GetTypebyExtension(string extend)
        {
            extend = extend ?? "";
            extend = extend.ToLower();
            switch (extend)
            {
                case ".pdf":
                    return 1;
                case ".bmp":
                    return 2;
                case ".doc":
                case ".docx":
                    return 3;
                case "folder":
                    return 4;
                case ".gif":
                    return 5;
                case ".html":
                    return 6;
                case ".jpg":
                    return 7;
                case ".mp3":
                    return 8;
                case ".mp4":
                    return 9;
                case ".png":
                    return 10;
                case ".ppt":
                case ".pptx":
                    return 11;
                case ".rar":
                    return 12;
                case ".tip":
                    return 13;
                case ".txt":
                    return 14;
                case ".xls":
                case ".xlsx":
                    return 15;
                case ".xml":
                    return 16;
                case ".zip":
                    return 17;
                default:
                    return 100;
            }
        }
        public static string[] GetExtensionByType(int[] types)
        {
            List<string> results = new List<string>();
            foreach (var item in types)
            {
                var result = GetExtensionByType(item);
                if (Utils.IsNotEmpty<string>(result))
                {
                    results.AddRange(result);
                }
            }
            return results.ToArray();
        }
        public static List<string> GetExtensionByType(int type)
        {
            string result = string.Empty;
            switch (type)
            {
                case 1: // PDF
                    result = ".pdf";
                    break;
                case 2: // Văn bản
                    result = ".doc,.docx,.txt";
                    break;
                case 3: // Cột tính/ excel
                    result = ".xls,.xlsx";
                    break;
                case 4: // Trình chiếu / Powpoint
                    result = ".ppt,.pptx";
                    break;
                case 5: // Ảnh
                    result = ".jpeg,.tiff,.gif,.bmp,.png,.ppm,.pgm,.pnm,.webp,.hdr,.heif,.bat,.bpg,.jpg";
                    break;
                case 6: // Video
                    result = ".webm,.mkv,.flv,.vob,.ogv,.ogg,.drc,.gif,.gilv,.mng,.avi,.mov,.qt,.avi,.mov,.qt,.wmv,.yuv,.rm,.rmvb,.asf,.mp4,.m4p,.m4v,.mpg,.mp2,.mpeg,.mpe,.mpv,.m4v,.svi,.3gp,.3g2,.mxf,.roq,.flv,.f4p";
                    break;
                case 7://Âm thanh
                    result = ".mp3,.3gp,.mpc,.tta,.vox,.wav,.wma,.wv,.webm,.8svx";
                    break;
                case 8:// project
                    result = ".mpp,.mpx,.mpd";
                    break;
                case 9: // File nén
                    result = ".7z,.s7z,.ace,.afa,.alz,.apk,.arc,.rar,.tar.gz,.tar.bz2,.zip,.zoo,.zz,.zpaq";
                    break;
                case 10: // folder
                    result = "folder";
                    break;
                default: //Tất cả
                    result = ".fsiextention";
                    break;
            }
            return result.Split(',').ToList();
        }
        public static void GetDateFromTypeCreate(int[] types, out DateTime sDate, out DateTime eDate)
        {
            sDate = DateTime.Now;
            eDate = DateTime.Now;
            bool isFirst = true;
            foreach (var item in types)
            {
                DateTime startDate, endDate;
                GetDateFromTypeCreate(item, out startDate, out endDate);
                if (isFirst)
                {
                    sDate = startDate;
                    eDate = endDate;
                }
                else
                {
                    if (startDate < sDate)
                        sDate = startDate;
                    if (endDate > eDate)
                        eDate = endDate;
                }
                isFirst = false;
            }

        }
        /// <summary>
        /// Lấy thời gian bắt đầu tạo và kết thúc tạo theo loại ngày tạo
        /// </summary>
        /// <param name="type">Loại tạo</param>
        /// <param name="startDate">Ngày bắt đầu tạo</param>
        /// <param name="endDate">Ngày kết thúc tạo</param>
        public static void GetDateFromTypeCreate(int type, out DateTime startDate, out DateTime endDate)
        {
            var now = DateTime.Now;
            startDate = now.AddYears(-100);
            endDate = now.AddYears(-100);
            switch (type)
            {
                case 1://Hôm nay
                    {
                        startDate = GetDateTimeStart(now);
                        endDate = GetDateTimeEnd(now);
                    }
                    break;
                case 2: //Hôm qua
                    {
                        startDate = GetDateTimeStart(now.AddDays(-1));
                        endDate = GetDateTimeEnd(now.AddDays(-1));
                    }
                    break;
                case 3: //7 ngày qua
                    {
                        startDate = GetDateTimeStart(now.AddDays(-7));
                        endDate = GetDateTimeEnd(now);
                    }
                    break;
                case 4: //30 ngày qua
                    {
                        startDate = GetDateTimeStart(now.AddDays(-30));
                        endDate = GetDateTimeEnd(now);
                    }
                    break;
                case 5: //90 ngày qua
                    {
                        startDate = GetDateTimeStart(now.AddDays(-90));
                        endDate = GetDateTimeEnd(now);
                    }
                    break;
            }

        }
        private static DateTime GetDateTimeStart(DateTime dt)
        {
            return new DateTime(dt.Year, dt.Month, dt.Day, 0, 0, 0);
        }
        private static DateTime GetDateTimeEnd(DateTime dt)
        {
            return new DateTime(dt.Year, dt.Month, dt.Day, 23, 59, 59);
        }
        public static Dictionary<int, string> DefineType()
        {
            Dictionary<int, string> dic = new Dictionary<int, string>();
            dic.Add(1, "PDF");
            dic.Add(2, "Tài liệu văn bản");
            dic.Add(3, "Bảng tính (Excel)");
            dic.Add(4, "PowPoint");
            dic.Add(5, "Ảnh và hình ảnh");
            dic.Add(6, "Video");
            dic.Add(7, "Project");
            dic.Add(8, "Tệp nén");
            dic.Add(9, "Thư mục");
            dic.Add(0, "Tất cả");
            return dic;
        }
        public static string RandomMath(int length)
        {
            Random random = new Random();
            const string chars = "0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        public static string InscreaseName(string input)
        {
            string result = string.Empty;
            int inscre = 1;
            result = input + inscre;
            if (!UserRepository.Instance.UsernameExists(result))
            {
                return result;
            }
            inscre++;
            result = input + inscre;
            while (UserRepository.Instance.UsernameExists(result))
            {
                inscre++;
                result = input + inscre;
            }
            return result;
        }
        /// <summary>
        /// Chuyển đổ tên sang tên viết tắt pham van dac ==? dacpv
        /// </summary>
        /// <param name="input">Tên đầu vào</param>
        /// <returns></returns>
        public static String ConvertBySortName(string input)
        {
            string result = string.Empty;
            while (input.Contains("  "))
                input = input.Replace("  ", " ");
            var arrs = input.Split(' ');
            if (!arrs.Any())
                return string.Empty;
            int lenght = arrs.Length;
            var name = arrs.LastOrDefault();
            var firstname = arrs.FirstOrDefault();
            var shortname = string.Empty;
            firstname = firstname.Substring(0, 1).ToLower();
            if (arrs.Length == 1)
                return name;
            else
            {
                result = name + firstname;
                for (int i = 0; i < lenght; i++)
                {
                    if (i == 0 || i == lenght - 1)
                        continue;
                    result += arrs[i].Substring(0, 1).ToLower();
                }
            }
            return RemoveDiacritics(result);
        }
        public static string RandomString(int length)
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        public static string RemoveDiacritics(string text)
        {
            string formD = text.Normalize(NormalizationForm.FormD);
            StringBuilder sb = new StringBuilder();

            foreach (char ch in formD)
            {
                UnicodeCategory uc = CharUnicodeInfo.GetUnicodeCategory(ch);
                if (uc != UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(ch);
                }
            }
            return sb.ToString().Normalize(NormalizationForm.FormC);
        }

        public static bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}