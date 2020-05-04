using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DocProAPI.Models.QLKH
{
    [PrimaryKey("ID", AutoIncrement = true)]
    [TableName("Contact")]
    public class Contact : DocProQLKHDataContext<Contact>
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
        public string Email { get; set; }

        [Column]
        public string Phone { get; set; }

        [Column]
        public string Describe { get; set; }

        [Column]
        public string Address { get; set; }

        [Column]
        public string Position { get; set; }

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