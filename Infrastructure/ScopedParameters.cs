using ApplicationCore.Interfaces;
using System.Security.Claims;

namespace Infrastructure
{
    public class ScopedParameters: IScopedParameters
    {
        public ClaimsPrincipal ClaimsPrincipal { get; set; }

        public ScopedParameters()
        {
        }
    }
}
