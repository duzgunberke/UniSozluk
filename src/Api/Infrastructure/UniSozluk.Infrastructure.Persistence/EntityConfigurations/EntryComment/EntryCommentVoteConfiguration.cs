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
    public class EntryCommentVoteConfiguration:BaseEntityConfiguration<EntryCommentVote>
    {
        public override void Configure(EntityTypeBuilder<EntryCommentVote> builder)
        {
            base.Configure(builder);

            builder.ToTable("entrycommentvote", UniSozlukContext.DEFAULT_SCHEMA);

            builder.HasOne(i => i.EntryComment)
                .WithMany(i => i.EntryCommentVotes)
                .HasForeignKey(i => i.EntryCommentId);
        }
    }
}
