using Pulumi;

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;

namespace ProgrammerAL.PulumiComponent.CloudflarePages.PagesFilesUpload;

public class CloudflarePagesFilesUpload : CustomResource
{
    public CloudflarePagesFilesUpload(string name, CloudflarePagesFilesUploadArgs args, CustomResourceOptions? options = null)
        : base(CloudflarePagesFilesUploadProvider.UploadCommandType, name, args, MakeResourceOptions(options, ""))
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
}
