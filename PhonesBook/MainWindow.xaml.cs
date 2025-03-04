using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.ComponentModel;
using System.Collections.ObjectModel;
namespace PhonesBook
{

    public class PhoneEntry : INotifyPropertyChanged
    {
        private string _name;
        private string _phone;

        public string Name
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }

        public string Phone
        {
            get => _phone;
            set
            {
                if (_phone != value)
                {
                    _phone = value;
                    OnPropertyChanged(nameof(Phone));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
    public partial class MainWindow : Window
    {
        static string path = "1.txt";
        PhoneBook phoneBook = new PhoneBook(path);
        Dictionary<string, string> phonesDict = new Dictionary<string, string>();
        string oldName = "";
        string oldPhone = "";
        public ObservableCollection<PhoneEntry> Phones { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            phonesDict = phoneBook.GetPhone();
            Phones = new ObservableCollection<PhoneEntry>(
                phonesDict.Select(kvp => new PhoneEntry { Name = kvp.Key, Phone = kvp.Value }));
            DataContext = this;
            /*for(int i = 10; i<50; i++)
            {
                phoneBook.AddPhone("891030710" + i.ToString(), "Кабинет " + i.ToString());
            }*/
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedItem = mainListBox.SelectedItem as PhoneEntry;

            if (selectedItem != null)
            {
                oldName = selectedItem.Phone;
                oldPhone = selectedItem.Name;
                Debug.WriteLine($"Выбрано: {oldName} - {oldPhone}");
            }
        }
        private void UpdatePhones()
        {
            Phones.Clear();
            foreach (var kvp in phonesDict)
            {
                Phones.Add(new PhoneEntry { Name = kvp.Key, Phone = kvp.Value });
            }
        }

        private void addNumber_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                phoneBook.AddPhone(numberTextBox.Text, nameTextBox.Text);
            }
            catch (Exception ee)
            {
				UniversalWindow universalWindow = new UniversalWindow(ee.Message);
				universalWindow.Show();
				Debug.WriteLine(ee);
            }
            phonesDict = phoneBook.GetPhone();
            UpdatePhones();
            phoneBook.WritePhonesToFile(path);
        }

        private void editNumber_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                phoneBook.EditPhone(oldPhone, oldName, numberTextBox.Text, nameTextBox.Text);
            }
            catch (Exception ee)
            {
				UniversalWindow universalWindow = new UniversalWindow(ee.Message);
                universalWindow.Show();

				Debug.WriteLine(ee);
            }
            phonesDict = phoneBook.GetPhone();
            UpdatePhones();
            phoneBook.WritePhonesToFile(path);
        }

        private void deleteNumber_Click(object sender, RoutedEventArgs e)
        {
            phoneBook.RemovePhone(oldPhone);
            phonesDict = phoneBook.GetPhone();
            UpdatePhones();
            phoneBook.WritePhonesToFile(path);
        }

        private void clearNumber_Click(object sender, RoutedEventArgs e)
        {
            nameTextBox.Text = "";
            numberTextBox.Text = "";
        }

        private void searchNumber_Click(object sender, RoutedEventArgs e)
        {
            phonesDict = phoneBook.SearchInDictionary(numberTextBox.Text, nameTextBox.Text);

            UpdatePhones();
        }
    }
}
