﻿using ARSoft.Tools.Net.Dns;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace dotBitNS.Server
{
    class NameServer
    {
        private static DnsServer server;

        [CallPriority(MemberPriority.Normal)]
        public static void Initialize()
        {
            Console.Write("Starting Nameserver.... ");
            server = new DnsServer(IPAddress.Any, 10, 10, ProcessQuery);
            server.Start();
            Console.WriteLine("Done.");
        }

        static DnsMessageBase ProcessQuery(DnsMessageBase message, IPAddress clientAddress, ProtocolType protocol)
        {
            message.IsQuery = false;

            DnsMessage query = message as DnsMessage;

            if (query != null)
            {
                if (query.Questions.Count == 1)
                {
                    DnsQuestion question = query.Questions[0];
          
                    Console.WriteLine("Query: {0} {1} {2}", question.Name, question.RecordType, question.RecordClass);

                    DnsMessage answer=null;

                    if(question.Name.EndsWith(".bit"))
                    {
                        var info = NmcClient.Instance.LookupHost(question.Name);
                        if (info != null)
                        {
                            answer = new DnsMessage()
                            {
                                Questions = query.Questions
                            };

                            // TODO: Make real and complete responses
                            IPAddress address;
                            var value = info.GetValue();
                            if(IPAddress.TryParse(value.ip,out address))
                            answer.AnswerRecords.Add(new ARecord(question.Name,60,address));
                        
                        }
                    }

                    if (answer == null)
                    {
                        // send query to upstream server
                        answer = DnsClient.Default.Resolve(question.Name, question.RecordType, question.RecordClass);
                    }

                    // if got an answer, copy it to the message sent to the client
                    if (answer != null)
                    {
                        foreach (DnsRecordBase record in (answer.AnswerRecords))
                        {
                            Console.WriteLine("Answer: {0}", record);
                            query.AnswerRecords.Add(record);
                        }
                        foreach (DnsRecordBase record in (answer.AdditionalRecords))
                        {
                            Console.WriteLine("Additional Answer: {0}", record);
                            query.AnswerRecords.Add(record);
                        }

                        query.ReturnCode = ReturnCode.NoError;
                        return query;
                    }
                }
                else
                    Debug.WriteLine("Too many questions ({0})", query.Questions.Count);
            }
            // Not a valid query or upstream server did not answer correct
            message.ReturnCode = ReturnCode.ServerFailure;
            return message;
        }


    }
}