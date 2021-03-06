﻿// Products: MeowBit dotBitNS
// THE BEASTLICK INTERNET POLICY COMMISSION & Alien Seed Software
// Author: Derrick Slopey derrick@alienseed.com
// March 4, 2014

using System;
using System.Collections.Generic;
using System.Web.Http;

namespace dotBitNs.UI.ApiControllers
{
    public class ControlController : ApiController
    {
        // GET api/Control 
        // GET api/Control/5 
        public dynamic Get(int? id = null)
        {
            return null;
        }

        // POST api/Control 
        public dynamic Post(NmcConfigJson value)
        {
            if (!string.IsNullOrWhiteSpace(value.User)) Properties.Settings.Default.RpcUsername = value.User;
            if (!string.IsNullOrWhiteSpace(value.Pass)) Properties.Settings.Default.RpcPassword = value.Pass;
            if (!string.IsNullOrWhiteSpace(value.Port))
            {
                ushort port;
                if (ushort.TryParse(value.Port, out port))
                    Properties.Settings.Default.RpcPort = port;
            }
            if (!string.IsNullOrWhiteSpace(value.Logging))
            {
                bool logging;
                if (bool.TryParse(value.Logging, out logging))
                {
                    if (Program.LoggingEnabled != logging)
                    {
                        if (!logging)
                            Console.WriteLine("Logging disabled by Api command.");
                        Program.LoggingEnabled = logging;
                        if (logging)
                            Console.WriteLine("Logging enabled by Api command.");
                    }

                }
            }
            return new { status = "ok" };
        }

        //// PUT api/Control/{value}
        //public void Put(string id, NmcConfigJson value)
        //{
        //        if (!string.IsNullOrWhiteSpace(value.User)) NmcConfig.RpcUser = value.User;
        //        if (!string.IsNullOrWhiteSpace(value.Pass)) NmcConfig.RpcPass = value.Pass;
        //        if (!string.IsNullOrWhiteSpace(value.Port)) NmcConfig.RpcPort = value.Port;
        //}

        // DELETE api/Control/5 
        public void Delete(int id)
        {
        }
    }

}
