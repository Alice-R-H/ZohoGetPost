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
using System.Windows.Markup;

namespace ZohoGetPost
{
    public class ZohoInteraction
    {
        //url & params
        public string postUrlEU = "https://accounts.zoho.eu/oauth/v2/token";
        public string getTicketUrlEU = "https://desk.zoho.eu/api/v1/tickets/";
        public string getAllTicketsWeek = "https://desk.zoho.eu/api/v1/tickets?include=contacts";
        public string ticketCountUrl = "https://desk.zoho.eu/api/v1/ticketsCount";
        public string getAllContacts = "https://desk.zoho.eu/api/v1/contacts";
        public string getAllUsers = "https://desk.zoho.eu/api/v1/users";

        public string client_id = "1000.GE3HJMH28Z4RDPSU0TKMM9TOF9VMJH";
        public string client_secret = "094c18c0951d37a91f07d3940e41a890c5f715f016";
        public string grant_type = "authorization_code";
        public string code = "";
        public string orgID = "20077371725";

        public string ID = "";
        public string ticketStatus = "";
        public string name = "";
        public string firstName = "";
        public string lastName = "";

        public string accessTokenJson = "";

        //txt file strings
        public string txtFolder = @"C:\Temp\";

        public string postFirstAccess = "ZohoFirstAccessTokenJson.txt";
        public string accessTokenTxt = "AccessToken.txt";
        public string postRefreshToken = "RefreshToken.txt";
        public string expiryTime = "ExpiryTime.txt";
        public string ticketContent = "TicketContent.txt";
        public string ticketsThisWeek = "TicketsThisWeek";
        public string ticketsWithStatus = "TicketsWithStatus";


        TxtFileMethods txtFileMethods = new();
        GetPostRequests getPostRequests = new();

        public List<(string,string)> ticketDatesID = new List<(string,string)>();

        public List<(string, string, string,string,string)> ticketStatusAndInfo = new List<(string, string, string,string,string)>();

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
            try { 
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
            catch (Exception e)
            {
                //error message already shown within method
            }
}

