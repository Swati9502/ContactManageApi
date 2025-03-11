using ContactManagementApi.Interfaces;
using ContactManagementApi.OutputDirectory;

namespace ContactManagementApi.Repository
{

    public class ContactRepository : IContactRepository
    {
        private readonly ContactDbContext _context;
 
        public ContactRepository(ContactDbContext context)
        {
            _context = context;
        }

         public void AddContact(Contact contact)
        {
            _context.Contacts.Add(contact);
            _context.SaveChanges();
        }
 
        public void UpdateContact(Contact contact)
        {
            _context.Contacts.Update(contact);
            _context.SaveChanges();
        }
 
        public void DeleteContact(int contactId)
        {
            // _context.Contacts.Remove(userId);
            // _context.SaveChanges();
            //DeleteContact(contactId);
            var contact= _context.Contacts.FirstOrDefault(c => c.ContactId == contactId );
            if(contact==null)
            {
                throw new Exception("Contact not found");
            }
            _context.Contacts.Remove(contact);
             _context.SaveChanges();
        }
         public IEnumerable<Contact> GetAllContacts(string? search = null)
        {
            var query = _context.Contacts.Where(c => c.Name == search);
 
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(c => c.Name.Contains(search) || c.Email.Contains(search) );
            }
 
            return query.ToList();
        }
 
        public Contact? GetContactById(int contactId)
        {
            return _context.Contacts.FirstOrDefault(c => c.ContactId == contactId );
        }
        public bool ContactExists(string email)
        {
            return _context.Contacts.Any(c => c.Email == email );
        }
        public Contact? GetContactByEmail(string email)
        {
            return _context.Contacts.FirstOrDefault(c => c.Email == email);
        }
        public IEnumerable<Contact> GetContactbyUserId(int userId)
        {
            return _context.Contacts.Where(c => c.UserId == userId).ToList();
        }
        
        
    }
}