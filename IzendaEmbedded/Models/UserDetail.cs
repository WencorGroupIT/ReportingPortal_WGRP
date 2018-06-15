using IzendaEmbedded.Controllers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace IzendaEmbedded.Models
{
    public class UserDetail
    {
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Please select at least one role.")]
        public List<string> RoleIds { get; set; }
        public string DomainName { get; set; }

        public string Name { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string EmailAddress { get; set; }
        public string PhysicalDeliveryOfficeName { get; set; }
        public string TenantDisplayId { get; set; }
        public Boolean InitPassword { get; set; }
        public Boolean Active { get; set; }
        public Boolean SystemAdmin { get; set; }
        public IList<RoleDetail> Roles { get; set; }
		public MultiSelectList RoleOptions { get; set; }

    }
}