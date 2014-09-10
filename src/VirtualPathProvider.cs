using System;
using System.Linq;

namespace Eklam.Lift
{
    public class VirtualPathProvider<T> : System.Web.Hosting.VirtualPathProvider where T : ILinfIndexTemplate
    {
        private readonly Type[] Types;
        private string VirtualIndexPath = "/Views/{0}/Index.cshtml";

        public VirtualPathProvider(params Type[] types)
        {
            this.Types = types;
        }

        public string GetVirtualPath(string name)
        {
            return string.Format(VirtualIndexPath, name);
        }

        public Type GetCustomType(string virtualPath)
        {
            return Types.FirstOrDefault(t => virtualPath.Contains(GetVirtualPath(t.Name)));
        }

        public override bool FileExists(string virtualPath)
        {
            var exists = base.FileExists(virtualPath);

            if (exists)
                return true;

            var customType = GetCustomType(virtualPath);

            return customType != null;
        }

        public override System.Web.Caching.CacheDependency GetCacheDependency(string virtualPath, System.Collections.IEnumerable virtualPathDependencies, DateTime utcStart)
        {
            var exists = base.FileExists(virtualPath);
            if (exists)
                return base.GetCacheDependency(virtualPath, virtualPathDependencies, utcStart);

            var customType = GetCustomType(virtualPath);
            if (customType != null)
                return null;

            return base.GetCacheDependency(virtualPath, virtualPathDependencies, utcStart);
        }

        public override System.Web.Hosting.VirtualFile GetFile(string virtualPath)
        {
            var exists = base.FileExists(virtualPath);
            if (exists)
                return base.GetFile(virtualPath);

            var customType = GetCustomType(virtualPath);
            if (customType != null)
            {
                var page = System.Activator.CreateInstance<T>();
                page.Data = customType;

                string pageContent = ((dynamic)page).TransformText();

                // TODO: Ajustar a acentuação...

                var bytes = System.Text.Encoding.UTF8.GetBytes(pageContent);

                return new VirtualFile(virtualPath, bytes);
            }

            return base.GetFile(virtualPath);
        }
    }
}