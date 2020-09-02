using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CSharpDuels.Models
{
    public class DuelEntityConfig : IEntityTypeConfiguration<UserDuel>
    {
        public void Configure(EntityTypeBuilder<UserDuel> builder)
        {
            builder.HasKey(x => x.UserId);
            builder.HasPartitionKey(x => x.UserId);
            builder.Property(x => x.UserId).ToJsonProperty("userId");
            
            builder.OwnsMany(x => x.CompletedDuelsList, cd =>
            {
                cd.ToJsonProperty("completedDuels");
                cd.HasKey(x => new { x.DuelId , x.ChallengeName});
                cd.Property(x => x.DuelId).ToJsonProperty("duelId");
                cd.Property(x => x.ChallengeName).ToJsonProperty("challengeName");
                cd.Property(x => x.RivalId).ToJsonProperty("rivalId");
                cd.Property(x => x.Solution).ToJsonProperty("solution");
                cd.Property(x => x.TimeCompleted).ToJsonProperty("timeCompleted");
                cd.Property(x => x.WonDuel).ToJsonProperty("wonDuel");
            });
            builder.ToContainer("DuelsData");
            builder.Property<string>("_self").ValueGeneratedOnAdd();
            builder.Property<string>("_etag").ValueGeneratedOnAddOrUpdate();
            builder.Property<long>("_ts").ValueGeneratedOnAddOrUpdate();
        }
    }
  
}
