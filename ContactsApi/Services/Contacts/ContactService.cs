using ContactsApi.Data;
using ContactsApi.Models;
using Microsoft.EntityFrameworkCore;

namespace ContactsApi.Services.Contacts
{
    public class ContactService
    {
        private readonly ContactsAPIDbContext dbContext;
        public ContactService(ContactsAPIDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task AddContact(Contact contact)
        {
            await dbContext.Contacts.AddAsync(contact);
            await dbContext.SaveChangesAsync();
        }

        public async Task<Contact> FindContact(string email)
        {
            var contact = await dbContext.Contacts.FindAsync(email);
            return contact;
        }

        public async Task<List<Contact>> GetContacts()
        {
            return await dbContext.Contacts.ToListAsync();
        }

        public async Task<bool> UpdateContact( Contact contact)
        {
            var contactdb = await FindContact(contact.Email);
            if(contactdb != null)
            {
                contactdb.Name = contact.Name;
                contactdb.PhoneNumber = contact.PhoneNumber;
                contactdb.PhoneNumberWork = contact.PhoneNumberWork;
                contactdb.Address = contact.Address;
                contactdb.Email = contact.Email;
                contactdb.BirthDate = contact.BirthDate;
                contactdb.Company = contact.Company;
                await dbContext.SaveChangesAsync();
                return true;
            }
            return false;
        }
        public async Task<bool> DeleteContact(string email)
        {
            var contact = await FindContact(email);
            if (contact != null)
            {
                dbContext.Remove(contact);
                dbContext.SaveChanges();
                return true;
            }
            return false;
        }
    }
}
