using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public static class FileHelper
{


    public static void SaveFile(string path, string content)
    {
        string fullPath = path + content;
        Directory.Delete(fullPath, true);
        Directory.CreateDirectory(fullPath);
    }

}
