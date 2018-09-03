using System.Collections.Generic;
namespace ForSibintek.Models
{
    public class FileError
    {
        public int Id { get; set; }
        public string ErrorMessage { get; set; }

        public ICollection<File> Files { get; set; }

        public FileError()
        {
            Files = new List<File>();
        }
    }
}