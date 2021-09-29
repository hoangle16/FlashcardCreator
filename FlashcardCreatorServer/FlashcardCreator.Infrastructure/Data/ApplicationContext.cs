using FlashcardCreator.Domain.Entities;
using FlashcardCreator.Infrastructure.Data.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashcardCreator.Infrastructure.Data
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserLogin> UserLogins { get; set; }
        public virtual DbSet<Card> Cards { get; set; }
        public virtual DbSet<CardGroup> CardGroups { get; set; }
        public virtual DbSet<UserCardGroups> UserCardGroups { get; set; }
        public virtual DbSet<UserCardReview> UserCardReviews { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserTypeEntityConfiguration());
            modelBuilder.ApplyConfiguration(new UserLoginTypeEntityConfiguration());
            modelBuilder.ApplyConfiguration(new CardGroupTypeEntityConfiguration());
            modelBuilder.ApplyConfiguration(new CardTypeEntityConfiguration());
            modelBuilder.ApplyConfiguration(new UserCardGroupTypeEntityConfiguration());
            modelBuilder.ApplyConfiguration(new UserCardReviewTypeEntityConfiguration());
        }
    }
}
