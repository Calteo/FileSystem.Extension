using System;
using System.Runtime.InteropServices;
using System.Text;

namespace FileSystem.Extension
{
    internal class Native
    {
        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        internal static extern bool CreateHardLink(string lpFileName, string lpExistingFileName, IntPtr lpSecurityAttributes);

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        internal static extern IntPtr FindFirstFileNameW(
           string lpFileName,
           uint dwFlags,
           ref int stringLength,
           StringBuilder fileName);

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        internal static extern bool FindNextFileNameW(
            IntPtr hFindStream,
            ref int stringLength,
            StringBuilder fileName);

        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern bool FindClose(IntPtr fFindHandle);

        internal static readonly IntPtr InvalidHandle = new IntPtr(-1);

        internal static ErrorCode GetLastError()
        {
            return (ErrorCode)Marshal.GetLastWin32Error();
        }
    }

    internal enum ErrorCode
    {
        HandleEof = 38,
        MoreData = 234,
    }
}
