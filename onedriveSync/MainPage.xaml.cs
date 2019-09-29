﻿

using Dropbox.Api;
using Microsoft.OneDrive.Sdk;
using Microsoft.Toolkit.Services.OneDrive;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// La plantilla de elemento Página en blanco está documentada en https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0xc0a

namespace onedriveSync
{
    /// <summary>
    /// Página vacía que se puede usar de forma independiente o a la que se puede navegar dentro de un objeto Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        // Client Apication Id.
        private string clientId = "a361e61e-5270-4246-8a8c-1aa1699c8733";

        // Return url.
        private string returnUrl = "https://login.live.com/oauth20_desktop.srf";

        // Define the permission scopes.
        private static readonly string[] scopes = new string[] {
            "onedrive.readwrite", "offline_access", "wl.signin", "wl.basic" };

        // Create the OneDriveClient interface.
        private IOneDriveClient oneDriveClient { get; set; }
       // private IOneDriveStorageFolderPlatform rootf { get; set; }

        public MainPage()
        {
            this.InitializeComponent();
           
        }
       // public OneDriveService inse = Microsoft.Toolkit.Services.OneDrive.OneDriveService.Instance.Initialize( appClientId:OneDriveService., scopes, null, null);

        private async void btn_Login_Click(object sender, RoutedEventArgs e)
        {
            if (this.oneDriveClient == null)
            {
                try
                {
                    // Setting up the client here, passing in our Client Id, Return Url, 
                    // Scopes that we want permission to, and building a Web Broker to 
                    // go do our authentication. 
                    
      

                    this.oneDriveClient = await OneDriveClient.GetAuthenticatedMicrosoftAccountClient(
                        clientId,
                        returnUrl,
                        scopes,
                        webAuthenticationUi: new WebAuthenticationBrokerWebAuthenticationUi());

                    

                   // Show in text box that we are connected.
                   txtBox_Response.Text = "We are now connected";

                    // We are either just autheticated and connected or we already connected, 
                    // either way we need the drive button now.
                    btn_GetDriveId.Visibility = Visibility.Visible;
                }
                catch (OneDriveException exception)
                {
                    // Eating the authentication cancelled exceptions and resetting our client. 
                    if (!exception.IsMatch(OneDriveErrorCode.AuthenticationCancelled.ToString()))
                    {
                        if (exception.IsMatch(OneDriveErrorCode.AuthenticationFailure.ToString()))
                        {
                            txtBox_Response.Text = "Authentication failed/cancelled, disposing of the client...";

                            ((OneDriveClient)this.oneDriveClient).Dispose();
                            this.oneDriveClient = null;
                        }
                        else
                        {
                            // Or we failed due to someother reason, let get that exception printed out.
                            txtBox_Response.Text = exception.Error.ToString();
                        }
                    }
                    else
                    {
                        ((OneDriveClient)this.oneDriveClient).Dispose();
                        this.oneDriveClient = null;
                    }
                }
            }
        }
        //public SaveToLocalFolder(IRandomAccessStream ,OneDriveStorageFile)
        //{

        //}
        private async void btn_GetDriveId_Click(object sender, RoutedEventArgs e)
        {
            if (this.oneDriveClient != null && this.oneDriveClient.IsAuthenticated == true)
            {
                var drive = await this.oneDriveClient.Drive.Request().GetAsync();
                  txtBox_Response.Text = drive.Id.ToString();
                

            }
            else
            {
              
                txtBox_Response.Text = "We should never get here...";
            }

        }

    }
  }
 

