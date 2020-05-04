using System.ComponentModel;

namespace DocProAPI.Customs.Enum
{
    public enum ResultSale
    {
        [Description("Chưa xác định")]
        ChuaXacDinh = 0,
        [Description("Có nhu cầu")]
        CoNhuCau = 1,
        [Description("Không có nhu cầu")]
        KhongCoNhuCau = 2,
    }
    public enum ResultBDH
    {
        [Description("Chưa xác định")]
        ChuaXacDinh = 0,
        [Description("Có nhu cầu")]
        CoNhuCau = 1,
        [Description("Không có nhu cầu")]
        KhongCoNhuCau = 2,
    }
    public class Enumtype
    {
        public enum TypeDoc
        {
            Kho=1,
            CaNhan=2,
            Sohoa=3,
            ChiaSe=4
        }
        public enum isPin
        {
            Co=1,
            Khong=0

        }
        public enum CodeMessage
        {
            Loi
        }
       
    }
}