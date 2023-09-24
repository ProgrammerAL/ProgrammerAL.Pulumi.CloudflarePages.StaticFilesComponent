using System;
using System.Collections.Immutable;
using System.Diagnostics;


//wrangler pages deploy --projectName "integration-example-project" --branch "my-prod" --commit-dirty=true --directory "C:\GitHub\ProgrammerAl\ProgrammerAL.Pulumi.CloudflarePages.StaticFilesComponent\static-content 2"
var proccessArgs = new ProcessStartInfo("wrangler", "pages deploy --projectName \"integration-example-project\" --branch \"my-prod\" --commit-dirty=true --directory \"C:/GitHub/ProgrammerAl/ProgrammerAL.Pulumi.CloudflarePages.StaticFilesComponent/static-content 2\"")
{
    RedirectStandardError = true,
    RedirectStandardOutput = true,
    //WorkingDirectory = "C:\\Users\\Progr\\AppData\\Roaming\\npm"
};
var proccess = Process.Start(proccessArgs);
proccess.WaitForExit();

Console.WriteLine($"Standard Output: {await proccess.StandardOutput.ReadToEndAsync()}");
Console.WriteLine($"Standard Error: {await proccess.StandardError.ReadToEndAsync()}");
Console.WriteLine("done");

