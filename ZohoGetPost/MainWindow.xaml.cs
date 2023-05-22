using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Newtonsoft.Json;
using RestSharp;
using Microsoft.Extensions.Configuration.Json;
using System.Reflection.PortableExecutable;
using System.Diagnostics.Metrics;

namespace ZohoGetPost
{
    public partial class MainWindow : Window
    {
        //url & params
        public string postUrlEU = "https://accounts.zoho.eu/oauth/v2/token";
        public string getTicketUrlEU = "https://desk.zoho.eu/api/v1/tickets/";

        public string client_id = "1000.GE3HJMH28Z4RDPSU0TKMM9TOF9VMJH";
        public string client_secret = "094c18c0951d37a91f07d3940e41a890c5f715f016";
        public string grant_type = "authorization_code";
        public string code = "";
        public string orgID = "20077371725";

        public string accessTokenJson = "";

        //txt file strings
        public string txtFolder = @"C:\Temp\";

        public string postFirstAccess = "ZohoFirstAccessTokenJson.txt";
        public string accessTokenTxt = "AccessToken.txt";
        public string postRefreshToken = "RefreshToken.txt";
        public string expiryTime = "ExpiryTime.txt";
        public string ticketContent = "TicketContent.txt";

        public List<(string, string)> parameterList = new List<(string, string)>();
        public List<(string, string)> headerList = new List<(string, string)>();

        //temporarily hard coding this 
        public string ticketID = "2350";

        public MainWindow()
        {
            InitializeComponent();
            //RefreshToken.IsEnabled = false;
            RefreshToken.IsEnabled = true;
            PostForAccessToken.IsEnabled = false;
        }

        public void Button_Click_Enter(object sender, RoutedEventArgs e)
        {
            //the "enter" button

            //for entering the code obtained from Zoho API console
            // application type: self client
            // scope: Desk.Tickets.READ         

            string codeInput = CodeInput.Text;

            //valid codes start will 1000, just to avoid silly mistakes
            if (codeInput.Contains("1000"))
            {
                code = CodeInput.Text;
                PostForAccessToken.IsEnabled = true;
                MessageBox.Show("Code entered. Click get a new access token.");
            }
            else
            {
                MessageBox.Show("Not a valid code");
            }
        }

        public void Button_Click_Get_New(object sender, RoutedEventArgs e)
        {
                //the "get a new access token" button

                //add parameters to parameterList
                parameterList = new List<(string, string)>
                          {
                            ("code", code),
                            ("grant_type", grant_type),
                            ("client_id", client_id),
                            ("client_secret", client_secret),
                            ("access_type", "offline"),
                            ("prompt", "consent")
                          };

                //do the api post
                AccessTokenObj.Root accessTokenObj = (AccessTokenObj.Root)Post_Request("AccessTokenObj", parameters: true);
  
                //get data from obj
                string refreshToken = accessTokenObj.refresh_token;
                string accessToken = accessTokenObj.access_token;

                //save expiry date
                WriteExpiryDate();

                //save refresh token
                string postRefreshTokenPath = txtFolder + postRefreshToken;
                WriteToTxtFile(postRefreshTokenPath, refreshToken);

                //save access token
                string fullPathAccessToken = txtFolder + accessTokenTxt;
                WriteToTxtFile(fullPathAccessToken, accessToken);

                //app UI changes
                PostForAccessToken.Background = Brushes.Black;
                PostForAccessToken.Foreground = Brushes.White;
                PostForAccessToken.IsEnabled = false;
                CheckIfExpired.IsEnabled = true;

                MessageBox.Show("Access token saved. Try getting a new ticket.");
        }
        public void Button_Click_CheckExpiry(object sender, RoutedEventArgs e)
        {
            //the "check if expired" button

            //check if the expiry date is reached by comparing local time and time recorded in text file

            string expiryTimePath = txtFolder + expiryTime;
            string theExpiryTime = ReadTxtFile(expiryTimePath);
            DateTime expiryDate = DateTime.Parse(theExpiryTime);

            DateTime localDate = DateTime.Now;

            if (DateTime.Compare(expiryDate, localDate) < 0)
            {
                RefreshToken.IsEnabled = true;
                MessageBox.Show("Access token has expired, you can now access the refresh token button.");
            }
            else
            {
                MessageBox.Show("Access token is still valid.");
            }
        }

