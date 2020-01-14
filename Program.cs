using Microsoft.Azure.KeyVault;
using Microsoft.Azure.KeyVault.Models;
using Microsoft.Azure.Services.AppAuthentication;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace Core_KeyVault_Console {
    class Program {
        static void Main(string[] args) {
            Console.Write("Enter a Key Vault name to read from: ");
            string key_vault_name = Console.ReadLine();
            key_vault_name = (key_vault_name.Length == 0) ? "my-key-vault-name" : key_vault_name; //Change this to whatever your key vault is named

            Console.Write("Enter a secret to read: ");
            string secret_to_read = Console.ReadLine();
            secret_to_read = (secret_to_read.Length == 0) ? "hello-world" : secret_to_read; //Change this to whatever you want

            //Get the secret
            string secret = GetSecret(key_vault_name, secret_to_read).GetAwaiter().GetResult();
            Console.WriteLine("Got Secret: " + secret);

            Environment.Exit(0);
        }



        public static async Task<string> GetSecret(string KeyVaultName, string secretName) {
            //string token = KeyVaultHelper.getToken();
            AzureServiceTokenProvider azureServiceTokenProvider = new AzureServiceTokenProvider();
            KeyVaultClient keyVaultClient = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(azureServiceTokenProvider.KeyVaultTokenCallback));

            //For Microsoft Azure Government Key Vaults, use the following URL: *.vault.usgovcloudapi.net
            //var secret = await keyVaultClient.GetSecretAsync($"https://{KeyVaultName}.vault.usgovcloudapi.net/secrets/{secretName}");
            
            var secret = await keyVaultClient.GetSecretAsync($"https://{KeyVaultName}.vault.azure.net/secrets/{secretName}");

            return secret.Value;
        }
    }
}
