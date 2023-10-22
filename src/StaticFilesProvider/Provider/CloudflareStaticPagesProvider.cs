using Pulumi.Experimental.Provider;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

public class CloudflareStaticPagesProvider : Pulumi.Experimental.Provider.Provider
{
    public const string UploadCommandType = "programmeral:cloudflare:pagesproject:StaticFiles";

    private readonly IHost _host;

    public CloudflareStaticPagesProvider(IHost host)
    {
        Console.WriteLine("Starting...");
        _host = host;
        _ = _host.LogAsync(new LogMessage(LogSeverity.Error, "In constructor"));
    }

    public override Task<ConfigureResponse> Configure(ConfigureRequest request, CancellationToken ct)
    {
        Console.WriteLine("Configure()...");
        return Task.FromResult(new ConfigureResponse());
    }

    public override async Task<CheckResponse> Check(CheckRequest request, CancellationToken ct)
    {
        Console.WriteLine("Check()...");
        await Task.CompletedTask;
        //await _host.LogAsync(new LogMessage(LogSeverity.Info, "In Check()"));
        if (IsUploadCommandType(request.Type))
        {
            return new CheckResponse() { Inputs = request.NewInputs };
        }

        throw new Exception($"Unknown resource type '{request.Type}'");
    }

    public override async Task<DiffResponse> Diff(DiffRequest request, CancellationToken ct)
    {
        Console.WriteLine("Diff()...");
        await Task.CompletedTask;
        //await _host.LogAsync(new LogMessage(LogSeverity.Info, "In Diff()"));
        //TODO: Impliment this better
        return new DiffResponse()
        {
            Changes = true
        };
    }

    public override async Task<CreateResponse> Create(CreateRequest request, CancellationToken ct)
    {
        Console.WriteLine("Create()...");
        //await _host.LogAsync(new LogMessage(LogSeverity.Info, "In Create()"));
        if (IsUploadCommandType(request.Type))
        {
            var projectName = "integration-example-project";
            var branch = "my-branch";
            var uploadDirectory = @"C:/GitHub/ProgrammerAl/ProgrammerAL.Pulumi.CloudflarePages.StaticFilesComponent/static-content";
            var command = $@"pages deploy ""{uploadDirectory}"" --project-name {projectName} --branch {branch} --commit-dirty=true";

            var processArgs = new ProcessStartInfo("wrangler.cmd", arguments: command);
            var process = Process.Start(processArgs);
            if (process is null)
            {
                throw new Exception("Upload process is null");
            }
            else
            {
                await process.WaitForExitAsync();
                return new CreateResponse() { Id = "abc123" };
            }
        }

        throw new Exception($"Unknown resource type '{request.Type}'");
    }

    private bool IsUploadCommandType(string type)
        => string.Equals(type, UploadCommandType, StringComparison.OrdinalIgnoreCase);
}
