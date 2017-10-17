using System;

namespace Angler
{
    [Flags]
    public enum Website
    {
        YGOrg = 1,
        CardCoal = 2,
        All = YGOrg | CardCoal
    }
}
