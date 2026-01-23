namespace TrustGateAPI.ModelsDto;

public class CreateApiTokenRequest
{
    public int ValidDays { get; set; } = 30;
}
