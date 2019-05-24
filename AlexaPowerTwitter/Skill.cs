using Alexa.NET;
using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Alexa.NET.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using AlexaPowerTwitter.Extensions;
using AlexaPowerTwitter.Models;
using AlexaPowerTwitter.Services;
using AlexaPowerTwitter.Dialogs;
using AlexaPowerTwitter.Common;
using AlexaPowerTwitter.Models.AlexaModels;

namespace AlexaPowerTwitter
{
    public class Skill
    {
        private readonly Accessor _accessor;
        private readonly FavDialog _favDialog;
        private readonly LanguageService _languageService;

        public Skill(LanguageService languageService, FavDialog favDialog, Accessor accessor)
        {
            _accessor = accessor;
            _languageService = languageService;
            _favDialog = favDialog;
        }
        
        [FunctionName("AlexaPowerTwitter")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req, ILogger log)
        {
            var json = await req.ReadAsStringAsync();
            var skillRequest = JsonConvert.DeserializeObject<SkillRequest>(json);

            // Verifies that the request is indeed coming from Alexa.
            var isValid = await skillRequest.ValidateRequestAsync(req, log);
            if (!isValid)
            {
                return new BadRequestResult();
            }

            _accessor.Session = skillRequest.Session;
            if (_accessor.Session.Attributes == null)
            {
                _accessor.Session.Attributes = new Dictionary<string, object>();
            }

            // Setup language resources. if lang slot detected set resources in slot's language, otherwise set store's one
            var store = _languageService.SetupLanguageResources();
            _accessor.LocaleSpeech = skillRequest.CreateLocale(store);

            _accessor.Request = skillRequest.Request;
            _accessor.Lang = _languageService.AdaptCulture(_accessor.Request.Locale);

            _accessor.LocaleResources = _languageService.SetupAuxLanguageResourcesByLang(_accessor.Lang);

            SkillResponse response = null;
            try
            {
                if (_accessor.Request is LaunchRequest launchRequest)
                {
                    log.LogInformation("Session started");

                    var welcomeMessage = await _accessor.LocaleSpeech.Get(LanguageKeys.Welcome, null);
                    var welcomeRepromptMessage = await _accessor.LocaleSpeech.Get(LanguageKeys.WelcomeReprompt, null);
                    response = ResponseBuilder.Ask(welcomeMessage, RepromptBuilder.Create(welcomeRepromptMessage));
                }
                else if (_accessor.Request is IntentRequest intentRequest)
                {
                    _accessor.IntentRequest = intentRequest;
                    // Checks whether to handle system messages defined by Amazon.
                    var systemIntentResponse = await HandleSystemIntentsAsync();
                    if (systemIntentResponse.IsHandled)
                    {
                        response = systemIntentResponse.Response;
                    }
                    else
                    {
                        // Processes request according to intentRequest.Intent.Name...
                        var message = await _accessor.LocaleSpeech.Get(LanguageKeys.Error, null);
                        response = ResponseBuilder.Tell(message);
                    }
                }
                else if (_accessor.Request is SessionEndedRequest sessionEndedRequest)
                {
                    log.LogInformation("Session ended");
                    response = ResponseBuilder.Empty();
                }
            }
            catch
            {
                var message = await _accessor.LocaleSpeech.Get(LanguageKeys.Error, null);
                response = ResponseBuilder.Tell(message);
                response.Response.ShouldEndSession = false;
            }

            return new OkObjectResult(response);
        }

        private async Task<(bool IsHandled, SkillResponse Response)> HandleSystemIntentsAsync()
        {
            SkillResponse response = null;

            if (_accessor.IntentRequest.Intent.Name == IntentNames.Cancel)
            {
                var message = await _accessor.LocaleSpeech.Get(LanguageKeys.Cancel, null);
                response = ResponseBuilder.Tell(message);
            }
            else if (_accessor.IntentRequest.Intent.Name == IntentNames.Help)
            {
                var message = await _accessor.LocaleSpeech.Get(LanguageKeys.Help, null);
                response = ResponseBuilder.Ask(message, RepromptBuilder.Create(message));
            }
            else if (_accessor.IntentRequest.Intent.Name == IntentNames.Stop)
            {
                var message = await _accessor.LocaleSpeech.Get(LanguageKeys.Stop, null);
                response = ResponseBuilder.Tell(message);
            }
            else if (_accessor.IntentRequest.Intent.Name == IntentNames.Favorites
                || Equals(_accessor.Session.Attributes.GetValueOrDefault(SessionAtributeNames.lastIntent), IntentNames.Favorites))
            {
                response = await _favDialog.HandleFavIntent();
            }
            return (response != null, response);
        }
    }
}
