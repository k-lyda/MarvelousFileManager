using System.IO;

namespace MarvelousFileManager.Helpers
{
    public static class Utilities
    {
        public static string GetAbsolutePath(string rootPath, string relativePath)
        {
            //combining paths requires relative path with no starting slash
            if (relativePath.StartsWith("/"))
            {
                relativePath = relativePath.TrimStart('/');
            }

            //combining relative with root windows path need to replace slash to quoted backslash
            relativePath = relativePath.Replace("/", "\\");
            return Path.Combine(rootPath, relativePath);
        }
    }
}
