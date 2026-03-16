using System.Security.Claims;

namespace DeclarationManagement.Api.Common;

public static class UserContextExtensions
{
    public static long GetUserId(this ClaimsPrincipal user)
    {
        var idText = user.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? user.FindFirstValue("sub")
            ?? throw new InvalidOperationException("未找到用户身份");

        return long.Parse(idText);
    }
}
