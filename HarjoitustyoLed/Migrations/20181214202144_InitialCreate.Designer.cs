﻿// <auto-generated />
using System;
using HarjoitustyoLed;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace HarjoitustyoLed.Migrations
{
    [DbContext(typeof(SequenceContext))]
    [Migration("20181214202144_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.0-rtm-35687");

            modelBuilder.Entity("HarjoitustyoLed.LedRow", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("PinId");

                    b.Property<int>("Status");

                    b.Property<int?>("TimeRowId");

                    b.HasKey("Id");

                    b.HasIndex("TimeRowId");

                    b.ToTable("LedRows");
                });

            modelBuilder.Entity("HarjoitustyoLed.LedSequence", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("LedSequences");
                });

            modelBuilder.Entity("HarjoitustyoLed.TimeRow", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("LedSequenceId");

                    b.Property<int>("Time");

                    b.HasKey("Id");

                    b.HasIndex("LedSequenceId");

                    b.ToTable("TimeRows");
                });

            modelBuilder.Entity("HarjoitustyoLed.LedRow", b =>
                {
                    b.HasOne("HarjoitustyoLed.TimeRow", "TimeRow")
                        .WithMany("LedRows")
                        .HasForeignKey("TimeRowId");
                });

            modelBuilder.Entity("HarjoitustyoLed.TimeRow", b =>
                {
                    b.HasOne("HarjoitustyoLed.LedSequence", "LedSequence")
                        .WithMany("TimeRows")
                        .HasForeignKey("LedSequenceId");
                });
#pragma warning restore 612, 618
        }
    }
}
