using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace CallofitMobileXamarin.Utils
{
    public static class AuthToken
    {
        private const string TokenKey = "AuthToken";

        public static async Task<string> GetTokenAsync()
        {
            return await SecureStorage.GetAsync(TokenKey);
        }

        public static async Task SetTokenAsync(string token)
        {
            await SecureStorage.SetAsync(TokenKey, token);
        }

        public static async Task ClearTokenAsync()
        {
            SecureStorage.RemoveAll();
        }

        public static async Task SetExpirationAsync(DateTime expiration)
        {
            var expirationString = expiration.ToString("o");
            await SecureStorage.SetAsync("expiration", expirationString);
        }
        public static async Task<string> GetExpirationAsync()
        {
            return await SecureStorage.GetAsync("expiration");
        }

        public static async Task<bool> IsTokenValidAsync()
        {
            var token = await GetTokenAsync();
            var expirationString = await SecureStorage.GetAsync("expiration");

            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(expirationString))
            {
                return false;
            }

            var expiration = DateTime.Parse(expirationString);

            if (expiration < DateTime.UtcNow)
            {
                return false;
            }

            return true;
        }

        public static async Task<bool> IsAuthenticatedAsync()
        {
            var token = await GetTokenAsync();
            if (string.IsNullOrEmpty(token))
            {
                return false;
            }

            var expirationString = await SecureStorage.GetAsync("expiration");
            if (string.IsNullOrEmpty(expirationString))
            {
                return false;
            }

            if (!DateTime.TryParse(expirationString, out var expiration))
            {
                return false;
            }

            if (expiration < DateTime.UtcNow)
            {
                // Token has expired
                return false;
            }

            return true;
        }


    }
}
