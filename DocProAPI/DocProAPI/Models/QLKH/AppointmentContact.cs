using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DocProAPI.Models.QLKH
{
    [PrimaryKey("ID", AutoIncrement = true)]
    [TableName("AppointmentContact")]
    public class AppointmentContact : DocProQLKHDataContext<AppointmentContact>
    {
        [Column]
        public int ID { get; set; }

        [Column]
        public int IDChannel { get; set; }

        [Column]
        public int IDAppointment { get; set; }

        [Column]
        public int  IDContact { get; set; }
        
        [Column]
        public DateTime Created { get; set; }
        
        [Column]
        public int CreatedBy { get; set; }





        [Column]
        public DateTime? Updated { get; set; }





        [Column]
        public int? UpdatedBy { get; set; }


    }
}