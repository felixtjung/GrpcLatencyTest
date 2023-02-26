// See https://aka.ms/new-console-template for more information

using Grpc.Net.Client;

using (var channel = GrpcChannel.ForAddress("http://localhost:9300"))
{
    var client = new Greeter.GreeterClient(channel);
    var firstTimeLatencyMsecs = await GetLatencyMsecsAsync(client);

    var msecLatencyList = new List<double>();
    for (int i = 0; i < 100; i++)
    {
        var latencyMsecs = await GetLatencyMsecsAsync(client);
        msecLatencyList.Add(latencyMsecs);
    }

    Console.WriteLine($"FirstTimeLatency: {firstTimeLatencyMsecs}msecs");
    Console.WriteLine($"Average 100 Latency: {msecLatencyList.Average()}msecs");
}

async Task<double> GetLatencyMsecsAsync(Greeter.GreeterClient client)
{
    var reply = await client.SayHelloAsync(new HelloRequest() { Name = DateTimeOffset.Now.ToFileTime().ToString() });
    var sendTime = DateTimeOffset.FromFileTime(long.Parse(reply.Message));
    return (DateTimeOffset.Now - sendTime).TotalMilliseconds;
}
