using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Security.Permissions;
using System.Threading;
using System.Xml.Linq;

namespace Mots_Glisses
{
    internal class Interface
    {
        /// <summary>
        /// The home page of our game
        /// </summary>
        /// <param name="game"></param>
        public static void menu(Jeu game)
        {
            bool is_submenu = true;
            int print_delay = 2000; // print delay in ms
            Console.WriteLine("Veuillez utiliser les flèches pour naviguer dans le menu et pressez entrer pour valider une commande");
            string[] cmd = new string[] { "Ajouter un joueur", "Supprimer un joueur", "Afficher les joueurs", "Lancer une partie","Règles"};
            int i = 0;
            bool running = true;
            ConsoleKeyInfo key;
            char[,] random_mat = Tools.random_char_mat(8, 8);
            do
            {
                Console.Clear();
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Tools.print_center("************");
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Tools.print_center("Mots Glisses");
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Tools.print_center("************");
                Console.ResetColor();
                Console.WriteLine();
                display();
                key = Console.ReadKey();
                if (key.Key == ConsoleKey.UpArrow)
                {
                    if (i == 0) i = cmd.Length - 1;
                    else i--;
                }
                if (key.Key == ConsoleKey.DownArrow)
                {
                    if (i == cmd.Length-1) i = 0;
                    else i++;
                }
                if (key.Key == ConsoleKey.Enter)
                {
                    call_function(i);
                    Console.Clear();
                }
                if (key.Key == ConsoleKey.Escape) running = false;
            }
            while (running);
            void display()
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("Menu");
                Console.WriteLine("*********************");
                Console.ResetColor();
                for (int j=0; j < cmd.Length; j++)
                {
                    if (j == i)
                    {
                        Console.BackgroundColor = ConsoleColor.DarkGray;
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine(cmd[i]);
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.WriteLine(cmd[j]);
                    }
                }
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("*********************");
                Console.ResetColor();
            }
            void call_function(int num_ex)
            {
                Console.Clear();
                switch (num_ex)
                {
                    case 0:
                        Console.WriteLine("Entrez un nouveau pseudo");
                        string playerName = Console.ReadLine();
                        bool verification = game.add_player(playerName);

                        if (verification)
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("Le nouveau joueur a été ajouté avec succès");
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Le pseudo est déjà utilisé. Veuillez réessayer.");
                        }
                        Thread.Sleep(print_delay);
                        break;

                    case 1:
                        Console.WriteLine("Veuillez entrer le pseudo du joueur à supprimer");
                        string nameToDelete = Console.ReadLine();
                        bool isDeleted = game.delete_player(nameToDelete);

                        if (isDeleted)
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine($"Le joueur {nameToDelete} a bien été supprimé");
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine($"Le joueur {nameToDelete} n'existe pas");
                        }

                        Thread.Sleep(print_delay);
                        break;

                    case 2:
                        Console.WriteLine(game.players_toString());
                        Thread.Sleep(print_delay);
                        break;

