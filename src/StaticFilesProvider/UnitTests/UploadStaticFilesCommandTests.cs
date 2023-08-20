using ProgrammerAL.PulumiComponent.CloudflarePages.StaticFilesComponent;

using Pulumi.Command.Local;
using Pulumi.Utilities;

using Shouldly;

using UnitTests.Utilities;

namespace UnitTests;

public class UploadStaticFilesCommandTests
{
    [Fact]
    public async Task WhenCreatingInstance_AssertCommand()
    {
        var resources = await Testing.RunAsync<HappyPathStack>();

        var staticFiles = resources.OfType<Command>().Single();
        var create = await OutputUtilities.GetValueAsync(staticFiles.Create);
        create.ShouldBe("wrangler pages deploy --projectName \"test-files\" --branch \"my-branch\" --commit-dirty=true \"./files\"");
    }
}
