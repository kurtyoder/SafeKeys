using SafeKeys.Library.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace SafeKeys.Library
{
    public class XamlDataAccess : IDataAccess
    {
        public SaveFileModel Get(string filePath)
        {
            if (File.Exists(filePath))
            {
                var xs = new XmlSerializer(typeof(SaveFileModel));

                using (var fs = new FileStream(filePath, FileMode.Open))
                {
                   return xs.Deserialize(fs) as SaveFileModel;
                }
            }
            else
            {
                return null;
            }
        }

        public void Save(SaveFileModel saveFile, string filePath)
        {
            var xs = new XmlSerializer(typeof(SaveFileModel));

            using (TextWriter writer = new StreamWriter(filePath))
            {
                xs.Serialize(writer, saveFile);
            }
        }      

    }
}