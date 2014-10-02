using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.IO;
using System.Runtime.Serialization;

namespace Traderdata.Client.TerminalWEB.DTO
{
    //Required using System.Runtime.Serialization;
    //- Silverlight doesn't have it in its plugin, have to add reference to your project
    public static class ExtensionMethods
    {
        public static T DeepCopy<T>(this T theSource)
        {
            T theCopy;
            DataContractSerializer theDataContactSerializer = new DataContractSerializer(typeof(T));
            using (MemoryStream memStream = new MemoryStream())
            {
                theDataContactSerializer.WriteObject(memStream, theSource);
                memStream.Position = 0;
                theCopy = (T)theDataContactSerializer.ReadObject(memStream);
            }
            return theCopy;
        }
    }

}

