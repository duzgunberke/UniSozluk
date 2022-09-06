using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniSozluk.Api.Domain.Models;
using UniSozluk.Infrastructure.Persistence.Context;

namespace UniSozluk.Infrastructure.Persistence.EntityConfigurations.EntryComment
{
    public class EntryCommentFavoriteEntityConfiguration:BaseEntityConfiguration<EntryCommentFavorite>
    {
        public override void Configure(EntityTypeBuilder<EntryCommentFavorite> builder)
        {
            base.Configure(builder);
            builder.ToTable("entrycommentfavorite", UniSozlukContext.DEFAULT_SCHEMA);

            builder.HasOne(i => i.EntryComment)
                .WithMany(i => i.EntryCommentFavorites)
                .HasForeignKey(i => i.EntryCommentId);

            builder.HasOne(i => i.CreatedUser)
                .WithMany(i => i.EntryCommentFavorites)
                .HasForeignKey(i => i.CreateById);
        }
    }
}
