using ProgrammerAL.PulumiComponent.CloudflarePages.StaticFilesComponent.Exceptions;

using Pulumi;
using Pulumi.Command.Local;

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgrammerAL.PulumiComponent.CloudflarePages.StaticFilesComponent;

public class UploadStaticFilesCommand
{
    public UploadStaticFilesCommand(string name, UploadStaticFilesCommandArgs args, CustomResourceOptions? customResourceOptions = null)
    {
        var internalCustomResourceOptions = MakeResourceOptions(customResourceOptions, id: "");

        var command = GenerateDeployCommand(args);

        var triggers = GenerateTriggers(args.UploadDirectory);

        _ = new Command(name, new CommandArgs
        {
            Create = command,
            Update = command,
            Environment = args.Environment,
            Triggers = triggers
        },
            internalCustomResourceOptions);

        Command = command;
    }

    public Output<string> Command { get; }

    private Output<string> GenerateDeployCommand(UploadStaticFilesCommandArgs args)
    {
        var command = Output.Tuple(args.ProjectName, args.UploadDirectory, args.Branch).Apply(x =>
        {
            var projectName = x.Item1;
            var uploadDirectory = x.Item2;
            var branch = x.Item3;

            var branchCommandArgument = "";
            if (!string.IsNullOrWhiteSpace(branch))
            {
                branchCommandArgument = $"--branch \"{branch}\" ";
            }

            return $"wrangler pages deploy --projectName \"{projectName}\" {branchCommandArgument}--commit-dirty=true \"{uploadDirectory}\"";
        });

        return command;
    }

    private InputList<object> GenerateTriggers(Input<string> path)
    {
        return path.Apply(x =>
        {
            var dirPath = x;

            if (!Directory.Exists(dirPath))
            {
                throw new DirectoryDoesNotExistException($"Directory does not exist at path: {path}");
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
    }

    private static CustomResourceOptions MakeResourceOptions(CustomResourceOptions? options, Input<string>? id)
    {
        var defaultOptions = new CustomResourceOptions
        {
            //TODO: Fill this in
            //Version = Pulumi.Utilities.Version,
        };
        var merged = CustomResourceOptions.Merge(defaultOptions, options);
        // Override the ID if one was specified for consistency with other language SDKs.
        merged.Id = id ?? merged.Id;
        return merged;
    }
}
