using SafeKeys.Library.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SafeKeys.Library
{
    public interface IDataAccess
    {
        SaveFileModel Get(string filePath);
        void Save(SaveFileModel saveFile, string filePath);
    }
}