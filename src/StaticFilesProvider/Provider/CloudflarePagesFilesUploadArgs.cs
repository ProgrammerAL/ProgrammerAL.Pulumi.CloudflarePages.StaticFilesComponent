using ProgrammerAL.PulumiComponent.CloudflarePages.PagesFilesUpload.Inputs;

using Pulumi;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgrammerAL.PulumiComponent.CloudflarePages.PagesFilesUpload;

public class CloudflarePagesFilesUploadArgs : ResourceArgs
{
    internal const string PropertyProjectName = "projectname";
    internal const string PropertyUploadDir = "uploaddir";
    internal const string PropertyBranch = "branch";
    internal const string PropertyEnvironment = "environment";
    internal const string PropertyAuthentication = "authentication";
    internal const string PropertyTriggers = "triggers";
    internal const string PropertyWorkingDirectory = "workingDirectory";


    /// <summary>
    /// The name of the Cloudflare Pages project to upload files to.
    /// </summary>
    [Input(PropertyProjectName, required: true)]
    public required Input<string> ProjectName { get; set; }

    /// <summary>
    /// The path to the directory with files in it we will upload
    /// </summary>
    [Input(PropertyUploadDir, required: true)]
    public required Input<string> UploadDirectory { get; set; }

    /// <summary>
    /// The branch to upload to
    /// </summary>
    [Input(PropertyBranch)]
    public Input<string?>? Branch { get; set; }

    [Input(PropertyEnvironment)]
    private InputMap<string>? _environment;

    /// <summary>
    /// Additional environment variables available to the command's process.
    /// </summary>
    public InputMap<string> Environment
    {
        get => _environment ?? (_environment = new InputMap<string>());
        set => _environment = value;
    }

    /// <summary>
    /// The authentication to use for running the command. 
    /// This is required if wrangler is not already logged in.
    /// </summary>
    [Input(PropertyAuthentication)]
    public Input<WranglerAuthenticationInput>? Authentication { get; set; }

    [Input(PropertyTriggers)]
    private InputList<string>? _triggers;

    /// <summary>
    /// Trigger replacements on changes to this input.
    /// </summary>
    public InputList<string> Triggers
    {
        get => _triggers ?? (_triggers = new InputList<string>());
        set => _triggers = value;
    }

    /// <summary>
    /// The directory to run the command from. If the directory does not exist, then an error will be thrown.
    /// </summary>
    [Input(PropertyWorkingDirectory)]
    public Input<string>? WorkingDirectory { get; set; }
}
