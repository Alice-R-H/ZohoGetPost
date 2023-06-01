using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ZohoGetPost.ObjectsForDeserialisation;

namespace ZohoGetPost.Methods
{
    public class GetPostRequests
    {
        //txt file strings
        public string txtFolder = @"C:\Temp\";

        public string postFirstAccess = "ZohoFirstAccessTokenJson.txt";
        public string ticketContent = "TicketContent.txt";
        public string noTicketsThisWeek = "NumTicketsThisWeek.txt";
        public string listAllContacts = "AllContactsJson.txt";

        //base urls
        public string postUrlEU = "https://accounts.zoho.eu/oauth/v2/token";

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
                foreach (var header in headerList)
                {
                    request.AddHeader(header.Item1, header.Item2);
                }
            }

            RestResponse response = client.ExecuteGet(request);

            //deserialise response into correct class 
            switch (className)
            {
                case "TicketByID":

                    string ticketByIDJson = response.Content;
                    string didThisWorkTicketByID = "Success";

                    try
                    {
                        TicketByID.Root ticketByIDObj = JsonConvert.DeserializeObject<TicketByID.Root>(ticketByIDJson);

                        

                        string ticketContentPath = txtFolder + ticketContent;
                        txtFileMethods.WriteToTxtFile(ticketContentPath, ticketByIDJson);


                        if (ticketByIDJson.Contains("invalid_code") || ticketByIDJson.Contains("errorCode") || ticketByIDJson == null)
                        {
                            didThisWorkTicketByID = "Unsuccessful. Ticket ID may be incorrect.";
                        }

                        MessageBox.Show("Action complete for get a ticket by ID. Response status: " + didThisWorkTicketByID);

                        return ticketByIDObj;
                    }
                    catch (Exception ex) 
                    {

                        MessageBox.Show("An error occured. Exception thrown:" + ex + " You might need to enter a new code:");
                        return null;
                    }

                case "AllTickets":

                    string ticketsThisWeekJson = response.Content;              
                    string didThisWorkTicketsThisWeek = "Success";

                    try
                    {
                        AllTickets.Root ticketsFromThisWeek = JsonConvert.DeserializeObject<AllTickets.Root>(ticketsThisWeekJson);

                        string ticketContentPath = txtFolder + ticketContent;
                        txtFileMethods.WriteToTxtFile(ticketContentPath, ticketsThisWeekJson);


                        if (ticketsThisWeekJson.Contains("invalid_code") || ticketsThisWeekJson.Contains("error") || ticketsThisWeekJson == null)
                        {
                            didThisWorkTicketByID = "Unsuccessful. Ticket ID may be incorrect.";
                        }

                        MessageBox.Show("Action complete. Response status: " + didThisWorkTicketsThisWeek);

                        return ticketsFromThisWeek;
                    }
                    catch (Exception ex)
                    {

                        MessageBox.Show("An error occured. Exception thrown:" + ex + " You might need to enter a new code:");
                        return null;
                    }


                case "No.TicketsThisWeek":

                    string noticketsThisWeekJson = response.Content;
                    string didNoThisWorkTicketsThisWeek = "Success";

                    try
                    {
                        NumTicketsThisWeek.Root ticketsFromThisWeek = JsonConvert.DeserializeObject<NumTicketsThisWeek.Root>(noticketsThisWeekJson);

                        string ticketContentPath = txtFolder + noTicketsThisWeek;
                        txtFileMethods.WriteToTxtFile(ticketContentPath, noticketsThisWeekJson);


                        if (noticketsThisWeekJson.Contains("invalid_code") || noticketsThisWeekJson.Contains("errorCode") || noticketsThisWeekJson == null)
                        {
                            didThisWorkTicketByID = "Unsuccessful.";
                        }

                        MessageBox.Show("Action complete. Response status: " + didNoThisWorkTicketsThisWeek);

                        return ticketsFromThisWeek;
                    }
                    catch (Exception ex)
                    {

                        MessageBox.Show("An error occured. Exception thrown:" + ex + " You might need to enter a new code:");
                        return null;
                    }

                default:
                    return null;
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
                foreach (var header in headerList)
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
                    try
                    {
                        AccessTokenOnRefresh.Root refreshedAccessTokenObj = JsonConvert.DeserializeObject<AccessTokenOnRefresh.Root>(refreshAccessTokenJson);

                    string didThisWorkRefresh = "Success";

                            if (refreshAccessTokenJson.Contains("invalid_code") || refreshAccessTokenJson.Contains("errorCode") || refreshAccessTokenJson == null)
                            {
                                didThisWorkRefresh = "Unsuccessful. Please enter a new code.";
                            }

                            MessageBox.Show("Action complete for refresh access token. Response status: " + didThisWorkRefresh);

                            return refreshedAccessTokenObj;
                     }

                    catch (Exception ex) 
                    {

                    MessageBox.Show("An error occured. Exception thrown:" + ex + " You might need to enter a new code:");

                    return null;
                    }

                case "AccessTokenObj":

                    string newAccessTokenJson = response.Content;
                    try
                    {
                        AccessTokenObj.Root accessTokenObj = JsonConvert.DeserializeObject<AccessTokenObj.Root>(newAccessTokenJson);

                    //save full json (for debugging)
                    string fullPath = txtFolder + postFirstAccess;
                    txtFileMethods.WriteToTxtFile(fullPath, newAccessTokenJson);

                    string didThisWork = "Success";

                            if (newAccessTokenJson.Contains("invalid_code") || newAccessTokenJson.Contains("errorCode") || newAccessTokenJson == null)
                            {
                                didThisWork = "Unsuccessful. Please enter a new code.";
                            }
                    
                            MessageBox.Show("Action complete for get new access token. Response status: " + didThisWork);


                            return accessTokenObj;
                    }

                    catch (Exception ex) 
                    {

                        MessageBox.Show("An error occured. Exception thrown:" + ex + " You might need to enter a new code:");
                        return null;
                    }

                default:
                    return null;
            }

        }
    }
}
