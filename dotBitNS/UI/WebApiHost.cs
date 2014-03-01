﻿using Owin;
using System.Web.Http;
using Microsoft.Owin.Hosting;
using System;
using System.Net.Http;

namespace dotBitNS.UI
{
    class WebApiHost
    {
        public const int DefaultPort = 9098;

        private static int _Port;

        public static int Port
        {
            get { return WebApiHost._Port; }
            private set 
            {
                WebApiHost._Port = value;
                InitializeApiServer();
            }
        }

        private static IDisposable app;

        public static void Initialize()
        {
            Port = DefaultPort;

            InitializeApiServer();
        }

        private static void InitializeApiServer()
        {
            if (app != null)
            {
                app.Dispose();
                app = null;
            }
            Console.WriteLine("Initializing api on port {0}...", Port);
            app = WebApp.Start<WebApiHost>(url: "http://localhost:" + Port.ToString());
        }

        // This code configures Web API. The Startup class is specified as a type
        // parameter in the WebApp.Start method.
        public void Configuration(IAppBuilder appBuilder)
        {
            // Configure Web API for self-host. 
            HttpConfiguration config = new HttpConfiguration();
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            appBuilder.UseWebApi(config);
        }
    }

}