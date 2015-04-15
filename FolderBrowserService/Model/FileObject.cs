using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace FolderBrowser.Service
{
    /// <summary>
    /// The File Object class which represents attributes of a file under consideration
    /// </summary>
    [Serializable]
    public class FileObject
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public long Size { get; set; }
        // Make these variables internal so that they don't appear on the grid
        // The Data for a File object is collected once only
        internal string DirectoryName { get;set;}
        internal bool IsReadOnly { get; set; }
        internal FileAttributes Attributes { get; set; }
        internal DateTime CreationTime { get; set; }
        internal string Extension { get; set; }
        internal DateTime LastAccessTime { get; set; }
    }
}
