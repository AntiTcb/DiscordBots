// Description:
//
// Solution: DiscordBots
// Project: BCL
//
// Created: 11/04/2016 1:37 PM
// Last Revised: 11/04/2016 1:42 PM
// Last Revised by: Alex Gravely

namespace BCL {

    using Interfaces;
    using System;
    using System.Reflection;

    public static partial class ConfigHandler {

        internal static class ConfigBuilder {

            internal static T CreateBotConfig<T>() where T : IBotConfig, new() {
                object boxedConfig = new T();
                foreach (var prop in typeof(T).GetRuntimeProperties()) {
                    if (prop.PropertyType != typeof(string) && !prop.PropertyType.GetTypeInfo().IsPrimitive) {
                        prop.SetValue(boxedConfig, Activator.CreateInstance(prop.PropertyType));
                        continue;
                    }
                    Console.WriteLine($"Input the value for property {prop.Name}:");
                    var propValue = Console.ReadLine();
                    prop.SetValue(boxedConfig, Convert.ChangeType(propValue, prop.PropertyType));
                }
                return (T)boxedConfig;
            }
        }
    }
}