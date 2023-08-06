using ProgrammerAL.Pulumi.CloudflarePages.StaticFilesProvider;
using Pulumi.Experimental.Provider;

public static class Program
{
    public static Task Main(string[] args)
    {
        return Provider.Serve(args, "0.0.1", host => new ClouadflarePagesStaticFilesProvider(host), CancellationToken.None);
    }
}
