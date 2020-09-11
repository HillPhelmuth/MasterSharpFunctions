using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SharedModels;

namespace CSharpDuels.Models
{
    public class DuelEntityConfig : IEntityTypeConfiguration<Arena>
    {
        public void Configure(EntityTypeBuilder<Arena> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasPartitionKey(x => x.Id);
            builder.Ignore(x => x.CurrentChallenge);
            builder.Property(x => x.Id).ToJsonProperty("id");
            builder.ToContainer("ActiveArenas");
            
            //builder.OwnsMany(x => x.CompletedDuelsList, cd =>
            //{
            //    cd.ToJsonProperty("completedDuels");
            //    cd.HasKey(x => new { x.DuelId , x.ChallengeName});
            //    cd.Property(x => x.DuelId).ToJsonProperty("duelId");
            //    cd.Property(x => x.ChallengeName).ToJsonProperty("challengeName");
            //    cd.Property(x => x.RivalId).ToJsonProperty("rivalId");
            //    cd.Property(x => x.Solution).ToJsonProperty("solution");
            //    cd.Property(x => x.TimeCompleted).ToJsonProperty("timeCompleted");
            //    cd.Property(x => x.WonDuel).ToJsonProperty("wonDuel");
            //});


        }
    }
  
}
