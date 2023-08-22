using ContactsApi.Models;
using System.Runtime.CompilerServices;

namespace ContactsApi.Data.Interfaces
{
    public interface ITaskRepositoryContact
    {
        Task<Contact?> GetByEmail(string email);
        Task<Contact?> GetByPhone(string phone);
        Task<IEnumerable<Contact>> GetAll();
        Task<IEnumerable<Contact>> GetAllByAddress(string address);
        Task Add(Contact contact);
        Task<bool> Update(Contact contact);
        Task<bool> Delete(string taskId);
    }
}
