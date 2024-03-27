using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace bmstu_bot.Models;

public partial class BmstuBotContext : DbContext
{
    public BmstuBotContext()
    {
    }

    public BmstuBotContext(DbContextOptions<BmstuBotContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Admin> Admins { get; set; }

    public virtual DbSet<Complain> Complains { get; set; }

    public virtual DbSet<Entry> Entries { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=bmstu-bot;Username=postgres;Password=Griver2070;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Admin>(entity =>
        {
            entity.HasKey(e => e.ChatId).HasName("Admins_pkey");

            entity.Property(e => e.ChatId)
                .ValueGeneratedNever()
                .HasColumnName("chatId");
            entity.Property(e => e.Link)
                .HasMaxLength(255)
                .HasColumnName("link");
        });

        modelBuilder.Entity<Complain>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Complains_pkey");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Admin)
                .HasMaxLength(255)
                .HasColumnName("admin");
            entity.Property(e => e.Answer).HasColumnName("answer");
            entity.Property(e => e.Category).HasColumnName("category");
            entity.Property(e => e.Date)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("date");
            entity.Property(e => e.From).HasColumnName("from ");
            entity.Property(e => e.IsAnon).HasColumnName("isAnon");
            entity.Property(e => e.Message).HasColumnName("message");
            entity.Property(e => e.Prev).HasColumnName("prev");
            entity.Property(e => e.Type).HasColumnName("type");

            entity.HasOne(d => d.FromNavigation).WithMany(p => p.Complains)
                .HasForeignKey(d => d.From)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Complains_from _fkey");
        });

        modelBuilder.Entity<Entry>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Entries_pkey");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AdminChat).HasColumnName("adminChat");
            entity.Property(e => e.ComplainId).HasColumnName("complainId");
            entity.Property(e => e.MessageId).HasColumnName("messageId");

            entity.HasOne(d => d.Complain).WithMany(p => p.Entries)
                .HasForeignKey(d => d.ComplainId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Entries_complainId_fkey");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.ChatId).HasName("users_pkey");

            entity.ToTable("users");

            entity.Property(e => e.ChatId)
                .ValueGeneratedNever()
                .HasColumnName("chatId");
            entity.Property(e => e.Anonim).HasColumnName("anonim");
            entity.Property(e => e.BmstuGroup)
                .HasMaxLength(255)
                .HasColumnName("bmstu_group ");
            entity.Property(e => e.ComandLine).HasMaxLength(255);
            entity.Property(e => e.ComplainCategory).HasColumnName("complain_category");
            entity.Property(e => e.ComplainType).HasColumnName("complain_type");
            entity.Property(e => e.Fio).HasMaxLength(255);
            entity.Property(e => e.TgLink)
                .HasMaxLength(255)
                .HasColumnName("tgLink");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
