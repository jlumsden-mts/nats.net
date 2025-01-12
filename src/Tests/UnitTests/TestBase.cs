﻿// Copyright 2019-2023 The NATS Authors
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using System.IO;
using System.Text;
using NATS.Client;
using Xunit;
using Xunit.Abstractions;

namespace UnitTests
{
    public class TestBase
    {
        // ----------------------------------------------------------------------------------------------------
        // console debug help 
        // ----------------------------------------------------------------------------------------------------
        /*
            private readonly ITestOutputHelper output;

            public TestMyClass(ITestOutputHelper output)
            {
	            this.output = output;
	            Console.SetOut(new ConsoleWriter(output));
            }
        */

        public class ConsoleWriter : StringWriter
        {
            private ITestOutputHelper output;

            public ConsoleWriter(ITestOutputHelper output)
            {
                this.output = output;
            }

            public override void WriteLine(string m)
            {
                output.WriteLine(m);
            }
        }

        // ----------------------------------------------------------------------------------------------------
        // unit test 
        // ----------------------------------------------------------------------------------------------------
        public const string Plain = "plain";
        public const string HasSpace = "has space";
        public const string HasPrintable = "has-print!able";
        public const string HasDot = "has.dot";
        public const string HasStar = "has*star";
        public const string HasGt = "has>gt";
        public const string HasDash = "has-dash";
        public const string HasUnder = "has_under";
        public const string HasDollar = "has$dollar";
        public const string HasLow = "has\tlower\rthan\nspace";
        public const string HasFwdSlash = "has/fwd/slash";
        public const string HasBackSlash = "has\\back\\slash";
        public const string HasEquals = "has=equals";
        public const string HasTic = "has`tic";
        public static readonly string Has127 = "has" + (char)127 + "127";

        public static string ReadDataFile(string name)
        {
            return File.ReadAllText(FileSpec(name));
        }

        public static string[] ReadDataFileLines(string name)
        {
            return File.ReadAllLines(FileSpec(name));
        }

        public static string FileSpec(string name)
        {
            string path = Directory.GetCurrentDirectory();
            return Path.Combine(path, "..", "..", "..", "Data", name);
        }

        public static DateTime AsDateTime(string dtString)
        {
            return DateTime.Parse(dtString).ToUniversalTime();
        }

        public static string TempConfFile()
        {
            return Path.GetTempPath() + "nats_net_test" + Guid.NewGuid() + ".conf";
        }

        public string[] SplitLines(String text)
        {
            return text.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
        }

        // ----------------------------------------------------------------------------------------------------
        // data makers
        // ----------------------------------------------------------------------------------------------------
        public const string STREAM = "stream";
        public const string MIRROR = "mirror";
        public const string SOURCE = "source";
        public const string SUBJECT = "subject";
        public const string SUBJECT_STAR = SUBJECT + ".*";
        public const string SUBJECT_GT = SUBJECT + ".>";
        public const string QUEUE = "queue";
        public const string DURABLE = "durable";
        public const string NAME = "name";
        public const string PUSH_DURABLE = "push-" + DURABLE;
        public const string PULL_DURABLE = "pull-" + DURABLE;
        public const string DELIVER = "deliver";
        public const string MESSAGE_ID = "mid";
        public const string BUCKET = "bucket";
        public const string KEY = "key";
        public const string DATA = "data";
        
        public static string Stream(object seq)
        {
            return $"{STREAM}-{seq}";
        }

        public static string Field(String field, object seq)
        {
            return $"{field}-{seq}";
        }

        public static string Subject(object seq)
        {
            return $"{SUBJECT}-{seq}";
        }

        public static string SubjectDot(string afterDot)
        {
            return $"{SUBJECT}.{afterDot}";
        }

        public static string Queue(object seq)
        {
            return $"{QUEUE}-{seq}";
        }

        public static string Durable(object seq)
        {
            return $"{DURABLE}-{seq}";
        }

        public static string Durable(string vary, object seq)
        {
            return $"{DURABLE}-{vary}-{seq}";
        }

        public static string Name(object seq)
        {
            return $"{NAME}-{seq}";
        }

        public static string Deliver(object seq)
        {
            return $"{DELIVER}-{seq}";
        }

        public static string Bucket(object seq)
        {
            return $"{BUCKET}-{seq}";
        }

        public static string Key(object seq)
        {
            return $"{KEY}-{seq}";
        }

        public static string Mrrr(object seq)
        {
            return $"{MIRROR}-{seq}";
        }

        public static string Src(object seq)
        {
            return $"{SOURCE}-{seq}";
        }

        public static string MessageId(object seq)
        {
            return $"{MESSAGE_ID}-{seq}";
        }

        public static string Data(object seq)
        {
            return $"{DATA}-{seq}";
        }

        public static byte[] DataBytes()
        {
            return Encoding.ASCII.GetBytes(DATA);
        }

        public static byte[] DataBytes(object seq)
        {
            return Encoding.ASCII.GetBytes(Data(seq));
        }
 
        public static void AssertClientError(ClientExDetail ced, Action testCode)
        {
            NATSJetStreamClientException e = Assert.Throws<NATSJetStreamClientException>(testCode);
            Assert.Contains(ced.Id, e.Message);
        }
    }
}
