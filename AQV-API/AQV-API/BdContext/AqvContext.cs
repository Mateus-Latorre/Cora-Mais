using System;
using System.Collections.Generic;
using AQV.WebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace AQV.WebAPI.BdContext;

public partial class AqvContext : DbContext
{
    public AqvContext()
    {
    }

    public AqvContext(DbContextOptions<AqvContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Aluno> Alunos { get; set; }

    public virtual DbSet<Feedback> Feedbacks { get; set; }

    public virtual DbSet<Encaminhamento> Encaminhamentos { get; set; }

    public virtual DbSet<Ocorrencia> Ocorrencias { get; set; }

    public virtual DbSet<Ostatus> Ostatuses { get; set; }

    public virtual DbSet<Responsavel> Responsavels { get; set; }

    public virtual DbSet<TipoUsuario> TipoUsuarios { get; set; }

    public virtual DbSet<Turma> Turmas { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=CORA;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Aluno>(entity =>
        {
            entity.HasKey(e => e.IdAluno).HasName("PK__Aluno__8092FCB3835249B9");

            entity.ToTable("Aluno");

            entity.HasIndex(e => e.Numero, "UQ__Aluno__7E532BC640F6DA2C").IsUnique();

            entity.HasIndex(e => e.Email, "UQ__Aluno__A9D10534AB4C01D7").IsUnique();

            entity.HasIndex(e => e.Cpf, "UQ__Aluno__C1F897311D77C7C8").IsUnique();

            entity.Property(e => e.IdAluno).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Cpf)
                .HasMaxLength(11)
                .IsUnicode(false)
                .HasColumnName("CPF");
            entity.Property(e => e.Email)
                .HasMaxLength(256)
                .IsUnicode(false);
            entity.Property(e => e.Nome)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Numero)
                .HasMaxLength(11)
                .IsUnicode(false);

            entity.HasOne(d => d.IdTurmaNavigation).WithMany(p => p.Alunos)
                .HasForeignKey(d => d.IdTurma)
                .HasConstraintName("FK_Aluno_Turma");
        });

