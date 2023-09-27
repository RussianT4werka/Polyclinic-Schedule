using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Polyclinic_Schedule.DTO;

namespace Polyclinic_Schedule.DB;

public partial class User30Context : DbContext
{
    public User30Context()
    {
    }

    public User30Context(DbContextOptions<User30Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Cabinet> Cabinets { get; set; }

    public virtual DbSet<Doctor> Doctors { get; set; }

    public virtual DbSet<Schedule> Schedules { get; set; }

    public virtual DbSet<Speciality> Specialities { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("server=192.168.200.35;database=user30;user=user30;password=42494;TrustServerCertificate=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cabinet>(entity =>
        {
            entity.HasKey(e => e.Number);

            entity.ToTable("cabinet");

            entity.Property(e => e.Number)
                .ValueGeneratedNever()
                .HasColumnName("number");
            entity.Property(e => e.IdDoctor).HasColumnName("idDoctor");

            entity.HasOne(d => d.IdDoctorNavigation).WithMany(p => p.Cabinets)
                .HasForeignKey(d => d.IdDoctor)
                .HasConstraintName("FK_cabinet_doctor");
        });

        modelBuilder.Entity<Doctor>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_doctors");

            entity.ToTable("doctor");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .HasColumnName("firstName");
            entity.Property(e => e.IdSpeciality).HasColumnName("idSpeciality");
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .HasColumnName("lastName");
            entity.Property(e => e.PatronymicName)
                .HasMaxLength(50)
                .HasColumnName("patronymicName");

            entity.HasOne(d => d.IdSpecialityNavigation).WithMany(p => p.Doctors)
                .HasForeignKey(d => d.IdSpeciality)
                .HasConstraintName("FK_doctor_speciality");
        });

        modelBuilder.Entity<Schedule>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_shedule");

            entity.ToTable("schedule");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IdDoctor).HasColumnName("idDoctor");
            entity.Property(e => e.IdUser).HasColumnName("idUser");
            entity.Property(e => e.StartTime)
                .HasColumnType("datetime")
                .HasColumnName("startTime");

            entity.HasOne(d => d.IdDoctorNavigation).WithMany(p => p.Schedules)
                .HasForeignKey(d => d.IdDoctor)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_shedule_doctor");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.Schedules)
                .HasForeignKey(d => d.IdUser)
                .HasConstraintName("FK_shedule_user");
        });

        modelBuilder.Entity<Speciality>(entity =>
        {
            entity.ToTable("speciality");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Title)
                .HasMaxLength(50)
                .HasColumnName("title");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_users");

            entity.ToTable("user");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .HasColumnName("email");
            entity.Property(e => e.FirstName)
                .HasMaxLength(250)
                .HasColumnName("firstName");
            entity.Property(e => e.LastName)
                .HasMaxLength(250)
                .HasColumnName("lastName");
            entity.Property(e => e.Login)
                .HasMaxLength(50)
                .HasColumnName("login");
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .HasColumnName("password");
            entity.Property(e => e.PatronymicName)
                .HasMaxLength(250)
                .HasColumnName("patronymicName");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
