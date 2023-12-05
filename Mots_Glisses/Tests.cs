using System;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mots_Glisses
{
    public class Tests
    {
        public static void mergeTest()
        {
            bool is_correct = true;
            string a = "abc";
            string b = "def";
            string c = "ghi";
            string d = "jklm";
            int j_max = 0;
            string[] s1 = new string[] { a, c };
            string[] s2 = new string[] { b, d };
            string[] merged = Tools.merge(s1, s2);
            string[] compare = new string[] { a, b, c, d };
            Tools.print_listof_string(s1);
            Tools.print_listof_string(s2);
            Tools.print_listof_string(merged);
            Tools.print_listof_string(compare);
            int i_max = merged.Length;
            for (int i = 0; is_correct && i < i_max; i++)
            {
                j_max = merged[i].Length;
                for (int j = 0; is_correct && j < j_max; j++)
                {
                    if (merged[i][j] != compare[i][j]) is_correct = false;
                }
            }
            if(!is_correct) Console.WriteLine("not correct");
        }

        public static void merge_sortTest()
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
            string[] t1 = Tools.merge_sort(new string[] { e, a, b });
            string[] t2 = Tools.merge_sort(new string[] { f, c, d });
            Tools.print_listof_string(t1);
            Tools.print_listof_string(t2);
            Tools.print_listof_string(Tools.merge(t1, t2));
            Console.WriteLine();
            Tools.print_listof_string(to_sort);
            Tools.print_listof_string(merge_sorted);
            Tools.print_listof_string(compare);

        }

        public static void searchTest()
        {
            Dictionnaire dict = new Dictionnaire();
            Stopwatch stopwatch = new Stopwatch();
            string[] correct_words = new string[] { "bonjour", "jaune", "vert", "ZYEuTER", "aa" }; // words that must be found in dict
            string[] incorrect_words = new string[] { "oeirjg", "lkdshlvkjh", "aaaaaa", "ZZZZZZZk" }; // words that must not be found in dict
            Console.WriteLine($"le mot bonjour est présent dans le dictionnaire : {dict.search("bonjour")}");
            stopwatch.Start();
            Console.WriteLine($"le mot {incorrect_words[0]} est présent dans le dictionnaire : {dict.search(correct_words[0])}");
            stopwatch.Stop();
            Console.WriteLine(stopwatch.ElapsedMilliseconds);
        }

        public static void searchTest2()
        {
            Dictionnaire dict = new Dictionnaire();
            string[] correct_words = new string[] { "bonjour", "jaune", "vert", "ZYEuTER", "aa" }; // words that must be found in dict
            string[] incorrect_words = new string[] { "oeirjg", "lkdshlvkjh", "aaaaaa", "ZZZZZZZk" }; // words that must not be found in dict
            bool is_correct = true;
            for (int i = 0; is_correct && i < correct_words.Length; i++)
            {
                Console.WriteLine($"le mot {correct_words[i]} est présent dans le dictionnaire : {dict.search(correct_words[i])}");
            }
            Console.WriteLine();
            Console.WriteLine();
            for (int i = 0; is_correct && i < incorrect_words.Length; i++)
            {
                Console.WriteLine($"le mot {incorrect_words[i]} est présent dans le dictionnaire : {dict.search(incorrect_words[i])}");
            }
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            dict.search("maman");
            stopwatch.Stop();
            Console.WriteLine(stopwatch.ElapsedMilliseconds);
        }

        public static void print_dictionnaireTest()
        {
            Dictionnaire dictionary = new Dictionnaire();
            Console.WriteLine(dictionary.toString());
        }

        public static void PlateauTest()
        {
            string a = "a;b;c";
            string b = a.Replace(";", "");
            Console.WriteLine($"{a} : {a.Length}");
            Console.WriteLine($"{b} : {b.Length}");
            string path = "../../Annexes/Test/Plateau/valide1.csv";
            Plateau board5 = new Plateau(path);
            char[,] compare = new char[,]
            {
                { 'a', 'b', 'c' },
                { 'e', 'j', 's'},
                { 'p', 't', 'c'}
            };
            Tools.print_matrix_chars(compare);
            Tools.print_matrix_chars(board5.Board);
            Console.WriteLine($"are aqual : {Tools.char_matrixs_equals(compare, compare)}");
            Console.WriteLine($"are aqual : {Tools.char_matrixs_equals(compare, board5.Board)}");
            Console.WriteLine($"{board5.Board.GetLength(0)} et {board5.Board.GetLength(1)}");
        }

        public static void plateau_searchTest()
        {
            Plateau board;
            board = new Plateau("../../../Mots_Glisses/Annexes/Test/Plateau/search/avoid_repetitions.csv"); // this test aims to verify that the search alogorithm can't consider twice the same square
            bool found;
            bool[,] correct_path;
            string word = "aaa";
            Tools.print_mat(board.Board);
            (found, correct_path) = board.search(word);
            Console.WriteLine($"the word {word} have been found : {found}");
            if (found) Tools.print_mat(correct_path);
            word = "aaaa";
            (found, correct_path) = board.search(word);
            Console.WriteLine($"the word {word} have been found : {found}");
            if (found) Tools.print_mat(correct_path);
            board = new Plateau("../../../Mots_Glisses/Annexes/Test/Plateau/search/valide3.csv"); // this test aims to verify that the search alogorithm for diagonale search enabled
            word = "thOmaS";
            (found, correct_path) = board.search(word);
            Console.WriteLine($"the word {word} have been found : {found}");
            if (found) Tools.print_mat(correct_path);
            board.enable_diagonale_search();
            (found, correct_path) = board.search(word);
            Console.WriteLine($"the word {word} have been found : {found}");
            if (found)
            {
                Tools.print_mat(board.Board);
                Tools.print_mat(correct_path);
            }
        }

        public static void handle_wordTest()
        {
            Plateau board = new Plateau("../../../Mots_Glisses/Annexes/board1.csv",true);
            Tools.print_mat(board.Board);
            string word = "Romain";
            bool found;
            bool[,] correct_path;
            board.handle_word(word);
            Tools.print_mat(board.Board);
            board.handle_word("Thomas");
            Tools.print_mat(board.Board);
            board = new Plateau("../../../Mots_Glisses/Annexes/Test/Plateau/valide3.csv", true);
            Tools.print_mat(board.Board);
            board.handle_word("maison");
            Tools.print_mat(board.Board);
        }
        public static void print_test()
        {
            StringBuilder result = new StringBuilder();
            char[,] matrix = {
                {'a', 'b', 'c', 'd'},
                {'e', 'f', 'g', 'h'},
                {'i', 'j', 'k', 'l'}
            };

            int rows = matrix.GetLength(0);
            int cols = matrix.GetLength(1);

            // Afficher le contenu de la matrice
            for (int i = 0; i < rows; i++)
            {
                result.AppendLine("----");

                for (int j = 0; j < cols; j++)
                {
                    result.AppendLine($"| {matrix[i, j]} |");
                }
            }

            result.AppendLine("----");

            Console.WriteLine(result.ToString());
        }
    }
}
