using ApiCadastraDados.Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace ApiCadastraDados.Repository
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options) : base(options)
        { }

        public virtual DbSet<Pessoa> Pessoas { get; set; }
        public virtual DbSet<Telefone> Telefones { get; set; }
        public virtual DbSet<Endereco> Enderecos { get; set; }
        public virtual DbSet<RegistroErro> RegistroErros { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { throw new NotSupportedException(); }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin_General_CP1_CI_AS");

            modelBuilder.Entity<Pessoa>(entity =>
            {
                entity.ToTable("Pessoa");

                entity.HasKey(p => p.ID);

                entity.Property(p => p.Nome).IsRequired().HasMaxLength(255);

                entity.Property(p => p.Idade).IsRequired().HasColumnType("INT");

                entity.Property(p => p.Sexo).IsRequired().HasMaxLength(10);

                entity.Property(p => p.Email).IsRequired().HasMaxLength(255);

                entity.Property(p => p.Cpf).IsRequired().HasMaxLength(255);

                entity.Property(p => p.DataNascimento).IsRequired().HasColumnType("Date");
            });

            modelBuilder.Entity<Endereco>(entity =>
            {
                entity.ToTable("Endereco");

                entity.HasKey(p => p.ID);

                entity.Property(p => p.Rua).IsRequired().HasMaxLength(255);

                entity.Property(p => p.Numero).IsRequired().HasColumnType("INT");

                entity.Property(p => p.Bairro).IsRequired().HasMaxLength(100);

                entity.Property(p => p.Cidade).IsRequired().HasMaxLength(100);

                entity.Property(p => p.UF).IsRequired().HasMaxLength(2);

                entity.Property(p => p.Pais).IsRequired().HasMaxLength(100);

                entity.Property(p => p.Cep).IsRequired().HasMaxLength(100);

                entity.Property(p => p.IdPessoa).IsRequired().HasColumnType("INT");
            });

            modelBuilder.Entity<Telefone>(entity =>
            {
                entity.ToTable("Telefone");

                entity.HasKey(p => p.ID);

                entity.Property(p => p.Ddi).IsRequired().HasMaxLength(255);

                entity.Property(p => p.Ddd).IsRequired().HasMaxLength(100);

                entity.Property(p => p.NumeroCelular).IsRequired().HasMaxLength(100);

                entity.Property(p => p.NumeroTelefone).IsRequired().HasMaxLength(100);

                entity.Property(p => p.IdPessoa).IsRequired().HasColumnType("INT");
            });

            modelBuilder.Entity<RegistroErro>(entity =>
            {
                entity.ToTable("RegistroErro");

                entity.HasKey(p => p.ID);

                entity.Property(p => p.CodErr).IsRequired().HasColumnType("INT");

                entity.Property(p => p.MsgErr).IsRequired().HasMaxLength(8000);

                entity.Property(p => p.Metodo).IsRequired().HasMaxLength(255);

                entity.Property(p => p.Entrada).IsRequired().HasMaxLength(8000);

                entity.Property(p => p.Exception).IsRequired().HasMaxLength(8000);

                entity.Property(p => p.DataHorAtualizacao).IsRequired().HasColumnType("DATETIME");
            });
        }
    }
}
