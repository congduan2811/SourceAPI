using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DocProAPI.Models.QLKH
{
    [PrimaryKey("ID", AutoIncrement = true)]
    [TableName("Appointment")]
    public class Appointment : DocProQLKHDataContext<Appointment>
    {
        [Column]
        public int ID { get; set; }

        [Column]
        public int IDChannel { get; set; }

        [Column]
        public int IDCustomer { get; set; }

        [Column]
        public string Name { get; set; }
        [Column]
        public string Describe { get; set; }
        [Column]
        public DateTime Worked { get; set; }

        [Column]
        public int ResultSale { get; set; }


        [Column]
        public string ResultSaleContent { get; set; }



        [Column]
        public int ResultBDH { get; set; }





        [Column]
        public string ResultBDHContent { get; set; }





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