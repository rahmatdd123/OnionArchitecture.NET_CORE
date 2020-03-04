using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace NET_CORE
{
    public class ConfigValues
    {
        private static readonly IConfigurationRoot Configuration;

        static ConfigValues()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            Configuration = builder.Build();
        }

        public string Get(string name)
        {
            return Configuration[name];
        }
    }
}
