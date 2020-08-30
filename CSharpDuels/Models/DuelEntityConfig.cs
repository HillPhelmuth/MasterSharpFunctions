using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CSharpDuels.Models
{
    public class DuelEntityConfig : IEntityTypeConfiguration<DuelModel>
    {
        public void Configure(EntityTypeBuilder<DuelModel> builder)
        {
            builder.Property(x => x.Id).ToJsonProperty("id");
            builder.HasKey(x => x.UserId);
            builder.HasPartitionKey(x => x.UserId);
            builder.OwnsMany(x => x.CompletedDuels, cd =>
            {
                cd.ToJsonProperty("Completed");
                cd.Property(x => x.DuelId).ToJsonProperty("DuelId");
                cd.Property(x => x.ChallengeName).ToJsonProperty("ChallengeName");
                cd.Property(x => x.RivalId).ToJsonProperty("RivalId");
                cd.Property(x => x.Solution).ToJsonProperty("Solution");
                cd.Property(x => x.Attempts).ToJsonProperty("Attempts");
                cd.Property(x => x.WonDuel).ToJsonProperty("WonDuel");
               
            });
            builder.ToContainer("UserDuels");
            builder.Property<string>("_self").ValueGeneratedOnAdd();
            builder.Property<string>("_etag").ValueGeneratedOnAddOrUpdate();
            builder.Property<long>("_ts").ValueGeneratedOnAddOrUpdate();
        }
    }
}
