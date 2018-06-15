using System;

namespace IzendaEmbedded.Models
{
	public class SAMLResponse
	{
		public string DecodedSAML { get; set; }
		public string EncodedeSAML { get; set; }
		public string Audience { get; set; }
		public string SubjectNameID { get; set; }
		public string SamAccountName { get; set; }
		public string Email { get; set; }
		public string LastName { get; set; }
		public bool AuthenticationStatus { get; set; }
		public string Issuer { get; set; }
		public string Destination { get; set; }
		public string ResponseID { get; set; }
		public bool VerifiedResponse { get; set; }
		public string SignatureValue { get; set; }
		public string SignatureReferenceDigestValue { get; set; }

		public string X509Certificate { get; set; }
		public DateTime AutheticationTime { get; set; }
		public string AuthenticationSession { get; set; }
	}
}