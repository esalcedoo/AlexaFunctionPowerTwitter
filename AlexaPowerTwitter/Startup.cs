using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using AlexaPowerTwitter.Services;
using AlexaPowerTwitter.Dialogs;
using AlexaPowerTwitter.Common;

[assembly: FunctionsStartup(typeof(AlexaPowerTwitter.Startup))]
namespace AlexaPowerTwitter
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddHttpClient<PowerTwitterService>(client =>
                {
                    client.BaseAddress = new Uri("https://prod-38.westeurope.logic.azure.com/workflows/67a65ef25c32469d88dc2d8d51fec364/triggers/manual/paths/invoke");
                }
            );

            builder.Services.AddHttpClient<TranslateService>(client =>
                {
                    client.BaseAddress = new Uri("https://api.cognitive.microsofttranslator.com");
                    client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", Environment.GetEnvironmentVariable("Translate.SubscriptionKey", EnvironmentVariableTarget.Process));
                }
            );

            builder.Services.AddScoped<Accessor>();
            builder.Services.AddScoped<FavDialog>();
            builder.Services.AddScoped<TranslateDialog>();
            builder.Services.AddScoped<LanguageService>();
        }
    }
}
