using ProgrammerAL.Pulumi.CloudflarePages.StaticFilesProvider;

using Pulumi;

using System.Collections.Generic;

return await Pulumi.Deployment.RunAsync(() =>
{
    var cloudflarePagesStaticFiles = new CloudflarePagesStaticFiles(name: "my-static-files", new CloudflarePagesStaticFilesArgs
    { 
        
    });

    return new Dictionary<string, object?>
    {
    };
});
