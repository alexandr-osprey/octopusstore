using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace ApplicationCore.Identity
{
    public class ClaimComparer : IEqualityComparer<Claim>
    {
        public bool Equals(Claim x, Claim y)
        {

            //Check whether the compared objects reference the same data.
            if (Object.ReferenceEquals(x, y)) return true;

            //Check whether any of the compared objects is null.
            if (Object.ReferenceEquals(x, null) || Object.ReferenceEquals(y, null))
                return false;

            //Check whether the claims' properties are equal.
            return x.Type == y.Type && x.Value == y.Value;
        }

        // If Equals() returns true for a pair of objects 
        // then GetHashCode() must return the same value for these objects.

        public int GetHashCode(Claim claim)
        {
            //Check whether the object is null
            if (Object.ReferenceEquals(claim, null)) return 0;

            //Get hash code for the Type field if it is not null.
            int hashProductName = claim.Type == null ? 0 : claim.Type.GetHashCode();

            //Get hash code for the Value field.
            int hashProductCode = claim.Value.GetHashCode();

            //Calculate the hash code for the claim.
            return hashProductName ^ hashProductCode;
        }

    }
}
