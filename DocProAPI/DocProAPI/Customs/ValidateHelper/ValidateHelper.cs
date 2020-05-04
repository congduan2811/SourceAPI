using DocProUtil;
using System;
using System.Data.SqlTypes;
using System.Net.Mail;

namespace DocProAPI.Customs.ValidateHelper
{
    public static class ValidateHelper
    {
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
        public static bool ValidateEmail(string email)
        {
            try
            {
                if (Utils.IsNotEmpty(email))
                {
                    var mailAddress = new MailAddress(email);
                    return true;
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        //public static bool ValidatePhone(string phone)
        //{

        //}
    }
}