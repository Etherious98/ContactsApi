using ContactsApi.Data;
using ContactsApi.Data.Interfaces;
using ContactsApi.Models;
using Microsoft.EntityFrameworkCore;

namespace ContactsApi.Services.Contacts
{
    public class InMemoryContactRepository : ITaskRepositoryContact
    {
        private readonly ContactsAPIDbContext dbContext;
        public InMemoryContactRepository(ContactsAPIDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Contact?> GetByEmail(string email)
        {
            var contact = await dbContext.Contacts.ToListAsync();
            return contact.Find(x => x.Email == email);
        }

        public async Task<Contact?> GetByPhone(string phone)
        {
            var contact = await dbContext.Contacts.ToListAsync();
            return contact.Find(x => x.PhoneNumber == phone);
        }

        public async Task<IEnumerable<Contact>> GetAll()
        {
            return await dbContext.Contacts.ToListAsync();
        }
        
        public async Task<IEnumerable<Contact>> GetAllByAddress(string address)
        {

            var contactList = await dbContext.Contacts.ToListAsync();
            return contactList.Where(c => c.Address.Contains(address));
        }
        public async Task Add(Contact contact)
        {
            try
            {
                await dbContext.Contacts.AddAsync(contact);
                await dbContext.SaveChangesAsync();
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> Update(Contact contact)
        {
            try
            {
                var contactdb = await GetByEmail(contact.Email);
                if (contactdb != null)
                {
                    contactdb.Name = contact.Name;
                    contactdb.PhoneNumber = contact.PhoneNumber;
                    contactdb.Address = contact.Address;
                    contactdb.BirthDate = contact.BirthDate;
                    contactdb.Company = contact.Company;
                    contactdb.ProfileImage = contact.ProfileImage;
                    await dbContext.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> Delete(string taskId)
        {
            try
            {
                var contact = await GetByEmail(taskId);
                if (contact != null)
                {
                    dbContext.Remove(contact);
                    dbContext.SaveChanges();
                    return true;
                }
                return false;
            }
            catch
            {
                throw;
            }
        }
    }
}
