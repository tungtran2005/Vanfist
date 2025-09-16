using Vanfist.Entities;

namespace Vanfist.DTOs.Responses;

public class AccountResponse
{
    public int Id { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Number { get; set; }
    public List<Address> Addresses { get; set; }

    public AccountResponse(int id, string email, string firstName, string lastName, string number, List<Address> addresses)
    {
        Id = id;
        Email = email;
        FirstName = firstName;
        LastName = lastName;
        Number = number;
        Addresses = addresses;
    }

    public Address? GetDefaultAddress()
    {
        foreach (Address address in Addresses)
        {
            if (address.IsDefault)
            {
                return address;
            }
        }
        return null;
    }

    public static AccountResponse From(Account account)
    {
        List<Address> addresses = account.Addresses.ToList();
        return new AccountResponse(
            account.Id,
            account.Email,
            account.FirstName ?? string.Empty,
            account.LastName ?? string.Empty,
            account.Number ?? string.Empty,
            addresses
        );
    }

    public override string ToString()
    {
        return $"\nAccountResponse {{" +
               $"\n\tId: {Id}, " +
               $"\n\tEmail: {Email}, " +
               $"\n\tFirstName: {FirstName}, " +
               $"\n\tLastName: {LastName}, " +
               $"\n\tNumber: {Number}" +
               $"\n}}";
    }
}