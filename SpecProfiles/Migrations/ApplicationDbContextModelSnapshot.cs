// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using SpecProfiles.Data;

#nullable disable

namespace SpecProfiles.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.12")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("SpecProfiles.Data.Organization", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnOrder(0);

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("AdditionalData")
                        .HasMaxLength(1024)
                        .HasColumnType("character varying(1024)")
                        .HasColumnOrder(10);

                    b.Property<string>("City")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)")
                        .HasColumnOrder(8);

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)")
                        .HasColumnOrder(7);

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)")
                        .HasColumnOrder(2);

                    b.Property<DateTime>("EstablishmentDate")
                        .HasColumnType("Date")
                        .HasColumnOrder(4);

                    b.Property<string>("Industry")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)")
                        .HasColumnOrder(6);

                    b.Property<string>("OwnerId")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnOrder(1);

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasMaxLength(16)
                        .HasColumnType("character varying(16)")
                        .HasColumnOrder(3);

                    b.Property<string>("ShortInfo")
                        .IsRequired()
                        .HasMaxLength(140)
                        .HasColumnType("text")
                        .HasColumnOrder(9);

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)")
                        .HasColumnOrder(5);

                    b.HasKey("Id");

                    b.ToTable("Organization");
                });

            modelBuilder.Entity("SpecProfiles.Data.Person", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnOrder(0);

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("AdditionalData")
                        .HasMaxLength(1024)
                        .HasColumnType("character varying(1024)")
                        .HasColumnOrder(10);

                    b.Property<DateTime>("BirthDate")
                        .HasColumnType("Date")
                        .HasColumnOrder(4);

                    b.Property<string>("City")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)")
                        .HasColumnOrder(7);

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)")
                        .HasColumnOrder(6);

                    b.Property<string>("FamilyStatus")
                        .IsRequired()
                        .HasMaxLength(22)
                        .HasColumnType("character varying(22)")
                        .HasColumnOrder(9);

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)")
                        .HasColumnOrder(2);

                    b.Property<string>("Gender")
                        .IsRequired()
                        .HasMaxLength(16)
                        .HasColumnType("character varying(16)")
                        .HasColumnOrder(5);

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)")
                        .HasColumnOrder(3);

                    b.Property<int>("OwnerId")
                        .HasColumnType("integer")
                        .HasColumnOrder(1);

                    b.Property<string>("ShortInfo")
                        .IsRequired()
                        .HasMaxLength(140)
                        .HasColumnType("text")
                        .HasColumnOrder(8);

                    b.HasKey("Id");

                    b.ToTable("Person");
                });
#pragma warning restore 612, 618
        }
    }
}
