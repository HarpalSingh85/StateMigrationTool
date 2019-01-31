using Microsoft.Win32;
using System;
using System.Security.Principal;


namespace StateMigrationBackend.Validation
{
    static class RegistryAccessValidation
    {
        internal static bool Validate(string _sid)
        {
            bool _regresult = false;
            string _regName = null;

            try
            {
               _regName =  Registry.Users.OpenSubKey($"{_sid}")?.Name ;
               _regresult = string.IsNullOrWhiteSpace(_regName) ? false : true;
            }
            catch(Exception)
            {
                throw;
            }
            return _regresult;
        }

        internal static string GetSID(string _userid)
        {
            NTAccount acc = new NTAccount(_userid);
            SecurityIdentifier sid = (SecurityIdentifier)acc.Translate(typeof(SecurityIdentifier));
            return sid.ToString();
        }
    }
}
