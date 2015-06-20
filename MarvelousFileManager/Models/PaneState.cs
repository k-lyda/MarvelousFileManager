using MarvelousFileManager.Helpers;
using System.IO;
using System.Linq;
namespace MarvelousFileManager.Models
{
    public class PaneState
    {
        public PaneType Type { get; set; }

        public string Path { get; set; }

        public FileInfo[] Files { get; set; }
        public DirectoryInfo[] Dirs { get; set; }

        public PaneState(PaneType type, string path)
        {
            this.Type = type;
            this.Path = path;
        }

        public void LoadFilesAndDirs(string rootPath)
        {
            var dir = new DirectoryInfo(Utilities.GetAbsolutePath(rootPath, Path));
            if (dir.Exists)
            {
                Files = dir.GetFiles();
                Dirs = dir.GetDirectories().Where(d => !d.Name.Contains(".git")).ToArray();
            }
        }

        public enum PaneType
        {
            Left, Right
        }
    }
}