﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using PolicyBased.Infra.Models;

namespace PolicyBased.Infra.Persistence.Context
{
    public partial class PolicyTestDBContext : DbContext
    {
        public PolicyTestDBContext()
        {
        }

        public PolicyTestDBContext(DbContextOptions<PolicyTestDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AppPolicy> AppPolicies { get; set; }
        public virtual DbSet<Application> Applications { get; set; }
        public virtual DbSet<Permission> Permissions { get; set; }
        public virtual DbSet<Policy> Policies { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserRole> UserRoles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.HasDefaultSchema("pbac");

            modelBuilder.Entity<AppPolicy>(entity =>
            {
                entity.ToTable("AppPolicies", "pbac");

                entity.HasOne(d => d.Permission)
                    .WithMany(p => p.AppPolicies)
                    .HasForeignKey(d => d.PermissionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AppPolicies_Permissions");

                entity.HasOne(d => d.Policy)
                    .WithMany(p => p.AppPolicies)
                    .HasForeignKey(d => d.PolicyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AppPolicies_Policies");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AppPolicies)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AppPolicies_Roles");
            });

            modelBuilder.Entity<Application>(entity =>
            {
                entity.ToTable("Applications", "pbac");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Permission>(entity =>
            {
                entity.ToTable("Permissions", "pbac");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Policy)
                    .WithMany(p => p.Permissions)
                    .HasForeignKey(d => d.PolicyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Permissions_Policies");
            });

            modelBuilder.Entity<Policy>(entity =>
            {
                entity.ToTable("Policies", "pbac");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Application)
                    .WithMany(p => p.Policies)
                    .HasForeignKey(d => d.ApplicationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Policies_Applications");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("Roles", "pbac");

                entity.Property(e => e.RoleName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Policy)
                    .WithMany(p => p.Roles)
                    .HasForeignKey(d => d.PolicyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Roles_Applications");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("Users", "pbac");

                entity.HasIndex(e => e.Id, "IX_Users_UserName")
                    .IsUnique();

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.ToTable("UserRoles", "pbac");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.UserRoles)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserRoles_Roles");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserRoles)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserRoles_Users");
            });

            OnModelCreatingGeneratedProcedures(modelBuilder);
            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}