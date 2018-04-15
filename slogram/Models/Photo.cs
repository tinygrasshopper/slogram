﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace slogram.Models
{
    public class Photo
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string Title { get; set; }
        public bool Processed{ get; set; }
        public string RawUrl { get; set; }
        public string ProcessedUrl { get; set; }
        public string Guid { get; set; }

        public Photo()
        {
        }
    }
}
