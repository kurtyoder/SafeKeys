using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SafeKeys.Library.API
{
    public class SettingHandler : ISettingHandler
    {
        private const string _filePath = "SafeKeys.txt";

        private string GetLocalFilePath(string fileName)
        {
            string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            return Path.Combine(appData, fileName);
        }

        public string GetPath()
        {
            if(File.Exists(GetLocalFilePath(_filePath)))
            {
                return File.ReadAllText(GetLocalFilePath(_filePath));
            }

            return null;
            
        }

        public void SavePath(string path)
        {
            File.WriteAllText(GetLocalFilePath(_filePath), path);
        }
    }
}
