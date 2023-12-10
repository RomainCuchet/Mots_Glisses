using System;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NAudio.Wave;

namespace Mots_Glisses
{
    internal class Jeu
    {
        Dictionnaire dictionary;
        Plateau board;
        List<Joueur> players = null;
        bool running = true;
        string score_weighting_path;
        int player_time;
        char separator = ',';
        int nb_round_by_player;
        int nb_lasting_round;
        double length_score_multiplicator;
        int print_delay = 2000; // en ms
        string sound_folder;

        private WaveOutEvent outputDevice;

        public Plateau Board
        {
            get { return board; }
            set { board = value; }
        }

        public List<Joueur> Players
        {
            get { return players; }
        }

        public int Nb_round_by_player
        {
            get { return nb_round_by_player; }
        }

        public int Player_time
        {
            get { return player_time; }
        }

        public double Length_score_multiplicator
        {
            get { return length_score_multiplicator; }
        }

        public Jeu(Dictionnaire dictionary, Plateau board, List<Joueur> players = null, int nb_round_by_player = 2, int player_time = 30, string score_weighting_path = "../../Annexes/generation_file/letters.txt", double length_score_multiplicator = 2, string sound_folder = "../../Annexes/sounds/") // time in secondes
        {
            this.dictionary = dictionary;
            this.board = board;
            this.player_time = player_time;
            this.score_weighting_path = score_weighting_path;
            this.nb_round_by_player = nb_round_by_player;
            this.players = players;
            this.length_score_multiplicator = length_score_multiplicator;
            this.sound_folder = sound_folder;
        }



