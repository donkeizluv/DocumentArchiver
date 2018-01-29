using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DocumentArchiver.EntityModels
{
    public partial class DocumentArchiverContext : DbContext
    {
        public virtual DbSet<Ability> Ability { get; set; }
        public virtual DbSet<Contract> Contract { get; set; }
        public virtual DbSet<ContractSharing> ContractSharing { get; set; }
        public virtual DbSet<EventLog> EventLog { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<UserAbility> UserAbility { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Ability>(entity =>
            {
                entity.HasKey(e => e.AbilityName);

                entity.Property(e => e.AbilityName)
                    .HasMaxLength(50)
                    .ValueGeneratedNever();

                entity.Property(e => e.Description).HasMaxLength(100);
            });

            modelBuilder.Entity<Contract>(entity =>
            {
                entity.Property(e => e.CreateTime).HasColumnType("datetime");

                entity.Property(e => e.CustomerName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.IdentityCard)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.Phone).HasMaxLength(50);

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.UsernameNavigation)
                    .WithMany(p => p.Contract)
                    .HasForeignKey(d => d.Username)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Contract_User");
            });

            modelBuilder.Entity<ContractSharing>(entity =>
            {
                entity.HasKey(e => e.ShareId);

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DisabledOn).HasColumnType("datetime");

                entity.Property(e => e.Keycode)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.HasOne(d => d.Contract)
                    .WithMany(p => p.ContractSharing)
                    .HasForeignKey(d => d.ContractId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ContractSharing_Contract");
            });

            modelBuilder.Entity<EventLog>(entity =>
            {
                entity.HasKey(e => e.EventId);

                entity.Property(e => e.CreateTime).HasColumnType("datetime");

                entity.Property(e => e.DateOfEvent).HasColumnType("datetime");

                entity.Property(e => e.Filename)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.Filetype)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Note).HasMaxLength(150);

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.Contract)
                    .WithMany(p => p.EventLog)
                    .HasForeignKey(d => d.ContractId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_EventLog_Contract");

                entity.HasOne(d => d.UsernameNavigation)
                    .WithMany(p => p.EventLog)
                    .HasForeignKey(d => d.Username)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_EventLog_User");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Username);

                entity.Property(e => e.Username)
                    .HasMaxLength(50)
                    .ValueGeneratedNever();

                entity.Property(e => e.LastLogin).HasColumnType("datetime");
            });

            modelBuilder.Entity<UserAbility>(entity =>
            {
                entity.HasKey(e => new { e.AbilityName, e.Username });

                entity.Property(e => e.AbilityName).HasMaxLength(50);

                entity.Property(e => e.Username).HasMaxLength(50);

                entity.HasOne(d => d.AbilityNameNavigation)
                    .WithMany(p => p.UserAbility)
                    .HasForeignKey(d => d.AbilityName)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserAbility_Ability");

                entity.HasOne(d => d.UsernameNavigation)
                    .WithMany(p => p.UserAbility)
                    .HasForeignKey(d => d.Username)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserAbility_User");
            });
        }
    }
}
