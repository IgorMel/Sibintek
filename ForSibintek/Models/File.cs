using System;
namespace ForSibintek.Models
{
    public class File
    {
        public int Id { get; set; }
        public string Path { get; set; }
        public byte[] Hashe { get; set; }
        public DateTime CreateTime { get; set; }

        public FileError FileError { get; set; }
        public int? FileErrorId { get; set; }
    }
}