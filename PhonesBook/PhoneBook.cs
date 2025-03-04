using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;

namespace PhonesBook
{
	public class PhoneBook
	{
		private Dictionary<string, string> _phones = new Dictionary<string, string>();
		public PhoneBook(string pathFile) 
		{
			_phones = ReadPhonesFromFile(pathFile);
		}

		public Dictionary<string, string> ReadPhonesFromFile(string pathFile)
		{
			Dictionary<string, string> result = new Dictionary<string, string>();

            using (StreamReader reader = new StreamReader(pathFile))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] parts = line.Split('=');
                    if (parts.Length == 2)
                    {
                        result[parts[0]] = parts[1];
                    }
                }
            }


            return result;
		}
		public void WritePhonesToFile(string pathFile)
		{
            using (StreamWriter writer = new StreamWriter(pathFile))
            {
                foreach (var kvp in _phones)
                {
                    writer.WriteLine($"{kvp.Key}={kvp.Value}");
                }
            }
        }
        public void AddPhone(string phone, string name)
        {
            if(name == "" || phone == "")
            {
                throw new ArgumentException("Пустые поля");

            }
            try
            {
                CheckData(phone, name);
            }
            catch (ArgumentException e)
            {
                throw new ArgumentException(e.Message);

            }
            _phones.Add(phone, name);
        }
        public void RemovePhone(string phone) 
        { 
            _phones.Remove(phone);
        }
        public void EditPhone(string oldPhone, string oldName, string phone, string name)
        {
            try
            {
                CheckData(phone, name);
            } catch(ArgumentException e)
            {
                throw new ArgumentException(e.Message);
                
            }
            RemovePhone(oldPhone);
            AddPhone(phone, name);
        }
        public Dictionary<string, string> SearchInDictionary(string keyPart = null, string valuePart = null)
        {
            // Фильтруем словарь по части ключа и/или части значения
            return _phones
                .Where(kvp =>
                    (keyPart == null || kvp.Key.Contains(keyPart)) &&
                    (valuePart == null || kvp.Value.Contains(valuePart)))
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        }

        public Dictionary<string, string> GetPhone() 
        {
            return _phones;
        }

        public void CheckData(string phone, string name)
        {
            Debug.WriteLine(phone);
            if (name.Contains('='))
            {
                throw new ArgumentException("Некорректный символ в имени (=)");
            }
            if (!System.Text.RegularExpressions.Regex.IsMatch(phone, @"^((8|\+7)[\- ]?)?(\(?\d{3}\)?[\- ]?)?[\d\- ]{7,10}$"))
            {
                throw new ArgumentException("Некорректный номер телефона");
            }
        }
    }
}
