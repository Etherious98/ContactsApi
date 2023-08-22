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

        /// <summary>
        /// Busca un usuario por su dirección de correo electrónico o número telefónico.
        /// </summary>
        /// <param name="search">Dirección de correo electrónico o número telefónico del usuario.</param>
        /// <returns>El usuario encontrado o NotFound si no se encuentra.</returns>
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

        /// <summary>
        /// obtiene todos los contactos existentes.
        /// </summary>
        /// <returns>Una lista de todos los contactos encontrados.</returns>
        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetContacts()
        {
            var contacts = await _repositoryContact.GetAll();
            if(contacts.Any())
            {
                return Ok(contacts);
            }
            return NoContent();
        }

        /// <summary>
        /// obtiene todos los contactos existentes que coincida con la dirección.
        /// </summary>
        /// <param name="address">Dirección del usuario.</param>
        /// <returns>Una lista de todos los contactos encontrados.</returns>
        [HttpGet]
        [Route("GetAll/{address}")]
        public async Task<IActionResult> GetContactsByAddress([FromRoute] string address)
        {
            var contacts = await _repositoryContact.GetAllByAddress(address);
            if (contacts.Any())
            {
                return Ok(contacts);
            }
            return NoContent();
        }

        // <summary>
        /// Crea un nuevo contacto.
        /// </summary>
        /// <param name="addContactRequest">Datos del nuevo contacto.</param>
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

        // <summary>
        /// Actualiza un contacto existente.
        /// </summary>
        /// <param name="addContactRequest">Datos del contacto.</param>
        [HttpPut]
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
        /// <summary>
        /// Elimina un contacto existente relacionado al email ingresado
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{email}")]
        public async Task<IActionResult> DeleteContact([FromRoute] string email)
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
