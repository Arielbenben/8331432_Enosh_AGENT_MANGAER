namespace Agents_Rest.Service
{
    public interface IJwtService
    {
        string GenerateToken(string uniqueIdentifier);
    }
}
