using System;

namespace UserManagement.Shared
{
    public abstract class UserBase<TKey>
    {
        public TKey Id { get; set; }
        public string UserName { get; set; }
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }        
        public DateTime? LockoutEndDateUtc { get; set; }
        public bool LockoutEnabled { get; set; }
        public int AccessFailedCount { get; set; }

        //        public virtual ICollection<TRole> Roles
        //        {
        //            get;
        //            private set;
        //        }
        //        /// <summary>
        //        ///     Navigation property for user claims
        //        /// </summary>
        //        public virtual ICollection<TClaim> Claims
        //        {
        //            get;
        //            private set;
        //        }
        //        /// <summary>
        //        ///     Navigation property for user logins
        //        /// </summary>
        //        public virtual ICollection<TLogin> Logins
        //        {
        //            get;
        //            private set;
        //        }



    }
}
