using Google.Protobuf.WellKnownTypes;
using PublishAndSubcribe;
using System;
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

            public void Subscribe()
            {
                this.subscription = new Subscription
                {
                    ClientId = Guid.NewGuid().ToString()
                };

                this.subscription.Topics.Add("config/changed");
                this.subscription.Topics.Add("config/reset");

                this.pubSubClient.Subscribe(this.subscription);
            }

            public void Publish()
            {
                var configChanged = new ConfigChanged();
                configChanged.Id = Guid.NewGuid().ToString();
                configChanged.Type = "change";

                this.pubSubClient.Publish(new Event { Topic = "config", Content = Any.Pack(configChanged) });
            }

            public void Unsubscribe()
            {
                this.pubSubClient.Unsubscribe(this.subscription);
            }
        }
    }
}
