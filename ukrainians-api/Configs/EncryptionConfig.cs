using Ukrainians.UtilityServices.Settings;

namespace NomadChat.WebAPI.Configs
{
    public static class EncryptionConfig
    {
        public static void RegisterEncryption(this IServiceCollection services, IConfiguration configuration)
        {
            EncryptionSettings encryptionSettings = new();
            configuration.Bind(nameof(encryptionSettings), encryptionSettings);
            services.AddSingleton(encryptionSettings);
        }
    }
}
