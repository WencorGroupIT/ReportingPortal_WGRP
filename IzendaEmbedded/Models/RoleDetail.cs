using System;
using System.Collections.Generic;

namespace IzendaEmbedded.Models
{
	public class RoleDetail
	{
		public Guid Id { get; set; }
		public string Name { get; set; }
		public Guid? TenantId { get; set; }
		public bool Active { get; set; }
		public bool Checked { get; set; }
		public List<UserDetail> Users { get; set; }
	}
}