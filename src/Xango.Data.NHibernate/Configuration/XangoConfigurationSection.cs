using System;
using System.Configuration;
using Xango.Common.Extensions;

namespace Xango.Data.NHibernate.Configuration
{
    public class XangoConfigurationSection : ConfigurationSection
    {
        [ConfigurationProperty("domainAssemblyName", DefaultValue = "", IsRequired = true)]
        public string DomainAssemblyName
        {
            get { return (string) this["domainAssemblyName"]; }
            set { this["domainAssemblyName"] = value; }
        }

        [ConfigurationProperty("isWeb", DefaultValue = "", IsRequired = true)]
        public string IsWeb
        {
            get { return this["isWeb"].ToStringSafe(); }
            set { this["isWeb"] = value; }
        }

        [ConfigurationProperty("database", DefaultValue = "", IsRequired = true)]
        public string Database
        {
            get { return this["database"].ToStringSafe(); }
            set { this["database"] = value; }
        }

        [ConfigurationProperty("validationAssemblyName", DefaultValue = "", IsRequired = true)]
        public string ValidationAssemblyName
        {
            get { return this["validationAssemblyName"].ToStringSafe(); }
            set { this["validationAssemblyName"] = value; }
        }

        [ConfigurationProperty("overrideMapAssemblyName", DefaultValue = "", IsRequired = false)]
        public string OverrideMapAssemblyName
        {
            get { return this["overrideMapAssemblyName"].ToStringSafe(); }
            set { this["overrideMapAssemblyName"] = value; }
        }
    }
}