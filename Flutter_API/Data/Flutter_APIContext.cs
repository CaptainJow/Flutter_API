using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Flutter_API.Model;

namespace Flutter_API.Data
{
    public class Flutter_APIContext : DbContext
    {
        public Flutter_APIContext (DbContextOptions<Flutter_APIContext> options)
            : base(options)
        {
        }

        public DbSet<Flutter_API.Model.User> User { get; set; } = default!;
    }
}
