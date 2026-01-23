namespace TrustGateAPI.ModelsDto;

public class ApiEndpointTokenAssignmentDto
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string HttpMethod { get; set; } = "";
    public string Route { get; set; } = "";

    public bool IsAssigned { get; set; }
}
