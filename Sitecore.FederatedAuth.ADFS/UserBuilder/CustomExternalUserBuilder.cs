using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Sitecore.Diagnostics;
using Sitecore.Owin.Authentication.Configuration;
using Sitecore.Owin.Authentication.Identity;
using Sitecore.Owin.Authentication.Services;
using Sitecore.SecurityModel.Cryptography;
using System;
using System.Text;

namespace Sitecore.FederatedAuth.ADFS.UserBuilder
{
    public class CustomExternalUserBuilder : ExternalUserBuilder
    {
        private readonly IHashEncryption _hashEncryption;

        public CustomExternalUserBuilder(ApplicationUserFactory applicationUserFactory, IHashEncryption hashEncryption)
        {
            Assert.ArgumentNotNull((object)applicationUserFactory, nameof(applicationUserFactory));
            Assert.ArgumentNotNull((object)hashEncryption, nameof(hashEncryption));
            this._hashEncryption = hashEncryption;
            this.ApplicationUserFactory = applicationUserFactory;
        }

        public bool IsPersistentUser { get; set; }

        protected ApplicationUserFactory ApplicationUserFactory { get; }

        public override ApplicationUser BuildUser(UserManager<ApplicationUser> userManager, ExternalLoginInfo externalLoginInfo)
        {
            ApplicationUser user = this.ApplicationUserFactory.CreateUser(this.CreateUniqueUserName(userManager, externalLoginInfo));
            user.IsVirtual = !this.IsPersistentUser;
            user.Email = externalLoginInfo.Email;
            return user;
        }

        protected virtual string CreateUniqueUserName(UserManager<ApplicationUser> userManager, ExternalLoginInfo externalLoginInfo)
        {
            Assert.ArgumentNotNull(userManager, nameof(userManager));
            Assert.ArgumentNotNull(externalLoginInfo, nameof(externalLoginInfo));
            IdentityProvider identityProvider = this.FederatedAuthenticationConfiguration.GetIdentityProvider(externalLoginInfo.ExternalIdentity);
            if (identityProvider == null)
                throw new InvalidOperationException("Unable to retrieve an identity provider for the given identity");
            string domain = identityProvider.Domain;
            string s = externalLoginInfo.Login.LoginProvider + externalLoginInfo.Login.ProviderKey;

            string userName;
            do
            {
                s += "$";
                string str1 = System.Convert.ToBase64String(this._hashEncryption.ComputeHash(Encoding.ASCII.GetBytes(s))).Trim('=').Replace("/", string.Empty).Replace("+", string.Empty);
                string str2 = str1.Substring(0, Math.Min(str1.Length, 10));
                userName = domain + "\\" + str2;
            }
            while (userManager.FindByName<ApplicationUser, string>(userName) != null);

            return userName;
        }
    }
}