using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MarvelousFileManager.Filters
{
    public class RootDirExitsFilter : ActionFilterAttribute, IActionFilter
    {
        public string RootPath { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!Directory.Exists(this.RootPath))
            {
                Directory.CreateDirectory(this.RootPath);
            }

            base.OnActionExecuting(filterContext);
        }
    }
}
