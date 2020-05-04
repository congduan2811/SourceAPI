using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DocProAPI.Models
{
    public class DocProQLKHDataContext<T> : DataContext<T> where T : class, new()
    {
        public DocProQLKHDataContext()
            : base("QLKHConnectionString")
        {
        }
    }
}