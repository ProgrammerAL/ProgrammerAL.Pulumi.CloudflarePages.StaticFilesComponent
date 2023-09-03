using ProgrammerAL.PulumiComponent.CloudflarePages.StaticFilesComponent.Inputs;

using Pulumi;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgrammerAL.PulumiComponent.CloudflarePages.StaticFilesComponent;

public class UploadStaticFilesCommandArgs : global::Pulumi.ResourceArgs
{
    /// <summary>
    /// The name of the Cloudflare Pages project to upload files to.
    /// </summary>
    [Input("projectname", required: true)]
    public required Input<string> ProjectName { get; set; }

    /// <summary>
    /// The path to the directory with files in it we will upload
    /// </summary>
    [Input("uploaddir", required: true)]
    public required Input<string> UploadDirectory { get; set; }

    /// <summary>
    /// The branch to upload to
    /// </summary>
    [Input("branch")]
    public Input<string?>? Branch { get; set; }

    [Input("environment")]
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
    [Input("authentication")]
    public Input<WranglerAuthenticationInput>? Authentication { get; set; }

    [Input("triggers")]
    private InputList<object>? _triggers;

    /// <summary>
    /// Trigger replacements on changes to this input.
    /// </summary>
    public InputList<object> Triggers
    {
        get => _triggers ?? (_triggers = new InputList<object>());
        set => _triggers = value;
    }

    /// <summary>
    /// The directory to run the command from. If the directory does not exist, then an error will be thrown.
    /// </summary>
    [Input("workingDirectory")]
    public Input<string>? WorkingDirectory { get; set; }
}
