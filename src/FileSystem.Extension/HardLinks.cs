using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSystem.Extension
{
    class HardLinks : IEnumerable<string>
    {
        public HardLinks(string filename)
        {
            Filename = filename;
        }

        public string Filename { get; private set; }

        public IEnumerator<string> GetEnumerator()
        {
            return new HardLinksEnumerator(Filename);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
