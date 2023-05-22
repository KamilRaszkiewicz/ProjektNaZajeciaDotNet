namespace Projekt.Interfaces
{
    public interface IAdminService
    {
        Task<bool> DeleteUserAsync(int userId);
    }
}
