using FlashcardCreator.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashcardCreator.Infrastructure.Data.EntityConfigurations
{
    public class UserLoginTypeEntityConfiguration : IEntityTypeConfiguration<UserLogin>
    {
        public void Configure(EntityTypeBuilder<UserLogin> builder)
        {
            builder.HasKey(e => new { e.LoginProvider, e.ProviderKey });
            builder.Property(e => e.LoginProvider)
                .IsRequired();
            builder.Property(e => e.ProviderKey)
                .IsRequired();
            builder.Property(e => e.UserId)
                .IsRequired();
        }
    }
}
