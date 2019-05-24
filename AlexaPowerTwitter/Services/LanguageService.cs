using Alexa.NET.LocaleSpeech;
using AlexaPowerTwitter.Models;
using System;
using System.Collections.Generic;

namespace AlexaPowerTwitter.Services
{
    public class LanguageService
    {
        internal DictionaryLocaleSpeechStore SetupLanguageResources()
        {
            // Creates the locale speech store for each supported languages.
            var store = new DictionaryLocaleSpeechStore();

            store.AddLanguage("en", new Dictionary<string, object>
            {
                [LanguageKeys.Welcome] = "Welcome to the skill!",
                [LanguageKeys.WelcomeReprompt] = "You can ask help if you need instructions on how to interact with the skill",
                [LanguageKeys.Response] = "This is just a sample answer",
                [LanguageKeys.Cancel] = "Canceling...",
                [LanguageKeys.Help] = "Help...",
                [LanguageKeys.Stop] = "Bye bye!",
                [LanguageKeys.Error] = "I'm sorry, there was an unexpected error. Please, try again later.",
                [LanguageKeys.Translate] = "Some tweets are written in other languages, ¿do you want me to translate them?",
                [LanguageKeys.TranslateReprompt] = "¿do you want me to translate them?"
            });

            store.AddLanguage("es", new Dictionary<string, object>
            {
                [LanguageKeys.Welcome] = "Bienvenido a la Skill de Power Twitter!",
                [LanguageKeys.WelcomeReprompt] = "Si tiene alguna duda, por favor, diga ayuda",
                [LanguageKeys.Cancel] = "Vale, cancelo",
                [LanguageKeys.Help] = "Puedo decirte los tweets que marcaste como favorito en twitter, di: lee mis favoritos en inglés",
                [LanguageKeys.Stop] = "Adiós!",
                [LanguageKeys.Error] = "Mis disculpas, ha habido un error inesperado. Por favor, prueba de nuevo más tarde.",
                [LanguageKeys.Translate] = "Algunos de los tweets están en otros idiomas, ¿quieres que los traduzca?",
                [LanguageKeys.TranslateReprompt] = "¿quieres que los traduzca?"
            });

            return store;
        }

        internal Dictionary<string, string> SetupAuxLanguageResourcesByLang(string lang)
        {
            var resources = new Dictionary<string, string>();
            if (lang.Contains("en", StringComparison.InvariantCultureIgnoreCase))
            {
                resources.Add(LanguageKeys.Favorites, "Here you are: {0}");
            }
            else if (lang.Contains("es", StringComparison.InvariantCultureIgnoreCase))
            {
                resources.Add(LanguageKeys.Favorites, "Aquí tienes tus últimos favoritos: {0}");
            }
            return resources;
        }
        
        internal string AdaptCulture(string lang)
        {
            switch (lang)
            {
                case "es":
                    return "es-ES";
                case "en":
                    return "en-US";
                default:
                    return lang;
            }
        }
    }
}
