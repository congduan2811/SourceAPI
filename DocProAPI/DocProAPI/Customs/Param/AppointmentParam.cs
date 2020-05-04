using DocProModel.Customs.Params;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DocProAPI.Customs.Param
{
    public class AppointmentParam : SearchParam
    {
        public int IDCustomer { get; set; }
    }
}