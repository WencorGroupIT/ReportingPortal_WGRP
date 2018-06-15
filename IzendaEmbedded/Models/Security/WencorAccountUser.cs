using Microsoft.AspNet.Identity;
using System.ComponentModel.DataAnnotations;

namespace IzendaEmbedded.Models.Security
{
    public class WencorAccountUser : IUser
    {
        public string Id { get; set; } //pk_user from User table

        [Required]
        public string UserName { get; set; } //user_name from User table

        public bool LockoutEnabled { get; set; } //returns false the default value.

        public int AccessFailedCount { get; set; } //returns 0 the default value.

        public string PasswordHash { get; set; }

		public bool SystemAdmin { get; set; }

    }
}