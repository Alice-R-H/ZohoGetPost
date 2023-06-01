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
    public partial class MainWindow : Window
    {
        ZohoInteraction zohoInteraction = new();

        public string ticketID = "";

        

        public MainWindow()
        {
            InitializeComponent();
        }

        public void Button_Click_EnterCode(object sender, RoutedEventArgs e)
        {
            //the "enter" button

            //for entering the code obtained from Zoho API console
            // application type: self client
            // scope: Desk.Tickets.READ         

            string codeInput = CodeInput.Text;
            zohoInteraction.Enter_Code(codeInput);
        }

        public void Button_Click_Get_New(object sender, RoutedEventArgs e)
        {
            //the "get a new access token" button
            if (zohoInteraction.Check_If_Token_Expired() == true)
            {
                zohoInteraction.Get_New_Token();
            }
            else
            {
                if (MessageBox.Show("The current token has not expired. Are you sure you want a new one?", "Question", MessageBoxButton.YesNo) == MessageBoxResult.No)
                {                  

                }
                else
                {
                    zohoInteraction.Get_New_Token();
                }              
            }
        }

        public void Button_Click_CheckExpiry(object sender, RoutedEventArgs e)
        {
            //the "check if expired" button
            //check if the expiry date is reached by comparing local time and time recorded in text file
            
            if(zohoInteraction.Check_If_Token_Expired() == false)
            {
                MessageBox.Show("Access token is still valid.");
            }
            else if (zohoInteraction.Check_If_Token_Expired() == true)
            {
                MessageBox.Show("Access token has expired, please use the refresh token button.");
            }

        }

        public void Button_Click_RefreshToken(object sender, RoutedEventArgs e)
        {
            //the "refresh token" button
            //returning "invalid_code" issue

            if (zohoInteraction.Check_If_Token_Expired() == true)
            {
                zohoInteraction.Refresh_Access_Token();
            }
            else if (zohoInteraction.Check_If_Token_Expired() == false)
            {
                if (MessageBox.Show("The current token has not expired. Are you sure you want a new one?", "Question", MessageBoxButton.YesNo) == MessageBoxResult.No)
                {

                }
                else
                {
                    zohoInteraction.Refresh_Access_Token();
                }
            }
        }

        public void Button_Click_EnterTicketID(object sender, RoutedEventArgs e)
        {
            //the "enter ticket id" button
            if(TicketIDInput.Text == "")
            {
                MessageBox.Show("You need to enter a real ticket ID first.");
            }
            else
            {
                ticketID = TicketIDInput.Text;
                MessageBox.Show("Ticket ID entered: " + ticketID);
            }

        }

        public void Button_Click_GetTicket(object sender, RoutedEventArgs e)
        {
            //get a ticket using the ticket ID provided

            if (ticketID == "")
            {
                MessageBox.Show("You need to add a ticket ID first.");

            }
            else if(zohoInteraction.Check_If_Token_Expired() == true)
            {
                MessageBox.Show("Access token has expired, please use the refresh token button.");
            }              
            else if(zohoInteraction.Check_If_Token_Expired() == false)               
            {
                zohoInteraction.Get_A_Ticket_By_ID(ticketID);
            }          
        }

        public void Button_Click_GetTicketsThisWeek(object sender, RoutedEventArgs e)
        {
            zohoInteraction.Get_Tickets_Within_One_Week();
        }

        public void Button_Click_GetTicketsByStatus(object sender, RoutedEventArgs e)
        {
            string statusType = statusComboBox.SelectedItem.ToString();
            const string prefixToRemove = "System.Windows.Controls.ComboBoxItem: ";
            string cleanedStatusType = statusType.Replace(prefixToRemove, string.Empty);

            if (cleanedStatusType != null)
            {
                zohoInteraction.Get_Tickets_By_Status(cleanedStatusType);
            }
            else
            {
                MessageBox.Show("Please select a status.");
            }
            
        }

        public void Button_Click_GetContacts(object sender, RoutedEventArgs e)
        {
            zohoInteraction.List_Contacts();
        }


        public void Button_Click_Close(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
