namespace TrustGateAPI.Contracts;

public class GenerateEndpointTokenRequest
{
    public List<int> EndpointIds { get; set; } = new();
}
