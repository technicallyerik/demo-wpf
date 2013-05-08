using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace CurrencyConversion.Services
{
    public class CurrencyServiceSettings : ConfigurationSection
    {
        private const string URL_PROPERTY_NAME = "url";
        private const string KEY_PROPERTY_NAME = "key";

        public static CurrencyServiceSettings Settings
        {
            get
            {
                return ConfigurationManager.GetSection("CurrencyServiceSettings") as CurrencyServiceSettings;
            }
        }

        [ConfigurationProperty(URL_PROPERTY_NAME, DefaultValue = "http://openexchangerates.org/api/", IsRequired = false)]
        public string Url
        {
            get { return (string)this[URL_PROPERTY_NAME]; }
            set { this[URL_PROPERTY_NAME] = value; }
        }

        [ConfigurationProperty(KEY_PROPERTY_NAME, IsRequired = true)]
        public string Key
        {
            get { return (string)this[KEY_PROPERTY_NAME]; }
            set { this[KEY_PROPERTY_NAME] = value; }
        }
    }
}