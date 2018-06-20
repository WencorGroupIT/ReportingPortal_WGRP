using IzendaEmbedded.IzendaBoundary;
using IzendaEmbedded.Models;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.IdentityModel.Claims;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using Microsoft.AspNet.Identity;
using System.Net;
using System.Security.Claims;

namespace IzendaEmbedded.Controllers
{
	
	[Authorize]
	public class AccountController : Controller
	{
		private SAMLResponse _samlResponse;

		private ApplicationSignInManager _signInManager;
		private static ApplicationUserManager _userManager;

		public ApplicationUserManager UserManager
		{
			get => _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
			private set => _userManager = value;
		}

		public ApplicationSignInManager SignInManager
		{
			get => _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
			private set => _signInManager = value;
		}

		private IAuthenticationManager AuthenticationManager => HttpContext.GetOwinContext().Authentication;

		public void LogOff()

		{
			var callbackUrl = Url.Action("Index", "Home", null, Request.Url.Scheme);

			HttpContext.GetOwinContext().Authentication.SignOut(
				new AuthenticationProperties {RedirectUri = callbackUrl});
		}
		[AllowAnonymous]
		public ActionResult LoginExternal(string username)
		{
            return Redirect(ConfigurationManager.AppSettings["WencorADFSLink"]);
		}

		[HttpPost]
		[AllowAnonymous]
		[Route("api/wencorAuth")]
		public async Task<ActionResult> WencorAuth()
		{
			string userName = null;
			var message = string.Empty;

				var result = await ValidateSamlRequest(out userName);

				if (!result)
				{
				}

			var model = new LoginModel
			{
				Username = userName,
				WencorUser = true
			};
			
			
			var wencorUser = await UserManager.FindByNameAsync(model.Username);


			if (wencorUser != null)
			{

				await SignInManager.SignInAsync(wencorUser, true, false).ConfigureAwait(false);
				return RedirectToAction("Index", "Home");
			}

			return Redirect("~/Home/NoAccessToIzenda");
		}

		private Task<bool> ValidateSamlRequest(out string userName)
		{
			bool valid = false;
			userName = string.Empty;

			_samlResponse = GetAndValidateSamlResponse().Result;

			if (_samlResponse != null)
			{
				valid = true;
			}

			if (_samlResponse == null)
			{
				throw new Exception("SAML Response Invalid");
			}


			userName = _samlResponse.SamAccountName;

			return Task.FromResult(valid);
		}

