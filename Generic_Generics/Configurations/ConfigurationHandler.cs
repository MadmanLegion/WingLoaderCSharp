using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace Generic_Generics.Configurations
{
    /// <summary>
    /// Provides utility methods to read and write application configuration values from JSON configuration files.
    /// </summary>
    public class ConfigurationHandler
    {
        /// <summary>
        /// Retrieves a configuration value using a colon-separated path where the first segment is the file name.
        /// </summary>
        /// <param name="configPath">The full path, formatted as 'FileName:Section:Key' (e.g., 'appsettings:Logging:LogLevel:Default').</param>
        /// <returns>The string value associated with the specified configuration path.</returns>
        public static string getConfig(string configPath)
        {
            string configFile = configPath.Split(':')[0];
            configPath = configPath.Replace($"{configFile}:", "");
            return getConfig(configFile, configPath);
        }

        /// <summary>
        /// Retrieves a configuration value from a specific JSON configuration file.
        /// </summary>
        /// <param name="file">The name of the JSON configuration file without the extension (e.g., 'appsettings').</param>
        /// <param name="configPath">The internal configuration key path (e.g., 'Logging:LogLevel:Default').</param>
        /// <returns>The string value associated with the configuration path.</returns>
        private static string getConfig(string file, string configPath)
        {
            try
            {
                var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile($"{file}.json", optional: false, reloadOnChange: true);
                IConfiguration config = builder.Build();
                return readConfig(config, configPath);
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// Core execution method that reads a configuration key from an active <see cref="IConfiguration"/> instance.
        /// </summary>
        private static string readConfig(IConfiguration config, string configPath)
        {
            //Single item in level
            //string connString = config.GetConnectionString("DefaultConnection");
            //Multiple items in level
            //string apiKey = config["MyCustomSettings:apiKey"];
            return config[configPath]!;
        }

        /// <summary>
        /// Writes a configuration value using a colon-separated path where the first segment dictates the destination file name.
        /// </summary>
        /// <param name="configPath">The full path, formatted as 'FileName:Section:Key' (e.g., 'appsettings:Settings:ApiKey').</param>
        /// <param name="value">The string value to write.</param>
        /// <remarks>
        /// Warning: <c>configPath.Replace</c> does not modify the string in-place. 
        /// This method currently forwards the unmodified path to the overload.
        /// </remarks>
        public static void writeConfig(string configPath, string value)
        {
            string configFile = configPath.Split(':')[0];
            configPath = configPath.Replace($"{configFile}:", "");
            writeConfig(configFile, configPath, value);
        }

        /// <summary>
        /// Writes a configuration value directly into a designated JSON configuration file.
        /// </summary>
        /// <param name="configFile">The name of the target JSON configuration file without the extension.</param>
        /// <param name="configPath">The internal configuration key path (e.g., 'Settings:ApiKey').</param>
        /// <param name="value">The new configuration value to store.</param>
        private static void writeConfig(string configFile, string configPath, string value)
        {
            ConfigurationWriter.UpdateAppSetting(configPath, value, $"{configFile}.json");

            // Re-build configuration to apply changes if you aren't using reloadOnChange: true
            //var config = new ConfigurationBuilder()
            //    .SetBasePath(Directory.GetCurrentDirectory())
            //    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            //    .Build();
        }
    }


    /// <summary>
    /// Provides low-level file write accessors to safely insert or modify deeply nested values inside JSON configuration files.
    /// </summary>
    internal static class ConfigurationWriter
    {
        /// <summary>
        /// Updates or inserts an application setting value inside a physical JSON file, creating the file if it is missing.
        /// </summary>
        /// <param name="key">The hierarchical key path separated by colons (e.g., 'Section:Subsection:Key').</param>
        /// <param name="value">The string value to assign to the key entry.</param>
        /// <param name="filePath">The absolute or relative file path to the destination JSON file.</param>
        public static void UpdateAppSetting(string key, string value, string filePath = "appsettings.json")
        {
            // 1. Ensure the file exists
            if (!File.Exists(filePath))
            {
                File.WriteAllText(filePath, "{}");
            }

            // 2. Read the entire JSON file structure
            string json = File.ReadAllText(filePath);

            // Deserialize into a flexible dictionary format
            var configObj = JsonSerializer.Deserialize<Dictionary<string, object>>(json)
                            ?? new Dictionary<string, object>();

            // 3. Handle nested keys (e.g., "appsettings:debug")
            string[] parts = key.Split(':');
            UpdateNestedDictionary(configObj, parts, 0, value);

            // 4. Serialize back into readable formatted JSON and save
            var options = new JsonSerializerOptions { WriteIndented = true };
            string updatedJson = JsonSerializer.Serialize(configObj, options);
            File.WriteAllText(filePath, updatedJson);
        }

        /// <summary>
        /// Recursively navigates an abstract JSON object structure to find or build the nested dictionary tree path and inject the final value.
        /// </summary>
        /// <param name="currentDict">The active level directory segment being evaluated.</param>
        /// <param name="parts">The full tokenized configuration key path sequence array.</param>
        /// <param name="index">The zero-based tracking position within the parts path sequence array.</param>
        /// <param name="value">The final payload string data value to be set.</param>

        private static void UpdateNestedDictionary(Dictionary<string, object> currentDict, string[] parts, int index, string value)
        {
            string currentKey = parts[index];

            if (index == parts.Length - 1)
            {
                // We reached the final key, update its value
                currentDict[currentKey] = value;
                return;
            }

            // If the section doesn't exist, create it as a new nested dictionary
            if (!currentDict.ContainsKey(currentKey) || !(currentDict[currentKey] is JsonElement))
            {
                currentDict[currentKey] = new Dictionary<string, object>();
            }
            else if (currentDict[currentKey] is JsonElement jsonElement && jsonElement.ValueKind == JsonValueKind.Object)
            {
                // Convert immutable JsonElement back into a modifiable dictionary
                currentDict[currentKey] = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonElement.GetRawText())!;
            }

            if (currentDict[currentKey] is Dictionary<string, object> nextDict)
            {
                UpdateNestedDictionary(nextDict, parts, index + 1, value);
            }
        }
    }
}
