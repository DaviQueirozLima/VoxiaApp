namespace Voxia.Domain.HttpContext
{
    public interface IUserContext
    {
        Guid GetCurrentUserId();
    }
}
