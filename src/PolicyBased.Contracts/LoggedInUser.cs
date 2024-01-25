using System;
namespace PolicyBased.Contracts
{
	public class LoggedInUser
	{
		public string Sub { get; set; }
		public string Name { get; set; }
		public int TenantId { get; set; }
		public string Email { get; set; }
	}
}