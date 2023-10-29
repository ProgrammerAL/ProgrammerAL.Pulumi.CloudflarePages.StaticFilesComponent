using ProgrammerAL.PulumiComponent.CloudflarePages.PagesFilesUpload;

using Pulumi;
using Pulumi.Cloudflare;

using System.Collections.Generic;

return await Pulumi.Deployment.RunAsync(() =>
{
    var config = new Pulumi.Config();
    var accountId = config.RequireSecret("account-id");
    var apiToken = config.RequireSecret("api-token");

    var cloudflareProvider = new Provider("my-cloudflare-provider", new ProviderArgs
    {
        ApiToken = apiToken
    });

    var projectName = "integration-example-project";
    var productionBranch = "my-prod";
    var pagesApp = new PagesProject(projectName, new PagesProjectArgs
    {
        Name = projectName,
        AccountId = accountId,
        ProductionBranch = productionBranch,
    }, options: new CustomResourceOptions
    { 
        Provider = cloudflareProvider
    });

    var staticFiles = new CloudflarePagesFilesUpload($"{projectName}-files", new CloudflarePagesFilesUploadArgs
    {
        ProjectName = pagesApp.Name,
        UploadDirectory = "C:/GitHub/ProgrammerAl/ProgrammerAL.Pulumi.CloudflarePages.StaticFilesComponent/static-content 2",
        Branch = productionBranch,
    });

    //return new Dictionary<string, object?>
    //{
    //    { "UploadResult", staticFiles.Stdout }
    //};
});
