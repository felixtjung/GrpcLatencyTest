// See https://aka.ms/new-console-template for more information

using Grpc.Net.Client;
using MathNet.Numerics.Statistics;

var grpcServerUrl = Environment.GetEnvironmentVariable("GRPC_SERVER_URL") ?? "http://localhost:9300";
Console.WriteLine($"Using GRPC Server Url : {grpcServerUrl}");

using (var channel = GrpcChannel.ForAddress(grpcServerUrl))
{
    var client = new Greeter.GreeterClient(channel);
    var firstTimeLatencyMsecs = await GetLatencyMsecsAsync(client);

    var msecLatencyList = new List<double>();
    for (int i = 0; i < 1000; i++)
    {
        var latencyMsecs = await GetLatencyMsecsAsync(client);
        msecLatencyList.Add(latencyMsecs);
    }

    var stdev = Statistics.StandardDeviation(msecLatencyList);
    var pct90 = Statistics.Percentile(msecLatencyList, 90);
    var pct95 = Statistics.Percentile(msecLatencyList, 95);
    var pct99 = Statistics.Percentile(msecLatencyList, 99);

    Console.WriteLine($"FirstTimeLatency: {firstTimeLatencyMsecs}msecs");
    Console.WriteLine($"Average 1000 Requests Latency: {msecLatencyList.Average()}msecs");
    Console.WriteLine($"Stdev 1000 Requests Latency: {stdev}msecs");
    Console.WriteLine($"Pct-90 1000 Requests Latency: {pct90}msecs");
    Console.WriteLine($"Pct-95 1000 Requests Latency: {pct95}msecs");
    Console.WriteLine($"Pct-99 1000 Requests Latency: {pct99}msecs");
}

async Task<double> GetLatencyMsecsAsync(Greeter.GreeterClient client)
{
    var reply = await client.SayHelloAsync(new HelloRequest() { Name = DateTimeOffset.Now.ToFileTime().ToString() });
    var sendTime = DateTimeOffset.FromFileTime(long.Parse(reply.Message));
    return (DateTimeOffset.Now - sendTime).TotalMilliseconds;
}
