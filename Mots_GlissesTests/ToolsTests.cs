using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mots_Glisses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Mots_Glisses.Tests
{
    [TestClass()]
    public class ToolsTests
    {

        [TestMethod()]
        public void get_maxTest()
        {
            int a = 5;
            int b = 6;
            int c = 5;
            int d = -2;
            Assert.AreEqual(5, Tools.min(a, b));
            Assert.AreEqual(5, Tools.min(b, a));
            Assert.AreEqual(5, Tools.min(a, c));
            Assert.AreNotEqual(6, Tools.min(a, b));
            Assert.AreEqual(-2, Tools.min(d, b));
        }

        [TestMethod()]
        public void is_alphabetically_orderedTest()
        {
            string a = "abc";
            string b = "aBcd";
            string c = "aCd";
            string d = "bAAa";
            string e = "c";

            // case s1 = s2 must return true
            Assert.IsTrue(Tools.is_alphabetically_ordered(a, a));

            // case s1 < s2 must return true
            Assert.IsTrue(Tools.is_alphabetically_ordered(a, b));
            Assert.IsTrue(Tools.is_alphabetically_ordered(a, c));
            Assert.IsTrue(Tools.is_alphabetically_ordered(a, d));
            Assert.IsTrue(Tools.is_alphabetically_ordered(b, e));

            // case s1 > s2 must return false
            Assert.IsFalse(Tools.is_alphabetically_ordered(b, a));
            Assert.IsFalse(Tools.is_alphabetically_ordered(d, a));
            Assert.IsFalse(Tools.is_alphabetically_ordered(d, b));
        }

        [TestMethod()]
        public void mergeTest()
        {
            string a = "abc";
            string b = "def";
            string c = "ghi";
            string d = "jklm";
            string[] s1 = new string[] {a,c};
            string[] s2 = new string[] {b,d};
            string[] merged = Tools.merge(s1 , s2);
            string[] compare = new string[] { a, b, c, d };
            int i_max = merged.Length;
            int j_max;
            bool is_correct = true;
            for(int i = 0; is_correct && i<i_max; i++)
            {
                j_max = merged[i].Length;
                for(int j = 0; is_correct && j<j_max; j++)
                {
                    if (merged[i][j] != compare[i][j]) is_correct = false;
                }
            }
            if (!is_correct) Assert.Fail();
        }

        [TestMethod()]
        public void merge_sortTest()
        {
            string a = "abc";
            string b = "def";
            string c = "ghi";
            string d = "jklm";
            string e = "nopqrstuvw";
            string f = "xyz";
            string[] to_sort = new string[] { e, a, b, f, c, d };
            string[] compare = new string[] { a, b, c, d, e, f };
            string[] merge_sorted = Tools.merge_sort(to_sort);
            int i_max = merge_sorted.Length;
            int j_max;
            bool is_correct = true;
            for (int i = 0; is_correct && i < i_max; i++)
            {
                j_max = merge_sorted[i].Length;
                for (int j = 0; is_correct && j < j_max; j++)
                {
                    if (merge_sorted[i][j] != compare[i][j]) is_correct = false;
                }
            }
            if (!is_correct) Assert.Fail();
        }

        [TestMethod()]
        public void is_sorted_french_words_sorted()
        {
            Assert.IsTrue(Tools.is_file_alphabetically_ordered("../../../Mots_Glisses/Annexes")); // enable to verify that sorted_french_words.txt is well sorted in the alpabetical order

        }

        [TestMethod()]
        public void search_wordsTest()
        {
            bool is_correct = true;
            string[] good_words = new string[] { "AAA", "BBB", "C","E" };
            string[] dict = new string[] { "AAA", "BBB", "C","DD", "E" };
            for (int i = 0;is_correct && i<good_words.Length; i++)
            {
                if (!Tools.search_word(good_words[i], dict)){ is_correct = false; }
            }
            if(!is_correct) Assert.Fail();
            Assert.IsFalse(Tools.search_word("EE", dict));
        }

        [TestMethod()]
        public void string_array_to_char_arrayTest()
        {
            bool is_valid = true;
            string[] valide = { "a", "b", "c", "d" };
            char[] compare = { 'a', 'b', 'c', 'd' };
            string[] invalide = { "a", "bc", "c", "d" };
            string[] invalide2 = null;
            char[] chars = Tools.string_array_to_char_array(valide);
            for(int i=0;is_valid && i < chars.Length; i++)
            {
                if (chars[i] != compare[i]) is_valid = false;
            }
            if(!is_valid) Assert.Fail();
            Assert.IsNull(Tools.string_array_to_char_array(invalide));
            Assert.IsNull(Tools.string_array_to_char_array(invalide2));
        }

        [TestMethod()]
        public void char_in_wordTest()
        {
            string word = "abcdefg";
            Assert.IsTrue(Tools.char_in_word('a', word));
            Assert.IsTrue(Tools.char_in_word('g', word));
            Assert.IsTrue(Tools.char_in_word('d', word));
            Assert.IsFalse(Tools.char_in_word('p', word));
            Assert.IsFalse(Tools.char_in_word('9', word));
        }
    }

}