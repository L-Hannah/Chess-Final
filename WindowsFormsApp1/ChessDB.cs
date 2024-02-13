using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.WebRequestMethods;

namespace WindowsFormsApp1
{
    internal class ChessDB
    {
        public static async Task<string> Test(string addition)
        {
            string baseURL = "https://www.chessdb.cn/cdb.php?action=querybest&board=";
            string queryString = addition.Replace(" ", "%20");
            string url = baseURL + queryString;
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync(url);

                    if (response.IsSuccessStatusCode)
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();
                        return responseBody;
                    }
                    else
                    {
                        return $"Failed to get response from {url}. Status code: {response.StatusCode}";
                    }
                }
                catch (HttpRequestException e)
                {
                    return $"An error occurred while sending the request: {e.Message}";
                }
            }
        }
    }
}
