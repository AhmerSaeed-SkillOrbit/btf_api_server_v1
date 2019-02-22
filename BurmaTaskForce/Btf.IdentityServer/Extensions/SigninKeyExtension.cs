using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using System.Security.Cryptography.X509Certificates;

namespace Btf.IdentityServer.Extensions
{
    public static class SigninKeyExtension
    {
        public static IIdentityServerBuilder AddCertificateFromStore(this IIdentityServerBuilder builder,
     IConfigurationSection options)
        {
            var keyIssuer = options.GetValue<string>("KeyStoreIssuer");
            //logger.LogDebug($"SigninCredentialExtension adding key from store by {keyIssuer}");

            X509Store store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
            store.Open(OpenFlags.ReadOnly);

            var certificates = store.Certificates.Find(X509FindType.FindByIssuerName, keyIssuer, true);

            if (certificates.Count > 0)
            {
                builder.AddSigningCredential(certificates[0]);
                return builder;
            }
            else
            {
                //logger.LogError("A matching key couldn't be found in the store");
                return builder;
            }
        }
    }
}
