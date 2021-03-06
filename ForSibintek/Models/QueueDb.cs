﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ForSibintek.Tasks;

namespace ForSibintek.Models
{
    public class QueueDb
    {
        TaskQueue<File> taskQueue;
        public int Count {  set { taskQueue.Count = value; } }
        public QueueDb(IRepository repository)
        {
            taskQueue = new TaskQueue<File>(1,
            n =>
            {
                using (var db = repository.Context())
                {
                    db.Create(n);
                }
            });

        }
        public void EncueueTask(File file) 
            => taskQueue.EnqueueTask(file);
    }
}