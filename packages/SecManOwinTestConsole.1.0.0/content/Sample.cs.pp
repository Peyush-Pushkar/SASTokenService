using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace  $rootnamespace$
{
    public class Sample
    {
        public async static Task RunSample()
        {    
            // TODO: Change the URL. This should be base url of the web application, 
            // which exposes the authentication endpoint.
            string baseAddress = "http://localhost:9000/";

            var provider = new TokenProvider(baseAddress);

            string m_accessToken;

            Dictionary<string, string> m_tokenDictionary;

            try
            {
                m_tokenDictionary = await provider.AcquireToken("manager@daenet.com", "password");

                m_accessToken = m_tokenDictionary["access_token"];

                HttpClient client = new HttpClient();

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", m_accessToken);

                var response = await client.GetAsync(baseAddress + "api/values");

                Console.WriteLine(await response.Content.ReadAsStringAsync());

                response = await client.GetAsync(baseAddress + "api/values");

                Console.WriteLine(await response.Content.ReadAsStringAsync());
            }
            catch (AggregateException ex)
            {
                // If it's an aggregate exception, an async error occurred:

                Console.WriteLine(ex.InnerExceptions[0].Message);
                Console.WriteLine("Press the Enter key to Exit...");
                Console.ReadLine();

                return;
            }

            catch (Exception ex)
            {

                // Something else happened:

                Console.WriteLine(ex.Message);

                Console.WriteLine("Press the Enter key to Exit...");

                Console.ReadLine();

                return;

            }


            foreach (var kvp in m_tokenDictionary)
            {
                Console.WriteLine("{0}: {1}", kvp.Key, kvp.Value);

                Console.WriteLine("");
            }
        }
    }
}
