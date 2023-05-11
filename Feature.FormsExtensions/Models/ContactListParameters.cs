using System;

namespace Feature.FormsExtensions.Models
{
    public class ContactListParameters
    {
        public Guid FirstNameFieldId { get; set; }
        public Guid LastNameFieldId { get; set; }
        public Guid EmailFieldId { get; set; }
        public Guid ContactListId { get; set; }
    }
}