                    case 3:
                        if(game.Players == null || game.Players.Count == 0)
                        {
                            Console.ForegroundColor = ConsoleColor.DarkRed;
                            Console.WriteLine("Il doit y avoir au moins un joueur pour lancer une partie");
                            Thread.Sleep(print_delay);
                            Console.ResetColor();
                        }
                        else
                        {
                            if(is_submenu)
                            {
                                lauch_game_menu(game);
                            }
                            else game.start();
                        }
                        break;
                    case 4:
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Tools.print_center("*********************");
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        Tools.print_center("Mots Glisses : Règles");
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Tools.print_center("*********************");
                        Console.WriteLine();
                        Console.ResetColor();
                        Console.WriteLine($"Le jeu est composé de {game.Nb_round_by_player} tours de {game.Player_time}s par joueur.");
                        Console.WriteLine("Le jeu se termine lorsque le plateau est vide ou que le nombre de tours par joueur est écoulé.");
                        Console.WriteLine("A chaque tour vous allez devoir trouver le plus de mots possibles dans le plateau. La première lettre d'un mot doit impérativement se situer dans la dernière ligne du plateau.");
                        Console.WriteLine("Une fois un mot trouvé le plateau s'actualise et vous continuez de jouer jusqu'à ce que le temps soit écoulé.");
                        Console.WriteLine();
                        Console.WriteLine();
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.WriteLine("Calculs des scores");
                        Console.WriteLine("*******************");
                        Console.ResetColor();
                        Console.WriteLine("Le calcul des scores prend en compte la longeur de chaque mot ainsi que les lettres qui les composent.");
                        Console.WriteLine($"Un mot de n lettres rapporte {game.Length_score_multiplicator}*n points.");
                        Console.ResetColor();
                        Dictionary<string, int> weight = new Dictionary<string, int>();
                        bool verif;
                        (verif, weight) = game.get_weighting();
                        Console.WriteLine();
                        if (verif) Tools.print_dictionary(weight);
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.DarkRed;
                            Console.WriteLine("Erreur dans l'importation du fichier de pondération");
                            Console.ResetColor();
                        }
                        Console.ReadKey();
                        break;    

                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Entrée non valide");
                        Thread.Sleep(print_delay);
                        break;
                }
            } 
        }
        /// <summary>
        /// Allow the player to lauch a game from the file and manner of its choice  
        /// </summary>
        /// <param name="game">our game object</param>
        public static void lauch_game_menu(Jeu game)
        {
            int print_delay = 2000; // print delay in ms
            string[] cmd = new string[] { "choisir un plateau à thème", "générer un plateau aléatoire", "choisir un plateau sauvegardé"};
            int i = 0;
            bool running = true;
            ConsoleKeyInfo key;
            char[,] random_mat = Tools.random_char_mat(8, 8);
            do
            {
                Console.Clear();
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Tools.print_center("************");
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Tools.print_center("Mots Glisses");
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Tools.print_center("************");
                Console.ResetColor();
                Console.WriteLine();
                display();
                key = Console.ReadKey();
                if (key.Key == ConsoleKey.UpArrow)
                {
                    if (i == 0) i = cmd.Length - 1;
                    else i--;
                }
                if (key.Key == ConsoleKey.DownArrow)
                {
                    if (i == cmd.Length - 1) i = 0;
                    else i++;
                }
                if (key.Key == ConsoleKey.Enter) call_function(i);
                if (key.Key == ConsoleKey.LeftArrow || key.Key == ConsoleKey.Escape) menu(game);
            }
            while (running);
            void display()
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("Lancer une partie");
                Console.WriteLine("*********************");
                Console.ResetColor();
                for (int j = 0; j < cmd.Length; j++)
                {
                    if (j == i)
                    {
                        Console.BackgroundColor = ConsoleColor.Gray;
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine(cmd[i]);
                        Console.ResetColor();
                    }
                    else Console.WriteLine(cmd[j]);
                }
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("*********************");
                Console.ResetColor();
            }
            void call_function(int num_ex)
            {
                Console.Clear();
                string sub_folder;
                switch (num_ex)
                {
                    case 0:
                        sub_folder = "checked_boards";
                        List<string> names = Tools.get_file_names_from_folder("../../Annexes/"+sub_folder,"csv");
                        start_game(game, names, sub_folder);
                        break;

                    case 1:
                        sub_folder = "generation_file";
                        names = Tools.get_file_names_from_folder("../../Annexes/"+sub_folder,"txt");
                        start_game(game, names, sub_folder,true);
                        break;
                    case 2:
                        sub_folder = "saved_boards";
                        names = Tools.get_file_names_from_folder("../../Annexes/"+sub_folder, "csv");
                        start_game(game, names, sub_folder);
                        break;

                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Entrée non valide");
                        Thread.Sleep(print_delay);
                        break;
                }
            }
        }

        /// <summary>
        /// enable to select a file located in root/sub_folder and lauch a game from it
        /// </summary>
        /// <param name="game">the game object</param>
        /// <param name="names">a list containing the names of all the file in the folder</param>
        /// <param name="sub_folder">a folder located in root</param>
        /// <param name="enable_save"></param>
        public static void start_game(Jeu game, List<string> names, string sub_folder, bool enable_save = false)
        {
            int print_delay = 2000; // print delay in ms
            Console.WriteLine("Veuillez utiliser les flèches pour naviguer dans le menu et pressez entrer pour valider une commande");
            int i = 0;
            bool running = true;
            ConsoleKeyInfo key;
            do
            {
                Console.Clear();
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Tools.print_center("************");
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Tools.print_center("Mots Glisses");
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Tools.print_center("************");
                Console.ResetColor();
                Console.WriteLine();
                display();
                key = Console.ReadKey();
                if (key.Key == ConsoleKey.UpArrow)
                {
                    if (i == 0) i = names.Count - 1;
                    else i--;
                }
                if (key.Key == ConsoleKey.DownArrow)
                {
                    if (i == names.Count - 1) i = 0;
                    else i++;
                }
                if (key.Key == ConsoleKey.Enter)
                {
                    game.Board = new Plateau($"../../Annexes/{sub_folder}/{names[i]}");
                    game.start();
                    if (enable_save)
                    {
                        save_menu(game);
                    }
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine("Appuyer sur une touche pour quitter la page");
                    Console.ResetColor();
                    Console.ReadKey();
                    menu(game);
                }
                if (key.Key == ConsoleKey.LeftArrow || key.Key == ConsoleKey.Escape)
                {
                    lauch_game_menu(game);
                }
            }
            while (running);
            void display()
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("Select Board");
                Console.WriteLine("*********************");
                Console.ResetColor();
                for (int j = 0; j < names.Count; j++)
                {
                    if (j == i)
                    {
                        Console.BackgroundColor = ConsoleColor.Gray;
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine(names[i]);
                        Console.ResetColor();
                    }
                    else Console.WriteLine(names[j]);
                }
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("*********************");
                Console.ResetColor();
            }
        }
        /// <summary>
        /// The menu to save a randomly generated board 
        /// </summary>
        /// <param name="game"></param>
        public static void save_menu(Jeu game)
        {
            ConsoleKeyInfo key;
            Console.WriteLine();
            Console.WriteLine(Tools.toString_mat(game.Board.Saved_board));
            Console.WriteLine("Appuyer sur s pour sauvegarder le plateau ou sur une autre touche pour passer");
            key = Console.ReadKey();
            if(key.Key == ConsoleKey.S)
            {
                try
                {
                    game.Board.save();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Le plateau a bien été sauvegardé");
                    Console.ResetColor();
                }
                catch
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("Une erreur est surevenue durant l'enregistrement");
                    Console.ResetColor();
                }
            }
            else
            {
                Console.WriteLine("Le plateau n'a pas été sauvegardé");
            }
        }
    }
}
