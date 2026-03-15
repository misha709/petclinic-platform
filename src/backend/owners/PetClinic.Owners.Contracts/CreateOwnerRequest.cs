namespace PetClinic.Owners.Contracts;

public record CreateOwnerRequest(
    string FirstName,
    string LastName,
    string Address,
    string City,
    string Telephone,
    string Email);
