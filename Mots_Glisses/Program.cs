using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using static System.Net.Mime.MediaTypeNames;

namespace Mots_Glisses
{
    internal class Program
    {

        static void Main(string[] args)
        {
            Jeu game = new Jeu(new Dictionnaire(), new Plateau()); // get the game
            Interface.menu(game); // enable the player to interact with the game using a command line interface
        }
    }
}