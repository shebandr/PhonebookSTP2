using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;

namespace PhonesBook
{
    [TestClass]
    public class PhoneBookTests
    {
        private string _testFilePath = "test_phones.txt";

        [TestInitialize]
        public void Setup()
        {
            File.WriteAllText(_testFilePath, "123456789=Alice\n987654321=Bob\n555555555=Charlie");
        }

        [TestCleanup]
        public void Cleanup()
        {
            if (File.Exists(_testFilePath))
            {
                File.Delete(_testFilePath);
            }
        }

        [TestMethod]
        public void ReadPhonesFromFile_ValidFile_ReturnsCorrectDictionary()
        {
            var phoneBook = new PhoneBook(_testFilePath);
            var result = phoneBook.GetPhone();
            Assert.AreEqual(3, result.Count);
            Assert.AreEqual("Alice", result["123456789"]);
            Assert.AreEqual("Bob", result["987654321"]);
            Assert.AreEqual("Charlie", result["555555555"]);
        }

        [TestMethod]
        public void WritePhonesToFile_ValidData_WritesCorrectly()
        {
            var phoneBook = new PhoneBook(_testFilePath);
            phoneBook.AddPhone("111222333", "David");
            phoneBook.WritePhonesToFile(_testFilePath);
            var lines = File.ReadAllLines(_testFilePath);
            Assert.AreEqual(4, lines.Length);
            Assert.IsTrue(lines.Contains("111222333=David"));
        }

        [TestMethod]
        public void AddPhone_ValidData_AddsToDictionary()
        {
            var phoneBook = new PhoneBook(_testFilePath);
            phoneBook.AddPhone("111222333", "David");
            var result = phoneBook.GetPhone();
            Assert.AreEqual(4, result.Count);
            Assert.AreEqual("David", result["111222333"]);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddPhone_EmptyFields_ThrowsException()
        {
            var phoneBook = new PhoneBook(_testFilePath);
            phoneBook.AddPhone("", "");
        }

        [TestMethod]
        public void RemovePhone_ExistingPhone_RemovesFromDictionary()
        {
            var phoneBook = new PhoneBook(_testFilePath);
            phoneBook.RemovePhone("123456789");
            var result = phoneBook.GetPhone();
            Assert.AreEqual(2, result.Count);
            Assert.IsFalse(result.ContainsKey("123456789"));
        }

        [TestMethod]
        public void RemovePhone_NonExistingPhone_NoChange()
        {
            var phoneBook = new PhoneBook(_testFilePath);
            phoneBook.RemovePhone("999999999");
            var result = phoneBook.GetPhone();
            Assert.AreEqual(3, result.Count);
        }

        [TestMethod]
        public void EditPhone_ValidData_UpdatesDictionary()
        {
            var phoneBook = new PhoneBook(_testFilePath);
            phoneBook.EditPhone("123456789", "Alice", "999999999", "Alice Smith");
            var result = phoneBook.GetPhone();
            Assert.AreEqual(3, result.Count);
            Assert.AreEqual("Alice Smith", result["999999999"]);
            Assert.IsFalse(result.ContainsKey("123456789"));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void EditPhone_InvalidPhone_ThrowsException()
        {
            var phoneBook = new PhoneBook(_testFilePath);
            phoneBook.EditPhone("123456789", "Alice", "invalid", "Alice Smith");
        }

        [TestMethod]
        public void SearchInDictionary_ByKeyPart_ReturnsMatchingEntries()
        {
            var phoneBook = new PhoneBook(_testFilePath);
            var result = phoneBook.SearchInDictionary(keyPart: "123");
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("Alice", result["123456789"]);
        }

        [TestMethod]
        public void SearchInDictionary_ByValuePart_ReturnsMatchingEntries()
        {
            var phoneBook = new PhoneBook(_testFilePath);
            var result = phoneBook.SearchInDictionary(valuePart: "Bob");
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("Bob", result["987654321"]);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CheckData_InvalidPhone_ThrowsException()
        {
            var phoneBook = new PhoneBook(_testFilePath);
            phoneBook.AddPhone("invalid", "David");
        }
    }
}