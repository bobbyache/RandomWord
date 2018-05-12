using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Gui
{
    public static class ConfigSettings
    {

        public static Version AssemblyVersion
        {
            get
            {
                return Assembly.GetExecutingAssembly().GetName().Version;
            }
        }

        public static string AssemblyTitle
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
                if (attributes.Length > 0)
                {
                    AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute)attributes[0];
                    if (titleAttribute.Title != "")
                        return titleAttribute.Title;
                }
                return Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
            }
        }

        public static string AssemblyDescription
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyDescriptionAttribute)attributes[0]).Description;
            }
        }

        public static string AssemblyProduct
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyProductAttribute)attributes[0]).Product;
            }
        }

        public static string AssemblyCopyright
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
            }
        }

        public static string AssemblyCompany
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyCompanyAttribute)attributes[0]).Company;
            }
        }

        public static Version ProjectFileVersion
        {
            get
            {
                Version version = AssemblyVersion;
                return new Version(version.Major, version.Minor);
            }
        }

        private static string lastOpenedFile;
        public static string LastOpenedFile
        {
            get
            {
                if (string.IsNullOrEmpty(lastOpenedFile))
                    lastOpenedFile = ConfigurationManager.AppSettings["LastOpenedFile"];

                return lastOpenedFile;
            }
            set
            {
                Configuration configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                configuration.AppSettings.Settings["LastOpenedFile"].Value = value;
                configuration.Save();
                ConfigurationManager.RefreshSection("appSettings");
            }
        }

        private static double? fontSize;
        public static double FontSize
        {
            get
            {
                if (!fontSize.HasValue)
                {
                    double fontSize;
                    bool success = double.TryParse(ConfigurationManager.AppSettings["FontSize"], out fontSize);
                    ConfigSettings.fontSize = (success && fontSize >= 0) ? fontSize : 10;
                }

                return fontSize.Value;
            }
            set
            {
                Configuration configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                configuration.AppSettings.Settings["FontSize"].Value = value.ToString();
                configuration.Save();
                ConfigurationManager.RefreshSection("appSettings");
            }
        }

        private static string fontFamily;
        public static string FontFamily
        {
            get
            {
                if (string.IsNullOrEmpty(fontFamily))
                    fontFamily = ConfigurationManager.AppSettings["FontFamily"];

                return fontFamily;
            }
            set
            {
                ConfigurationManager.AppSettings["RegistryPath"] = value;
            }
        }

        public static void Refresh()
        {
            lastOpenedFile = ConfigurationManager.AppSettings["LastOpenedFile"];
            fontSize = Int32.Parse(ConfigurationManager.AppSettings["FontSize"]);
            fontFamily = ConfigurationManager.AppSettings["FontFamily"];
        }

    }
}
