using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarvelousFileManager.Models
{
    public class ManagerState
    {
        public string RootAbsolutePath { get; set; }

        public PaneState LeftPane { get; set; }
        public PaneState RightPane { get; set; }

        public ManagerState(string rootPath, string leftPath, string rightPath)
        {
            RootAbsolutePath = rootPath;
            LeftPane = new PaneState(PaneState.PaneType.Left, leftPath);
            RightPane = new PaneState(PaneState.PaneType.Right, rightPath);

            LoadFiles();
        }

        public void LoadFiles()
        {
            LeftPane.LoadFilesAndDirs(RootAbsolutePath);
            RightPane.LoadFilesAndDirs(RootAbsolutePath);
        }
    }
}
