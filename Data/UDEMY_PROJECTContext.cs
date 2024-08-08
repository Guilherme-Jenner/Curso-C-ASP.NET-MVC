using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UDEMY_PROJECT.Models;

namespace UDEMY_PROJECT.Data
{
    public class UDEMY_PROJECTContext : DbContext
    {
        public UDEMY_PROJECTContext (DbContextOptions<UDEMY_PROJECTContext> options)
            : base(options)
        {
        }

        public DbSet<Department> Department { get; set; } = default!;
        public DbSet<Seller> Seller { get; set; } = default!;
        public DbSet<SalesRecord> SalesRecords { get; set; } = default!;

    }
}
