using ProgrammerAL.PulumiComponent.CloudflarePages.StaticFilesComponent;
using ProgrammerAL.PulumiComponent.CloudflarePages.StaticFilesComponent.Inputs;

using Pulumi;

using UnitTests.Utilities;

namespace UnitTests;

public class HappyPathStack : Stack
{
    public HappyPathStack()
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
    }
}

public class NoBranchStack : Stack
{
    public NoBranchStack()
    {
        _ = Directory.CreateDirectory(@"./files");
        _ = new UploadStaticFilesCommand($"test-files", new UploadStaticFilesCommandArgs
        {
            ProjectName = "test-files",
            UploadDirectory = @"./files",
        });
    }
}

public class TestValueBranchStack : Stack
{
    public TestValueBranchStack()
    {
        _ = Directory.CreateDirectory(@"./files");
        _ = new UploadStaticFilesCommand($"test-files", new UploadStaticFilesCommandArgs
        {
            ProjectName = "test-files",
            UploadDirectory = @"./files",
            Branch = TestValues.Branch
        });
    }
}

public class HappyPathWithAuthStack : Stack
{
    public HappyPathWithAuthStack()
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
    }
}

public class UploadPathDoesNotExistStack : Stack
{
    public UploadPathDoesNotExistStack()
    {
        _ = new UploadStaticFilesCommand($"test-files", new UploadStaticFilesCommandArgs
        {
            ProjectName = "test-files",
            UploadDirectory = @"./this-path-does-not-exist",
            Branch = "my-branch",
            Triggers = new[] { "my-trigger-1", "my-trigger-2", "my-trigger-3" }
        });
    }
}
