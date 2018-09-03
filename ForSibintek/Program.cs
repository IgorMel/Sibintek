using System;
using static System.Console;
using System.IO;
using System.Collections.Generic;
using System.Security.Cryptography;
using ForSibintek.Models;
using System.Linq;
using ForSibintek.Tasks;

namespace ForSibintek
{
    class Program
    {
        static void Main(string[] args)
        {

            string startDirectory = null;
            if(args?.Length > 0)
            {
                startDirectory = args[0];
            }
            else
            {
                Write("Введите путь к директории: ");
                startDirectory = ReadLine();
            }
            DirectoryInfo firstDirectory;
            while (!TryDirectoryInfo(startDirectory,out  firstDirectory))
            {
                WriteLine("Папка отсутствует или некорректное имя");
                Write("Повторите ввод(для выхода exit) ");
                startDirectory = ReadLine();
                if (startDirectory.ToLower() == "exit")
                    return;
            }
            new FindFile(firstDirectory,new EfRepository()).Start(); //Можно заменить репозиторий EfRepository на AdoRepository

            ReadLine();
        }

        private static bool TryDirectoryInfo(string path,out DirectoryInfo dir)
        {
            try
            {
                dir = new DirectoryInfo(path);
                if (dir.Exists)
                    return true;
                return false;
            }
            catch
            {
                dir = null;
                return false;
            }
        }
    }
}
