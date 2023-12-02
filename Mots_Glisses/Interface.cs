using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Threading;

namespace Mots_Glisses
{
    internal class Interface
    {
        public static void menu(Jeu game)
        {
            bool is_submenu = true;
            int print_delay = 2000; // print delay in ms
            Console.WriteLine("Veuillez utiliser les flèches pour naviguer dans le menu et pressez entrer pour valider une commande");
            string[] cmd = new string[] { "Ajouter un joueur", "Supprimer un joueur", "Afficher les joueurs", "Lancer une partie" };
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
                if (key.Key == ConsoleKey.Enter) call_function(i);
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

                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Entrée non valide");
                        Thread.Sleep(print_delay);
                        break;
                }
            } 
        }

        public static void lauch_game_menu(Jeu game)
        {
            int print_delay = 2000; // print delay in ms
            string[] cmd = new string[] { "choisir son plateau", "générer un plateau aléatoire"};
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
                switch (num_ex)
                {
                    case 0:
                        List<string> names = Tools.get_csv_file_names_from_folder("../../Annexes");
                        start_game(game,names);
                        break;

                    case 1:
                        Console.WriteLine("A implémenter");
                        break;

                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Entrée non valide");
                        Thread.Sleep(print_delay);
                        break;
                }
            }

            
            
        }

        public static void start_game(Jeu game, List<string> names)
        {
            int print_delay = 2000; // print delay in ms
            Console.WriteLine("Veuillez utiliser les flèches pour naviguer dans le menu et pressez entrer pour valider une commande");
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
                    game.Board = new Plateau("../../Annexes/" + names[i]);
                    game.start();
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
    }
}