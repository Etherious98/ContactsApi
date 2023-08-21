using ContactsApi.Data;
using ContactsApi.Data.Interfaces;
using ContactsApi.Models;
using ContactsApi.Services.Contacts;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace ContactsApi.Controllers.Contacts
{
    [ApiController]
    [Route("[controller]")]
    public class ContactsController : ControllerBase
    {
        private readonly ITaskRepositoryContact _repositoryContact;
        public ContactsController(ITaskRepositoryContact taskRepositoryContact) 
        {
            _repositoryContact = taskRepositoryContact;
        }

        [HttpGet]
        public async Task<IActionResult> GetContacts()
        {
            var contacts = await _repositoryContact.GetAll();
            if(contacts.Any())
            {
                return Ok(contacts);
            }
            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> AddContact(Contact addContactRequest)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _repositoryContact.Add(addContactRequest);
                    return Ok();
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }

        [HttpGet]
        [Route("{search}")]
        public async Task<IActionResult> GetContact([FromRoute] string search)
        {
            if (!string.IsNullOrEmpty(search))
            {
                if (IsEmail(search))
                {
                    var contact = await _repositoryContact.GetByEmail(search);
                    if (contact != null) return Ok(contact);
                }
                if (IsPhoneNumber(search))
                {
                    var contact = await _repositoryContact.GetByPhone(search);
                    if (contact != null) return Ok(contact);
                }
            }
            return NotFound();
        }

        [HttpPut]
        [Route("{email}")]
        public async Task<IActionResult> UpdateContact(Contact updateContactRequest)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (await _repositoryContact.Update(updateContactRequest))
                    {
                        return Ok();
                    }
                    return NotFound();
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }

        [HttpDelete]
        [Route("{email}")]
        public async Task<IActionResult> DeleteContact(string email)
        {
            try
            {
                var task = await _repositoryContact.Delete(email);
                if (task)
                {
                    return Ok();
                }
                return NotFound();
            }
            catch(Exception ex) 
            {
                return BadRequest(ex.Message);
            }
        }

        private static bool IsEmail(string input)
        {
            // Utilizamos una expresión regular para validar el formato de email
            string emailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            return Regex.IsMatch(input, emailPattern);
        }

        private static bool IsPhoneNumber(string input)
        {
            // Utilizamos una expresión regular para validar el formato de número de teléfono
            // Este es un ejemplo simple y puede no cubrir todos los formatos posibles
            string phonePattern = @"^\d{10}$"; // Suponiendo un formato de 10 dígitos
            return Regex.IsMatch(input, phonePattern);
        }
    }
}
