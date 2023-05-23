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
using ZohoGetPost.Methods;
using ZohoGetPost.ObjectsForDeserialisation;

namespace ZohoGetPost
{
    public class ZohoInteraction
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

        TxtFileMethods txtFileMethods = new();
        GetPostRequests getPostRequests = new();


        public void Enter_Code(string codeInput)
        {
            //valid codes start will 1000, just to avoid silly mistakes
            if (codeInput.Contains("1000"))
            {
                code = codeInput;
                MessageBox.Show("Code entered. Click get a new access token.");
            }
            else
            {
                MessageBox.Show("Not a valid code");
            }
        }
        
        public void Get_New_Token()
        {
            //add parameters to parameterList
            getPostRequests.parameterList = new List<(string, string)>
                          {
                            ("code", code),
                            ("grant_type", grant_type),
                            ("client_id", client_id),
                            ("client_secret", client_secret),
                            ("access_type", "offline"),
                            ("prompt", "consent")
                          };

            //do the api post
            AccessTokenObj.Root accessTokenObj = (AccessTokenObj.Root)getPostRequests.Post_Request("AccessTokenObj", parameters: true);

            //get data from obj
            string refreshToken = accessTokenObj.refresh_token;
            string accessToken = accessTokenObj.access_token;

            //save expiry date
            txtFileMethods.WriteExpiryDate(txtFolder, expiryTime);

            //save refresh token
            string postRefreshTokenPath = txtFolder + postRefreshToken;
            txtFileMethods.WriteToTxtFile(postRefreshTokenPath, refreshToken);

            //save access token
            string fullPathAccessToken = txtFolder + accessTokenTxt;
            txtFileMethods.WriteToTxtFile(fullPathAccessToken, accessToken);

        }

        public bool Check_If_Token_Expired()
        {
            string expiryTimePath = txtFolder + expiryTime;
            string theExpiryTime = txtFileMethods.ReadTxtFile(expiryTimePath);
            DateTime expiryDate = DateTime.Parse(theExpiryTime);

            DateTime localDate = DateTime.Now;

            if (DateTime.Compare(expiryDate, localDate) < 0)
            {

                MessageBox.Show("Access token has expired, please use the refresh token button.");
                return true;
            }
            if (theExpiryTime == null)
            {
                MessageBox.Show("There is no saved access token. You may need to enter a new code.");
                return true;
            }
            else
            {
                return false;
            }

        }

        public void Refresh_Access_Token()
        {
            //get the refresh token by reading from file
            string postRefreshTokenPath = txtFolder + postRefreshToken;
            string refreshToken = txtFileMethods.ReadTxtFile(postRefreshTokenPath);

            //add parameters to parameterList
            getPostRequests.parameterList = new List<(string, string)>
                              {
                                ("refresh_token", refreshToken),
                                ("client_id", client_id),
                                ("client_secret", client_secret),
                                ("scope", "Desk.tickets.READ"),
                                ("grant_type", "refresh_token")
                              };

            //do the api post
            AccessTokenOnRefresh.Root accessTokenOnRefresh = (AccessTokenOnRefresh.Root)getPostRequests.Post_Request("AccessTokenOnRefresh", parameters: true);

            //get data from obj
            string newAccessToken = accessTokenOnRefresh.access_token;

            //save access token to txt file
            string newAccessTokenPath = txtFolder + accessTokenTxt;
            txtFileMethods.WriteToTxtFile(newAccessTokenPath, newAccessToken);

            //save expiry date
            txtFileMethods.WriteExpiryDate(txtFolder, expiryTime);

        }

        public void Get_A_Ticket_By_ID(string ticketID)
        {
            string newAccessTokenPath = txtFolder + accessTokenTxt;
            string accessToken = txtFileMethods.ReadTxtFile(newAccessTokenPath);

            //set up url
            string getTicketUrl = getTicketUrlEU + ticketID + "?include=contacts,products,assignee,departments,team";

            //add headers to headerList
            getPostRequests.headerList = new List<(string, string)>
              {
                        ("orgId", orgID),
                        ("Authorization", "Zoho-oauthtoken " +  accessToken)
               };

            //do the api get
            TicketByID.Root ticketByID = (TicketByID.Root)getPostRequests.Get_Request("TicketByID", getTicketUrlEU, getTicketUrl, headers: true);

            //some saving of data will be here when the request is working, full json response string is saved into the TicketContent.txt during the switch case

        }
    }
}
