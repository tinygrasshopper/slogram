﻿// <auto-generated />
using slogram.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using System;

namespace slogram.Migrations
{
    [DbContext(typeof(MvcPhotoContext))]
    [Migration("20180407144515_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.0-rtm-26452");

            modelBuilder.Entity("slogram.Models.Photo", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Processed");

                    b.Property<string>("ProcessedUrl");

                    b.Property<string>("RawUrl");

                    b.Property<string>("Title");

                    b.HasKey("ID");

                    b.ToTable("Photo");
                });
#pragma warning restore 612, 618
        }
    }
}
