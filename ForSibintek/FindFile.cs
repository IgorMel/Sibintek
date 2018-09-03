using System;
using System.IO;
using ForSibintek.Models;
using ForSibintek.Tasks;
using System.Security.Cryptography;
using System.Threading;

namespace ForSibintek
{
    public class FindFile
    {
        DirectoryInfo dir;
        TaskQueue<string> FilesQueue;
        QueueDb queueDb;
        int count;
        public FindFile(DirectoryInfo directory)
        {
            queueDb = new QueueDb();
            FilesQueue = new TaskQueue<string>(4, n =>
              {
                  var file = new Models.File
                  {
                      Path = n,
                      CreateTime = DateTime.Now
                  };
                  try
                  {
                      using (var fs = System.IO.File.OpenRead(n))
                      {
                          file.Hashe =
                          new SHA1Managed()
                              .ComputeHash(fs);
                      }
                  }
                  catch (Exception e)
                  {
                      file.FileError =
                          new FileError
                          {
                              ErrorMessage = e.Message
                          };
                  }
                  queueDb.EncueueTask(file);
              });
            dir = directory;
            count = 0;
        }

        public void Start()
        {
            var thread = new Thread(() => FindFiles(dir));
            thread.Start();
            thread.Join();
            queueDb.Count=count;
            FilesQueue.Count = count;
            Console.WriteLine($"Кол-во {count}");
        }

        private void FindFiles(DirectoryInfo directory)
        {
            var files = directory.GetFiles();
            count += files?.Length??0;
            foreach (var file in files)
            {
                FilesQueue.EnqueueTask(file.FullName);
            }
            
            foreach (var dir in directory.GetDirectories())
            {
                FindFiles(dir);
            }
        }
    }
}