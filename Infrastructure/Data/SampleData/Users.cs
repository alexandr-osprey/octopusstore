using System.Collections.Generic;
using System.Security.Claims;

namespace Infrastructure.Data.SampleData
{
    public class Users
    {
        public static string AdminId { get; } = "admin@mail.com";
        public static string JohnId { get; } = "john@mail.com";
        public static string JenniferId { get; } = "jennifer@mail.com";
        public static string User1Id { get; } = "user1@mail.com";

        public static ClaimsPrincipal JohnPrincipal { get; } = GetPrincipal(JohnId);
        public static ClaimsPrincipal JenniferPrincipal { get; } = GetPrincipal(JenniferId);
        public static ClaimsPrincipal AdminPrincipal { get; } = GetPrincipal(AdminId);
        public static ClaimsPrincipal User1Principal { get; } = GetPrincipal(User1Id);

        private static ClaimsPrincipal GetPrincipal(string id) => new ClaimsPrincipal(
                    new ClaimsIdentity(
                        new List<Claim>()
                        {
                            new Claim(ClaimTypes.NameIdentifier, id),
                            new Claim(ClaimTypes.Name, id)
                        }));

    }
}
