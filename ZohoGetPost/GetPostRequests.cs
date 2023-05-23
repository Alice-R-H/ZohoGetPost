using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ZohoGetPost
{
    public class GetPostRequests
    {
        //txt file strings
        public string txtFolder = @"C:\Temp\";

        public string postFirstAccess = "ZohoFirstAccessTokenJson.txt";
        public string ticketContent = "TicketContent.txt";

        //base urls
        public string postUrlEU = "https://accounts.zoho.eu/oauth/v2/token";
        public string getTicketUrlEU = "https://desk.zoho.eu/api/v1/tickets/";

        TxtFileMethods txtFileMethods = new TxtFileMethods();

        public List<(string, string)> parameterList = new List<(string, string)>();
        public List<(string, string)> headerList = new List<(string, string)>();

        // GET / POST Methods:

        public object Get_Request(string className, string getBaseUrl, string fullUrl, bool headers = false, bool parameters = false)
        {
            var ourMethod = Method.Get;

            RestClient client = new RestClient(getBaseUrl);
            RestRequest request = new RestRequest(fullUrl, ourMethod);

            //add headers and params
            if (parameters)
            {
                foreach (var parameter in parameterList)
                {
                    request.AddParameter(parameter.Item1, parameter.Item2);
                }
            }
            if (headers)
            {
                foreach (var header in parameterList)
                {
                    request.AddHeader(header.Item1, header.Item2);
                }
            }

            RestResponse response = client.ExecuteGet(request);

            //deserialise response into correct class 
            switch (className)
            {
                case "TicketByID":

                    //this is where oAuth error occuring, response is "Unauthorised"

                    string ticketByIDJson = response.Content;
                    TicketByID.Root ticketByIDObj = JsonConvert.DeserializeObject<TicketByID.Root>(ticketByIDJson);

                    string ticketContentPath = txtFolder + ticketContent;
                    txtFileMethods.WriteToTxtFile(ticketContentPath, ticketByIDJson);

                    string statusCode = response.StatusCode.ToString();

                    return ticketByIDObj;

                default:
                    return MessageBox.Show("Class not found.");
            }

        }

        public object Post_Request(string className, bool headers = false, bool parameters = false)
        {
            var ourMethod = Method.Post;

            RestClient client = new RestClient(postUrlEU);
            RestRequest request = new RestRequest(postUrlEU, ourMethod);

            //add headers and params
            if (parameters)
            {
                foreach (var parameter in parameterList)
                {
                    request.AddParameter(parameter.Item1, parameter.Item2);
                }

            }
            if (headers)
            {
                foreach (var header in parameterList)
                {
                    request.AddHeader(header.Item1, header.Item2);
                }
            }

            RestResponse response = client.ExecutePost(request);

            //deserialise response into correct class 
            switch (className)
            {
                case "AccessTokenOnRefresh":

                    string refreshAccessTokenJson = response.Content;
                    AccessTokenOnRefresh.Root refreshedAccessTokenObj = JsonConvert.DeserializeObject<AccessTokenOnRefresh.Root>(refreshAccessTokenJson);

                    string statusCode = response.StatusCode.ToString();

                    return refreshedAccessTokenObj;


                case "AccessTokenObj":

                    string newAccessTokenJson = response.Content;
                    AccessTokenObj.Root accessTokenObj = JsonConvert.DeserializeObject<AccessTokenObj.Root>(newAccessTokenJson);

                    //save full json (for debugging)
                    string fullPath = txtFolder + postFirstAccess;
                    txtFileMethods.WriteToTxtFile(fullPath, newAccessTokenJson);

                    return accessTokenObj;

                default:
                    return MessageBox.Show("Class not found.");
            }

        }
    }
}
