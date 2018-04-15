using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using slogram.Models;
using Steeltoe.CloudFoundry.Connector.MySql.EFCore;
using Steeltoe.Extensions.Configuration.CloudFoundry;

public class MvcPhotoContext : DbContext
    {
        
        public DbSet<slogram.Models.Photo> Photo { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                //.AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                 // Add to configuration the Cloudfoundry VCAP settings
                .AddCloudFoundry()
                .Build();
             optionsBuilder.UseMySql(configuration);
             //For local
             //optionsBuilder.UseMySql("server=localhost;port=3306;database=mycontext;uid=root");
        }
    }
