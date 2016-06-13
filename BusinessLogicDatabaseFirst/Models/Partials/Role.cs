using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNet.Identity;

namespace BusinessLogic.Models
{
    public partial class Role : IRole<int>
    {
        public Role(string name) : base()
        {
            this.Name = name;
        }
    }
}
