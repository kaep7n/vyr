using PubSub;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Vyr.Playground.Grpc
{
    partial class Program
    {
        public class Subscriber
        {
            private readonly Broker.BrokerClient pubSubClient;
            private Subscription subscription;

            public Subscriber(Broker.BrokerClient pubSubClient)
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
                //this.pubSubClient.Publish(new Message { Topic = topic, Content = Any.Pack(configChanged) });
            }

            public async Task AttachAsync()
            {
                using var call = this.pubSubClient.Attach(this.subscription);

                var responseStream = call.ResponseStream;

                while (await responseStream.MoveNext())
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
