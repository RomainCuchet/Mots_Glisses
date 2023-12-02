using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mots_Glisses
{
    public class Dictionnaire
    {
        string storing_file_name; // the name of the txt file storing the words of the dict. The file must be SORTED
        string folder; // the name of the folder containing the txt_file
        string language; // the language of our dictionary

        public Dictionnaire(string folder = "../../Annexes", string storing_file_name = "sorted_french_words.txt", string language = "Français")
        {
            this.folder = folder;
            this.storing_file_name = storing_file_name;
            this.language = language;
        }

        public string Language { get { return language; } }

        /// <summary>
        /// allow to represent the dictionnary through its language and the number of words starting with each letter
        /// </summary>
        /// <returns>return a string that represents the dictionary</returns>
        public string toString()
        {
            try
            {
                string line;
                string[] words;
                List<(char, int)> nb_words_by_letters = new List<(char, int)>(); // list that will contain the first caractere of each line and the number of words in each of them
                StreamReader sr = new StreamReader(Path.Combine(folder, storing_file_name));
                while ((line = sr.ReadLine()) != null)
                {
                    words = line.Split(' '); // transform the string into an array of strings
                    nb_words_by_letters.Add((line[0], words.Length)); // add the letter an its given number of words in the array
                }
                string dictionnaire = $"Dictionnaire {this.language}";
                foreach ((char, int) tuple in nb_words_by_letters)
                {
                    dictionnaire += $"\n\t{tuple.Item1} : {tuple.Item2}";
                }
                return dictionnaire;
            }
            catch (Exception e)
            {
                // Let the user know what went wrong.
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
                return "error";
            }

        }
        /// <summary>
        /// print the content of storing_file_name in the console
        /// </summary>
        public void print_content()
        {
            string line;
            StreamReader sr = new StreamReader(Path.Combine(folder, storing_file_name));
            while ((line = sr.ReadLine()) != null)
            {
                Console.WriteLine(line);
            }
        }

        public bool search(string word)
        {
            bool is_word_in_dict = false;
            word = word.ToUpper(); // words are stored in upper case into the storage file
            // get the line of the starting letter 65-90
            int indice = (int)word[0] - 65; // we substract 65 because its the ascci value of "A" and we want a to be line : 0, b : 1 .... z : 25
            if (indice >= 0 && indice <= 25) // we verify that the fist letter is in the alphabet
            {
                int nb_line = (int)word[0] - 65;
                StreamReader sr = new StreamReader(Path.Combine(folder, storing_file_name));
                string line;
                string[] words;
                int i = 0;
                while (i < nb_line) { line = sr.ReadLine(); i++ ; } // do nothing
                line = sr.ReadLine();
                words = line.Split(' ');
                is_word_in_dict = Tools.search_word(word, words);
                sr.Close();
            }
            return is_word_in_dict;
        }
    }
}
