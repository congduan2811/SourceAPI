using DocProUtil;
using System;

namespace DocProAPI.Customs.Helper
{
    public class CheckInvalidHelper
    {
        /// <summary>
        /// Check định dạng email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public static bool CheckEmail(string email, out string message)
        {
            message = string.Empty;
            if (String.IsNullOrEmpty(email))
            {
                message = Locate.T("Email không được để trống");
                return false;
            }
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                if( addr.Address == email)
                {
                    message = Locate.T("Email hợp lệ");
                    return true;
                }
                else
                {
                    message = Locate.T("Email không hợp lệ");
                    return false;
                }
            }
            catch (Exception)
            {
                message = Locate.T("Email không hợp lệ");
                return false;
            }
        }

    }
}