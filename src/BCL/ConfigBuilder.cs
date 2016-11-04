#region Header

// Description:
// 
// Solution: DiscordBots
// Project: BCL
// 
// Created: 11/04/2016 1:37 PM
// Last Revised: 11/04/2016 1:42 PM
// Last Revised by: Alex Gravely

#endregion

namespace BCL {
    #region Using

    using System;
    using System.Reflection;
    using Interfaces;

    #endregion

    public static partial class ConfigHandler {
        #region Internal Structs + Classes

        internal static class ConfigBuilder {
            #region Internal Methods

            internal static T CreateBotConfig<T>() where T : IBotConfig, new() {
                object boxedConfig = new T();
                foreach (var prop in typeof(T).GetRuntimeProperties()) {
                    if ((prop.PropertyType != typeof(string)) || (prop.PropertyType != typeof(ulong))) {
                        continue;
                    }
                    Console.WriteLine($"Input the value for property {prop.Name}:");
                    var propValue = Console.ReadLine();
                    prop.SetValue(boxedConfig, Convert.ChangeType(propValue, prop.PropertyType));
                }
                return (T) boxedConfig;
            }

            #endregion Internal Methods
        }

        #endregion Internal Structs + Classes
    }
}