using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using BankConsoleApp.Entities;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.EntityFrameworkCore;
using BankConsoleApp.DAL.Configurations;



namespace BankConsoleApp.DAL
{
    public class BankDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Card> Cards { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            DbContextConfiguration.Configure(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Transaction>()
                .HasOne(b => b.forCard)
                .WithMany(u => u.transactions)
                .HasForeignKey(b => b.forCardId);
            modelBuilder.Entity<Card>()
                .HasOne(c => c.UserCardOwner)
                .WithMany(u => u.CardList)
                .HasForeignKey(c => c.UserCardOwnerId);

            modelBuilder.Entity<Card>().HasData(
            new Card
            {
                Id = 1,
                CardNumber = "1234567812345678",
                HolderName = "Ali Ahmadi",
                Balance = 1000.0f,
                IsActive = true,
                Password = "password123",
                TryToEnterTimes = 0,
                UserCardOwnerId = 1
            },
            new Card
            {
                Id = 2,
                CardNumber = "2345678923456789",
                HolderName = "Maryam Mohammadi",
                Balance = 1500.0f,
                IsActive = true,
                Password = "password456",
                TryToEnterTimes = 0,
                UserCardOwnerId = 1
            },
            new Card
            {
                Id = 3,
                CardNumber = "3456789034567890",
                HolderName = "Reza Hosseini",
                Balance = 2000.0f,
                IsActive = true,
                Password = "password789",
                TryToEnterTimes = 0,
                UserCardOwnerId = 1
            },
            new Card
            {
                Id = 4,
                CardNumber = "4567890145678901",
                HolderName = "Sara Karimi",
                Balance = 2500.0f,
                IsActive = true,
                Password = "password101",
                TryToEnterTimes = 0,
                UserCardOwnerId = 1
            },
            new Card
            {
                Id = 5,
                CardNumber = "5678901256789012",
                HolderName = "Hossein Soltani",
                Balance = 3000.0f,
                IsActive = true,
                Password = "password202",
                TryToEnterTimes = 0,
                UserCardOwnerId = 1
            }
        );

            modelBuilder.Entity<User>().HasData(
               new User
               {
                   Id = 1,
                   Firstname = "Ali",
                   Lastname = "Ahmadi",
                   Username = "ali.ahmadi",
                   Password = "ali123",
                   phone = "09197980113"
               }
            );
        
           base.OnModelCreating(modelBuilder);
        }
    }
}
