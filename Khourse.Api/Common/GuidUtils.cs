using System;

namespace Khourse.Api.Common;

public static class GuidUtils
{
    public static bool TryParse(string input, out Guid id)
    {
        return Guid.TryParse(input, out id);
    }
}