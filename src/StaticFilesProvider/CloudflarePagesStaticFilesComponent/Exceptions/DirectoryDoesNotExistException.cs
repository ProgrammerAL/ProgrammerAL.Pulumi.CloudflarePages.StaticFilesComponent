using Microsoft.Extensions.Hosting;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgrammerAL.PulumiComponent.CloudflarePages.StaticFilesComponent.Exceptions;

public class DirectoryDoesNotExistException : Exception
{
    public DirectoryDoesNotExistException(string message)
        : base(message)
    { 
    }
}
