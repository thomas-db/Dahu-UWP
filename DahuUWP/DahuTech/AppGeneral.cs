﻿using DahuUWP.DahuTech.ViewNotification;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.ApplicationModel;

namespace DahuUWP.DahuTech
{
    /// <summary>
    /// Enum for the languages
    /// Comporting like a enum
    /// https://stackoverflow.com/questions/630803/associating-enums-with-strings-in-c-sharp
    /// </summary>
    public class Languages
    {
        private Languages(string value) { Value = value; }

        public string Value { get; set; }

        public static Languages French { get { return new Languages("fr-FR"); } }
        public static Languages English { get { return new Languages("en-US"); } }
    }



    public static class AppGeneral
    {
        /// <summary>
        /// List the last messages that API has send for the user interface
        /// </summary>
        private static List<string> _ApiToUserMsg;
        public static List<string> ApiToUserMsg
        {
            get { return _ApiToUserMsg; }

            set
            {
                //_ApiToUserMsg = ListUtils.EmptyList(_ApiToUserMsg);
                _ApiToUserMsg = value;
            }
        }

        public static Dictionary<string, Notification> UserInterfaceStatusDico { get; set; }

        //TODO detecter le language pour pouvoir charger le bon xml celon la langue
        // XML tuto: http://www.c-sharpcorner.com/article/read-xml-file-in-windows-10-universal-app/
        static private void InitUserInterfaceStatusDico()
        {
            UserInterfaceStatusDico = new Dictionary<string, Notification>();
            string pathToXml = Path.Combine(Package.Current.InstalledLocation.Path, "Strings\\" + Languages.French.Value + "\\UserInterfaceStatus.xml");
            XDocument UserInterfaceStatusXML = XDocument.Load(pathToXml);

            foreach (XElement elem in UserInterfaceStatusXML.Descendants("status"))
            {
                Notification notif = new Notification
                {
                    Value = elem.Attribute("value").Value,
                    PropertyName = elem.Attribute("varName").Value
                };  
                UserInterfaceStatusDico.Add(elem.Attribute("key").Value, notif);
            }
        }

        static AppGeneral()
        {
            InitUserInterfaceStatusDico();
        }
    }
}