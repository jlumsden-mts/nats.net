﻿// Copyright 2015 Apcera Inc. All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace NATS.Client
{
    /// <summary>
    /// Represents an encoded connection to the NATS server.
    /// </summary>
    public interface IEncodedConnection : IDisposable
    {
        /// <summary>
        /// Returns the options used to create this connection.
        /// </summary>
        Options Opts { get; }

        /// <summary>
        /// Returns the url of the server currently connected, null otherwise.
        /// </summary>
        string ConnectedUrl { get; }

        /// <summary>
        /// Returns the id of the server currently connected.
        /// </summary>
        string ConnectedId { get; }

        /// <summary>
        /// Servers returns the list of potential servers, including those updated after 
        /// a connection has been made.
        /// </summary>
        string[] Servers { get; }

        /// <summary>
        /// LastError reports the last error encountered via the Connection.
        /// </summary>
        Exception LastError { get; }

        /// <summary>
        /// Publish serializes and publishes an object to the given subject. The object
        /// argument is left untouched and needs to be correctly interpreted on
        /// the receiver.
        /// </summary>
        /// <param name="subject">Subject to publish the message to.</param>
        /// <param name="obj">The object to send</param>
        void Publish(string subject, object obj);

        /// <summary>
        /// Publish will perform a Publish() expecting a response on the
        /// reply subject. Use Request() for automatically waiting for a response
        /// inline.
        /// </summary>
        /// <param name="subject">Subject to publish on</param>
        /// <param name="reply">Subject the receiver will on.</param>
        /// <param name="obj">The object to send</param>
        void Publish(string subject, string reply, object obj);

        /// <summary>
        /// Request will create an Inbox and perform a Request() call
        /// with the Inbox reply and return the first reply received.
        /// This is optimized for the case of multiple responses.
        /// </summary>
        /// <remarks>
        /// A negative timeout blocks forever, zero is not allowed.
        /// </remarks>
        /// <param name="subject">Subject to send the request on.</param>
        /// <param name="obj">The object to send</param>
        /// <param name="timeout">time to block</param>
        /// <returns>An object from the replier.</returns>
        object Request(string subject, object obj, int timeout);

        /// <summary>
        /// Request will create an Inbox and perform a Request() call
        /// with the Inbox reply and return the first reply received.
        /// This is optimized for the case of multiple responses.
        /// </summary>
        /// <param name="subject">Subject to send the request on.</param>
        /// <param name="obj">The object to send</param>
        /// <returns>An object from the replier.</returns>
        object Request(string subject, object obj);

        /// <summary>
        /// NewInbox will return an inbox string which can be used for directed replies from
        /// subscribers. These are guaranteed to be unique, but can be shared and subscribed
        /// to by others.
        /// </summary>
        /// <returns>A string representing an inbox.</returns>
        string NewInbox();

        /// <summary>
        /// SubscribeAsync will create an AsyncSubscriber with
        /// interest in a given subject, assign the handler, and immediately
        /// start receiving messages.
        /// </summary>
        /// <param name="subject">Subject of interest.</param>
        /// <param name="handler">An encoded message handler delegate.</param>
        /// <returns>A new Subscription</returns>
        IAsyncSubscription SubscribeAsync(string subject, EventHandler<EncodedMessageEventArgs> handler);

        /// <summary>
        /// This method creates an asynchronous queue subscriber on the given subject.
        /// All subscribers with the same queue name will form the queue group and
        /// only one member of the group will be selected to receive any given
        /// message asynchronously. Start() must be called to 
        /// begin receiving messages.
        /// </summary>
        /// <param name="subject">Subject of interest</param>
        /// <param name="queue">Name of the queue group</param>
        /// <param name="handler">A message handler to initialize the MessageHandler event with.</param>
        /// <returns>A new Subscription</returns>
        IAsyncSubscription SubscribeAsync(string subject, string queue, EventHandler<EncodedMessageEventArgs> handler);

        /// <summary>
        /// Flush will perform a round trip to the server and return when it
        /// receives the internal reply.
        /// </summary>
        /// <param name="timeout">The timeout in milliseconds.</param>
        void Flush(int timeout);

        /// <summary>
        /// Flush will perform a round trip to the server and return when it
        /// receives the internal reply.
        /// </summary>
        void Flush();

        /// <summary>
        /// Close will close the connection to the server. This call will release
        /// all blocking calls, such as Flush() and NextMsg().
        /// </summary>
        void Close();

        /// <summary>
        /// Test if this connection has been closed.
        /// </summary>
        /// <returns>true if closed, false otherwise.</returns>
        bool IsClosed();

        /// <summary>
        /// Test if this connection is reconnecting.
        /// </summary>
        /// <returns>true if reconnecting, false otherwise.</returns>
        bool IsReconnecting();

        /// <summary>
        /// Gets the current state of the connection.
        /// </summary>
        ConnState State { get; }

        // Stats will return a race safe copy of connection statistics.
        /// <summary>
        /// Returns a race safe copy of connection statistics.
        /// </summary>
        IStatistics Stats { get; }

        /// <summary>
        /// Resets connection statistics.
        /// </summary>
        void ResetStats();

        /// <summary>
        /// Returns the server defined size limit that a message payload can have.
        /// </summary>
        long MaxPayload { get; }

        /// <summary>
        /// Overrides the default binary serialization.
        /// </summary>
        Serializer OnSerialize { get; set; }

        /// <summary>
        /// Overrides the default binary deserialization.
        /// </summary>
        Deserializer OnDeserialize { get; set; }
    }
}
