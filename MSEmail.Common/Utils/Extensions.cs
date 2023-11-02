using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class Extensions
{
    public static bool IsNull(this object obj)
    {
        return obj == null;
    }
}
