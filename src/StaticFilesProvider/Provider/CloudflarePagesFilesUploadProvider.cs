using ProgrammerAL.PulumiComponent.CloudflarePages.PagesFilesUpload;
using ProgrammerAL.PulumiComponent.CloudflarePages.PagesFilesUpload.Exceptions;
using ProgrammerAL.PulumiComponent.CloudflarePages.PagesFilesUpload.ProviderArgs;

using Pulumi;
using Pulumi.Experimental.Provider;

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

public class CloudflarePagesFilesUploadProvider : Pulumi.Experimental.Provider.Provider
{
    public const string UploadCommandType = "programmeral:cloudflare:pagesproject:StaticFiles:Upload";

    private readonly IHost _host;

    public CloudflarePagesFilesUploadProvider(IHost host)
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
        var runArgs = new RunArgs(request.Properties);
        var commandName = "wrangler.cmd";
        var commandArgumentsString = GenerateDeployCommandArguments(runArgs);
        await Console.Out.WriteLineAsync($"CommandArguments: {commandArgumentsString}");
        //var triggers = GenerateTriggers(args);
        //var environmentVariables = GenerateEnvironmentVariables(args);


        Console.WriteLine("Create()...");
        //await _host.LogAsync(new LogMessage(LogSeverity.Info, "In Create()"));
        if (IsUploadCommandType(request.Type))
        {
            //var projectName = "integration-example-project";
            //var branch = "my-branch";
            //var uploadDirectory = @"C:/GitHub/ProgrammerAl/ProgrammerAL.Pulumi.CloudflarePages.StaticFilesComponent/static-content";
            //var command = $@"pages deploy ""{uploadDirectory}"" --project-name {projectName} --branch {branch} --commit-dirty=true";

            var processArgs = new ProcessStartInfo(commandName, arguments: commandArgumentsString);
            if (!string.IsNullOrWhiteSpace(runArgs.WorkingDirectory))
            {
                processArgs.WorkingDirectory = runArgs.WorkingDirectory;
            }

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

    public override async Task<UpdateResponse> Update(UpdateRequest request, CancellationToken ct)
    {
        var runArgs = new RunArgs(request.News);
        var commandName = "wrangler.cmd";
        var commandArgumentsString = GenerateDeployCommandArguments(runArgs);
        await Console.Out.WriteLineAsync($"CommandArguments: {commandArgumentsString}");
        //var triggers = GenerateTriggers(args);
        //var environmentVariables = GenerateEnvironmentVariables(args);


        Console.WriteLine("Create()...");
        //await _host.LogAsync(new LogMessage(LogSeverity.Info, "In Create()"));
        if (IsUploadCommandType(request.Type))
        {
            //var projectName = "integration-example-project";
            //var branch = "my-branch";
            //var uploadDirectory = @"C:/GitHub/ProgrammerAl/ProgrammerAL.Pulumi.CloudflarePages.StaticFilesComponent/static-content";
            //var command = $@"pages deploy ""{uploadDirectory}"" --project-name {projectName} --branch {branch} --commit-dirty=true";

            var processArgs = new ProcessStartInfo(commandName, arguments: commandArgumentsString);
            if (!string.IsNullOrWhiteSpace(runArgs.WorkingDirectory))
            {
                processArgs.WorkingDirectory = runArgs.WorkingDirectory;
            }

            var process = Process.Start(processArgs);
            if (process is null)
            {
                throw new Exception("Upload process is null");
            }
            else
            {
                await process.WaitForExitAsync();
                return new UpdateResponse() { };
            }
        }

        throw new Exception($"Unknown resource type '{request.Type}'");
    }

    public override Task Delete(DeleteRequest request, CancellationToken ct)
    {
        return Task.CompletedTask;
    }

    private bool IsUploadCommandType(string type)
        => string.Equals(type, UploadCommandType, StringComparison.OrdinalIgnoreCase);


    private InputMap<string> GenerateEnvironmentVariables(CloudflarePagesFilesUploadArgs args)
    {
        var map = args.Environment;
        if (args.Authentication is object)
        {
            var accountId = args.Authentication.Apply(auth => auth.AccountId);
            var apiToken = args.Authentication.Apply(auth => auth.ApiToken);

            map.Add("CLOUDFLARE_ACCOUNT_ID", accountId);
            map.Add("CLOUDFLARE_API_TOKEN", apiToken);
        }

        return map;
    }

    private string GenerateDeployCommandArguments(RunArgs args)
    {
        //Get the parameter to set for the branch
        string branchCommandArgument = "";
        if (!string.IsNullOrWhiteSpace(args.Branch))
        {
            //Note: Space at end matters
            branchCommandArgument = $"--branch {args.Branch} ";
        }

        var command = $@"pages deploy ""{args.UploadDirectory}"" --projectName ""{args.ProjectName}"" {branchCommandArgument}--commit-dirty=true";
        return command;
    }

    private InputList<object> GenerateTriggers(CloudflarePagesFilesUploadArgs args)
    {
        var fullUploadPath = DetermineFullUploadDirectoryPath(args);

        var pathTriggers = fullUploadPath.Apply(x =>
        {
            var dirPath = x;

            if (!Directory.Exists(dirPath))
            {
                throw new DirectoryDoesNotExistException($"Directory does not exist at path: {dirPath}");
            }

            var staticFilesChecksums = Directory.GetFiles(dirPath, "*", SearchOption.AllDirectories)
            .OrderBy(fileName => fileName)
            .Select(fileName =>
            {
                var fileBytes = File.ReadAllBytes(fileName);
                var fileChecksum = System.Security.Cryptography.SHA256.HashData(fileBytes);
                var fileChecksumBase64 = Convert.ToBase64String(fileChecksum);
                return fileChecksumBase64;
            }).ToImmutableArray();

            return staticFilesChecksums;
        });

        var inputTriggers = args.Triggers;

        inputTriggers.AddRange(pathTriggers);
        return inputTriggers;
    }

    private static Output<string> DetermineFullUploadDirectoryPath(CloudflarePagesFilesUploadArgs args)
    {
        var workingDirectoryInput = args.WorkingDirectory ?? "";

        var fullUploadPath = Output.Tuple(workingDirectoryInput, args.UploadDirectory).Apply(x =>
        {
            var workingDirectory = x.Item1;
            var uploadDirectory = x.Item2;

            if (string.IsNullOrWhiteSpace(workingDirectory))
            {
                return uploadDirectory;
            }
            else
            {
                return Path.Combine(workingDirectory, uploadDirectory);
            }
        });

        return fullUploadPath;
    }
}
