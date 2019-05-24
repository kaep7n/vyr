﻿using Google.Protobuf.WellKnownTypes;
using PublishAndSubcribe;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using static PublishAndSubcribe.PubSub;

namespace Vyr.Playground.Grpc
{
    partial class Program
    {
        public class Subscriber
        {
            private readonly PubSubClient pubSubClient;
            private Subscription subscription;

            public Subscriber(PubSubClient pubSubClient)
            {
                this.pubSubClient = pubSubClient;
            }

            public event EventHandler<EventArgs> MessageReceived;

            private void OnMessageReceived()
            {
                this.MessageReceived?.Invoke(this, new EventArgs());
            }

            public void Subscribe(params string[] topics)
            {
                this.subscription = new Subscription
                {
                    ClientId = Guid.NewGuid().ToString()
                };

                foreach (var topic in topics)
                {
                    this.subscription.Topics.Add(topic);
                }

                this.pubSubClient.Subscribe(this.subscription);
            }

            public void Publish(string topic)
            {
                var configChanged = new ConfigChanged();
                configChanged.Id = Guid.NewGuid().ToString();
                configChanged.Type = "change";

                this.pubSubClient.Publish(new Event { Topic = topic, Content = Any.Pack(configChanged) });
            }

            public async Task AttachAsync()
            {
                using var call = this.pubSubClient.Attach(this.subscription);

                var responseStream = call.ResponseStream;

                Console.WriteLine("Receiving Messages");

                while(await responseStream.MoveNext())
                {
                    var @event = responseStream.Current;
                    this.OnMessageReceived();
                }
            }

            public void Unsubscribe()
            {
                this.pubSubClient.Unsubscribe(this.subscription);
            }
        }
    }
}