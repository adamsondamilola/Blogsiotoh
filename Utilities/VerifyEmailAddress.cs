using System;
namespace Blogsiotoh.Utilities
{
	public class VerifyEmailAddress
	{
        public static bool ValidateEmail(string email)
        {
            if (email != null)
            {
                if (email.Trim().EndsWith("."))
                {
                    return false;
                }
                try
                {
                    var addr = new System.Net.Mail.MailAddress(email);
                    return addr.Address == email;
                }
                catch
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }
}

