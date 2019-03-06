using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Wiki.Models
{
    public class Person
    {
        public int PersonId { get; set; }
        public string PersonName { get; set; }

        public virtual ICollection<Course> Courses { get; set; }
    }
}