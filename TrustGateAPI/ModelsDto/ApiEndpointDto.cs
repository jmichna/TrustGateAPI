namespace TrustGateAPI.ModelsDto;

public class ApiEndpointDto
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string HttpMethod { get; set; } = "";
    public string Route { get; set; } = "";

    public int ProjectId { get; set; }
    public string ProjectName { get; set; } = "";
}