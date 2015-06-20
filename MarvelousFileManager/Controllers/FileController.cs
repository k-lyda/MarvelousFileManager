using MarvelousFileManager.Filters;
using MarvelousFileManager.Helpers;
using MarvelousFileManager.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace MarvelousFileManager.Controllers
{
    public class FileController : Controller
    {
        private string RootRelativePath { get { return System.Configuration.ConfigurationManager.AppSettings["rootPath"].ToString(); } }

        private string RootAbsolutePath { get { return Server.MapPath(this.RootRelativePath); } }

        // GET: File
        [HttpGet]
        public ActionResult Index(string leftPath = "", string rightPath = "")
        {
            if (!Directory.Exists(this.RootAbsolutePath))
            {
                Directory.CreateDirectory(this.RootAbsolutePath);
            }

            var messages = new List<Message>();
            if (!System.IO.Directory.Exists(Utilities.GetAbsolutePath(RootAbsolutePath, leftPath)))
            {
                messages.Add(new Message { Type = "warning", Text = String.Format("Folder {0} nie istnieje.", leftPath) });
                leftPath = "";
            }
            if (!System.IO.Directory.Exists(Utilities.GetAbsolutePath(RootAbsolutePath, rightPath)))
            {
                messages.Add(new Message { Type = "warning", Text = String.Format("Folder {0} nie istnieje.", rightPath) });
                rightPath = "";
            }

            ViewBag.Messages = messages;
            var state = new ManagerState(RootAbsolutePath, leftPath, rightPath);
            return View("Manager", state);
        }

        [HttpParamAction]
        public ActionResult Move(string[] files, string from, string to, string leftPath, string rightPath)
        {
            if (files == null || files.Length == 0)
            {
                return RedirectToAction("Index", new { leftPath = leftPath, rightPath = rightPath });
            }

            var copyStatus = CopyFiles(files, from, to, moveFile: true);

            if (copyStatus.StatusCode == 200)
            {
                return RedirectToAction("Index", new { leftPath = leftPath, rightPath = rightPath });
            }
            else
            {
                return View("Error");
            }

        }

        [HttpParamAction]
        public ActionResult Copy(string[] files, string from, string to, string leftPath, string rightPath)
        {
            if (files == null || files.Length == 0)
            {
                return RedirectToAction("Index", new { leftPath = leftPath, rightPath = rightPath });
            }

            var copyStatus = CopyFiles(files, from, to);

            if (copyStatus.StatusCode == 200)
            {
                return RedirectToAction("Index", new { leftPath = leftPath, rightPath = rightPath });
            }
            else
            {
                return View("Error");
            }

        }

        [HttpParamAction]
        public ActionResult Delete(string[] files, string from, string leftPath, string rightPath)
        {
            foreach (var fileName in files)
            {
                var fileAbsolutePath = Path.Combine(Utilities.GetAbsolutePath(RootAbsolutePath, from), fileName);

                FileAttributes attributes = System.IO.File.GetAttributes(fileAbsolutePath);

                if (attributes == FileAttributes.Directory)
                {
                    DirectoryInfo dir = new DirectoryInfo(fileAbsolutePath);
                    if (dir.Exists)
                    {
                        dir.Delete(recursive: true);
                    }
                }
                else
                {
                    FileInfo file = new FileInfo(fileAbsolutePath);

                    if (file.Exists)
                    {
                        file.Delete();
                    }
                }
            }

            return RedirectToAction("Index", new { leftPath = leftPath, rightPath = rightPath });
        }

        private HttpStatusCodeResult CopyFiles(string[] fileNames, string from, string to, bool moveFile = false)
        {
            if (fileNames != null && fileNames.Length == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.OK, "Nothing to do");
            }

            string fromAbsolutePath = Utilities.GetAbsolutePath(this.RootAbsolutePath, from);
            string toAbsolutePath = Utilities.GetAbsolutePath(this.RootAbsolutePath, to);

            foreach (var fileName in fileNames)
            {
                var fileAbsolutePath = Path.Combine(fromAbsolutePath, fileName);
                FileAttributes attributes = System.IO.File.GetAttributes(fileAbsolutePath);

                if (attributes == FileAttributes.Directory)
                {
                    DirectoryInfo dir = new DirectoryInfo(fileAbsolutePath);
                    if (dir.Exists)
                    {
                        if (moveFile)
                        {
                            dir.MoveTo(toAbsolutePath, recursive: true);
                        }
                        else
                        {
                            dir.CopyTo(toAbsolutePath, recursive: true);
                        }
                    }
                }
                else
                {
                    FileInfo file = new FileInfo(fileAbsolutePath);

                    if (file.Exists)
                    {
                        if (moveFile)
                        {
                            file.MoveTo(Path.Combine(toAbsolutePath, file.Name), overwrite: true);
                        }
                        else
                        {
                            file.CopyTo(Path.Combine(toAbsolutePath, file.Name), overwrite: true);
                        }
                    }
                }
            }

            return new HttpStatusCodeResult(HttpStatusCode.OK, "Ok");
        }

        public class HttpParamActionAttribute : ActionNameSelectorAttribute
        {
            public override bool IsValidName(ControllerContext controllerContext, string actionName, MethodInfo methodInfo)
            {
                if (actionName.Equals(methodInfo.Name, StringComparison.InvariantCultureIgnoreCase))
                    return true;

                var request = controllerContext.RequestContext.HttpContext.Request;
                return request[methodInfo.Name] != null;
            }
        }

        
    }
}