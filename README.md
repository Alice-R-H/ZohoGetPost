# ZohoGetPost

Before using need to create a self client application in the Zoho API Console & using this edit the client_id and client_secret variables in MainWindow.xmal.cs
Link: https://api-console.zoho.eu/

UI Buttons and what they do:

- "Enter Code:"
Enter code obtained from the Zoho API Console, using scope: Desk.tickets.READ.

- "Get a new access token"
Click to store refresh token, access token and an expiry date for the access token.

- "Check if token expired"
Use to enable refresh access token if the current token has expired.

- "Refresh access token"
Refresh access token if the current token has expired, stores new token and rewrites the expiry date.
Currently not working, you will need to input a new code for a new access token, response "code_invalid".

- "Get a ticket"
Gets data of a ticket using ticket number. Currently ticket # is hard-coded until the get request is working. 
Currently not working, response "Unauthorised". 

Creates these .txt files in C:\Temp\

        ZohoFirstAccessTokenJson.txt
        AccessToken.txt
        RefreshToken.txt
        ExpiryTime.txt
        TicketContent.txt
