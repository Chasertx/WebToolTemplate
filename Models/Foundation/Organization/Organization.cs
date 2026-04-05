using System;

namespace Template.Api.Models.Foundation.Organization
{
    public class Organization
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Status { get; set; }
    }
}