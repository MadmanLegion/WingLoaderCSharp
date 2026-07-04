using Generic_Generics.Logging;

namespace WingLoader_Generics.Debugger
{
    public class DosBox_Debugger : Debugger
    {
        HttpClient client = new HttpClient();

        /// <summary>
        /// Cleanup any instantiated objects
        /// For DosBox this is the HttpClient
        /// </summary>
        public override void Dispose()
        {
            client.Dispose();
        }

        /// <summary>
        /// The initial memory offset (in hex format as a string) from which all other memory addresses are based.  
        /// By default in DOS this is 0x00 and in Windows it's 0x400000
        /// This value cannot be set by outsiders, but may need to be read
        /// </summary>
        public override string initialOffset { get => "0x00"; }
        //private async Task InvokeAPI(string uri)
        //{
        //    lastResponse = await client.GetAsync(uri);
        //}

        //private async Task<string> InvokeAPI(string uri)
        //{
        //    try
        //    {
        //        using HttpResponseMessage response = await client.GetAsync(uri);
        //        response.EnsureSuccessStatusCode();
        //        string responsebody = await response.Content.ReadAsStringAsync();
        //        return responsebody;
        //    }
        //    catch (HttpRequestException e)
        //    {
        //        Console.WriteLine($"Request error: {e.Message}");
        //        return null;
        //    }
        //}

        /// <summary>
        /// Retrieve an array of bytes from the debugger - starting at offset "memory" and for length "size"
        /// offset and size are given in hex format as a string
        /// </summary>
        /// <param name="memory">offset to start reading from in 0x00 string format</param>
        /// <param name="size">number of bytes to read in 0000 string format</param>
        /// <returns></returns>
        public override Byte[] getMemory_Sync(string memory, string size)
        {
            string uri = string.Format("http://127.0.0.1:8080/api/v1/memory/{0}/{1}", memory, size);
            byte[] bytes = new byte[] { 0x00, 0x00, 0x00 }; ;
            try
            {
                var response = client.GetAsync(uri).Result;
                response.EnsureSuccessStatusCode();
                bytes = response.Content.ReadAsByteArrayAsync().Result;
            }
            catch (Exception e)
            {
                Debug_Logger.log(e.Message);
            }
            return bytes;
        }

        /// <summary>
        /// Retrieve an array of bytes from the debugger - starting at offset "memory" and for length "size"
        /// offset and size are given in hex format as a string
        /// </summary>
        /// <param name="memory">offset to start reading from in 0x00 string format</param>
        /// <param name="size">number of bytes to read in 0000 string format</param>
        /// <returns></returns>
        public override async Task<Byte[]> getMemory_ASync(string memory, string size)
        {
            string uri = string.Format("http://127.0.0.1:8080/api/v1/memory/{0}/{1}", memory, size);
            byte[] bytes = new byte[] { 0x00, 0x00, 0x00 }; ;
            try
            {
                bytes = await client.GetByteArrayAsync(uri);
            }
            catch (Exception e)
            {
                Debug_Logger.log(e.Message);
            }
            return bytes;
        }
    }
}