        public void Button_Click_RefreshToken(object sender, RoutedEventArgs e)
        {
                //the "refresh token" button
                //returning "invalid_code" issue

                //get the refresh token by reading from file
                string postRefreshTokenPath = txtFolder + postRefreshToken;
                string refreshToken = ReadTxtFile(postRefreshTokenPath);

                //add parameters to parameterList
                parameterList = new List<(string, string)>
                              {
                                ("refresh_token", refreshToken),
                                ("client_id", client_id),
                                ("client_secret", client_secret),
                                ("scope", "Desk.tickets.READ"),
                                ("grant_type", "refresh_token")
                              };

                //do the api post
                AccessTokenOnRefresh.Root accessTokenOnRefresh = (AccessTokenOnRefresh.Root)Post_Request("AccessTokenOnRefresh", parameters: true);          

                //get data from obj
                string newAccessToken = accessTokenOnRefresh.access_token;

                //save access token to txt file
                string newAccessTokenPath = txtFolder + accessTokenTxt;
                WriteToTxtFile(newAccessTokenPath, newAccessToken);

                //save expiry date
                WriteExpiryDate();

                //app UI changes
                RefreshToken.Background = Brushes.Black;
                RefreshToken.Foreground = Brushes.White;
                RefreshToken.IsEnabled = true;
        }

        public void Button_Click_GetTicket(object sender, RoutedEventArgs e)
        {          
            string newAccessTokenPath = txtFolder + accessTokenTxt;
            string accessToken = ReadTxtFile(newAccessTokenPath);

            //set up url
            string getTicketUrl = getTicketUrlEU + ticketID + "?include=contacts,products,assignee,departments,team";

            //add headers to list
            headerList = new List<(string, string)>
              {
                        ("orgId", orgID),
                        ("Authorisation", "Zoho-oauthtoken" +  accessToken)
               };

            //do the api get
            TicketByID.Root ticketByID = (TicketByID.Root)Get_Request("TicketByID", getTicketUrl, headers : true);

            //some saving of data will be here when the request is working, json is saved into the TicketContent.txt during the switch case

        }


        // GET / POST Methods:

        public object Get_Request(string className, string fullUrl, bool headers = false, bool parameters = false)
        {

            //response not as expected on line 292, "unauthorised" 
            var ourMethod = Method.Get;

            RestClient client = new RestClient(getTicketUrlEU);
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
                    WriteToTxtFile(ticketContentPath, ticketByIDJson);

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
                    AccessTokenOnRefresh.Root newAccessTokenObj = JsonConvert.DeserializeObject<AccessTokenOnRefresh.Root>(refreshAccessTokenJson);

                    string statusCode = response.StatusCode.ToString();

                    return newAccessTokenObj;


                case "AccessTokenObj":

                    string newAccessTokenJson = response.Content;
                    AccessTokenObj.Root accessTokenObj = JsonConvert.DeserializeObject<AccessTokenObj.Root>(newAccessTokenJson);

                    //save full json (for debugging)
                    string fullPath = txtFolder + postFirstAccess;
                    WriteToTxtFile(fullPath, newAccessTokenJson);                

                    return accessTokenObj;

                default:
                    return MessageBox.Show("Class not found.");
            }
                      
        }

        // Write/Read .txt methods

        public void WriteToTxtFile(string fullPath, string result)
        {
            //write to a txt file

            StreamWriter writeToTxt = new StreamWriter(fullPath);

            writeToTxt.WriteLine(result);
            writeToTxt.Close();
        }

        public string ReadTxtFile(string fullPath)
        {
            //read from a txt file

            StreamReader readTxtFile = new StreamReader(fullPath);

            string fileContent = readTxtFile.ReadToEnd();

            readTxtFile.Close();

            return fileContent;
        }

        public void WriteExpiryDate()
        {
            //write the new expiry date for the token

            double seconds = 3600;
            DateTime localDate = DateTime.Now;
            string expiryTimeDate = localDate.AddSeconds(seconds).ToString();
            string expiryTimePath = txtFolder + expiryTime;

            WriteToTxtFile(expiryTimePath, expiryTimeDate);
        }

        //close
        public void Button_Click_Close(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
