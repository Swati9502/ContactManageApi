using ContactManagementApi.OutputDirectory;

namespace ContactManagementApi.Interfaces;

public interface IContactRepository
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="search"></param>
    /// <returns></returns>
    IEnumerable<Contact> GetAllContacts(string search);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="contactId"></param>
    /// <returns></returns>
    Contact? GetContactById(int contactId);
    void AddContact(Contact contact);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="contact"></param>
    void UpdateContact(Contact contact);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="contactId"></param>
    void DeleteContact(int contactId);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="email"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    bool ContactExists(string email);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    IEnumerable<Contact> GetContactbyUserId(int userId);
    Contact? GetContactByEmail(string email);
}