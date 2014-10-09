using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace Traderdata.Server.App.TerminalWeb
{
    public static class Util
    {
        private static NumberFormatInfo numberProvider = new NumberFormatInfo();

        /// <summary>
        /// Provider padrão para número. Utiliza 2 casas decimais e separador decimal ".".
        /// </summary>
        public static NumberFormatInfo NumberProvider
        {
            get
            {
                numberProvider.NumberDecimalDigits = 2;
                numberProvider.NumberDecimalSeparator = ".";
                numberProvider.NumberGroupSeparator = "";

                return numberProvider.Clone() as NumberFormatInfo;
            }
        }
    }
}
