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
    public class UserCardReviewTypeEntityConfiguration : IEntityTypeConfiguration<UserCardReview>
    {
        public void Configure(EntityTypeBuilder<UserCardReview> builder)
        {
            builder.HasKey(e => new { e.UserId, e.CardId });
            builder.Property(e => e.UserId)
                .IsRequired();
            builder.Property(e => e.CardId)
                .IsRequired();
            builder.HasIndex(e => e.GroupId);
        }
    }
}
