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
    public class UserCardGroupTypeEntityConfiguration : IEntityTypeConfiguration<UserCardGroups>
    {
        public void Configure(EntityTypeBuilder<UserCardGroups> builder)
        {
            builder.HasKey(e => new { e.GroupId, e.UserId });
            builder.Property(e => e.GroupId)
                .IsRequired();
            builder.Property(e => e.UserId)
                .IsRequired();
        }
    }
}
