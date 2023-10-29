using Pulumi;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgrammerAL.PulumiComponent.CloudflarePages.PagesFilesUpload.Inputs;

/// <summary>
/// Custom input for the authentication to use for running the command. 
/// These values are set as Environment Variables for the Wranger command that is run.
/// If you are already logged in to Wrangler, you do not need to provide these values.
/// </summary>
public class WranglerAuthenticationInput : ResourceArgs
{
    internal const string PropertyAccountId = "accountid";
    internal const string PropertyApiToken = "apitoken";

    /// <summary>
    /// Cloudflare account id to use for authentication
    /// </summary>
    [Input(PropertyAccountId, required: true)]
    public required Input<string> AccountId { get; set; }

    /// <summary>
    /// API Token to the Cloudflare account
    /// </summary>
    [Input(PropertyApiToken, required: true)]
    public required Input<string> ApiToken { get; set; }
}
