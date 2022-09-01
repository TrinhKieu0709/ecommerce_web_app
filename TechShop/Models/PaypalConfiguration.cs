using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PayPal.Api;

namespace TechShop.Models
{
    // Get Configuration from web.config file
    public class PaypalConfiguration
    {
        public readonly static string ClientId;
        public readonly static string ClientSecret;
        static  PaypalConfiguration()
        {
            var config = GetConfig();
            ClientId = "AfP6ZXsqebKS0J7JPHQ7wkhZHO4guw1VmXXTPT9uf21z2HCeUSttoGxgpm1EbNShbd0SnzNHapm8nCHx";
            ClientSecret = "EKrs8XJEm_hjwB8ZUIiS2NoBr9PAFYb8hhV3Tki7LKC2KcsIP5zCodLOSqT9rN22uEcJzj1C_lUg0XP4";

        }
        
        public static  Dictionary< string, string> GetConfig()
        {
            return PayPal.Api.ConfigManager.Instance.GetProperties();
        }
        // Create access token
        private static  string  GetAccessToken()
        {
            string accessToken = new OAuthTokenCredential(ClientId, ClientSecret, GetConfig()).GetAccessToken();
            return accessToken;
        }
        // this will return to APIcontext object
        public  static APIContext GetAPIContext ()
        {
            var apiContext = new APIContext(GetAccessToken());
            apiContext.Config = GetConfig();
            return apiContext;
        }
    }
}