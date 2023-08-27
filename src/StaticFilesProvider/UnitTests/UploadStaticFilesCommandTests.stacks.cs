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
        _ = new UploadStaticFilesCommand($"test-files", new UploadStaticFilesCommandArgs
        {
            ProjectName = "test-files",
            UploadDirectory = @"./files",
            Branch = "my-branch",
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
