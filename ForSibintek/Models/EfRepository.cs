using System;
using System.Linq;

namespace ForSibintek.Models
{
    internal class EfRepository : IRepository
    {
        private ApplicationContext db;
        public EfRepository()
        {
            this.db = new ApplicationContext();
        }
        public void Create(File item)
        {
            FileError error;
            if (item.FileError != null)
            {
                error = db.FileErrors
                  .FirstOrDefault(n => n.ErrorMessage == item.FileError.ErrorMessage);
                if (error != null)
                    item.FileError = error;
            }
            db.Files.Add(item);
            db.SaveChanges();
        }
        private bool disposed = false;
        public void Dispose(bool disposing)
        {
            if(!this.disposed)
            {
                if (disposing)
                    db.Dispose();
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}