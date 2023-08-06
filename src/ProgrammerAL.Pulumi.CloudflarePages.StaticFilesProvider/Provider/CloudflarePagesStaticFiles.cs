using OneOf.Types;

using Pulumi;
using Pulumi.Experimental.Provider;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgrammerAL.Pulumi.CloudflarePages.StaticFilesProvider;

public class CloudflarePagesStaticFilesArgs : ResourceArgs
{ 

}

public class CloudflarePagesStaticFiles : ComponentResource
{
    public CloudflarePagesStaticFiles(string name, ResourceArgs? args, ComponentResourceOptions? options = null, bool remote = false)
        : base(type: nameof(CloudflarePagesStaticFiles), name, args, options, remote)
    {
        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "wrangler",
                Arguments = "pages deploy --project-name practice \"C:\\GitHub\\ProgrammerAl\\ProgrammerAL.Pulumi.CloudflarePages.StaticFilesProvider\\static-content\"",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            }
        };

        _ = process.Start();
        process.WaitForExit();
    }
}
