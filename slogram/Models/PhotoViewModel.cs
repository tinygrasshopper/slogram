using System;
using System.Collections.Generic;
using System.Linq;

namespace slogram.Models
{
    public class PhotoViewModel
    {
        public IEnumerable<Photo> Processed { get; set; }
        public IEnumerable<Photo> Unprocessed { get; set; }
        public PhotoViewModel(IEnumerable<Photo> photos)
        {
            Processed = photos.Where(p => p.Processed);
            Unprocessed = photos.Where(p => !p.Processed);
        }
    }
}
