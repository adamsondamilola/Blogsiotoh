using System;
using Blogsiotoh.Models;

namespace Blogsiotoh.Services
{
	
        public interface IAuthServices
        {
        Task<Response> Signup(SignUp sign);
    }

}

