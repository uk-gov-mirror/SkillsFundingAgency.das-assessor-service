﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using SFA.DAS.AssessorService.Data;
using System;

namespace SFA.DAS.AssessorService.Data.Migrations
{
    [DbContext(typeof(AssessorDbContext))]
    [Migration("20180307192855_PrimaryContactChange")]
    partial class PrimaryContactChange
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.1-rtm-125")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("SFA.DAS.AssessorService.Domain.Entities.Certificate", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CertificateData")
                        .IsRequired();

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasMaxLength(30);

                    b.Property<DateTime?>("DeletedAt");

                    b.Property<string>("DeletedBy")
                        .HasMaxLength(30);

                    b.Property<int>("EndPointAssessorCertificateId");

                    b.Property<Guid>("OrganisationId");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(20);

                    b.Property<DateTime?>("UpdatedAt");

                    b.Property<string>("UpdatedBy")
                        .HasMaxLength(30);

                    b.HasKey("Id");

                    b.HasIndex("OrganisationId");

                    b.ToTable("Certificates");
                });

            modelBuilder.Entity("SFA.DAS.AssessorService.Domain.Entities.CertificateLog", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Action")
                        .IsRequired()
                        .HasMaxLength(400);

                    b.Property<Guid>("CertificateId");

                    b.Property<DateTime>("CreatedAt");

                    b.Property<DateTime?>("DeletedAt");

                    b.Property<DateTime>("EventTime");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(12);

                    b.Property<DateTime?>("UpdatedAt");

                    b.HasKey("Id");

                    b.HasAlternateKey("EventTime");

                    b.HasIndex("CertificateId");

                    b.ToTable("CertificateLogs");
                });

            modelBuilder.Entity("SFA.DAS.AssessorService.Domain.Entities.Contact", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedAt");

                    b.Property<DateTime?>("DeletedAt");

                    b.Property<string>("DisplayName")
                        .IsRequired()
                        .HasMaxLength(120);

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(120);

                    b.Property<string>("EndPointAssessorOrganisationId")
                        .IsRequired()
                        .HasMaxLength(12);

                    b.Property<Guid>("OrganisationId");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(10);

                    b.Property<DateTime?>("UpdatedAt");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasMaxLength(30);

                    b.HasKey("Id");

                    b.HasAlternateKey("UserName");

                    b.HasIndex("OrganisationId");

                    b.ToTable("Contacts");
                });

            modelBuilder.Entity("SFA.DAS.AssessorService.Domain.Entities.Organisation", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedAt");

                    b.Property<DateTime?>("DeletedAt");

                    b.Property<string>("EndPointAssessorName")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<string>("EndPointAssessorOrganisationId")
                        .IsRequired()
                        .HasMaxLength(12);

                    b.Property<int>("EndPointAssessorUkprn");

                    b.Property<string>("PrimaryContact")
                        .HasMaxLength(30);

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(10);

                    b.Property<DateTime?>("UpdatedAt");

                    b.HasKey("Id");

                    b.HasAlternateKey("EndPointAssessorOrganisationId");

                    b.ToTable("Organisations");
                });

            modelBuilder.Entity("SFA.DAS.AssessorService.Domain.Entities.Certificate", b =>
                {
                    b.HasOne("SFA.DAS.AssessorService.Domain.Entities.Organisation", "Organisation")
                        .WithMany("Certificates")
                        .HasForeignKey("OrganisationId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SFA.DAS.AssessorService.Domain.Entities.CertificateLog", b =>
                {
                    b.HasOne("SFA.DAS.AssessorService.Domain.Entities.Certificate", "Certificate")
                        .WithMany()
                        .HasForeignKey("CertificateId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SFA.DAS.AssessorService.Domain.Entities.Contact", b =>
                {
                    b.HasOne("SFA.DAS.AssessorService.Domain.Entities.Organisation", "Organisation")
                        .WithMany("Contacts")
                        .HasForeignKey("OrganisationId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
