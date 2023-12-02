using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mots_Glisses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mots_Glisses.Tests
{
    [TestClass()]
    public class JoueurTests
    {
        [TestMethod()]
        public void nb_joeursTest()
        {
            Joueur.Nb_player = 0;
            Joueur player1 = new Joueur();
            Joueur player2 = new Joueur();
            Joueur player3 = new Joueur();
            if (Joueur.Nb_player != 3) Assert.Fail();
            if (player3.Name != "Player3") Assert.Fail();
        }

        [TestMethod()]
        public void add_wordsTest()
        {
            Joueur player1 = new Joueur();
            if (player1.Words.Count() != 0) Assert.Fail();
            bool add = player1.add_word("bonjour");
            if (!add) Assert.Fail();
            bool add2 = player1.add_word("es");
            if (!add2) Assert.Fail();
            if (player1.Words.Count() != 2) Assert.Fail();
            bool add3 = player1.add_word("bonjour");
            if (add3) Assert.Fail();
            if (player1.Words.Count() != 2) Assert.Fail();
            if (player1.words_toString() != "bonjour es ") Assert.Fail();
        }

        [TestMethod()]
        public void scoreTest()
        {
            Joueur player1 = new Joueur("Didier");
            player1.add_score(5.595595);
            if (player1.Score != 5.595595) Assert.Fail();
            if (player1.get_score() != 5.60) Assert.Fail();
            if (player1.get_score(3) != 5.596) Assert.Fail();
            player1.reset_score();
            if (player1.get_score() != 0) Assert.Fail();
            player1.add_score(-12.4);
            if (player1.get_score(0) != -12) Assert.Fail();
        }
    }
}