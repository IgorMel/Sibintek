using System;

namespace ForSibintek.Models
{
    public interface IRepository:IDisposable
    {
        void Create(File item);
        IRepository Context();
    }
}