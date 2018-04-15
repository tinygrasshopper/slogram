﻿// <auto-generated />
using ;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace slogram.Migrations
{
    [DbContext(typeof(MvcPhotoContext))]
    partial class MvcPhotoContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn)
                .HasAnnotation("ProductVersion", "2.0.1-rtm-125");

            modelBuilder.Entity("slogram.Models.Photo", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Guid");

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
