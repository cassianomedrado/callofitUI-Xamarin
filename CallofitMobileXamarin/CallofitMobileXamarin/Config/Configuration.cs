using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

public static class Configuration
{
    public static string ApiUrl { get; private set; }

    static Configuration()
    {
        var assembly = typeof(Configuration).GetTypeInfo().Assembly;
        var resourceName = assembly.GetManifestResourceNames().SingleOrDefault(x => x.EndsWith("appsettings.xml"));

        if (resourceName == null)
        {
            throw new Exception("appsettings.xml not found");
        }

        using (var stream = assembly.GetManifestResourceStream(resourceName))
        using (var reader = new StreamReader(stream))
        {
            var doc = XDocument.Parse(reader.ReadToEnd());
            ApiUrl = doc.Root.Element("apiUrl").Value;
        }

        //var assembly = typeof(Configuration).GetTypeInfo().Assembly;
        //var resourceName = assembly.GetManifestResourceNames().Single(x => x.EndsWith("appsettings.xml"));

        //using (var stream = assembly.GetManifestResourceStream(resourceName))
        //using (var reader = new StreamReader(stream))
        //{
        //    var doc = XDocument.Parse(reader.ReadToEnd());
        //    ApiUrl = doc.Root.Element("apiUrl").Value;
        //}
    }
}
