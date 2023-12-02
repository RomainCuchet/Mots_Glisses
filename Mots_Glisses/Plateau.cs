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
        bool enable_diagonal_search;
        string separator = ";";
        char empty_char = ' ';

        public char[,] Board { get { return board; } }
        public bool Enable_diagonal_search { get { return enable_diagonal_search; } }

        public Plateau(string fichier = "../../Annexes/board1.csv", bool enable_diagonal_search = true)
        {
            this.enable_diagonal_search = enable_diagonal_search;
            if (fichier.Substring(fichier.Length - 3) == "csv")
            {
                try
                {
                    List<string> Liste = new List<string>();
                    StreamReader sr = new StreamReader(fichier); // trows an exeption if not possible
                    string s;
                    int taille = -1;
                    bool verif = true;
                    while ((s = sr.ReadLine()) != null && verif)
                    {
                        verif = Is_Valid(s);
                        if (taille == -1) taille = s.Length;
                        if (taille == s.Length) verif = Is_Valid(s);
                        s = s.Replace(separator, "");
                        s = s.ToLower();
                        Liste.Add(s);
                    }
                    if (verif)
                    {
                        board = new char[Liste.Count, taille/2+1]; // in taille we counted all the separator. There isn't one at the end
                        for (int i = 0; i < Liste.Count; i++)
                        {
                            for (int j = 0; j < Liste[i].Length; j++)
                            {
                                board[i, j] = Liste[i][j];
                            }
                        }
                    }
                    sr.Close();
                }
                catch (Exception e)
                {
                    // Let the user know what went wrong.
                    Console.WriteLine("The file couldn't be read:");
                    Console.WriteLine(e.Message);
                }
            }
            else if (fichier.Substring(fichier.Length - 3) == "txt")
            {
                Console.WriteLine("not implemented yet");
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
        /// save the current board in a new csv file
        /// </summary>
        /// <param name="folder">the saving folder</param>
        /// <param name="file_name">the generic name of our saved board, will be completed afterward</param>
        /// <param name="counter_path">the path to the counter</param>
        /// <exception cref="Exception">raise if it's impossible to save the board</exception>
        public void save(string folder = "../../Annexes/saved_boards/", string file_name = "board", string counter_path = "counter.txt")
        {
            if(board!=null && board.Length > 0)
            {
                try
                {
                    StreamReader sr = new StreamReader(folder + counter_path); //open the counter file
                    string line = sr.ReadLine();
                    sr.Close();
                    int counter = Convert.ToInt32(line);
                    Console.WriteLine(folder + file_name + counter + ".csv");
                    StreamWriter sw = new StreamWriter(folder + file_name+counter+".csv");
                    Tools.print_mat(board);
                    for (int i = 0; i < board.GetLength(0); i++) // copy the board into a new csv file
                    {
                        line = "";
                        for (int j = 0; j < board.GetLength(1) - 1; j++)
                        {
                            line += board[i, j] + separator;
                        }
                        line += board[i, board.GetLength(1) - 1];
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
            if(board == null)
            {
                return "board is null";
            }
            if (!in_tab)
            {
                return Tools.toString_mat(board);
            }
            else
            {
                int rows = board.GetLength(0);
                int cols = board.GetLength(1);
                string result = "";
                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < cols; j++)
                    {
                        result += $"|{board[i, j]}";
                    }
                    result += "|"+ Environment.NewLine;
                    for (int j = 0; j < cols; j++)
                    {
                        result += "--";
                    }
                    result += Environment.NewLine;
                }
                result += Environment.NewLine;
                result.Replace(Environment.NewLine, "\r");
                return result;
            }
        }
        /// <summary>
        /// checks whether the board is empty or not  
        /// </summary>
        /// <returns>true if empty, false otherwise</returns>
        public bool is_empty()
        {
            if (board == null) return true;
            foreach(char c in board) if(c!=empty_char) return false;
            return true;
        }
    }
}