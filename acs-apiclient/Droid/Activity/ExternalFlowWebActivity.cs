﻿
using System;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Webkit;
using acs_apiclient.Droid.CustomViews;
using Android.Widget;

namespace acs_apiclient.Droid
{
    [Activity(Label = "ExternalFlowWebActivity", Theme = "@android:style/Theme.NoTitleBar")]
    public class ExternalFlowWebActivity : Activity
    {
        public static string UrlParamKey = "url";
        private static Android.Graphics.Color ObsidianGrey = Android.Graphics.Color.ParseColor("#222b3c");
        private WebView contentWebView;
        private String urlString;
        private Android.Net.Uri uri;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            this.Window.AddFlags(WindowManagerFlags.Fullscreen);
            this.SetContentView(Resource.Layout.activity_externalFlowWeb);
            InitUri();
            SetupTitle(uri.Host);
            SetupCloseButton();
            SetupSyncButton();
            SetupWebView();
        }

        private void InitUri()
        {
            this.urlString = this.Intent.GetStringExtra(UrlParamKey);
            if (String.IsNullOrEmpty(this.urlString))
            {
                throw new ArgumentException($"'url' must be provided to start {typeof(ExternalFlowWebActivity)}");
            }
            this.uri = Android.Net.Uri.Parse(this.urlString);
        }

        private void SetupTitle(string newTitle)
        {
            TextView title = FindViewById<TextView>(Resource.Id.textview_toolbarContainer_title);
            title.Text = newTitle;
        }

        private void SetupSyncButton()
        {
            EasyTintImageButton syncButton = FindViewById<EasyTintImageButton>(Resource.Id.easyTintImageButton_titlebarWithExitAndRefereshButtons_sync);
            syncButton.SelectedTintColor = ObsidianGrey;
            syncButton.Click += (object sender, EventArgs e) => 
            {
                this.contentWebView.LoadUrl(this.urlString);
            };
        }

        private void SetupCloseButton()
        {
            EasyTintImageButton closeButton = FindViewById<EasyTintImageButton>(Resource.Id.easyTintImageButton_titlebarWithExitAndRefereshButtons_close);
            closeButton.SelectedTintColor = ObsidianGrey;
            closeButton.Click += (object sender, EventArgs e) => 
            {
                //externalFlowDelegate.UserCancelledLogin();//TODO need to determine how to do this
                Finish();
            };
        }

        private void SetupWebView()
        {
            this.contentWebView = FindViewById<WebView>(Resource.Id.webview_externalFlowWeb_content);
            this.contentWebView.Settings.JavaScriptEnabled = true;
            this.contentWebView.SetWebViewClient(new ExternalFlowWebViewClient());
            this.contentWebView.LoadUrl(this.urlString);
        }
    }
}
