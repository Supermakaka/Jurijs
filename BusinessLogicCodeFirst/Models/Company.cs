using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace BusinessLogic.Models
{
    public class Company
    {
        public int Id { get; set; }

        [Required]
        public virtual string Name { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}
