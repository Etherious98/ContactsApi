using ContactsApi.Models;

namespace ContactsApi.Data.Interfaces
{
    public interface ITaskRepositoryContact
    {
        Task<Contact?> GetByEmail(string email);
        Task<Contact?> GetByPhone(string phone);
        Task<IEnumerable<Contact>> GetAll();
        Task Add(Contact contact);
        Task<bool> Update(Contact contact);
        Task<bool> Delete(string taskId);
    }
}
