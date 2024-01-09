using BlogAPI.Controllers;
using BlogAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BlogAPI.Data
{
    public class BaseContext : DbContext
    {
        public BaseContext(DbContextOptions<BaseContext> options) : base(options) { }

        public DbSet<Postagem> Postagem { get; set; }
        public DbSet<Usuario> Usuario { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Postagem>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<Postagem>()
                .Property(x => x.Titulo)
                .IsRequired()
                .HasColumnType("varchar(255)");

            modelBuilder.Entity<Postagem>()
                .Property(x => x.Conteudo)
                .IsRequired()
                .HasColumnType("varchar(255)");

            modelBuilder.Entity<Usuario>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<Usuario>()
                .Property(x => x.Nome)
                .IsRequired()
                .HasColumnType("varchar(255)");

            modelBuilder.Entity<Usuario>()
                .Property(x => x.Senha)
                .IsRequired()
                .HasColumnType("varchar(255)");

            modelBuilder.Entity<Usuario>()
                .Property(x => x.Email)
                .IsRequired()
                .HasColumnType("varchar(255)");

            base.OnModelCreating(modelBuilder);            
        }
    }
}
