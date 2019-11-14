using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace FileSystem.Extension
{
    class HardLinksEnumerator : IEnumerator<string>
    {
        public HardLinksEnumerator(string filename)
        {
            Filename = filename;
            Drive = Path.GetPathRoot(Path.GetFullPath(filename)).TrimEnd('\\');
        }

        public string Filename { get; private set; }
        public string Drive { get; private set; }

        private StringBuilder _current;
        private IntPtr _handle;

        public string Current => _current?.ToString();

        object IEnumerator.Current => Current;

        public bool MoveNext()
        {
            if (_handle == IntPtr.Zero)
            {
                _current = new StringBuilder();
                var length = _current.Length;
                _handle = Native.FindFirstFileNameW(Filename, 0, ref length, _current);
                if (_handle == Native.InvalidHandle)
                {
                    var lastError = Native.GetLastError();
                    if (lastError == ErrorCode.MoreData)
                    {
                        _current.Length = length + 1;
                        _handle = Native.FindFirstFileNameW(Filename, 0, ref length, _current);
                        if (_handle == Native.InvalidHandle)
                            throw new Win32Exception((int)Native.GetLastError());
                    }
                    else
                    {
                        throw new Win32Exception((int)lastError);
                    }
                }
            }
            else
            {
                var length = _current.Length;
                if (!Native.FindNextFileNameW(_handle, ref length, _current))
                {
                    var lastError = Native.GetLastError();
                    if (lastError == ErrorCode.MoreData)
                    {
                        _current.Length = length + 1;
                        if (!Native.FindNextFileNameW(_handle, ref length, _current))
                            throw new Win32Exception((int)Native.GetLastError());
                    }
                    else if (lastError == ErrorCode.HandleEof)
                    {
                        _current = null;
                        return false;
                    }
                    else
                    {
                        throw new Win32Exception((int)lastError);
                    }
                }
                
            }
            _current.Insert(0, Drive);
            return true;
        }

        public void Reset()
        {
            _current = null;
            if (_handle != null)
            {
                if (!Native.FindClose(_handle))
                    throw new Win32Exception(Marshal.GetLastWin32Error());

                _handle = IntPtr.Zero;
            }
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {                    
                }

                Reset();

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // Do not change this code. Put cleanup code in Dispose(bool disposing) above.        
        ~HardLinksEnumerator()
        {        
           Dispose(false);
        }        

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
