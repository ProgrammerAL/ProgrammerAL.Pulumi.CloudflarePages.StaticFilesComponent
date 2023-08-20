using Pulumi;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgrammerAL.PulumiComponent.CloudflarePages.StaticFilesComponent;

public class StaticFilesArgs : global::Pulumi.ResourceArgs
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
    public required Input<string> Branch { get; set; }

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
}
