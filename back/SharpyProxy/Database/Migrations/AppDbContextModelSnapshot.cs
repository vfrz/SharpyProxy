﻿// <auto-generated />
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using SharpyProxy.Database;
using SharpyProxy.Database.Entities;

#nullable disable

namespace SharpyProxy.Database.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("SharpyProxy.Database.Entities.CertificateEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedDateUtc")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Domain")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("ExpirationDateUtc")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Key")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid?>("LetsEncryptAccountId")
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Pem")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Type")
                        .HasColumnType("integer");

                    b.Property<DateTime>("UpdatedDateUtc")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("LetsEncryptAccountId");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Certificates");
                });

            modelBuilder.Entity("SharpyProxy.Database.Entities.ClusterEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedDateUtc")
                        .HasColumnType("timestamp with time zone");

                    b.Property<ClusterDestination[]>("Destinations")
                        .IsRequired()
                        .HasColumnType("jsonb");

                    b.Property<bool>("Enabled")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("UpdatedDateUtc")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Clusters");
                });

            modelBuilder.Entity("SharpyProxy.Database.Entities.LetsEncryptAccountEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedDateUtc")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<byte[]>("RSABytes")
                        .IsRequired()
                        .HasColumnType("bytea");

                    b.Property<DateTime>("UpdatedDateUtc")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("LetsEncryptAccounts");
                });

            modelBuilder.Entity("SharpyProxy.Database.Entities.LetsEncryptChallengeEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedDateUtc")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Domain")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("LetsEncryptAccountId")
                        .HasColumnType("uuid");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("UpdatedDateUtc")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("LetsEncryptAccountId");

                    b.ToTable("LetsEncryptChallenges");
                });

            modelBuilder.Entity("SharpyProxy.Database.Entities.RouteEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("ClusterId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedDateUtc")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("Enabled")
                        .HasColumnType("boolean");

                    b.Property<List<string>>("MatchHosts")
                        .IsRequired()
                        .HasColumnType("text[]");

                    b.Property<string>("MatchPath")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("UpdatedDateUtc")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("ClusterId");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Routes");
                });

            modelBuilder.Entity("SharpyProxy.Database.Entities.CertificateEntity", b =>
                {
                    b.HasOne("SharpyProxy.Database.Entities.LetsEncryptAccountEntity", "LetsEncryptAccount")
                        .WithMany("Certificates")
                        .HasForeignKey("LetsEncryptAccountId");

                    b.Navigation("LetsEncryptAccount");
                });

            modelBuilder.Entity("SharpyProxy.Database.Entities.LetsEncryptChallengeEntity", b =>
                {
                    b.HasOne("SharpyProxy.Database.Entities.LetsEncryptAccountEntity", "LetsEncryptAccount")
                        .WithMany("Challenges")
                        .HasForeignKey("LetsEncryptAccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("LetsEncryptAccount");
                });

            modelBuilder.Entity("SharpyProxy.Database.Entities.RouteEntity", b =>
                {
                    b.HasOne("SharpyProxy.Database.Entities.ClusterEntity", "Cluster")
                        .WithMany("Routes")
                        .HasForeignKey("ClusterId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Cluster");
                });

            modelBuilder.Entity("SharpyProxy.Database.Entities.ClusterEntity", b =>
                {
                    b.Navigation("Routes");
                });

            modelBuilder.Entity("SharpyProxy.Database.Entities.LetsEncryptAccountEntity", b =>
                {
                    b.Navigation("Certificates");

                    b.Navigation("Challenges");
                });
#pragma warning restore 612, 618
        }
    }
}
