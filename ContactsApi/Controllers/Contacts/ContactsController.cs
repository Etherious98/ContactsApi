using ContactsApi.Models;
using ContactsApi.Services.Contacts;
using Microsoft.AspNetCore.Mvc;

namespace ContactsApi.Controllers.Contacts
{
    [ApiController]
    [Route("[controller]")]
    public class ContactsController : ControllerBase
    {
        private readonly ContactService _contactService;

        public ContactsController(ContactService contactService) 
        {
            _contactService = contactService;
        }

        [HttpGet]
        public async Task<IActionResult> GetContacts()
        {
            var contacts = await _contactService.GetContacts();
            if(contacts.Any())
            {
                return Ok(contacts);
            }
            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> AddContact(Contact addContactRequest)
        {
            await _contactService.AddContact(addContactRequest);
            return Ok(addContactRequest);
        }
        [HttpGet]
        [Route("{email}")]
        public async Task<IActionResult> GetContact([FromRoute] string email)
        {
            var contact = await _contactService.FindContact(email);
            if (contact == null)
            {
                return NotFound();
            }
            return Ok(contact);
        }

        [HttpPut]
        [Route("{email}")]
        public async Task<IActionResult> UpdateContact(Contact updateContactRequest)
        {
            if (await _contactService.UpdateContact(updateContactRequest))
            {
                return Ok();
            }
            return NotFound();
        }

        [HttpDelete]
        [Route("{email}")]
        public async Task<IActionResult> DeleteContact(string email)
        {
            var task = await _contactService.DeleteContact(email);
            if (task)
            {                
                return Ok();
            }
            return NotFound();
        }
    }
}
