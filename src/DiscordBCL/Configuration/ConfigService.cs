using System;
using System.Collections.Generic;
using System.Text;

namespace DiscordBCL.Configuration
{
    public class ConfigService
    {
        public T GetConfig<T>(string filePath) where T : ConfigBase
        {
            return ConfigBase.Load<T>();
        }
    }
}
