using Microsoft.EntityFrameworkCore;
using TransactionalDomain.Entities;

namespace TransactionalDal
{
    public class TransactionsContext : DbContext
    {
        public TransactionsContext(DbContextOptions<TransactionsContext> options) : base(options)
        {

        }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<AccountType> AccountTypes { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<TransactionType> TransactionTypes { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) 
        {
            modelBuilder.Entity<Account>().ToTable("Account");
            modelBuilder.Entity<AccountType>().ToTable("AccountType")
                .HasData(
                 new { Id=1, Name="Ahorros" }, 
                 new { Id=2, Name="Corriente" }
                );
            modelBuilder.Entity<Client>().ToTable("Client").Navigation(x=>x.User).AutoInclude();
            modelBuilder.Entity<Transaction>().ToTable("Transaction");
            modelBuilder.Entity<TransactionType>().ToTable("TransactionType")
                .HasData(
                 new { Id = 1, Name = "Retiro" },
                 new { Id = 2, Name = "Deposito" }
                ); ;
            modelBuilder.Entity<User>().ToTable("User");
        }

    }
}