        public bool Check_If_Token_Expired()
        {
            string expiryTimePath = txtFolder + expiryTime;
            string theExpiryTime = txtFileMethods.ReadTxtFile(expiryTimePath);
            DateTime expiryDate = DateTime.Parse(theExpiryTime);

            DateTime localDate = DateTime.Now;

            if (DateTime.Compare(expiryDate, localDate) < 0)
            {              
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


            try { 
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
            catch (Exception e)
            {
                //error message already shown within method
            }
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
            try
            {
                TicketByID.Root ticketByID = (TicketByID.Root)getPostRequests.Get_Request("TicketByID", getTicketUrlEU, getTicketUrl, headers: true);

                string ticketContentPath = txtFolder + ticketContent;

                //string 
            }
            catch (Exception e)
            {
                //error message already shown within method
            }

        }

        public void Get_Tickets_Within_One_Week()
        {
        
            string newAccessTokenPath = txtFolder + accessTokenTxt;
            string accessToken = txtFileMethods.ReadTxtFile(newAccessTokenPath);

            int seconds = -604800;

            DateTime localDate = DateTime.Now; // The DateTime object you want to convert
            string localISO = localDate.ToString("s");
            //string localISO = localDate.ToString("yyyy-MM-ddTHH:mm:ss.fffK");


            DateTime dateOneWeekAgo = localDate.AddSeconds(seconds);
            string weekAgoISO = dateOneWeekAgo.ToString("s");

            string fullString = weekAgoISO + ".000Z" + "," + localISO + ".000Z";

            //add headers to headerList
            getPostRequests.headerList = new List<(string, string)>
                  {
                            ("orgId", orgID),
                            ("Authorization", "Zoho-oauthtoken " +  accessToken)//requires space after oauthtoken
                   };

            getPostRequests.parameterList = new List<(string, string)>
                              { 
                               ("createdTimeRange", fullString)
                              };

            //do the api get
            NumTicketsThisWeek.Root NoTickets = (NumTicketsThisWeek.Root)getPostRequests.Get_Request("No.TicketsThisWeek", ticketCountUrl, ticketCountUrl, headers: true, parameters: true);

            string numOfTickets = NoTickets.count.ToString();

            //add headers to headerList
            getPostRequests.headerList = new List<(string, string)>
                  {
                            ("orgId", orgID),
                            ("Authorization", "Zoho-oauthtoken " +  accessToken)//requires space after oauthtoken
                   };

            getPostRequests.parameterList = new List<(string, string)>
                              {
                                ("sortBy", "-createdTime"),
                               ("limit", numOfTickets)

                              };

            AllTickets.Root AllTickets = (AllTickets.Root)getPostRequests.Get_Request("AllTickets", getAllTicketsWeek, getAllTicketsWeek, headers: true, parameters: true);




        }

        public void Get_Tickets_By_Status(string status)
        {          
            ticketStatusAndInfo.Clear();

            string newAccessTokenPath = txtFolder + accessTokenTxt;
            string accessToken = txtFileMethods.ReadTxtFile(newAccessTokenPath);

            //set up url
            string getTicketUrl = getAllTicketsWeek + "?include=contacts,products,assignee,departments,team";

            //add headers to headerList
            getPostRequests.headerList = new List<(string, string)>
                  {
                            ("orgId", orgID),
                            ("Authorization", "Zoho-oauthtoken " +  accessToken)//requires space after oauthtoken
                   };

            //do the api get
            AllTickets.Root allTickets = (AllTickets.Root)getPostRequests.Get_Request("AllTickets", getTicketUrl, getTicketUrl, headers: true);

            foreach (var data in allTickets.data)
            {
                if (data.status == status)
                {
                    ID = data.id;
                    name = data.contact.account.accountName;
                    ticketStatus = data.status;
                    firstName = data.contact.firstName;
                    lastName = data.contact.lastName;
                    
                    ticketStatusAndInfo.Add((ID, name, ticketStatus, firstName, lastName));
                }
            }

            if (ticketStatusAndInfo != null)
            {
                string concatenatedString = "Below are the tickets listed as having the status: " + status + "\n" + "\n";
                int totalTickets = 0;

                foreach (var item in ticketStatusAndInfo)
                {
                    concatenatedString += "Ticket ID:" + item.Item1 + "\n" + " Account Name:" + item.Item2 + "\n" + " Status:" + item.Item3 + "\n" + " Name: " + item.Item4 + " " + item.Item5 + "\n" + "\n" + "\n";
                    totalTickets++;
                }

                concatenatedString += "Total Number:" + totalTickets;

                string ticketsWithThisStatusPath = txtFolder + ticketsWithStatus + status;
                txtFileMethods.WriteToTxtFile(ticketsWithThisStatusPath, concatenatedString);

                try
                {
                    Process.Start("notepad.exe", ticketsWithThisStatusPath);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred while opening the file: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("There are no tickets with this status.");
            }

        }

        public void List_Contacts()
        {

            string newAccessTokenPath = txtFolder + accessTokenTxt;
            string accessToken = txtFileMethods.ReadTxtFile(newAccessTokenPath);

            //set up url
            string getContacts = getAllContacts;
            string getUsers = getAllUsers;

            //add headers to headerList
            getPostRequests.headerList = new List<(string, string)>
                  {
                            ("orgId", orgID),
                            ("Authorization", "Zoho-oauthtoken " +  accessToken)//requires space after oauthtoken
                   };
            getPostRequests.parameterList = new List<(string, string)>
                              {
                                ("limit", "10")       
                              };
            AllTickets.Root allTickets = (AllTickets.Root)getPostRequests.Get_Request("AllTickets", getContacts, getContacts, headers: true, parameters: true);

            //THIS WILL FAIL (expected)
            // add a breakpoint on line 97 in GetPostRequests to view the json response. 


        }









        //IGNORE : past methods for writing pretty to text file

        //foreach (var data in AllTickets.data)
        //{
        //    int seconds = -604800;

        //    DateTime localDate = DateTime.Now;
        //    DateTime dateOneWeekAgo = localDate.AddSeconds(seconds);

        //    if (DateTime.Compare(dateOneWeekAgo, data.createdTime) < 0)
        //    {
        //        string date = data.createdTime.ToString();
        //        string ID = data.id;

        //        ticketDatesID.Add((ID,date));                         
        //    }
        //}

        //if(ticketDatesID != null)
        //{
        //    string concatenatedString = "Below are the tickets submitted in the last week:" + "\n" + "\n";
        //    int totalTickets = 0;

        //    foreach (var item in ticketDatesID)
        //    {
        //        concatenatedString += "Ticket ID:" + item.Item1 + " Date created:" + item.Item2 + "\n";
        //        totalTickets++;
        //    }

        //    concatenatedString += "Total Number:" + totalTickets;

        //    string ticketsThisWeekPath = txtFolder + ticketsThisWeek;
        //    txtFileMethods.WriteToTxtFile(ticketsThisWeekPath, concatenatedString);

        //    try
        //    {
        //        Process.Start("notepad.exe", ticketsThisWeekPath);
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("An error occurred while opening the file: " + ex.Message);
        //    }
        //}
        //else
        //{
        //    MessageBox.Show("There were no tickets this week.");
        //}



    }
}
