using System;

namespace Angler
{
    [Flags]
    public enum Website
    {
        YGOrg = 1,
        YGOrganization = YGOrg,
        CardCoal = 2,
        CardfightCoalition = CardCoal,
        All = YGOrg | CardCoal,
        Both = All
    }
}
