using ProgrammerAL.PulumiComponent.CloudflarePages.StaticFilesComponent.Exceptions;

using Pulumi;
using Pulumi.Command.Local;

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;

namespace ProgrammerAL.PulumiComponent.CloudflarePages.StaticFilesComponent;

public class UploadStaticFilesCommand : global::Pulumi.CustomResource
{
    public UploadStaticFilesCommand(string name, UploadStaticFilesCommandArgs args, CustomResourceOptions? options = null)
        : base(CloudflareStaticPagesProvider.UploadCommandType, name, args, MakeResourceOptions(options, ""))
    {
        //var commandString = GenerateDeployCommand(args);
        //var triggers = GenerateTriggers(args);
        //var environmentVariables = GenerateEnvironmentVariables(args);

        //var commandResource = new Command($"{name}-upload-files-command", new CommandArgs
        //{
        //    Create = commandString,
        //    Update = commandString,
        //    Environment = environmentVariables,
        //    Triggers = triggers,
        //    Dir = args.WorkingDirectory
        //},
        //new CustomResourceOptions
        //{
        //    Parent = this
        //});

        //Command = commandString;
        //Environment = commandResource.Environment;
        //WorkingDirectory = commandResource.Dir;
        //Stderr = commandResource.Stderr;
        //Stdin = commandResource.Stdin;
        //Stdout = commandResource.Stdout;

        //RegisterOutputs();
    }

    private static CustomResourceOptions MakeResourceOptions(CustomResourceOptions? options, Input<string>? id)
    {
        var defaultOptions = new CustomResourceOptions
        {
            //Version = Utilities.Version,
            Version = "0.0.1"
        };
        var merged = CustomResourceOptions.Merge(defaultOptions, options);
        // Override the ID if one was specified for consistency with other language SDKs.
        merged.Id = id ?? merged.Id;
        return merged;
    }

    ///// <summary>
    ///// The command that is run when creating or updating the static files uploaded to the Cloudflare Pages project
    ///// </summary>
    //[Output("command")]
    //public Output<string> Command { get; private set; }

    ///// <summary>
    ///// The environment variables to use when running the command
    ///// </summary>
    //[Output("environment")]
    //public Output<ImmutableDictionary<string, string>?> Environment { get; private set; }

    ///// <summary>
    ///// The directory from which to run the command from. If `dir` does not exist, then
    ///// `Command` will fail.
    ///// </summary>
    //[Output("workingDirectory")]
    //public Output<string?> WorkingDirectory { get; private set; }

    ///// <summary>
    ///// The standard error of the command's process
    ///// </summary>
    //[Output("stderr")]
    //public Output<string> Stderr { get; private set; }

    ///// <summary>
    ///// Pass a string to the command's process as standard in
    ///// </summary>
    //[Output("stdin")]
    //public Output<string?> Stdin { get; private set; }

    ///// <summary>
    ///// The standard output of the command's process
    ///// </summary>
    //[Output("stdout")]
    //public Output<string> Stdout { get; private set; }

    //private InputMap<string> GenerateEnvironmentVariables(UploadStaticFilesCommandArgs args)
    //{
    //    var map = args.Environment;
    //    if (args.Authentication is object)
    //    {
    //        var accountId = args.Authentication.Apply(auth => auth.AccountId);
    //        var apiToken = args.Authentication.Apply(auth => auth.ApiToken);

    //        map.Add("CLOUDFLARE_ACCOUNT_ID", accountId);
    //        map.Add("CLOUDFLARE_API_TOKEN", apiToken);
    //    }

    //    return map;
    //}

    //private Output<string> GenerateDeployCommand(UploadStaticFilesCommandArgs args)
    //{
    //    //Get the parameter to set for the branch
    //    Input<string> branchCommandArgument = "";
    //    if (args.Branch is object)
    //    {
    //        branchCommandArgument = args.Branch.Apply(branch =>
    //        {
    //            if (!string.IsNullOrWhiteSpace(branch))
    //            {
    //                return $"--branch {branch} ";
    //            }

    //            return "";
    //        });
    //    }

    //    var command = Output.Tuple(args.ProjectName, args.UploadDirectory, branchCommandArgument).Apply(x =>
    //    {
    //        var projectName = x.Item1;
    //        var uploadDirectory = x.Item2;
    //        var branch = x.Item3;

    //        //_ = Debugger.Launch();

    //        var aaa = $@"wrangler pages deploy --projectName {projectName} {branch}--commit-dirty=true --directory ""{uploadDirectory}""";
    //        return aaa;
    //    });

    //    return command;
    //}

    //private InputList<object> GenerateTriggers(UploadStaticFilesCommandArgs args)
    //{
    //    var fullUploadPath = DetermineFullUploadDirectoryPath(args);

    //    var pathTriggers = fullUploadPath.Apply(x =>
    //    {
    //        var dirPath = x;

    //        if (!Directory.Exists(dirPath))
    //        {
    //            throw new DirectoryDoesNotExistException($"Directory does not exist at path: {dirPath}");
    //        }

    //        var staticFilesChecksums = Directory.GetFiles(dirPath, "*", SearchOption.AllDirectories)
    //        .OrderBy(fileName => fileName)
    //        .Select(fileName =>
    //        {
    //            var fileBytes = File.ReadAllBytes(fileName);
    //            var fileChecksum = System.Security.Cryptography.SHA256.HashData(fileBytes);
    //            var fileChecksumBase64 = Convert.ToBase64String(fileChecksum);
    //            return fileChecksumBase64;
    //        }).ToImmutableArray();

    //        return staticFilesChecksums;
    //    });

    //    var inputTriggers = args.Triggers;

    //    inputTriggers.AddRange(pathTriggers);
    //    return inputTriggers;
    //}

    //private static Output<string> DetermineFullUploadDirectoryPath(UploadStaticFilesCommandArgs args)
    //{
    //    var workingDirectoryInput = args.WorkingDirectory ?? "";

    //    var fullUploadPath = Output.Tuple(workingDirectoryInput, args.UploadDirectory).Apply(x =>
    //    {
    //        var workingDirectory = x.Item1;
    //        var uploadDirectory = x.Item2;

    //        if (string.IsNullOrWhiteSpace(workingDirectory))
    //        {
    //            return uploadDirectory;
    //        }
    //        else
    //        {
    //            return Path.Combine(workingDirectory, uploadDirectory);
    //        }
    //    });

    //    return fullUploadPath;
    //}
}
