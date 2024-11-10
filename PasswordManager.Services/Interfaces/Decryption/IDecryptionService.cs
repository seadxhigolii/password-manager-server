using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.Services.Interfaces.Decryption
{
    public interface IDecryptionService
    {
        byte[] DeriveKeyFromPassword(string password, byte[] salt); 
    }
}
