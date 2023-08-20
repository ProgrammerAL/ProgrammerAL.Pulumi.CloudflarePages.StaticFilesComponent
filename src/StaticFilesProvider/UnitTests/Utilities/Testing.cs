using Pulumi.Testing;
using Pulumi;

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NSubstitute;

namespace UnitTests.Utilities;

public static class Testing
{
    public static Task<ImmutableArray<Resource>> RunAsync<T>() where T : Stack, new()
    {
        var mocks = Substitute.For<IMocks>();
        _ = mocks.NewResourceAsync(default!)
            .ReturnsForAnyArgs(x =>
            {
                var args = x[0] as MockResourceArgs;
                var result = (args.Id ?? "", args.Inputs);
                return Task.FromResult<(string?, object)>(result);
            });

        _ = mocks.CallAsync(default!)
            .ReturnsForAnyArgs(x =>
            {
                var args = x[0] as MockCallArgs;
                var result = args.Args;
                return Task.FromResult(result);
            });

        return Deployment.TestAsync<T>(mocks, new TestOptions { IsPreview = false });
    }
}
