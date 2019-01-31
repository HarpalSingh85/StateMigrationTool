using System;
using System.IO;


namespace StateMigrationBackend.Validation
{
    class AccessValidator
    {
       protected internal static Predicate<string> CanRead = (_filepath) => {
            bool _result = false;

            try
            {
                FileStream fs = new FileStream(_filepath, FileMode.OpenOrCreate, FileAccess.Read);
                if (fs.CanRead)
                {
                    _result = true;
                }
            }
            catch (UnauthorizedAccessException)
            {
                return false;
            }

            catch (Exception)
            {
                return false;
            }

            return _result;
        
        };

    }
}
