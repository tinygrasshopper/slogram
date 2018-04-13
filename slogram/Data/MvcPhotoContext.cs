using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using slogram.Models;

    public class MvcPhotoContext : DbContext
    {
        
        public DbSet<slogram.Models.Photo> Photo { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=slogram-local.db");
        }
    }
