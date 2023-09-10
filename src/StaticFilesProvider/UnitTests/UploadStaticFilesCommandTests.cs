using NSubstitute;

using ProgrammerAL.PulumiComponent.CloudflarePages.StaticFilesComponent;
using ProgrammerAL.PulumiComponent.CloudflarePages.StaticFilesComponent.Exceptions;
using ProgrammerAL.PulumiComponent.CloudflarePages.StaticFilesComponent.Inputs;

using Pulumi;
using Pulumi.Command.Local;
using Pulumi.Testing;
using Pulumi.Utilities;

using Shouldly;

namespace UnitTests;

public class UploadStaticFilesCommandTests
{
    private readonly IMocks _pulumiMocks;

    public UploadStaticFilesCommandTests()
    {
        _pulumiMocks = Substitute.For<IMocks>();
        _ = _pulumiMocks.NewResourceAsync(default!)
            .ReturnsForAnyArgs(x =>
            {
                var args = x[0] as MockResourceArgs;
                var result = (args.Id ?? "", args.Inputs);
                return Task.FromResult<(string?, object)>(result);
            });

        _ = _pulumiMocks.CallAsync(default!)
            .ReturnsForAnyArgs(x =>
            {
                var args = x[0] as MockCallArgs;
                var result = args.Args;
                return Task.FromResult(result);
            });
    }

    [Fact]
    public async Task WhenCreatingInstance_AssertCommand()
    {
        var resources = await Deployment.TestAsync(_pulumiMocks, new TestOptions { IsPreview = false }, () =>
        {
            _ = Directory.CreateDirectory(@"./files");
            File.Copy(@"./static-content/index.html", @"./files/index.html", overwrite: true);

            _ = new UploadStaticFilesCommand($"test-files", new UploadStaticFilesCommandArgs
            {
                ProjectName = "test-files",
                UploadDirectory = @"./files",
                Branch = "my-branch",
                Triggers = new[] { "my-trigger-1", "my-trigger-2", "my-trigger-3" }
            });
        });

        var staticFiles = resources.OfType<Command>().Single();
        var create = await OutputUtilities.GetValueAsync(staticFiles.Create);
        var triggers = await OutputUtilities.GetValueAsync(staticFiles.Triggers);

        create.ShouldBe("wrangler pages deploy --projectName \"test-files\" --branch \"my-branch\" --commit-dirty=true \"./files\"");

        //Triggers will be 4. 3 for the custom ones we added, plus 1 for each file in the directory (which is only 1)
        triggers.Length.ShouldBe(4);
        triggers.ShouldContain("my-trigger-1");
        triggers.ShouldContain("my-trigger-2");
        triggers.ShouldContain("my-trigger-3");
    }

    [Fact]
    public async Task WhenCreatingInstanceWithBranchNotSet_AssertCommand()
    {
        var resources = await Deployment.TestAsync(_pulumiMocks, new TestOptions { IsPreview = false }, () =>
        {
            _ = Directory.CreateDirectory(@"./files");
            _ = new UploadStaticFilesCommand($"test-files", new UploadStaticFilesCommandArgs
            {
                ProjectName = "test-files",
                UploadDirectory = @"./files",
                //Branch = ""//This is what we're testing, the Branch property not being set
            });
        });

        var command = resources.OfType<Command>().Single();
        var createCommand = await OutputUtilities.GetValueAsync(command.Create);
        createCommand.ShouldBe("wrangler pages deploy --projectName \"test-files\" --commit-dirty=true \"./files\"");

        var uploadComponent = resources.OfType<UploadStaticFilesCommand>().Single();
        var uploadCommand = await OutputUtilities.GetValueAsync(uploadComponent.Command);
        uploadCommand.ShouldBe("wrangler pages deploy --projectName \"test-files\" --commit-dirty=true \"./files\"");
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData(" ")]
    [InlineData("         ")]
    [InlineData("\t")]
    public async Task WhenCreatingInstanceWithEmptyBranch_AssertCommand(string? branch)
    {
        var resources = await Deployment.TestAsync(_pulumiMocks, new TestOptions { IsPreview = false }, () =>
        {
            _ = Directory.CreateDirectory(@"./files");
            _ = new UploadStaticFilesCommand($"test-files", new UploadStaticFilesCommandArgs
            {
                ProjectName = "test-files",
                UploadDirectory = @"./files",
                Branch = branch
            });
        });

        var command = resources.OfType<Command>().Single();
        var createCommand = await OutputUtilities.GetValueAsync(command.Create);
        createCommand.ShouldBe("wrangler pages deploy --projectName \"test-files\" --commit-dirty=true \"./files\"");

        var uploadComponent = resources.OfType<UploadStaticFilesCommand>().Single();
        var uploadCommand = await OutputUtilities.GetValueAsync(uploadComponent.Command);
        uploadCommand.ShouldBe("wrangler pages deploy --projectName \"test-files\" --commit-dirty=true \"./files\"");
    }

    [Fact]
    public async Task WhenCreatingInstanceWithAuth_AssertAuthUsedAsEnvironmentVariables()
    {
        var resources = await Deployment.TestAsync(_pulumiMocks, new TestOptions { IsPreview = false }, () =>
        {
            _ = Directory.CreateDirectory(@"./files");
            _ = new UploadStaticFilesCommand($"test-files", new UploadStaticFilesCommandArgs
            {
                ProjectName = "test-files",
                UploadDirectory = @"./files",
                Branch = "my-branch",
                Authentication = new WranglerAuthenticationInput
                {
                    AccountId = "1234567890",
                    ApiToken = "555"
                }
            });
        });

        var command = resources.OfType<Command>().Single();
        var commandEnvironmentValue = await OutputUtilities.GetValueAsync(command.Environment);

        var environment = commandEnvironmentValue.ShouldNotBeNull();
        environment.Count.ShouldBe(2);
        environment.Single(x => x.Key == "CLOUDFLARE_ACCOUNT_ID").Value.ShouldBe("1234567890");
        environment.Single(x => x.Key == "CLOUDFLARE_API_TOKEN").Value.ShouldBe("555");

        var component = resources.OfType<UploadStaticFilesCommand>().Single();
        var componentEnvironmentValue = await OutputUtilities.GetValueAsync(component.Environment);

        var componentEnvironment = componentEnvironmentValue.ShouldNotBeNull();
        componentEnvironment.Count.ShouldBe(2);
        componentEnvironment.Single(x => x.Key == "CLOUDFLARE_ACCOUNT_ID").Value.ShouldBe("1234567890");
        componentEnvironment.Single(x => x.Key == "CLOUDFLARE_API_TOKEN").Value.ShouldBe("555");
    }

    [Fact]
    public async Task WhenUploadDirectoryDoesNotExist_AssertError()
    {
        var ex = await Should.ThrowAsync<Exception>(async () => {
            var resources = await Deployment.TestAsync(_pulumiMocks, new TestOptions { IsPreview = false }, () =>
            {
                _ = new UploadStaticFilesCommand($"test-files", new UploadStaticFilesCommandArgs
                {
                    ProjectName = "test-files",
                    UploadDirectory = @"./this-path-does-not-exist",
                    Branch = "my-branch",
                    Triggers = new[] { "my-trigger-1", "my-trigger-2", "my-trigger-3" }
                });
            });
        });

        var message = ex.Message;
        message.ShouldContain(typeof(DirectoryDoesNotExistException).FullName!);
    }
}
