using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySaveV1
{
    internal class LanguageManager
    {
        private Language currentLanguage;

        public LanguageManager(Language defaultLanguage = Language.ENGLISH)
        {
            currentLanguage = defaultLanguage;
        }

        public void SetLanguage(Language language)
        {
            currentLanguage = language;
        }

        public Language GetLanguage()
        {
            return currentLanguage;
        }
    }

    public enum Language
    {
        FRENCH,
        ENGLISH
    }
}