		private async Task<SAMLResponse> GetAndValidateSamlResponse()
		{
			var context = HttpContext;

			if (!context.Request.Form["SAMLResponse"].Any())
				throw new Exception("Request Form does not have SAMLResponse" + Environment.NewLine);

			_samlResponse = new SAMLResponse();
			var xDoc = new XmlDocument();

			var samlResponseString = context.Request.Form["SAMLResponse"];
			if (samlResponseString.Contains('%')) samlResponseString = HttpUtility.UrlDecode(samlResponseString);
			
			var fromEncode64 = Encoding.UTF8.GetString(
				Convert.FromBase64String(samlResponseString));

			if (!fromEncode64.StartsWith(@"<?xml version="))
				fromEncode64 = Decode64Bit("PD94bWwgdmVyc2lvbj0iMS4wIiBlbmNvZGluZz0iVVRGLTgiPz4=") + fromEncode64;

			var replace = fromEncode64.Replace(@"\", "");
			xDoc.LoadXml(replace);
			xDoc.PreserveWhitespace = true;

			var xMan = new XmlNamespaceManager(xDoc.NameTable);
			xMan.AddNamespace("samlp", "urn:oasis:names:tc:SAML:2.0:protocol");
			xMan.AddNamespace("saml", "urn:oasis:names:tc:SAML:2.0:assertion");
			xMan.AddNamespace("ds", "http://www.w3.org/2000/09/xmldsig#");

			var xNode = xDoc.SelectSingleNode("/samlp:Response/samlp:Status/samlp:StatusCode/@Value", xMan);
			if (xNode != null)
			{
				_samlResponse.AuthenticationStatus = false;
				var statusCode = xNode.Value;
				if (statusCode.EndsWith("status:Success")) _samlResponse.AuthenticationStatus = true;
			}

			xNode = xDoc.SelectSingleNode("/samlp:Response/@Destination", xMan);
			if (xNode != null) _samlResponse.Destination = xNode.Value;

			xNode = xDoc.SelectSingleNode("/samlp:Response/@IssueInstant", xMan);
			if (xNode != null) _samlResponse.AutheticationTime = Convert.ToDateTime(xNode.Value);

			xNode = xDoc.SelectSingleNode("/samlp:Response/@ID", xMan);
			if (xNode != null) _samlResponse.ResponseID = xNode.Value;

			xNode = xDoc.SelectSingleNode("/samlp:Response/saml:Issuer", xMan);
			if (xNode != null) _samlResponse.Issuer = xNode.InnerText;

			xNode = xDoc.SelectSingleNode(
				"/samlp:Response/saml:Assertion/ds:Signature/ds:SignedInfo/ds:Reference/ds:DigestValue", xMan);
			if (xNode != null) _samlResponse.SignatureReferenceDigestValue = xNode.InnerText;

			xNode = xDoc.SelectSingleNode("/samlp:Response/saml:Assertion/ds:Signature/ds:SignatureValue", xMan);
			if (xNode != null) _samlResponse.SignatureValue = xNode.InnerText;

			xNode = xDoc.SelectSingleNode(
				"/samlp:Response/saml:Assertion/ds:Signature/ds:KeyInfo/ds:X509Data/ds:X509Certificate", xMan);
			if (xNode != null) _samlResponse.X509Certificate = xNode.InnerText;

			xNode = xDoc.SelectSingleNode("/samlp:Response/saml:Assertion/@ID", xMan);
			if (xNode != null) _samlResponse.AuthenticationSession = xNode.Value;

			xNode = xDoc.SelectSingleNode(
				"/samlp:Response/saml:Assertion/saml:Subject/saml:SubjectConfirmation/saml:SubjectConfirmationData/@Recipient",
				xMan);
			if (xNode != null) _samlResponse.SubjectNameID = xNode.InnerText;

			xNode = xDoc.SelectSingleNode(
				"/samlp:Response/saml:Assertion/saml:Conditions/saml:AudienceRestriction/saml:Audience", xMan);
			if (xNode != null) _samlResponse.Audience = xNode.InnerText;

			xNode = xDoc.SelectSingleNode(
				"/samlp:Response/saml:Assertion/saml:AttributeStatement/saml:Attribute[@Name = 'SAM-Account-Name']/saml:AttributeValue",
				xMan);
			if (xNode != null) _samlResponse.SamAccountName = xNode.InnerText;

			if (!ValidateX509CertificateSignature(xDoc))
				throw new Exception("Validation of certificate failed." + Environment.NewLine);

			return _samlResponse;
		}

		private bool ValidateX509CertificateSignature(XmlDocument xDoc)
		{
			var XMLSignatures = xDoc.GetElementsByTagName("Signature", "http://www.w3.org/2000/09/xmldsig#");
			if (XMLSignatures.Count != 1) return false;

			var SignedSAML = new SignedXml(xDoc);

			SignedSAML.LoadXml((XmlElement) XMLSignatures[0]);

			var certPath = Server.MapPath(ConfigurationManager.AppSettings["CertificatePath"]);
			var SigningCert = new X509Certificate2(certPath);

			return SignedSAML.CheckSignature(SigningCert, true);
		}

		private static string Decode64Bit(string rawSamlData)
		{
			var samlData = Convert.FromBase64String(rawSamlData);

			// read back into a UTF string
			var samlAssertion = Encoding.UTF8.GetString(samlData);
			return samlAssertion;
		}


		public sealed class LoginModel
		{
			public string Username { get; set; }
			public string Email { get; set; }

			public string Password { get; set; }

			public bool WencorUser { get; set; }
			public bool WencorLoginFailed { get; set; }
		}
	}
}