using ProgrammerAL.PulumiComponent.CloudflarePages.PagesFilesUpload.Inputs;

using Pulumi;
using Pulumi.Experimental.Provider;

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgrammerAL.PulumiComponent.CloudflarePages.PagesFilesUpload.ProviderArgs;

internal class RunArgs
{
    public RunArgs(ImmutableDictionary<string, PropertyValue> properties)
    {
        if (properties.TryGetValue(CloudflarePagesFilesUploadArgs.PropertyProjectName, out var projectNameProperty))
        {
            ProjectName = projectNameProperty.ToString() ?? throw new ArgumentNullException($"properties.{CloudflarePagesFilesUploadArgs.PropertyProjectName}", $"properties.{CloudflarePagesFilesUploadArgs.PropertyProjectName} is null");
        }
        else
        {
            throw new ArgumentNullException($"properties.{CloudflarePagesFilesUploadArgs.PropertyProjectName}", $"properties.{CloudflarePagesFilesUploadArgs.PropertyProjectName} does not exist");
        }

        if (properties.TryGetValue(CloudflarePagesFilesUploadArgs.PropertyUploadDir, out var uploadDirProperty))
        {
            UploadDirectory = uploadDirProperty.ToString() ?? throw new ArgumentNullException($"properties.{CloudflarePagesFilesUploadArgs.PropertyUploadDir}", $"properties.{CloudflarePagesFilesUploadArgs.PropertyUploadDir} is null");
        }
        else
        {
            throw new ArgumentNullException($"properties.{CloudflarePagesFilesUploadArgs.PropertyUploadDir}", $"properties.{CloudflarePagesFilesUploadArgs.PropertyUploadDir} does not exist");
        }

        if (properties.TryGetValue(CloudflarePagesFilesUploadArgs.PropertyBranch, out var branchProperty)
            && branchProperty.TryGetString(out var branchString))
        {
            Branch = branchString;
        }

        if (properties.TryGetValue(CloudflarePagesFilesUploadArgs.PropertyEnvironment, out var environmentProperty)
            && environmentProperty.TryGetObject(out var environmentObject)
            && environmentObject is object)
        {
            var builder = ImmutableDictionary.CreateBuilder<string, string>();
            foreach (var item in environmentObject)
            {
                if (item.Value.TryGetString(out var itemValue) 
                    && itemValue is object)
                {
                    builder.Add(item.Key, itemValue);
                }
            }

            Environment = builder.ToImmutableDictionary();
        }
        else
        { 
            Environment = ImmutableDictionary.Create<string, string>();
        }

        if (properties.TryGetValue(CloudflarePagesFilesUploadArgs.PropertyAuthentication, out var authenticationProperty)
            && authenticationProperty.TryGetObject(out var authenticationObject) && authenticationObject is object
            && authenticationObject.TryGetValue(WranglerAuthenticationInput.PropertyAccountId, out var accountIdProperty)
            && accountIdProperty.TryGetString(out var accountIdString) && accountIdString is object
            && authenticationObject.TryGetValue(WranglerAuthenticationInput.PropertyApiToken, out var apitokenProperty)
            && apitokenProperty.TryGetString(out var apitokenString) && apitokenString is object)
        {
            Authentication = new WranglerAuthenticationInput
            {
                AccountId = accountIdString,
                ApiToken = apitokenString
            };
        }

        if (properties.TryGetValue(CloudflarePagesFilesUploadArgs.PropertyTriggers, out var triggersProperty)
            && triggersProperty.TryGetArray(out var triggersArray))
        {
            var builder = ImmutableArray.CreateBuilder<string>();
            foreach (var item in triggersArray)
            {
                if (item.TryGetString(out var key) && key is object)
                {
                    builder.Add(key);
                }
            }

            Triggers = builder.ToImmutableArray();
        }
        else
        {
            Triggers = new ImmutableArray<string>();
        }

        if (properties.TryGetValue(CloudflarePagesFilesUploadArgs.PropertyWorkingDirectory, out var workingDirectoryProperty)
            && workingDirectoryProperty.TryGetString(out var workingDirectoryString)
            && workingDirectoryString is object)
        {
            WorkingDirectory = workingDirectoryString;
        }
    }

    public string ProjectName { get; }

    public string UploadDirectory { get; }

    public string? Branch { get; }

    public ImmutableDictionary<string, string> Environment { get; }

    public WranglerAuthenticationInput? Authentication { get; }

    public ImmutableArray<string> Triggers { get; }
    public string? WorkingDirectory { get; }
}
