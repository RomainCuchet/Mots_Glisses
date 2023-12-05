using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mots_Glisses
{
    public class Plateau
    {
        char[,] board = null; // char of lower case
        char[,] saved_board;
        bool enable_diagonal_search;
        string separator = ";";
        char empty_char = ' ';

        public char[,] Board { get { return board; } }
        public char[,] Saved_board { get { return saved_board; } }
        public bool Enable_diagonal_search { get { return enable_diagonal_search; } }

        public Plateau(string file = "../../Annexes/board1.csv", bool enable_diagonal_search = true)
        {
            this.enable_diagonal_search = enable_diagonal_search;
            if (file.Substring(file.Length - 3) == "csv")
            {
                Constructor_csv(file);
            }
            else if (file.Substring(file.Length - 3) == "txt")
            {
                Constructor_txt(file);
            }
            else throw new Exception("file extension not accepted");
        }

        public void enable_diagonale_search()
        {
            enable_diagonal_search = true;
        }

        public void disable_diagonale_search()
        {
            enable_diagonal_search = false;
        }

        /// <summary>
        /// create a file.csv from another file.csv already existing and following standards.
        /// </summary>
        /// <param name="file">enables to reach the file we want to open</param>
        public void Constructor_csv(string file)
        {
            try
            {
                StreamReader sr = new StreamReader(file);
                try
                {
                    List<string> Liste = new List<string>();
                    string s;
                    int len = -1;
                    bool verif = true;
                    while ((s = sr.ReadLine()) != null && verif)
                    {
                        if (len == -1) len = s.Length;
                        if (len == s.Length) verif = Is_Valid_csv(s);
                        else verif = false;
                        s = s.Replace(";", "");
                        s = s.ToLower();
                        Liste.Add(s);
                    }
                    if (verif)
                    {
                        board = new char[Liste.Count, len / 2 + 1];
                        for (int i = 0; i < Liste.Count; i++)
                        {
                            for (int j = 0; j < Liste[i].Length; j++)
                            {
                                board[i, j] = Liste[i][j];
                            }
                            Console.WriteLine();
                        }
                    }
                    saved_board = new char[board.GetLength(0), board.GetLength(1)];
                    for (int i = 0; i < board.GetLength(0); i++)
                    {
                        for (int j = 0; j < board.GetLength(1); j++)
                        {
                            saved_board[i, j] = board[i, j];
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("File couldn't be read");
                    Console.WriteLine(e.Message);
                }
                finally { sr.Close(); }
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("File not found");
            }
        }

        /// <summary>
        /// Check that string s is following standards for file.csv
        /// </summary>
        /// <param name="s">is one line of the file wee use as foundation</param>
        /// <returns>true if s is following standards, false if not</returns>
        public bool Is_Valid_csv(string s)
        {
            bool dot_komma = true;
            bool verif = true;
            for (int i = 0; i < s.Length && verif; i++)
            {
                if (Char.IsLetter(s[i]) && dot_komma) dot_komma = false;
                else if (s[i] == ';' && !dot_komma) dot_komma = true;
                else verif = false;
            }
            return verif;
        }

        /// <summary>
        /// create a file.txt from another file.txt already existing and following standards.
        /// </summary>
        /// <param name="file">enables to reach the file we want to open</param>
        public void Constructor_txt(string file)
        {
            try
            {
                StreamReader sr = new StreamReader(file);
                try
                {
                    List<string[]> Liste = new List<string[]>();
                    Random r = new Random();
                    string s;
                    string[] strings;
                    int val;
                    int[] value;
                    while ((s = sr.ReadLine()) != null)
                    {
                        strings = s.Split(',');
                        strings[0] = strings[0].ToLower(); //faux
                        Liste.Add(strings);
                    }
                    if (Is_Valid_txt(Liste))
                    {
                        value = new int[Liste.Count];
                        string[] max_iteration = new string[Liste.Count];
                        for (int i = 0; i < Liste.Count; i++)
                        {
                            for (int j = 0; j < Convert.ToInt32(Liste[i][1]); j++) max_iteration[i] += Liste[i][0];
                            value[i] = Convert.ToInt32(Liste[i][2]);
                        }
                        board = new char[8, 8];
                        for (int i = 0; i < 8; i++)
                        {
                            for (int j = 0; j < 8; j++)
                            {
                                do
                                {
                                    val = r.Next(26);
                                    if (max_iteration[val].Length > 0)
                                    {
                                        board[i, j] = max_iteration[val][0];
                                    }
                                } while (max_iteration[val].Length == 0);
                                max_iteration[val] = max_iteration[val].Remove(0, 1);
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("File couldn't be read");
                    Console.WriteLine(e.Message);
                }
                finally { sr.Close(); }
                saved_board = new char[8,8];
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        saved_board[i, j] = board[i, j];
                    }
                }
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("File not found");
            }
        }

        /// <summary>
        /// Check that List<string[]> s is following standards for file.txt
        /// </summary>
        /// <param name="s">is the file separates with Split()</param>
        /// <returns>true if s is following standards, false otherwise</returns>
        public bool Is_Valid_txt(List<string[]> s)
        {
            bool verif = true;
            if (s.Count != 26) verif = false;
            for (int i = 0; i < s.Count && verif == true; i++)
            {
                if (Char.TryParse(s[i][0], out char letter))
                {
                    if (letter != 97 + i) verif = false;
                }
                else verif = false;
                if (int.TryParse(s[i][1], out int result))
                {
                    if (result < 0) verif = false;
                }
                else verif = false;
                if (int.TryParse(s[i][2], out int result2))
                {
                    if (result2 < 0) verif = false;
                }
                else verif = false;
            }
            return verif;
        }

        /// <summary>
        /// Checks if the input string is a valid sequence of letters separated by semicolons.
        /// </summary>
        /// <param name="s">The input string to validate.</param>
        public bool Is_Valid(string s)
        {
            bool dot_komma = true;
            bool verif = true;
            for (int i = 0; i < s.Length && verif; i++)
            {
                if (Char.IsLetter(s[i]) && dot_komma) dot_komma = false;
                else if (s[i] == ';' && !dot_komma) dot_komma = true;
                else verif = false;
            }
            return verif;
        }

        /// <summary>
        /// given a word tell wether you can write it in the board according to the rulls of the game
        /// </summary>
        /// <param name="word"></param>
        /// <returns>true or false wether you can write the word or not</returns>
        public (bool, bool[,]) search(string word)
        {
            bool[,] correct_path = new bool[board.GetLength(0), board.GetLength(1)]; // will contain the path of the given word if it is in the board
            if (word == null || board == null) return (false, correct_path); // verify that the conditions to lauch the search algorithm are requiered
            word = word.ToLower();
            bool found = false; // switch to true is the word is found
            bool[,] was_here = new bool[board.GetLength(0), board.GetLength(1)]; // enable to avoid going twice on the same square
            for(int j=0; !found && j < board.GetLength(1); j++) // for all the square in the last row call dicho_search
            {
                found = dicho_search(board.GetLength(0) - 1, j, 0);
            }
            return (found, correct_path);

            // declaration of a local function because it is requiered only for our search algorithm. Enable to avoid any call by mistake
            bool dicho_search(int i, int j, int i_word)
            {
                if (board[i, j] != word[i_word] || was_here[i,j]) return false; // if the current square doesn't contain the right letter or if it has already been checked return false
                was_here[i, j] = true; // marck the square has checked
                if (i_word == word.Length - 1) { correct_path[i, j] = true; return true; } // if we are at word's last letter and the current square contain it then return true, we finished sucessfully our research
                if (i - 1 > -1) // if the top square is in the board
                {
                    if (dicho_search(i - 1, j, i_word + 1)) // recalls method for the top square
                    {
                        correct_path[i, j] = true;
                        return true; // we found the word in the board
                    }
                }
                if (i + 1 < board.GetLength(0)) // if the bottom square is in the board
                {
                    if (dicho_search(i + 1, j, i_word + 1)) // recalls method for the bottom square
                    {
                        correct_path[i, j] = true;
                        return true; // we found the word in the board
                    }
                }
                if(j - 1 > -1) // if the left square is in the board
                {
                    if (dicho_search(i, j-1, i_word + 1)) // recalls method for the left square
                    {
                        correct_path[i, j] = true;
                        return true; // we found the word in the board
                    }
                }
                if (j + 1 < board.GetLength(1)) // if the right square is in the board
                {
                    if (dicho_search(i, j+1, i_word + 1)) // recalls method for the right square
                    {
                        correct_path[i, j] = true;
                        return true; // we found the word in the board
                    }
                }

                if (enable_diagonal_search)
                {
                    if (i - 1 > -1 && j-1 > -1) // if the top left diagonal square is in the board
                    {
                        if (dicho_search(i - 1, j-1, i_word + 1)) // recalls method for the top left diagonal square
                        {
                            correct_path[i, j] = true;
                            return true; // we found the word in the board
                        }
                    }
                    if (i + 1 < board.GetLength(0) && j-1 > -1) // if the bottom left diagonale square is in the board
                    {
                        if (dicho_search(i + 1, j-1, i_word + 1)) // recalls method for the bottom left diagonale square
                        {
                            correct_path[i, j] = true;
                            return true; // we found the word in the board
                        }
                    }
                    if (i-1 > -1 && j+1<board.GetLength(1)) // if the top right diagonale square is in the board
                    {
                        if (dicho_search(i-1, j + 1, i_word + 1)) // recalls method for the top right diagonale square is in the board
                        {
                            correct_path[i, j] = true;
                            return true; // we found the word in the board
                        }
                    }
                    if (i+1<board.GetLength(0) && j + 1 < board.GetLength(1)) // if the bottom right diagonale square is in the board
                    {
                        if (dicho_search(i +1, j + 1, i_word + 1)) // recalls method for the bottom right diagonale square is in the board
                        {
                            correct_path[i, j] = true;
                            return true; // we found the word in the board
                        }
                    }
                }

                return false; // word not found

            }
        }

        public void update_matrix(bool[,] correct_path)
        {
            bool did_go_down = false;
            for (int i = 0; i < correct_path.GetLength(0); i++)
            {
                for (int j = 0; j < correct_path.GetLength(1); j++)
                {
                    if (did_go_down && j != 0) { j--; did_go_down = false; }
                    else if (did_go_down && j == 0) { j = correct_path.GetLength(1) - 1; i--; did_go_down = false; }
                    if (correct_path[i, j])
                    {
                        down(correct_path, board, i, j);
                        did_go_down = true;
                    }
                }
            }
        }

        public void down(bool[,] correct_path, char[,] mat, int min_x, int min_y)
        {
            for (int i = min_x; i > 0; i--)
            {
                mat[i, min_y] = mat[i - 1, min_y];
                correct_path[i, min_y] = correct_path[i - 1, min_y];
            }
            mat[0, min_y] = empty_char;
            correct_path[0, min_y] = false;
        }
        /// <summary>
        /// allow to handle a new word
        /// </summary>
        /// <param name="word">the word to compute</param>
        /// <returns>true is the word could be played, false otherwise</returns>
        public bool handle_word(string word)
        {
            bool found;
            bool[,] correct_path;
            (found, correct_path) = search(word);
            if (found)
            {
                update_matrix(correct_path); // update the matrix 
                return true;
            }
            else return false;
        }
        /// <summary>
        /// Saves the current state of a board to a CSV file in a specified folder.
        /// </summary>
        /// <param name="folder">The folder path where the saved boards will be stored. Default is "../../Annexes/saved_boards/".</param>
        /// <param name="file_name">The base name for the saved CSV files. Default is "board".</param>
        /// <param name="counter_path">The file path for storing the counter used in file names. Default is "counter.txt".</param>
        /// <remarks>
        ///   This function saves the current state of the board to a CSV file in the specified folder.
        ///   The file name is constructed by appending a counter value to the base file name.
        ///   The counter is read from a file, and after saving the board, the counter is incremented.
        ///   The new counter value is then written back to the file for the next save.
        ///   If any issues occur during the process, an exception is thrown with an error message.
        /// </remarks>
        public void save(string folder = "../../Annexes/saved_boards/", string file_name = "board", string counter_path = "counter.txt")
        {
            if(saved_board!=null && saved_board.Length > 0)
            {
                try
                {
                    StreamReader sr = new StreamReader(folder + counter_path); //open the counter file
                    string line = sr.ReadLine();
                    sr.Close();
                    int counter = Convert.ToInt32(line);
                    Console.WriteLine(folder + file_name + counter + ".csv");
                    StreamWriter sw = new StreamWriter(folder + file_name+counter+".csv");
                    for (int i = 0; i < saved_board.GetLength(0); i++) // copy the board into a new csv file
                    {
                        line = "";
                        for (int j = 0; j < saved_board.GetLength(1) - 1; j++)
                        {
                            line += saved_board[i, j] + separator;
                        }
                        line += saved_board[i, saved_board.GetLength(1) - 1];
                        sw.WriteLine(line);
                    }
                    sw.Close();
                    sw = new StreamWriter(folder + counter_path); // increment the counter
                    sw.WriteLine(counter + 1);
                    sw.Close();
                }
                catch
                {
                    throw new Exception("counter doesn't fit the requiered format or board isn't correct, coudn't save board");
                }
            }
        }

        public string toString(bool in_tab = false)
        {
            if (board == null) return "matrice nulle";
            else if (board.Length == 0) return "matrice vide";
            string str = "";
            for (int i = 0; i < board.GetLength(0); i++)
            {
                for (int j = 0; j < board.GetLength(1); j++)
                {
                    str += "____";
                }
                str += "\n\n| ";
                for (int j = 0; j < board.GetLength(1); j++)
                {
                    str += (board[i, j] + " | ");
                }
                str += "\n";
            }
            for (int j = 0; j < board.GetLength(1); j++)
            {
                str += "____";
            }
            str += "\n";
            return str;
        }
        /// <summary>
        /// checks whether the board is empty or not  
        /// </summary>
        /// <returns>true if empty, false otherwise</returns>
        public bool is_empty()
        {
            bool empty = true;
            if (board != null)
            {
                for (int j = 0; j < board.GetLength(1) && empty; j++)
                {
                    if (Char.IsLetter(board[board.GetLength(0) - 1, j])) empty = false;
                }
            }
            return empty;
        }
    }
}