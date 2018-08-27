using Acquaint.ModelContracts;
using Weborb.Service;

namespace Acquaint.Models
{
  [ExcludeProperty( "AddressString" )]
  [ExcludeProperty( "DisplayName" )]
  [ExcludeProperty( "DisplayLastNameFirst" )]
  [ExcludeProperty( "StatePostal" )]
  public class Acquaintance : IAcquaintance
  {
    [SetClientClassMemberName( "objectId" )]
    public string ObjectId { get; set; }
    public string DataPartitionId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Company { get; set; }
    public string JobTitle { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Street { get; set; }
    public string City { get; set; }
    public string PostalCode { get; set; }
    public string State { get; set; }
    public string PhotoUrl { get; set; }
    public string SmallPhotoUrl => PhotoUrl;
    public string AddressString => string.Format(
        "{0} {1} {2} {3}",
        Street,
        !string.IsNullOrWhiteSpace( City ) ? City + "," : "",
        State,
        PostalCode );

    public string DisplayName => ToString();
    public string DisplayLastNameFirst => $"{LastName}, {FirstName}";
    public string StatePostal => State + " " + PostalCode;
    public override string ToString() => $"{FirstName} {LastName}";
  }
}