        modelBuilder.Entity<Encaminhamento>(entity =>
        {
            entity.HasKey(e => e.IdEncaminhamento).HasName("PK__Encaminh__74C3BD4BBB7A6CB7");

            entity.ToTable("Encaminhamento");

            entity.Property(e => e.IdEncaminhamento).HasDefaultValueSql("(newid())");
            entity.Property(e => e.DataDevolutiva).HasColumnType("datetime");
            entity.Property(e => e.DataEnvio)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IdUsuarioAqv).HasColumnName("IdUsuarioAQV");
            entity.Property(e => e.Observacao)
                .HasMaxLength(300)
                .IsUnicode(false);

            entity.HasOne(d => d.IdOcorrenciaNavigation).WithMany(p => p.Encaminhamentos)
                .HasForeignKey(d => d.IdOcorrencia)
                .HasConstraintName("FK_Encaminhamento_Ocorrencia");

            entity.HasOne(d => d.IdResponsavelNavigation).WithMany(p => p.Encaminhamentos)
                .HasForeignKey(d => d.IdResponsavel)
                .HasConstraintName("FK_Encaminhamento_Responsavel");

            entity.HasOne(d => d.IdUsuarioAqvNavigation).WithMany(p => p.EncaminhamentoIdUsuarioAqvNavigations)
                .HasForeignKey(d => d.IdUsuarioAqv)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Encaminhamento_AQV");

            entity.HasOne(d => d.IdUsuarioCoordenadorNavigation).WithMany(p => p.EncaminhamentoIdUsuarioCoordenadorNavigations)
                .HasForeignKey(d => d.IdUsuarioCoordenador)
                .HasConstraintName("FK_Encaminhamento_Coordenador");
        });

        modelBuilder.Entity<Feedback>(entity =>
        {
            entity.HasKey(e => e.IdFeedback).HasName("PK__Feedback__408FF103A4A22688");

            entity.ToTable("Feedback");

            entity.Property(e => e.IdFeedback).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Anexo)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.DataEnvio)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Lido).HasDefaultValue(false);
            entity.Property(e => e.Mensagem)
                .HasMaxLength(300)
                .IsUnicode(false);

            entity.HasOne(d => d.IdOcorrenciaNavigation).WithMany(p => p.Feedbacks)
                .HasForeignKey(d => d.IdOcorrencia)
                .HasConstraintName("FK__Feedback__IdOcor__7C4F7684");
        });

        modelBuilder.Entity<Ocorrencia>(entity =>
        {
            entity.HasKey(e => e.IdOcorrencia).HasName("PK__Ocorrenc__831D923991173D65");

            entity.ToTable("Ocorrencia");

            entity.Property(e => e.IdOcorrencia).HasDefaultValueSql("(newid())");
            entity.Property(e => e.DataAbertura)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Descricao)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.IdOstatus)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Prioridade)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Titulo)
                .HasMaxLength(55)
                .IsUnicode(false);

            entity.HasOne(d => d.IdAlunoNavigation).WithMany(p => p.Ocorrencia)
                .HasForeignKey(d => d.IdAluno)
                .HasConstraintName("FK__Ocorrenci__IdAlu__74AE54BC");

            entity.HasOne(d => d.IdOstatusNavigation).WithMany(p => p.Ocorrencia)
                .HasForeignKey(d => d.IdOstatus)
                .HasConstraintName("FK__Ocorrenci__IdOst__75A278F5");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Ocorrencia)
                .HasForeignKey(d => d.IdUsuario)
                .HasConstraintName("FK__Ocorrenci__IdUsu__76969D2E");
        });

        modelBuilder.Entity<Ostatus>(entity =>
        {
            entity.HasKey(e => e.IdOstatus).HasName("PK__Ostatus__E76FF4A8763A2458");

            entity.ToTable("Ostatus");

            entity.Property(e => e.IdOstatus)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Titulo)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Responsavel>(entity =>
        {
            entity.HasKey(e => e.IdResponsavel).HasName("PK__Responsa__CDF1DCAD8E5F5FEB");

            entity.ToTable("Responsavel");

            entity.HasIndex(e => e.Numero, "UQ__Responsa__7E532BC69F7E985C").IsUnique();

            entity.HasIndex(e => e.Email, "UQ__Responsa__A9D10534DA8E759E").IsUnique();

            entity.Property(e => e.IdResponsavel).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Email)
                .HasMaxLength(256)
                .IsUnicode(false);
            entity.Property(e => e.Nome)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Numero)
                .HasMaxLength(11)
                .IsUnicode(false);

            entity.HasOne(d => d.IdAlunoNavigation).WithMany(p => p.Responsavels)
                .HasForeignKey(d => d.IdAluno)
                .HasConstraintName("FK__Responsav__IdAlu__6E01572D");
        });

        modelBuilder.Entity<TipoUsuario>(entity =>
        {
            entity.HasKey(e => e.IdTipoUsuario).HasName("PK__TipoUsua__CA04062B619A6846");

            entity.ToTable("TipoUsuario");

            entity.Property(e => e.IdTipoUsuario).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Titulo)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Turma>(entity =>
        {
            entity.HasKey(e => e.IdTurma).HasName("PK__Turma__C1ECFFC97FAD934E");

            entity.ToTable("Turma");

            entity.Property(e => e.IdTurma).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Nome)
                .HasMaxLength(200)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.IdUsuario).HasName("PK__Usuario__5B65BF9738D8A2A0");

            entity.ToTable("Usuario");

            entity.HasIndex(e => e.Numero, "UQ__Usuario__7E532BC6D919333E").IsUnique();

            entity.HasIndex(e => e.Email, "UQ__Usuario__A9D105341EC743F5").IsUnique();

            entity.Property(e => e.IdUsuario).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Email)
                .HasMaxLength(256)
                .IsUnicode(false);
            entity.Property(e => e.Nome)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Numero)
                .HasMaxLength(11)
                .IsUnicode(false);
            entity.Property(e => e.Senha)
                .HasMaxLength(60)
                .IsUnicode(false);

            entity.HasOne(d => d.IdTipoUsuarioNavigation).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.IdTipoUsuario)
                .HasConstraintName("FK__Usuario__IdTipoU__628FA481");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
