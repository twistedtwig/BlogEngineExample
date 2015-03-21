using System;
using System.Security.Claims;
using UserManagement.Shared;
using UserManagementService.Interfaces;

namespace UserManagementService.Implementations
{
    public class ClaimsIdentityFactory<TUser, TKey> : IClaimsIndentityService<TUser, TKey> 
        where TUser : UserBase<TKey>, 
        new() where TKey : IEquatable<TKey>
    {
        internal const string IdentityProviderClaimType = "http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider";
        internal const string DefaultIdentityProviderClaimValue = "ASP.NET Identity";
        
        public string RoleClaimType { get; set; }
        public string UserNameClaimType { get; set; }
        public string UserIdClaimType { get; set; }
        public string SecurityStampClaimType { get; set; }

        public ClaimsIdentityFactory()
        {
            this.RoleClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role";
            this.UserIdClaimType = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier";
            this.UserNameClaimType = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name";
            this.SecurityStampClaimType = "AspNet.Identity.SecurityStamp";
        }
        /// <summary>
        ///     Create a ClaimsIdentity from a user
        /// </summary>
        /// <param name="manager"></param>
        /// <param name="user"></param>
        /// <param name="authenticationType"></param>
        /// <returns></returns>
        public ClaimsIdentity CreateAsync(TUser user, string authenticationType)
        {            
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            var claimsIdentity = new ClaimsIdentity(authenticationType, UserNameClaimType, RoleClaimType);
            claimsIdentity.AddClaim(new Claim(UserIdClaimType, ConvertIdToString(user.Id), "http://www.w3.org/2001/XMLSchema#string"));
            claimsIdentity.AddClaim(new Claim(UserNameClaimType, user.UserName, "http://www.w3.org/2001/XMLSchema#string"));
            claimsIdentity.AddClaim(new Claim("http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider", "ASP.NET Identity", "http://www.w3.org/2001/XMLSchema#string"));
//            if (manager.SupportsUserSecurityStamp)
//            {
//                claimsIdentity.AddClaim(new Claim(this.SecurityStampClaimType, await manager.GetSecurityStampAsync(user.Id).WithCurrentCulture<string>()));
//            }
//            if (manager.SupportsUserRole)
//            {
//                IList<string> list = await manager.GetRolesAsync(user.Id).WithCurrentCulture<IList<string>>();
//                foreach (string current in list)
//                {
//                    claimsIdentity.AddClaim(new Claim(this.RoleClaimType, current, "http://www.w3.org/2001/XMLSchema#string"));
//                }
//            }
//            if (manager.SupportsUserClaim)
//            {
//                claimsIdentity.AddClaims(await manager.GetClaimsAsync(user.Id).WithCurrentCulture<IList<Claim>>());
//            }
            return claimsIdentity;
        }
        /// <summary>
        ///     Convert the key to a string, by default just calls .ToString()
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string ConvertIdToString(TKey key)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }
            return key.ToString();
        }
    }
}
