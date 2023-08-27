using ProgrammerAL.PulumiComponent.CloudflarePages.StaticFilesComponent;

using Pulumi.Command.Local;
using Pulumi.Utilities;

using Shouldly;

using UnitTests.Utilities;

namespace UnitTests;

public class UploadStaticFilesCommandTests
{
    public UploadStaticFilesCommandTests()
    { 
        TestValues.Reset();
    }

    [Fact]
    public async Task WhenCreatingInstance_AssertCommand()
    {
        var resources = await Testing.RunAsync<HappyPathStack>();

        var staticFiles = resources.OfType<Command>().Single();
        var create = await OutputUtilities.GetValueAsync(staticFiles.Create);
        create.ShouldBe("wrangler pages deploy --projectName \"test-files\" --branch \"my-branch\" --commit-dirty=true \"./files\"");
    }

    [Fact]
    public async Task WhenCreatingInstanceWithNoBranch_AssertCommand()
    {
        var resources = await Testing.RunAsync<NoBranchStack>();

        var staticFiles = resources.OfType<Command>().Single();
        var create = await OutputUtilities.GetValueAsync(staticFiles.Create);
        create.ShouldBe("wrangler pages deploy --projectName \"test-files\" --commit-dirty=true \"./files\"");
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData(" ")]
    [InlineData("         ")]
    [InlineData("\t")]
    public async Task WhenCreatingInstanceWithEmptyBranch_AssertCommand(string? branch)
    {
        TestValues.Branch = branch;
        var resources = await Testing.RunAsync<TestValueBranchStack>();

        var staticFiles = resources.OfType<Command>().Single();
        var create = await OutputUtilities.GetValueAsync(staticFiles.Create);
        create.ShouldBe("wrangler pages deploy --projectName \"test-files\" --commit-dirty=true \"./files\"");
    }

    [Fact]
    public async Task WhenCreatingInstanceWithAuth_AssertAuthUsedAsEnvironmentVariables()
    {
        var resources = await Testing.RunAsync<HappyPathWithAuthStack>();

        var staticFiles = resources.OfType<Command>().Single();
        var environmentResult = await OutputUtilities.GetValueAsync(staticFiles.Environment);

        var environment = environmentResult.ShouldNotBeNull();
        environment.Count.ShouldBe(2);
        environment.Single(x => x.Key == "CLOUDFLARE_ACCOUNT_ID").Value.ShouldBe("1234567890");
        environment.Single(x => x.Key == "CLOUDFLARE_API_TOKEN").Value.ShouldBe("555");
    }
}
