using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Processing.Filters;
using slogram.Models;

namespace slogram.Controllers
{
    public class PhotosController : Controller
    {
        private readonly MvcPhotoContext _context;
        private readonly IHostingEnvironment _hostingEnvironment;


        public PhotosController(MvcPhotoContext context, IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }

        // GET: Photos
        public async Task<IActionResult> Index()
        {
            return View(new PhotoViewModel(await _context.Photo.ToListAsync()));
        }

        // GET: Photos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var photo = await _context.Photo
                .SingleOrDefaultAsync(m => m.ID == id);
            if (photo == null)
            {
                return NotFound();
            }

            return View(photo);
        }

        // GET: Photos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Photos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Title,Processed,RawUrl,ProcessedUrl,Guid")] Photo photo, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                var imageGuid = Guid.NewGuid().ToString();
                var savedImagePath = Path.Combine("images", "uploads", imageGuid + Path.GetExtension(file.FileName));
                var processedImagePath = Path.Combine("images", "uploads", imageGuid + "_processed" + Path.GetExtension(file.FileName));

                var savedImageFullPath = Path.Combine(Path.Combine(_hostingEnvironment.WebRootPath, savedImagePath));
                var processedFullPath = Path.Combine(Path.Combine(_hostingEnvironment.WebRootPath, processedImagePath));

                photo.Guid = imageGuid;

                var oldBlob = AzureBlob(photo.Guid + "unprocessed");
                //oldBlob.StartCopyAsync()
                await oldBlob.UploadFromStreamAsync(file.OpenReadStream());

                photo.RawUrl = oldBlob.StorageUri.PrimaryUri.AbsoluteUri;

                FileStream fileStream = new FileStream(savedImageFullPath, FileMode.OpenOrCreate);
                file.CopyTo(fileStream);
                fileStream.Close();

                _context.Add(photo);
                await _context.SaveChangesAsync();

                Task.Run(() => ProcessImageAsync(photo.ID));

                return RedirectToAction(nameof(Index));
            }
            return View(photo);
        }

        public static async void ProcessImageAsync(int photoId)
        {
            Thread.Sleep(5000);
            var guid = Guid.NewGuid().ToString();
            var rootPath = Path.GetTempPath();
            var savedImageFullPath = Path.Combine(Path.Combine(rootPath, guid+ "unprocessed"));
            var processedFullPath = Path.Combine(Path.Combine(rootPath, guid+ "processed.jpg"));



            using (var db = new MvcPhotoContext())
            {
                var photo = db.Find<Photo>(photoId);
                var oldBlob = AzureBlob(photo.Guid + "unprocessed");
                await oldBlob.DownloadToFileAsync(savedImageFullPath, FileMode.Create);

                using (Image<Rgba32> image = Image.Load(savedImageFullPath))
                {
                    image.Mutate(ctx => ctx.Polaroid());

                    image.Save(processedFullPath); // Automatic encoder selected based on extension.
                }

                var newBlob = AzureBlob(photo.Guid + "processed");
                await newBlob.UploadFromFileAsync(processedFullPath);

                photo.ProcessedUrl = newBlob.StorageUri.PrimaryUri.AbsoluteUri;
                photo.Processed = true;
                db.Update(photo);
                db.SaveChanges();
            }
        }

        public static CloudBlockBlob AzureBlob(string blobpath)  {
            var cloudStorageAccount = CloudStorageAccount.Parse(Environment.GetEnvironmentVariable("storageconnectionstring"));
            var cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();

            var container = cloudBlobClient.GetContainerReference("slogramblobcontainer");
            return container.GetBlockBlobReference(blobpath);
        }

        // GET: Photos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var photo = await _context.Photo.SingleOrDefaultAsync(m => m.ID == id);
            if (photo == null)
            {
                return NotFound();
            }
            return View(photo);
        }

        // POST: Photos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Title,Processed,RawUrl,ProcessedUrl,Guid")] Photo photo)
        {
            if (id != photo.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(photo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PhotoExists(photo.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(photo);
        }

        // GET: Photos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var photo = await _context.Photo
                .SingleOrDefaultAsync(m => m.ID == id);
            if (photo == null)
            {
                return NotFound();
            }

            return View(photo);
        }

        // POST: Photos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var photo = await _context.Photo.SingleOrDefaultAsync(m => m.ID == id);
            _context.Photo.Remove(photo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PhotoExists(int id)
        {
            return _context.Photo.Any(e => e.ID == id);
        }
    }
}
