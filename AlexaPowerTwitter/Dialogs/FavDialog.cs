using Alexa.NET;
using Alexa.NET.Response;
using AlexaPowerTwitter.Common;
using AlexaPowerTwitter.Models;
using AlexaPowerTwitter.Models.AlexaModels;
using AlexaPowerTwitter.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlexaPowerTwitter.Dialogs
{
    public class FavDialog
    {
        private IList<FavModel> _favList;
        private readonly TranslateDialog _translateDialog;
        private readonly TranslateService _translateService;
        private readonly Accessor _accessor;
        private readonly PowerTwitterService _powerTwitterService;

        public FavDialog(Accessor accessor, PowerTwitterService powerTwitterService, TranslateService translateService, TranslateDialog translateDialog)
        {
            _translateService = translateService;
            _accessor = accessor;
            _powerTwitterService = powerTwitterService;
            _translateDialog = translateDialog;
        }

        internal async Task<SkillResponse> HandleFavIntent()
        {
            SkillResponse response = null;

            // get favs
            _favList = await _powerTwitterService.GetFavouritesList();

            // detect every fav lang
            bool differentLang = false;
            foreach (FavModel fav in _favList)
            {
                fav.lang = await _translateService.DetectLanguage(fav.cr825_message);
                differentLang = fav.lang != _accessor.Lang || differentLang;
            }


            if (_accessor.IntentRequest.Intent.Name == IntentNames.Favorites) // firstTime
            {
                response = await _translateDialog.HandleTranslationPossibilityAsync();
            }
            else
            {
                if (_accessor.IntentRequest.Intent.Name == IntentNames.YesIntent)
                {
                    await ChangeLanguage();
                }
                var message = string.Join("<break time=\"2s\"/>", _favList.Select(fav => fav.ToSSML()));
                message = string.Format(_accessor.LocaleResources.GetValueOrDefault(key: LanguageKeys.Favorites), message);
                message = $"<speak>{message}</speak>";

                response = ResponseBuilder.Tell(new SsmlOutputSpeech() { Ssml = message });
            }
            return response;
        }

        private async Task ChangeLanguage()
        {
            foreach (FavModel fav in _favList)
            {
                if (!_accessor.Lang.Contains(fav.lang, StringComparison.InvariantCultureIgnoreCase))
                {
                    fav.cr825_message = await _translateService.TranslateToLanguage(fav.cr825_message, _accessor.Lang);
                    fav.lang = _accessor.Lang;
                }
            }
        }
    }
}
