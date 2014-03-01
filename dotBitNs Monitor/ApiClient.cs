﻿using dotBitNS.UI.ApiControllers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;

namespace dotBitNs_Monitor
{
    class ApiClient : DependencyObject, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public static DependencyProperty PortProperty = DependencyProperty.Register("Port", typeof(int), typeof(ApiClient), new PropertyMetadata(dotBitNS.UI.WebApiHost.DefaultPort, OnPropertyChanged));

        public ApiClient()
        {
        }

        public async Task<ApiMonitorResponse> GetStatus()
        {
            using (var client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromMilliseconds(3000);
                var cts = new System.Threading.CancellationTokenSource();

                HttpResponseMessage response=null;
                try
                {
                    response = await client.GetAsync("http://localhost:" + Port + "/api/monitor", cts.Token);
                }
                catch (HttpRequestException ex)
                {
                    Debug.WriteLine(string.Format("ApiClient.GetStatus(): {0}: {1}", ex.GetType().ToString(), ex.Message));
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(string.Format("ApiClient.GetStatus(): {0}: {1}", ex.GetType().ToString(), ex.Message));
                }
                if (response != null)
                {
                    if (response.IsSuccessStatusCode)
                    {
                        Debug.WriteLine("Read Json, getting response...");
                        string json = await response.Content.ReadAsStringAsync();
                        Debug.WriteLine(json);

                        return JsonConvert.DeserializeObject<ApiMonitorResponse>(json);
                    }
                    else
                    {
                        Debug.WriteLine(string.Format("ApiClient.GetStatus(): Http Error: {0}", response.StatusCode));
                    }
                }
            }
            
            return null;
        }

        public int Port
        {
            get { return (int)GetValue(PortProperty); }
            set { SetValue(PortProperty, value); }
        }

        private static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var target = d as ApiClient;
            if (target != null)
                target.OnPropertyChanged(e.Property.Name);
        }

        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
