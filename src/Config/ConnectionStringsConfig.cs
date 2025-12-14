using System;

namespace am.kon.packages.services.dac.doris.Config
{
    /// <summary>
    /// Class representing a map of named Doris (MySQL wire) connection strings.
    /// </summary>
    public class ConnectionStringsConfig : Dictionary<string, string>
    {
        public const string SectionDefaultName = "ConnectionStrings";
    }
}