        public void start()
        {

            if (players == null || players.Count < 1)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("Il doit y avoir au moins un joueur pour lancer une partie");
                Thread.Sleep(print_delay);
                Console.ResetColor();
            }
            else if (board.Board == null || board.Board.Length == 0)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("Erreur dans la génération du plateau");
                Thread.Sleep(print_delay);
                Console.ResetColor();
            }
            else
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Tools.print_center("************");
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Tools.print_center("Mots Glisses");
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Tools.print_center("************");
                Console.ResetColor();
                Console.WriteLine();
                Console.WriteLine();
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.ResetColor();
                Console.WriteLine($"Bon courage\n{players_toString()}");
                StopMp3();
                PlayMp3(sound_folder+"good_luck.mp3");
                Console.WriteLine("Que le meilleur gagne !");
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine();
                Thread.Sleep(4000);
                nb_lasting_round = nb_round_by_player * players.Count(); // get the total number of rounds according to the given parameters
                int i = 0;
                while (nb_lasting_round > 0 && running)
                {
                    Console.Clear();
                    Console.WriteLine($"Prépare toi {players[i].Name} ton tour va commencer");
                    Thread.Sleep(print_delay);
                    round(players[i]);
                    nb_lasting_round--;
                    i++;
                    if (i == players.Count) i = 0;
                }
                game_over();
            }
        }

        public void game_over()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("Le jeu est terminé !");
            Console.ResetColor();
            (bool valid, Dictionary<string, int> weighting) = get_weighting();
            if (valid) assign_scores(weighting);
            else
            {
                assign_scores(null);
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("Erreur dans l'importation du fichier de pondération, on calculera les scores sans prendre en compte les lettres qui compose le mot");
                Console.ResetColor();
            }
            sort_players();
            StopMp3();
            PlayMp3(sound_folder+"applause.mp3");
            Console.WriteLine($"Féliciation {players[0].Name} tu es premier avec un score de {players[0].Score} !");
            Console.WriteLine($"Les mots que tu as trouvé sont :  {players[0].found_words_toString()}");
            for (int i = 1; i < players.Count; i++)
            {
                Console.WriteLine();
                Console.WriteLine($"{players[i].Name} tu es à la {i + 1} ème position avec {players[i].Score} points");
                Console.WriteLine($"Les mots que tu as trouvé sont :  {players[i].found_words_toString()}");
            }
            foreach (Joueur player in players)
            {
                player.reset_score();
                player.reset_found_words();
            }
            running = true;
        }

        public bool add_player(string name)
        {
            Joueur player = new Joueur(name);
            if (players == null)
            {
                players = new List<Joueur>();
                players.Add(player);
                return true;
            }
            else
            {
                bool not_exist = true;
                for (int i = 0; not_exist && i < players.Count; i++)
                {
                    if (players[i].Name == player.Name) not_exist = false;
                }
                if (not_exist) players.Add(player);
                return not_exist;
            }
        }

        public bool delete_player(string name)
        {
            if (players == null) return false;
            else
            {
                bool deleted = false;
                for (int i = 0; !deleted && i < players.Count; i++)
                {
                    if (name == players[i].Name)
                    {
                        players.RemoveAt(i);
                        deleted = true;
                    }
                }
                return deleted;
            }
        }
        public void sort_players()
        {
            if (players != null && players.Count != 0)
            {
                Joueur p;
                for (int i = 0; i < players.Count; i++) // there is no use to improuve our sorting algorithm because the number of player will always be very low
                {
                    for (int j = 0; j < players.Count - 1; j++)
                    {
                        if (players[j] < players[j + 1])
                        {
                            p = players[j + 1];
                            players[j + 1] = players[j];
                            players[j] = p;
                        }
                    }
                }
            }
        }

        public string players_toString()
        {
            if (players == null || players.Count < 1) return "aucun joueur";
            string res = "";
            foreach (Joueur player in players) res += $"{player.Name}\n";
            return res.Substring(0, res.Length - 1);

        }

        public (bool, Dictionary<string, int>) get_weighting()
        {
            Dictionary<string, int> weighting = new Dictionary<string, int>();
            try
            {
                StreamReader sr = new StreamReader(score_weighting_path);
                string line;
                string[] string_tab;
                while ((line = sr.ReadLine()) != null)
                {
                    string_tab = line.Split(separator);
                    try
                    {
                        weighting.Add(string_tab[0], Convert.ToInt32(string_tab[2]));
                    }
                    catch (ArgumentException)
                    {
                        Console.WriteLine($"an element with key {string_tab[0]} already exist");
                        return (false, weighting);
                    }
                }
                sr.Close();
                return (true, weighting);
            }
            catch (Exception e)
            {
                // Let the user know what went wrong.
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
                return (false, weighting);
            }
        }

        public void assign_scores(Dictionary<string, int> weighting)
        {
            double score;
            foreach (Joueur player in players) // assign its score to each player
            {
                score = 0;
                foreach (string w in player.Words)
                {
                    string word = w.ToUpper();
                    score += word.Length * length_score_multiplicator; // increase score according to the length of the word and length_score_multiplicator
                    if (weighting != null)
                    {
                        foreach (char letter in word) // increase score according to the weighting of each letter
                        {
                            try { score += weighting[char.ToString(letter)]; }
                            catch { throw new Exception($"key {letter} is missing in weighting"); };
                        }
                    }
                }
                player.add_score(score);
            }
        }

        public void round(Joueur player)
        {
            DateTime start_time = DateTime.Now;
            string word;
            int counter = 0;
            do
            {
                Console.Clear();
                Console.Clear();
                Console.WriteLine($"La main est à {player.Name} pour {Math.Round(player_time - (DateTime.Now - start_time).TotalSeconds, 1)} secondes");
                Console.WriteLine();
                Console.WriteLine(board.toString());
                Console.WriteLine("Entrez un mot présent dans le plateau");
                word = Console.ReadLine();
                Console.ForegroundColor = ConsoleColor.DarkRed;
                if ((DateTime.Now - start_time).TotalSeconds < player_time) // we must check the time
                {
                    if (word == null || word.Length < 2)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine("Votre mot doit avoir une longeur supérieure à 1");
                        StopMp3();
                        PlayMp3(sound_folder+"oh_no.mp3");
                        Thread.Sleep(print_delay);
                        Console.ResetColor();
                    }
                    else
                    {
                        if (!player.is_previously_found(word))
                        {
                            if (dictionary.search(word)) // if the word is in the given dictionnary try to handle it by the board
                            {
                                if (board.handle_word(word)) // handle the word and update the board if possible
                                {
                                    StopMp3();
                                    PlayMp3(sound_folder+"well_done.mp3");
                                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                                    Console.WriteLine($"Félicitation vous avez trouvé {word}");
                                    Thread.Sleep(print_delay);
                                    player.add_word(word); // save the word as found by the player
                                    counter++;
                                    if (board.is_empty())
                                    {
                                        running = false;
                                    }
                                }
                                else
                                {
                                    Console.WriteLine($"Il est impossible d'écrire {word} dans le plateau");
                                    StopMp3();
                                    PlayMp3(sound_folder+"oh_no.mp3");
                                    Thread.Sleep(print_delay);
                                }
                            }
                            else
                            {
                                Console.WriteLine($"{word} n'est pas dans le dictionnaire français");
                                StopMp3();
                                PlayMp3(sound_folder+"oh_no.mp3");
                                Thread.Sleep(print_delay);
                            }
                        }
                        else
                        {
                            Console.WriteLine($"Dommage vous avez déjà joué le mot {word}. Vous ne pouvez plus l'utiliser ;)");
                            StopMp3();
                            PlayMp3(sound_folder+"oh_no.mp3");
                            Thread.Sleep(print_delay);
                        }
                    }
                }
                else
                {
                    Console.WriteLine($"Trop tard ! Vous n'avez pas eu le temps de jouer {word}");
                    StopMp3();
                    PlayMp3(sound_folder+"oh_no.mp3");
                    Thread.Sleep(print_delay);
                }
                Console.ResetColor();
            }
            while ((DateTime.Now - start_time).TotalSeconds < player_time && running);
            if (running) Console.WriteLine($"Le temps est écoulé. Vous avez trouvé {counter} mots");
            else Console.WriteLine($"La matrice est vide. Vous avez trouvé {counter} mots");
            Thread.Sleep(print_delay);
        }

        /// <summary>
        /// Play a song. when the file has finished, outputDevice is disposed
        /// </summary>
        /// <param name="filePath">Path of the mp3 file</param>
        public void PlayMp3(string filePath)
        {
            using (var audioFile = new AudioFileReader(filePath))
            {
                outputDevice = new WaveOutEvent(); // Create a WaveOutEvent object to output the audio with the good format
                outputDevice.PlaybackStopped += (s, e) => outputDevice.Dispose(); // When the audio has stopped, outputDevice is disposed to free ressources
                outputDevice.Init(audioFile); // Initialize outputDevice to play the file
                outputDevice.Play(); // Play the file
            }
        }

        /// <summary>
        /// Stops the music and disposes outputDevice
        /// </summary>
        public void StopMp3()
        {
            if (outputDevice != null) // If there is a file in process
            {
                outputDevice.Stop(); // Stops the audio
                outputDevice.Dispose(); // Disposes outputDevice to free ressources
                outputDevice = null; // outputDevice is free
            }
        }
    }

}
