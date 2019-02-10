using ApplicationCore.Interfaces;
using System.Security.Claims;

namespace Infrastructure
{
    public class ScopedParameters: IScopedParameters
    {
        public ClaimsPrincipal ClaimsPrincipal { get; set; }
        public string CurrentUserId => ClaimsPrincipal?.Identity?.Name;

        public ScopedParameters()
        {
        }
    }
}
