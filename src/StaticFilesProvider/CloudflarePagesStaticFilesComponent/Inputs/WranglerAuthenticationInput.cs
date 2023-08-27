using Pulumi;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgrammerAL.PulumiComponent.CloudflarePages.StaticFilesComponent.Inputs;

/// <summary>
/// Custom input for the authentication to use for running the command. 
/// These values are set as Environment Variables for the Wranger command that is run.
/// If you are already logged in to Wrangler, you do not need to provide these values.
/// </summary>
public class WranglerAuthenticationInput : global::Pulumi.ResourceArgs
{
    /// <summary>
    /// Cloudflare account id to use for authentication
    /// </summary>
    [Input("accountid", required: true)]
    public required Input<string> AccountId { get; set; }

    /// <summary>
    /// API Token to the Cloudflare account
    /// </summary>
    [Input("apitoken", required: true)]
    public required Input<string> ApiToken { get; set; }
}
