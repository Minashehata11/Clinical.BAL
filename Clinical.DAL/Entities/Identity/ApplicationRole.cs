using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinical.DAL.Entities.Identity
{
    public class ApplicationRole:IdentityRole
    {
        public DateTime createdAt { get; set; } = DateTime.Now;
    }
}
