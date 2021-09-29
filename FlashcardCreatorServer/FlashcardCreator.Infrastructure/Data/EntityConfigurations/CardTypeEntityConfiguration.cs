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
    public class CardTypeEntityConfiguration : IEntityTypeConfiguration<Card>
    {
        public void Configure(EntityTypeBuilder<Card> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id)
                .IsRequired();
            builder.HasIndex(e => new { e.Word, e.GroupId });
            builder.Property(e => e.Word)
                .IsRequired();
            builder.Property(e => e.Definition)
                .IsRequired();

            builder.HasMany(e => e.UserCardReviews)
                .WithOne(e => e.Card)
                .HasForeignKey(e => e.CardId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
