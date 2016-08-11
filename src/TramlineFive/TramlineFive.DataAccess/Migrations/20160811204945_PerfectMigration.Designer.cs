using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using TramlineFive.DataAccess;

namespace TramlineFive.DataAccess.Migrations
{
    [DbContext(typeof(TramlineFiveContext))]
    [Migration("20160811204945_PerfectMigration")]
    partial class PerfectMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.0-rtm-21431");

            modelBuilder.Entity("TramlineFive.DataAccess.Entities.Day", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("DirectionID");

                    b.Property<string>("Type");

                    b.HasKey("ID");

                    b.HasIndex("DirectionID");

                    b.ToTable("Days");
                });

            modelBuilder.Entity("TramlineFive.DataAccess.Entities.Direction", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("LineID");

                    b.Property<string>("Name");

                    b.HasKey("ID");

                    b.HasIndex("LineID");

                    b.ToTable("Directions");
                });

            modelBuilder.Entity("TramlineFive.DataAccess.Entities.Favourite", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("StopID");

                    b.HasKey("ID");

                    b.HasIndex("StopID");

                    b.ToTable("Favourites");
                });

            modelBuilder.Entity("TramlineFive.DataAccess.Entities.Line", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Number");

                    b.Property<int>("Type");

                    b.HasKey("ID");

                    b.ToTable("Lines");
                });

            modelBuilder.Entity("TramlineFive.DataAccess.Entities.Stop", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Code");

                    b.Property<int?>("DayID");

                    b.Property<string>("Name");

                    b.Property<string>("TimingsAsString");

                    b.HasKey("ID");

                    b.HasIndex("DayID");

                    b.ToTable("Stops");
                });

            modelBuilder.Entity("TramlineFive.DataAccess.Entities.Day", b =>
                {
                    b.HasOne("TramlineFive.DataAccess.Entities.Direction", "Direction")
                        .WithMany("Days")
                        .HasForeignKey("DirectionID");
                });

            modelBuilder.Entity("TramlineFive.DataAccess.Entities.Direction", b =>
                {
                    b.HasOne("TramlineFive.DataAccess.Entities.Line", "Line")
                        .WithMany("Directions")
                        .HasForeignKey("LineID");
                });

            modelBuilder.Entity("TramlineFive.DataAccess.Entities.Favourite", b =>
                {
                    b.HasOne("TramlineFive.DataAccess.Entities.Stop", "Stop")
                        .WithMany("Favourites")
                        .HasForeignKey("StopID");
                });

            modelBuilder.Entity("TramlineFive.DataAccess.Entities.Stop", b =>
                {
                    b.HasOne("TramlineFive.DataAccess.Entities.Day", "Day")
                        .WithMany("Stops")
                        .HasForeignKey("DayID");
                });
        }
    }
}
