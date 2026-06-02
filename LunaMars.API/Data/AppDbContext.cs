using Microsoft.EntityFrameworkCore;

namespace LunaMars.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Models.ColoniaEspacial> Colonias { get; set; }

        public DbSet<Models.SetorColonia> Setores { get; set; }

        public DbSet<Models.SensorAmbiental> Sensores { get; set; }

        public DbSet<Models.LeituraSensor> Leituras { get; set; }

        public DbSet<Models.AlertaColonia> Alertas { get; set; }

        public DbSet<Models.RecursoVital> Recursos { get; set; }

        public DbSet<Models.MissaoResposta> Missoes { get; set; }

        public DbSet<Models.RoverExploracao> Rovers { get; set; }

        public DbSet<Models.SateliteOrbital> Satelites { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Models.SetorColonia>()
                .Property(s => s.ativo)
                .HasConversion<int>()
                .HasColumnType("NUMBER(1)");

            modelBuilder.Entity<Models.SensorAmbiental>()
                .Property(s => s.ativo)
                .HasConversion<int>()
                .HasColumnType("NUMBER(1)");

            modelBuilder.Entity<Models.RoverExploracao>()
                .Property(r => r.ativo)
                .HasConversion<int>()
                .HasColumnType("NUMBER(1)");

            modelBuilder.Entity<Models.SateliteOrbital>()
                .Property(s => s.ativo)
                .HasConversion<int>()
                .HasColumnType("NUMBER(1)");
        }
    }
}
