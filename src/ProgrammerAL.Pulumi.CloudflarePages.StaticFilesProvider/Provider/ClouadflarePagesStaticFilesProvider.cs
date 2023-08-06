using Pulumi.Experimental.Provider;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgrammerAL.Pulumi.CloudflarePages.StaticFilesProvider;

public class ClouadflarePagesStaticFilesProvider : Provider
{
    private readonly IHost _host;

    public ClouadflarePagesStaticFilesProvider(IHost host)
    {
        _host = host;
    }
}
