using Grpc.Core;
using GrpcServer;
using GrpcService;

namespace GrpcService.Services;

public class GreeterService : GreeterProtoService.GreeterProtoServiceBase
{
    private readonly ILogger<GreeterService> _logger;

    public GreeterService(ILogger<GreeterService> logger)
    {
        _logger = logger;
    }

    public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
    {
        Console.WriteLine("Received request");
        return Task.FromResult(new HelloReply
        {
            Message = "Hello " + request.Name
        });
    }
}