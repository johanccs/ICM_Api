using System;

namespace AECI.ICM.Application.ApplicationExceptions
{
    public class AuthException:Exception
    {
        public AuthException(object item, string message):
            base($"{item} - {message}")
        {
        }
    }
}
