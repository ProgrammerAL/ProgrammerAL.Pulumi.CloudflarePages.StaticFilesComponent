using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.Utilities;

public static class TestValues
{
    public static void Reset()
    {
        Branch = null;
    }

    public static string? Branch { get; set; }
}
