using StateMigrationBackend.Validation;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;
using System.Threading.Tasks;

namespace StateMigrationBackend.StateMigrationEngine
{
    
    class EnumData
    {
        CountModel model = new CountModel();

        private CountModel GetCount(DirectoryInfo source)
        {
            int count = 0;     
            
            foreach(var fileInfo in source.GetFiles())
            {
                if (fileInfo.Exists && ((File.GetAttributes(fileInfo.FullName) & FileAttributes.ReparsePoint) != FileAttributes.ReparsePoint))
                {
                    count++;
                    model.FileCount++;
                }
            }

            foreach (var childDirectoryInfo in source.GetDirectories())
            {

                if (childDirectoryInfo.Exists && (childDirectoryInfo.Attributes & FileAttributes.ReparsePoint) != FileAttributes.ReparsePoint)
                {
                    model.DirCount++;
                    count += GetCount(childDirectoryInfo).FileCount;
                }

            }
            
            return model;
        }

        internal async Task<CountModel> GetCountAsync(DirectoryInfo source)
        {
           return await Task.Factory.StartNew(() => GetCount(source));
        }

    }
    
    class CountModel
    {
        public int FileCount { get; set; } = 0;
        public int DirCount { get; set; } = 0;
    }

}
