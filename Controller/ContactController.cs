using ContactManagementApi.Dtos;
using ContactManagementApi.Interfaces;
using ContactManagementApi.OutputDirectory;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// Code formatting
namespace ContactManagementApi.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class ContactController : ControllerBase
    {
        private readonly IContactRepository _contactRepository;

        public ContactController(IContactRepository contactRepository)
        {
            _contactRepository = contactRepository;
        }
        /// <summary>
        /// Here by using userid and contactdto we can create a contact in database
        /// </summary>
        /// <param name="contactDto"></param>
        /// <param name="userId"></param>
        /// <returns>CreateContact</returns>
        // POST: api/contacts
        [Authorize(Roles = "Admin")]
        [HttpPost("AddContact")]
        public IActionResult AddContact(ContactDto contactDto, [FromQuery] int userId)
        {
            //Console.WriteLine(contactDto);
            // Ensure userId is passed in the query parameters
            if (userId == 0)
                return BadRequest("User ID is required.");
            if (contactDto == null)
                return BadRequest("null reference");

            if (_contactRepository.ContactExists(contactDto.Email))
                return BadRequest("A contact with this email already exists.");

            var contact = new Contact
            {
                Name = contactDto.Name,
                Email = contactDto.Email,
                Phone = contactDto.Phone,
                UserId = userId,
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = DateTime.UtcNow
            };

            _contactRepository.AddContact(contact);
            return Ok(contact);
        }
        /// <summary>
        /// Here by using Contactid,userid and information from contactdto we can modify the information of a contact
        /// </summary>
        /// <param name="id"></param>
        /// <param name="contactDto"></param>
        /// <param name="userId"></param>
        /// <returns>Update Contact</returns>
        // PUT: api/contacts/{id}
        [Authorize(Roles = "Admin")]
        [HttpPut("UpdateContact")]
        public IActionResult UpdateContact(int contactId, ContactDto contactDto)
        {
            // Ensure userId is passed in the query parameters
            // if (userId == 0)
            //     return BadRequest("User ID is required.");

            var contact = _contactRepository.GetContactById(contactId);
            if (contact == null)
                return NotFound("Contact not found or you are not authorized to update it.");
            var existingContact = _contactRepository.GetContactByEmail(contactDto.Email);
            if (existingContact != null && existingContact.ContactId != contactId)
                return BadRequest("A contact with this email already exists.");
            contact.Name = contactDto.Name;
            contact.Email = contactDto.Email;
            contact.Phone = contactDto.Phone;
            contact.ModifiedDate = DateTime.UtcNow;

            _contactRepository.UpdateContact(contact);
            return Ok(contact);
        }
        /// <summary>
        /// Here with the help of Contactid and userid we can delete a contact from database
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userId"></param>
        /// <returns>DeleteContact</returns>
        // DELETE: api/contacts/{id}
        [Authorize(Roles = "Admin")]
        [HttpDelete("DeleteContact/{contactId}")]
        public IActionResult DeleteContact(int contactId)
        {
            // Ensure userId is passed in the query parameters
            if (contactId == 0)
                return BadRequest("Contact ID is required.");

            var contact = _contactRepository.GetContactById(contactId);
            if (contact == null)
                return NotFound("Contact not found or you are not authorized to delete it.");

            _contactRepository.DeleteContact(contactId);
            return Ok(new { message = "Contact deleted successfully." });

        }

        // GET: api/contacts
        /// <summary>
        /// Here with the help of userId and some part of the query we can able to find the details of a contact that is related to that userid
        /// </summary>
        /// <param name="search"></param>
        /// <returns>Contact Information</returns>
        // GET: api/contacts
        [Authorize(Roles = "User,Admin")]
        [HttpGet("GetContact")]
        public IActionResult GetAllContacts([FromQuery] string? search = null)
        {
            // Ensure userId is passed in the query parameters
            // if (userId == 0)
            //     return BadRequest("User ID is required.");
            var searchValue = search ?? string.Empty;
            var contacts = _contactRepository.GetAllContacts(searchValue);
            return Ok(contacts);
        }
        /// <summary>
        /// Here id refers to ContactId and with the both contactid and userid we can able to find the contact information
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userId"></param>
        /// <returns>ContactInformation</returns>
        // GET: api/contacts/{id}
        [Authorize(Roles = "User,Admin")]
        [HttpGet("GetContactbyId")]
        public IActionResult GetContactById(int id)
        {
            // Ensure userId is passed in the query parameters
            // if (userId == 0)
            //     return BadRequest("User ID is required.");

            var contact = _contactRepository.GetContactById(id);
            if (contact == null)
                return NotFound("Contact not found or you are not authorized to view it.");

            return Ok(contact);
        }
        [Authorize(Roles = "User,Admin")]
        [HttpGet("GetContactbyUserId/{userId}")]
        public IActionResult GetContactbyUserId([FromRoute] int userId)
        {
            
            if (userId == 0)
                return BadRequest("User ID is required.");

            var contact = _contactRepository.GetContactbyUserId(userId);
            if (contact == null)
                return NotFound("Contact not found or you are not authorized to view it.");

            return Ok(contact);
        }
    }
}