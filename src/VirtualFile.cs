using System;
using System.IO;
using System.Linq;

namespace Eklam.Lift
{
    public class VirtualFile : System.Web.Hosting.VirtualFile
    {
        private byte[] data;

        public VirtualFile(string virtualPath, byte[] data)
            : base(virtualPath)
        {
            this.data = data;
        }

        public override System.IO.Stream Open()
        {
            return new MemoryStream(data);
        }
    }
}