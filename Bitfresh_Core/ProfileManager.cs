using System;
using System.Collections.Generic;
using System.IO;
using System.Security;
using System.Security.Cryptography;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Bitfresh_Core
{
    public class ProfileManager
    {
        private string CurrentPath, TargetPath;

        public System.ComponentModel.BindingList<string> UserNames;

        public DirectoryInfo ProfileFolder;

        private XmlSerializer Serializer;

        private List<User> CurrentUsers;

        public ProfileManager()
        {
            Serializer = new XmlSerializer(typeof(User));

            CurrentUsers = new List<User>();

            CurrentPath = Directory.GetCurrentDirectory();

            TargetPath = Path.Combine(CurrentPath, "Profiles");

            try
            {
                ProfileFolder = Directory.CreateDirectory(TargetPath);
            }
            catch { throw; }

            UserNames = new System.ComponentModel.BindingList<string>();

            foreach (FileInfo folder in ProfileFolder.EnumerateFiles("*.xml"))
            {
                try
                {
                    ReadUserFromFile(folder.FullName);
                }
                catch
                {
                    throw;
                }
            }

            foreach (User usr in CurrentUsers)
            {
                UserNames.Add(usr.Name);
            }
        }

        private void ReadUserFromFile(string path)
        {
            try
            {
                using (FileStream reader = new FileStream(path, FileMode.Open))
                {
                    CurrentUsers.Add((User)Serializer.Deserialize(reader));
                }
            }
            catch { throw; }
        }

        public bool AddNewUser(string Name, string KeyApi, string KeySecret, string Password)
        {
            if (String.IsNullOrWhiteSpace(Name) || String.IsNullOrWhiteSpace(KeyApi) || String.IsNullOrWhiteSpace(KeySecret) || String.IsNullOrWhiteSpace(Password))
            {
                return false;
            }

            User newUser = new User(Name, KeyApi);

            newUser.SetSecret(Encrypter.StringtoSecureString(KeySecret), Encrypter.StringtoSecureString(Password));

            try
            {
                WriteUser(newUser);
            }
            catch
            {
                throw;
            }

            CurrentUsers.Add(newUser);
            UserNames.Add(newUser.Name);

            KeySecret = string.Empty;
            Password = string.Empty;

            return true;
        }

        private void WriteUser(User _user)
        {
            using (FileStream file = new FileStream(Path.Combine(TargetPath, _user.Name + ".xml"), FileMode.Create, FileAccess.ReadWrite))
            {
                using (StreamWriter Writer = new StreamWriter(file))
                {
                    Serializer.Serialize(Writer, _user);
                }
            }
        }

        public bool GetUserData(string name, string password, out Tuple<string, string> data)
        {
            User _user = CurrentUsers.Find((x) => x.Name == name);

            if (_user == null)
            {
                data = null;
                return false;
            }

            try
            {
                data = new Tuple<string, string>(_user.KeyApi, System.Text.Encoding.ASCII.GetString(_user.GetSecret(Encrypter.StringtoSecureString(password))));
            }
            catch
            {
                throw;
            }
            password = string.Empty;
            return true;
        }

        public bool EraseUser(string name)
        {
            if (UserNames.Remove(name))
            {
                try
                {
                    CurrentUsers.RemoveAt(CurrentUsers.FindIndex((x) => x.Name == name));
                }
                catch
                {
                    UserNames.Add(name);
                    return false;
                }

                File.Delete(Path.Combine(TargetPath, name + ".xml"));
            }
            else
            {
                return false;
            }

            return true;
        }

        [Serializable]
        public class User : IXmlSerializable
        {
            private byte[] _secret;
            private string passHash;

            private Rijndael encryptorParams;

            public string Name { get; set; }
            public string KeyApi { get; set; }

            public string EncryptedKeySecret { get { return _secret != null ? Convert.ToBase64String(_secret) : String.Empty; } }
            public string IV { get { return Convert.ToBase64String(encryptorParams.IV); } }

            private User()
            {
                encryptorParams = Rijndael.Create();
                encryptorParams.KeySize = 256;
                encryptorParams.Padding = PaddingMode.Zeros;
            }

            public User(string name, string keyApi)
            {
                Name = name;
                KeyApi = keyApi;

                //Encryption initialization

                encryptorParams = Rijndael.Create();
                encryptorParams.GenerateIV();
                encryptorParams.KeySize = 256;
                encryptorParams.Padding = PaddingMode.Zeros;
            }

            public void SetSecret(SecureString secret, SecureString password)
            {
                passHash = BCrypt.Net.BCrypt.HashPassword(Encrypter.SecureStringToString(password));

                _secret = Encrypter.Encrypt(encryptorParams, password, secret);
            }

            public byte[] GetSecret(SecureString password)
            {
                try
                {
                    if (!CheckPassword(password))
                    {
                        throw new InvalidPasswordException();
                    }
                }
                catch
                {
                    throw;
                }

                return Encrypter.Decrypt(encryptorParams, password, _secret);
            }

            public bool CheckPassword(SecureString password)
            {
                if (_secret == null || String.IsNullOrEmpty(passHash))
                    throw new NoPasswordException();

                return BCrypt.Net.BCrypt.Verify(Encrypter.SecureStringToString(password), passHash);
            }

            public XmlSchema GetSchema()
            {
                return null;
            }

            public void ReadXml(XmlReader reader)
            {
                Name = reader["Name"];
                KeyApi = reader["KeyAPi"];
                _secret = Convert.FromBase64String(reader["KeySecret"]);
                encryptorParams.IV = Convert.FromBase64String(reader["IV"]);
                passHash = reader["PassHash"];
            }

            public void WriteXml(XmlWriter writer)
            {
                writer.WriteAttributeString("Name", Name);
                writer.WriteAttributeString("KeyAPi", KeyApi);
                writer.WriteAttributeString("KeySecret", EncryptedKeySecret);
                writer.WriteAttributeString("IV", IV);
                writer.WriteAttributeString("PassHash", passHash);
            }
        }
    }

    public class InvalidPasswordException : Exception
    {
        public InvalidPasswordException() : base()
        {
        }

        public InvalidPasswordException(string message) : base(message)
        {
        }

        public InvalidPasswordException(string message, System.Exception inner) : base(message, inner)
        {
        }
    }

    public class NoPasswordException : Exception
    {
        public NoPasswordException() : base()
        {
        }

        public NoPasswordException(string message) : base(message)
        {
        }

        public NoPasswordException(string message, System.Exception inner) : base(message, inner)
        {
        }
    }
}