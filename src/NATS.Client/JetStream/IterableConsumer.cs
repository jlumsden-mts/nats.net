﻿// Copyright 2023 The NATS Authors
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

namespace NATS.Client.JetStream
{
    /// <summary>
    /// SIMPLIFICATION IS EXPERIMENTAL AND SUBJECT TO CHANGE
    /// </summary>
    internal class IterableConsumer : MessageConsumer, IIterableConsumer
    {
        internal IterableConsumer(SubscriptionMaker subscriptionMaker, ConsumeOptions opts) 
            : base(subscriptionMaker, null, opts) {}

        public Msg NextMessage(int timeoutMillis)
        {
            try
            {
                return ((JetStreamPullSubscription)sub).NextMessage(timeoutMillis);
            }
            catch (NATSTimeoutException)
            {
                return null;
            }
            catch (NATSBadSubscriptionException)
            {
                // this happens if the consumer is stopped, since it is
                // drained/unsubscribed, so don't pass it on if it's expected
                return null;
            }
        }
    }
}
