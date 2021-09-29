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
    public class CardGroupTypeEntityConfiguration : IEntityTypeConfiguration<CardGroup>
    {
        public void Configure(EntityTypeBuilder<CardGroup> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id)
                .IsRequired();
            builder.Property(e => e.Title)
                .IsRequired();
            builder.Property(e => e.Description)
                .IsRequired();
            builder.HasIndex(e => e.OwnerId);

            builder.HasMany(e => e.UserCardGroups)
                .WithOne(e => e.CardGroup)
                .HasForeignKey(e => e.GroupId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(e => e.UserCardReviews)
                .WithOne(e => e.CardGroup)
                .HasForeignKey(e => e.GroupId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(e => e.Cards)
                .WithOne(e => e.CardGroup)
                .HasForeignKey(e => e.GroupId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
