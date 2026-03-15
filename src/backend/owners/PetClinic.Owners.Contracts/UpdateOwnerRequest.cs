namespace PetClinic.Owners.Contracts;

public record UpdateOwnerRequest(
    string FirstName,
    string LastName,
    string Address,
    string City,
    string Telephone,
    string Email);
