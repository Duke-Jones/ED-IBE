using System;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;

namespace EDCompanionAPI.Models
{
    [Serializable]
    internal class Profile
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public bool LoggedIn { get; set; }
        public CookieContainer Cookies { get; set; }

        public Profile()
        {
            Cookies = new CookieContainer();
        }

        public static Profile Load(string email)
        {
            string filePath = GetFilePath(email);
            if (File.Exists(filePath))
            {
                BinaryFormatter bin = new BinaryFormatter();
                using (Stream fileStream = File.OpenRead(filePath))
                {
                    var profile = (Profile)bin.Deserialize(fileStream);
                    profile.LoggedIn = false;
                    return profile;
                }
            }
            return null;
        }

        public void Save()
        {
            //Save
            string filePath = GetFilePath(Email);
            BinaryFormatter bin = new BinaryFormatter();
            using (var fileStream = File.OpenWrite(filePath))
            {
                bin.Serialize(fileStream, this);
            }
        }

        public static void Delete(string email)
        {
            //Save
            string filePath = GetFilePath(email);
            if (File.Exists(filePath))
                File.Delete(filePath);

        }

        private static string GetFilePath(string email)
        {
            return Path.Combine(EliteCompanion.Instance.DataPath, "CompanionProfile_" + email.ToLower().GetHashCode().ToString() + ".dat");
        }
    }
}