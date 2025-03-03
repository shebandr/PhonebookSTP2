using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
	internal class PhoneBook
	{
		private Dictionary<string, string> _phones = new Dictionary<string, string>();
		public PhoneBook(string pathFile) 
		{
			_phones = ReadPhonesFromFile(pathFile);
		}

		private Dictionary<string, string> ReadPhonesFromFile(string pathFile)
		{
			Dictionary<string, string> result = new Dictionary<string, string>();




			return result;
		}
		private void WritePhonesToFile(string pathFile)
		{
			
		}
	}
}
