using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
    public virtual DbSet<Users> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configuração específica para a propriedade que mudou de char para varchar
        modelBuilder.Entity<Users>()
            .Property(u => u.Name) // Substitua pelo nome real da propriedade
            .HasColumnType("character varying"); // ou "varchar"

        modelBuilder.Entity<Users>()
            .Property(u => u.Email) // Substitua pelo nome real da propriedade
            .HasColumnType("character varying"); // ou "varchar"

        // Outras configurações de modelo podem vir aqui
    }
}