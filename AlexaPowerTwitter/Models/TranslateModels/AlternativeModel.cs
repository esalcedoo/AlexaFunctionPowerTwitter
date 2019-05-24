namespace AlexaPowerTwitter.Models.TranslateModels
{
    public class AlternativeModel
    {
        public string Language { get; set; }
        public float Score { get; set; }
        public bool IsTranslationSupported { get; set; }
        public bool IsTransliterationSupported { get; set; }
    }
}