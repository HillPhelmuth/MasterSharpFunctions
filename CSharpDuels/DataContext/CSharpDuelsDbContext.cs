using System;
using System.Collections.Generic;
using System.Text;
using CSharpDuels.Models;
using Microsoft.EntityFrameworkCore;

namespace CSharpDuels.DataContext
{
    public class CSharpDuelsDbContext : DbContext
    {
        public DbSet<DuelModel> Duels { get; set; }
        public CSharpDuelsDbContext(DbContextOptions<CSharpDuelsDbContext> options)
            :base(options){}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new DuelEntityConfig());
            
        }
    }
}
