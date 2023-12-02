using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using Mots_Glisses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mots_Glisses.Tests
{
    [TestClass()]
    public class DictionnaireTests
    {

        [TestMethod()]
        public void searchTest()
        {
            Dictionnaire dict = new Dictionnaire("../../../Mots_Glisses/Annexes"); // one more folder in Mot glisse test compared to Mots_Glisses
            string[] correct_words = new string[] { "bonjour", "jaune", "vert", "ZYEuTER", "aa" }; // words that must be found in dict
            string[] incorrect_words = new string[] { "oeirjg", "lkdshlvkjh", "aaaaaa", "ZZZZZZZk" }; // words that must not be found in dict
            bool is_correct = true;
            for(int i = 0; is_correct && i < correct_words.Length; i++)
            {
                is_correct = dict.search(correct_words[i]);
            }
            if (!is_correct) Assert.Fail();
            for (int i = 0; is_correct && i < incorrect_words.Length; i++)
            {
                is_correct = !dict.search(incorrect_words[i]);
            }
            if (!is_correct) Assert.Fail();
            Assert.IsFalse(dict.search(" "));
            Assert.IsFalse(dict.search("abcd@"));
            Assert.IsFalse(dict.search("ab78èçcd"));
        }

        [TestMethod()]
        public void root_files()
        {
            Dictionnaire dict = new Dictionnaire("../../../Mots_Glisses/Annexes"); // one more folder in Mot glisse test compared to Mots_Glisses
            string[] correct_words = new string[] { "bonjour", "jaune", "vert", "ZYEuTER", "aa" }; // words that must be found in dict
            string[] incorrect_words = new string[] { "oeirjg", "lkdshlvkjh", "aaaaaa", "ZZZZZZZk" }; // words that must not be found in dict
            bool is_correct = true;
            for (int i = 0; is_correct && i < correct_words.Length; i++)
            {
                is_correct = dict.search(correct_words[i]);
            }
            if (!is_correct) Assert.Fail();
            for (int i = 0; is_correct && i < incorrect_words.Length; i++)
            {
                is_correct = !dict.search(incorrect_words[i]);
            }
            if (!is_correct) Assert.Fail();
            Assert.IsFalse(dict.search(" "));
            Assert.IsFalse(dict.search("abcd@"));
            Assert.IsFalse(dict.search("ab78èçcd"));
        }
    }
}