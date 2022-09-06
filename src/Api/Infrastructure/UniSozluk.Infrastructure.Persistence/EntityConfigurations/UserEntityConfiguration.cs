using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UniSozluk.Api.Domain.Models;
using UniSozluk.Infrastructure.Persistence.Context;

namespace UniSozluk.Infrastructure.Persistence.EntityConfigurations
{
    public class UserEntityConfiguration:BaseEntityConfiguration<User>
    {
        public override void Configure(EntityTypeBuilder<User> builder)
        {
            base.Configure(builder);

            builder.ToTable("user", UniSozlukContext.DEFAULT_SCHEMA);
        }
    }
}
