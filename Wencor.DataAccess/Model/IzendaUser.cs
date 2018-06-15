using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Wencor.DataAccess.Model
{
    [Table("IzendaUser")]
    public class IzendaUser
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public bool Active { get; set; }
        public bool Deleted { get; set; }
        public bool SystemAdmin { get; set; }

    }
}
