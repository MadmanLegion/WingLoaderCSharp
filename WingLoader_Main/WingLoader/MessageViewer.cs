using System;
using System.Collections.Generic;
using System.Text;

namespace WingLoader_NS
{
    /// <summary>
    /// Wrapper class for holding the reference to the main thread display for messages from the WingLoader (e.g. current text updates etc.)
    /// </summary>
    public class MessageViewer
    {
        /// <summary>
        /// Internal library of current messages
        /// </summary>
        private Dictionary<string, string> records;

        /// <summary>
        /// Callback handler to be used to update the UI or other interested subscriber
        /// </summary>
        public event EventHandler<KeyValuePair<string, string>>? ItemChanged;

        /// <summary>
        /// Getters and Setters for each of the interesting memory objects / UI entities
        /// When getting - just pull form the records dictionary
        /// When setting - update the dictionary, and trigger the event handler to update subscribers
        /// </summary>
        public string message1 { get => records["tb_Message1"]; set { string key = "tb_Message1"; records[key] = value; ItemChanged?.Invoke(this, new KeyValuePair<string, string>(key, records[key])); } }
        public string message2 { get => records["tb_Message2"]; set { string key = "tb_Message2"; records[key] = value; ItemChanged?.Invoke(this, new KeyValuePair<string, string>(key, records[key])); } }
        public string firstName { get => records["tb_Firstname"]; set { string key = "tb_Firstname"; records[key] = value; ItemChanged?.Invoke(this, new KeyValuePair<string, string>(key, records[key])); } }
        public string surName { get => records["tb_Surname"]; set { string key = "tb_Surname"; records[key] = value; ItemChanged?.Invoke(this, new KeyValuePair<string, string>(key, records[key])); } }
        public string callSign { get => records["tb_Callsign"]; set { string key = "tb_Callsign"; records[key] = value; ItemChanged?.Invoke(this, new KeyValuePair<string, string>(key, records[key])); } }
        public string system { get => records["tb_System"]; set { string key = "tb_System"; records[key] = value; ItemChanged?.Invoke(this, new KeyValuePair<string, string>(key, records[key])); } }
        public string year { get => records["tb_Year"]; set { string key = "tb_Year"; records[key] = value; ItemChanged?.Invoke(this, new KeyValuePair<string, string>(key, records[key])); } }
        public string day { get => records["tb_Day"]; set { string key = "tb_Day"; records[key] = value; ItemChanged?.Invoke(this, new KeyValuePair<string, string>(key, records[key])); } }

        public string hex { get => records["tb_Hex"]; set { string key = "tb_Hex"; records[key] = value; ItemChanged?.Invoke(this, new KeyValuePair<string, string>(key, records[key])); } }
        public string address { get => records["tb_Address"]; set { string key = "tb_Address"; records[key] = value; ItemChanged?.Invoke(this, new KeyValuePair<string, string>(key, records[key])); } }
        public string textstring { get => records["tb_string"]; set { string key = "tb_string"; records[key] = value; ItemChanged?.Invoke(this, new KeyValuePair<string, string>(key, records[key])); } }
        public string hexstring { get => records["tb_hexstring"]; set { string key = "tb_hexstring"; records[key] = value; ItemChanged?.Invoke(this, new KeyValuePair<string, string>(key, records[key])); } }
        public string messages { get => records["tb_messages"]; set { string key = "tb_messages"; records[key] = value; ItemChanged?.Invoke(this, new KeyValuePair<string, string>(key, records[key])); } }

        /// <summary>
        /// Initialise the default values.
        /// </summary>
        public MessageViewer()
        {
            records = new Dictionary<string, string>();
            records.Add("tb_Message1", "");
            records.Add("tb_Message2", "");
            records.Add("tb_Firstname", "");
            records.Add("tb_Surname", "");
            records.Add("tb_Callsign", "");
            records.Add("tb_System", "");
            records.Add("tb_Year", "");
            records.Add("tb_Day", "");

            records.Add("tb_Hex", "");
            records.Add("tb_Address", "");
            records.Add("tb_string", "");
            records.Add("tb_hexstring", "");

            records.Add("tb_messages", "");
        }
    }
}