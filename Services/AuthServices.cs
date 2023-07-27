using System;
using Blogsiotoh.Models;
using Blogsiotoh.Utilities;
using System.Web.Helpers;
namespace Blogsiotoh.Services
{
	public class AuthServices : IAuthServices
	{
        private readonly BlogContext db;
        
        public AuthServices(BlogContext _db)
        {
            db = _db;
        }

        public async Task<Response> Signup(SignUp sign)
		{
			Response response = new Response();
            response.Status = 0;

            var obj = db.Users.Where(a => a.Email.Equals(sign.Email)).FirstOrDefault();

            if (!VerifyEmailAddress.ValidateEmail(sign.Email))
			{
				response.Message = "Invalid email address";
			}
            else if(obj != null)
            {
                response.Message = "Email address already registered";
            }
            else if (sign.FirstName.Length < 1)
            {
                response.Message = "First name cannot be empty";
            }
            else if (sign.FirstName.Length < 1)
            {
                response.Message = "Last name cannot be empty";
            }
            else if (sign.Password.Length < 6)
            {
                response.Message = "Password should be at least 6 characters long";
            }
            else if (sign.Password2.Length < 6)
            {
                response.Message = "Password should be at least 6 characters long";
            }
            else if (sign.Password != sign.Password2)
            {
                response.Message = "Password do not match";
            }
            else
            {
                User user = new User
                {
                    Email = sign.Email,
                    Password = Crypto.HashPassword(sign.Password),
                    FirstName = sign.FirstName,
                    LastName = sign.LastName,
                    DateCreated = DateTime.Now,
                    DateUpdate = DateTime.Now
                };
                //save to database
                db.Add(user);
                db.SaveChanges();
                response.Status = 1;
				response.Message = "Account Created!";
            }

            return response;
		}

        
    }
}

