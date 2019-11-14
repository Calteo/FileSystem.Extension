using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace FileSystem.Extension
{
    public static class HardLink
    {
        public static void Create(string filename, string existingFilename)
        {
            if (!File.Exists(existingFilename))
                throw new FileNotFoundException("source not found", existingFilename);

            if (!Native.CreateHardLink(filename, existingFilename, IntPtr.Zero))
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }            
        }

        public static IEnumerable<string> Enumerate(string filename)
        {
            return new HardLinks(filename);
        }
        
    }
}
