using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mots_Glisses
{
    public class Joueur
    {
        string name;
        List<string> found_words;
        double score;

        static int nb_player = 0;

        static public int Nb_player { get { return nb_player; } set { nb_player = value; } }
        public string Name { get { return name; } }

        public List<string> Words { get { return found_words; } set { found_words = value; } }

        public double Score { get { return score; } set { score = value; } }

        public static bool operator > (Joueur a, Joueur b)
        {
            return a.Score > b.Score;
        }

        public static bool operator < (Joueur a, Joueur b)
        {
            return a.Score < b.Score;
        }

        public Joueur(string name = "default")
        {
            nb_player++;
            if (name == "default") { this.name = $"Player{nb_player}"; }
            else { this.name = name; }
            found_words = new List<string>();
        }

        /// <summary>
        /// save word as a foud word if not previously founded
        /// </summary>
        /// <param name="word"></param>
        /// <returns>true is added, false otherwise</returns>
        public bool add_word(string word)
        {
            if (!is_previously_found(word)) { found_words.Add(word); return true; }
            else return false;
        }

        public string toString()
        {
            return $"Player : \n\tname: {name}\n\tpoints : {get_score()}\n\tnumber of found words : {found_words.Count()}\n\twords : {words_toString()}";
        }

        public void add_score(double val) { score += val; }
        /// <summary>
        /// round and return the score to the player to the specified precision
        /// </summary>
        /// <param name="nb_digits">number of decimal digits</param>
        /// <returns>rounded score</returns>
        public double get_score(int nb_digits = 2)
        {
            return Math.Round(score, nb_digits);
        }

        public void reset_score() { score = 0; }

        /// <summary>
        /// return wether the word have been previously find or not 
        /// </summary>
        /// <param name="word">the word to search</param>
        /// <returns>bool representing if word have been previously find or not</returns>
        public bool is_previously_found(string word)
        {
            bool verif = false;
            foreach (string m in found_words)
            {
                if (m == word) verif = true;
            }
            return verif;
        }
        /// <summary>
        /// transforms the list of found words into a string
        /// </summary>
        /// <returns>a string representing the list of found words</returns>
        public string words_toString()
        {
            try
            {
                int n = found_words.Count();
                string words = "";
                foreach (string i in found_words) { words += i + " "; }
                return words;
            }
            catch { return "error in the list"; }
        }

        public string found_words_toString()
        {
            if (found_words == null || found_words.Count == 0) return "aucun mot trouvé";
            string str = "";
            foreach (string word in found_words) str += word + " ";
            return str;
        }
    }
}