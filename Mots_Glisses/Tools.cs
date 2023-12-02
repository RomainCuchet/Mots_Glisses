using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Runtime.Remoting.Messaging;

namespace Mots_Glisses
{
    public class Tools
    {
        /// <summary>
        /// Merge two sorted arrays of strings into one sorted array of string
        /// </summary>
        /// <param name="tab1">the first array of strings</param>
        /// <param name="tab2">the second array of strings</param>
        /// <returns>a sorted array of strings composed with the element of the two given arrays</returns>
        public static string[] merge(string[] tab1, string[] tab2)
        {
            string[] tab = new string[tab1.Length + tab2.Length];
            int i = 0;
            int j = 0;
            int n = 0;
            while (i < tab1.Length && j < tab2.Length)
            {
                if (is_alphabetically_ordered(tab1[i],tab2[j]))
                {
                    tab[n] = tab1[i];
                    i++;
                }
                else
                {
                    tab[n] = tab2[j];
                    j++;
                }
                n++;
            }
            while (i < tab1.Length)
            {
                tab[n] = tab1[i];
                i++;
                n++;
            }

            while (j < tab2.Length)
            {
                tab[n] = tab2[j];
                j++;
                n++;
            }
            return tab;

        }

        /// <summary>
        /// sorts an array of strings by the alphabetical order
        /// </summary>
        /// <param name="tab">an array of strinfs</param>
        /// <returns>a sorted copy of the given strings array by the alphabetical order</returns>
        public static string[] merge_sort(string[] tab)
        {
            if (tab == null) return null;
            else if (tab.Length <= 1) return tab;
            int m = tab.Length / 2;
            string[] left = new string[m];
            string[] right = new string[tab.Length - m];
            for (int i = 0; i < m; i++) // construction of the lef array 
            {
                left[i] = tab[i];
            }
            for (int i = 0; i < tab.Length - m; i++) // construction of the right array
            {
                right[i] = tab[i+m];
            }

            return merge(merge_sort(left), merge_sort(right));
        }
        public static void get_sorted_mots_français(string folder = "../../Annexes", string origin_file = "french_words.txt", string destination_file = "sorted_french_words.txt")
        {
            try
            {
                // Create an instance of StreamReader to read from a file.
                StreamReader sr = new StreamReader(Path.Combine(folder, origin_file)); // using Path.combine to avoid any problem with different os
                // Create an instance of StreamWriter to write into a file.
                StreamWriter sw = new StreamWriter(Path.Combine(folder, destination_file), false); // must be switched to true in order to write in append mode, default = false 
                string line;
                string[] words;
                string[] sorted_words;
                // read the line, sort it and save it the specified file
                while ((line = sr.ReadLine()) != null)
                {
                    words = line.Split(' '); // transform the string into an array of strings
                    sorted_words = merge_sort(words); // recursively sort words 
                    sw.WriteLine(string.Join(" ", sorted_words));
                }
                sr.Close();
                sw.Close();
            }
            catch (Exception e)
            {
                // Let the user know what went wrong.
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// This function takes two string as parameters and return whether they are sorted alphabetically or not. Do not make the difference between upper and lower case letters
        /// </summary>
        /// <param name="s1">the reference string</param>
        /// <param name="s2">the string to compare too</param>
        /// <returns>true if s1 is before s2 in alphabetical order</returns>
        public static bool is_alphabetically_ordered(string s1, string s2)
        {
            s1 = s1.ToUpper();
            s2 = s2.ToUpper();
            bool is_ordered = true;
            bool is_finished = false;
            int min_len = min(s1.Length, s2.Length);
            for(int i=0; !is_finished && i<min_len; i++)
            {
                if ((int)s1[i] < (int)s2[i])
                {
                    is_finished = true;
                }
                if ((int)s1[i] > (int)s2[i]) { 
                    is_ordered = false;
                    is_finished = true;
                }
            }
            if(!is_finished && s1.Length > min_len) is_ordered = false;
            return is_ordered;
        }

        /// <summary>
        /// Verifies that all the words in the given txt file are ordered in the alphabetical order 
        /// </summary>
        /// <param name="folder">the to the folder containing the fil</param>
        /// <param name="file">the file path</param>
        /// <returns></returns>
        public static bool is_file_alphabetically_ordered(string folder = "Annexes", string file = "sorted_french_words.txt")
        {
            StreamReader sr = new StreamReader(Path.Combine(folder, file));
            string line;
            string[] words;
            bool is_ordered = true;
            int i_max;
            while (is_ordered && (line = sr.ReadLine()) != null)
            {
                words = line.Split(' '); // transform the string into an array of strings
                i_max = words.Length;
                for (int i = 0; is_ordered && i < i_max - 1; i++)
                {
                    if (!is_alphabetically_ordered(words[i], words[i + 1])) { is_ordered = false; }
                }
            }
            sr.Close();
            return is_ordered;
        }

        /// <summary>
        /// return the min of the two given integers
        /// </summary>
        /// <param name="a">first integer</param>
        /// <param name="b">second integer</param>
        /// <returns>min(a,b)</returns>
        public static int min(int a, int b)
        {
            if (a > b) return b;
            return a;
        }

        public static void print_listof_string(string[] array)
        {
            foreach(string s in array){ foreach (char c in s) { Console.Write(c);} Console.Write(" "); }
            Console.WriteLine();
        }

        /// <summary>
        /// search word in the given SORTED array of strings. Return true if founded, false otherwise
        /// </summary>
        /// <param name="word"></param>
        /// <param name="array"></param>
        /// <returns>a bool representing whether the word is in the array or not </returns>
        public static bool search_word(string word, string[] array)
        {
            if (array == null || array.Length == 0) return false;
            // use of local function in order to automatically generate requiered parameters and avoid mistakes
            bool recursive_search(int i_start, int i_end)
            {
                int i_middle = (i_start + i_end) / 2;
                if (i_start > i_end) return false;
                else if (array[i_middle] == word) return true;
                else if (is_alphabetically_ordered(array[i_middle], word)) return recursive_search(i_middle + 1, i_end);
                return recursive_search(i_start, i_middle - 1);
            }
            return recursive_search(0, array.Length-1);
        }
        /// <summary>
        /// given an array of string with each a len of one return an array of the cooresponding chars. Otherwise return null
        /// </summary>
        /// <param name="strings">an array of string with each a len of one</param>
        /// <returns>an array of char if strings is valid, null otherwise</returns>
        public static char[] string_array_to_char_array(string[] strings)
        {
            if (strings == null) return null;
            bool is_valid = true;
            char[] chars = new char[strings.Length];
            for(int i=0; is_valid && i<strings.Length; i++)
            {
                if (strings[i].Length != 1) is_valid = false; // chack that each string is of length 1
                else { chars[i] = strings[i][0]; }
            }
            if (!is_valid) chars = null;
            return chars;
        }

        public static void print_mat<T>(T[,] m)
        {
            if (m == null) Console.WriteLine("matrix null");
            else if (m.Length == 0) Console.WriteLine("hollow matrix");
            for (int i = 0; i < m.GetLength(0); i++)
            {
                for (int j = 0; j < m.GetLength(1); j++)
                {
                    Console.Write(m[i, j] + " ");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        public static string toString_mat<T>(T[,] m)
        {
            if (m == null) return "matrix null";
            else if (m.Length == 0) return "hollow matrix";
            string str = "";
            for (int i = 0; i < m.GetLength(0); i++)
            {
                
                for (int j = 0; j < m.GetLength(1); j++)
                {
                    str += (m[i, j] + " ");
                }
                str += "\n";
            }
            str += "\n";
            return str;
        }

        public static void print_matrix_chars(char[,] m)
        {
            if (m == null) Console.WriteLine("matrix null");
            else if (m.Length == 0) Console.WriteLine(" hollow matrix");
            for (int i = 0; i < m.GetLength(0); i++)
            {
                for(int j=0; j < m.GetLength(1); j++)
                {
                    Console.Write(m[i, j] + " ");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        public static void print_matrix_bool(bool[,] matrice)
        {
            if (matrice == null) Console.WriteLine("matrix null");
            else if (matrice.Length == 0) Console.WriteLine(" hollow matrix");
            else
            {
                for (int i = 0; i < matrice.GetLength(0); i++)
                {
                    for (int j = 0; j < matrice.GetLength(1); j++)
                    {
                        Console.Write(matrice[i, j] + "\t");
                    }
                    Console.WriteLine();
                }
            }
        }

        public static bool char_in_word(char c, string word)
        {
            bool is_founded = false;
            for(int i=0; !is_founded && i<word.Length; i++) { if (word[i] == c) { is_founded = true;} }
            return is_founded;
        }

        public static bool char_matrixs_equals(char[,] m1, char[,] m2)
        {
            {
                if (m1 == null || m2 == null)
                {
                    Console.WriteLine("matrix null");
                    return false;
                }
                if (m1.GetLength(0) != m2.GetLength(0) || m1.GetLength(1) != m2.GetLength(1))
                {
                    Console.WriteLine("not same length");
                    return false;
                }
                for (int i = 0; i < m1.GetLength(0); i++)
                {
                    for (int j = 0; j < m1.GetLength(1); j++)
                    {
                        if (m1[i, j] != m2[i, j])
                        {
                            Console.WriteLine($"{m1[i, j] != m2[i, j]}");
                            return false;
                        }
                    }
                }
                return true;
            }
        }

        public static void print_center(string text)
        {
            int screenWidth = Console.WindowWidth;

            int leftPadding = (screenWidth - text.Length) / 2;
            if (leftPadding < 0)
            {
                leftPadding = 0; // Éviter une valeur négative si le texte est plus large que la console
            }

            Console.SetCursorPosition(leftPadding, Console.CursorTop);
            Console.WriteLine(text);
        }

        public static bool bool_matrixs_equals(bool[,] m1, bool[,] m2)
        {
            {
                if (m1 == null || m2 == null)
                {
                    Console.WriteLine("matrix null");
                    return false;
                }
                if (m1.GetLength(0) != m2.GetLength(0) || m1.GetLength(1) != m2.GetLength(1))
                {
                    Console.WriteLine("not same length");
                    return false;
                }
                for (int i = 0; i < m1.GetLength(0); i++)
                {
                    for (int j = 0; j < m1.GetLength(1); j++)
                    {
                        if (m1[i, j] != m2[i, j])
                        {
                            Console.WriteLine($"{m1[i, j] != m2[i, j]}");
                            return false;
                        }
                    }
                }
                return true;
            }
        }
        public static char[,] random_char_mat(int height, int width)
        {
            char[,] matrix = new char[height, width];
            Random random = new Random();

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    // Générer un caractère aléatoire entre 'a' et 'z'
                    matrix[i, j] = (char)('a' + random.Next(26));
                }
            }

            return matrix;
        }

        public static string random_board_string()
        {
            char[,] board = random_char_mat(8, 8);
            int rows = board.GetLength(0);
            int cols = board.GetLength(1);
            string result = "";
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols-1; j++)
                {
                    result += $"|{board[i, j]} ";
                }
                result += "| "+ board[i,cols-1] + "|" + Environment.NewLine;
            }
            result += Environment.NewLine;
            result.Replace(Environment.NewLine, "\r");
            return result;
        }

        public static List<string> get_csv_file_names_from_folder(string folderPath)
        {
            List<string> csvFileNames = new List<string>();

            try
            {
                // Vérifier si le dossier existe
                if (Directory.Exists(folderPath))
                {
                    // Obtenir tous les fichiers CSV dans le dossier
                    string[] csvFiles = Directory.GetFiles(folderPath, "*.csv");

                    // Obtenir uniquement les noms de fichiers
                    foreach (string csvFile in csvFiles)
                    {
                        string fileName = Path.GetFileName(csvFile);
                        csvFileNames.Add(fileName);
                    }
                }
                else
                {
                    Console.WriteLine("Le dossier spécifié n'existe pas.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Une erreur s'est produite : {ex.Message}");
            }

            return csvFileNames;
        }
    }
}
