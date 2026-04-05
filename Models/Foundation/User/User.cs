using System;

namespace Template.Api.Models.Foundation.User
{
    public class User
    {
        public Guid Id { get; set; }
        public Guid OrganizationId { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public required string Phone { get; set; }
        public required string Role { get; set; }
        public required DateTime RegistrationDate { get; set; }
        public bool IsActive { get; set; }
        public required string PasswordHash { get; set; }
    }
}