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
    public class UserTypeEntityConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id)
                .IsRequired();
            builder.HasIndex(e => e.Email);
            builder.Property(e => e.Email)
                .IsRequired();
            builder.Property(e => e.FullName)
                .HasMaxLength(64)
                .IsRequired();
            builder.Property(e => e.Role)
                .IsRequired();

            builder.HasMany(e => e.UserLogins)
                .WithOne(e => e.User)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(e => e.CardGroups)
                .WithOne(e => e.User)
                .HasForeignKey(e => e.OwnerId)
                .OnDelete(DeleteBehavior.NoAction);
            builder.HasMany(e => e.UserCardGroups)
                .WithOne(e => e.User)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(e => e.UserCardReviews)
                .WithOne(e => e.User)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
