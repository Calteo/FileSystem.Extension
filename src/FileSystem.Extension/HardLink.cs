using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;

namespace FileSystem.Extension
{
    /// <summary>
    /// Class to access hard links in an NTFS file system
    /// </summary>
    public static class HardLink
    {
        /// <summary>
        /// Creates a new hard link
        /// </summary>
        /// <param name="filename">hard link to create</param>
        /// <param name="existingFilename">existing file to link to</param>
        /// <remarks>
        /// The files must reside on the same volume.
        /// </remarks>
        public static void Create(string filename, string existingFilename)
        {
            if (!File.Exists(existingFilename))
                throw new FileNotFoundException("source not found", existingFilename);

            if (!Native.CreateHardLink(filename, existingFilename, IntPtr.Zero))
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }            
        }

        /// <summary>
        /// Enumerates all hard links of a given file
        /// </summary>
        /// <param name="filename">All hard links of this file will be enumerated.</param>
        /// <param name="includeSelf"><c>true</c> if the give file should be include in the list - else not.</param>
        /// <returns></returns>
        public static IEnumerable<string> Enumerate(string filename, bool includeSelf = true)
        {
            return new HardLinks(filename, includeSelf);
        }

        /// <summary>
        /// Get all hard links of a given file
        /// </summary>
        /// <param name="filename">All hard links of this file will be enumerated.</param>
        /// <param name="includeSelf"><c>true</c> if the give file should be include in the list - else not.</param>
        /// <returns>An array of hard links</returns>
        public static string[] GetLinks(string filename, bool includeSelf = true)
        {
            return Enumerate(filename, includeSelf).ToArray();
        }
        
    }
}
