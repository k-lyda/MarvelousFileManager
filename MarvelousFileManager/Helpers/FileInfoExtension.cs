using System;
using System.IO;

namespace MarvelousFileManager.Helpers
{
    public static class FileInfoExtensions
    {
        #region FileInfoExtenstions
        public static void MoveTo(this FileInfo file, string destFileName, bool overwrite)
        {
            file.CopyTo(destFileName, overwrite: true);
            file.Delete();
        }
        #endregion

        #region DirectoryInfoExtenstions
        public static void MoveTo(this DirectoryInfo source, string rootDestinationDirectory, bool recursive)
        {
            CopyTo(source, rootDestinationDirectory, recursive);
            source.Delete(recursive);
        }
       
        //recursive version of standard CopyTo method
        public static void CopyTo(this DirectoryInfo source, string rootDestinationDirectory, bool recursive)
        {
            if (string.IsNullOrEmpty(rootDestinationDirectory))
            {
                throw new ArgumentNullException("destinationDirectory");
            }

            if (!Directory.Exists(rootDestinationDirectory))
            {
                Directory.CreateDirectory(rootDestinationDirectory);
            }

            string destinationDirectoryToCreate = Path.Combine(rootDestinationDirectory, source.Name);

            if (!Directory.Exists(destinationDirectoryToCreate))
            {
                Directory.CreateDirectory(destinationDirectoryToCreate);
            }

            foreach (string file in Directory.GetFiles(source.FullName))
            {
                File.Copy(file, Path.Combine(destinationDirectoryToCreate, Path.GetFileName(file)), true);
            }

            if (recursive)
            {
                foreach (DirectoryInfo directory in source.GetDirectories())
                {
                    CopyTo(directory, destinationDirectoryToCreate, true);
                }
            }
        }

        public static void CopyTo(this DirectoryInfo source, string destinationDirectory)
        {
            CopyTo(source, destinationDirectory, false);
        }
        #endregion
    }
}
