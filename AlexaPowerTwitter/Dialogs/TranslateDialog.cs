using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Alexa.NET;
using Alexa.NET.LocaleSpeech;
using Alexa.NET.Response;
using AlexaPowerTwitter.Common;
using AlexaPowerTwitter.Extensions;
using AlexaPowerTwitter.Models;
using AlexaPowerTwitter.Models.AlexaModels;

namespace AlexaPowerTwitter.Dialogs
{
    public class TranslateDialog
    {
        private readonly Accessor _accessor;
        public TranslateDialog(Accessor accessor)
        {
            _accessor = accessor;
        }
        internal async Task<SkillResponse> HandleTranslationPossibilityAsync()
        {
            _accessor.Session.Attributes.Add(SessionAtributeNames.lastIntent, _accessor.IntentRequest.Intent.Name);

            var message = await _accessor.LocaleSpeech.Get(LanguageKeys.Translate, null);
            var messageReprompt = await _accessor.LocaleSpeech.Get(LanguageKeys.TranslateReprompt, null);

            var response = ResponseBuilder.Ask(message, RepromptBuilder.Create(messageReprompt), _accessor.Session);
            return response;
        }
    }
}
