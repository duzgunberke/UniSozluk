using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniSozluk.Infrastructure.Persistence.Context;

namespace UniSozluk.Infrastructure.Persistence.EntityConfigurations.Entry
{
    public class EntryFavoriteEntityConfiguration:BaseEntityConfiguration<Api.Domain.Models.EntryFavorite>
    {
        public override void Configure(EntityTypeBuilder<Api.Domain.Models.EntryFavorite> builder)
        {
            base.Configure(builder);

            builder.ToTable("entryfavorite",UniSozlukContext.DEFAULT_SCHEMA);

            builder.HasOne(i => i.Entry)
                .WithMany(i => i.EntryFavorites)
                .HasForeignKey(i => i.EntryId);

            builder.HasOne(i => i.CreatedUser)
              .WithMany(i => i.EntryFavorites)
              .HasForeignKey(i => i.CreatedById);
        }
    }
}
