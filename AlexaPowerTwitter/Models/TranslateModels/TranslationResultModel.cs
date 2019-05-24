using System.Collections.Generic;

namespace AlexaPowerTwitter.Models.TranslateModels
{
    public class TranslationResultModel
    {
        public LanguageDetectedModel DetectedLanguage { get; set; }
        public List<TranslationModel> Translations { get; set; }
    }
}
