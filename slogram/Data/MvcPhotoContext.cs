using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using slogram.Models;
using Microsoft.Extensions.Configuration;

public class MvcPhotoContext : DbContext
{
    public MvcPhotoContext(DbContextOptions<MvcPhotoContext> options)
        : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            //var dbHost     = Config["vcap:services:mysql:0:credentials:host"];
            //var dbUsername = Config["vcap:services:mysql:0:credentials:username"];
            //var dbPassword = Config["vcap:services:mysql:0:credentials:password"];
            //var dbDatabase = Config["vcap:services:mysql:0:credentials:name"];

            //optionsBuilder.UseMySql("Server=" + dbHost + ";User Id=" + dbUsername + ";Password=" + dbPassword + ";Database=" + dbDatabase);              
        }
    }

    public DbSet<slogram.Models.Photo> Photo { get; set; }
}
