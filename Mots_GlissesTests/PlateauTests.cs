using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using Mots_Glisses;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mots_Glisses.Tests
{
    [TestClass()]
    public class PlateauTests
    {
        [TestMethod()]
        public void PlateauTest()
        {

            string path = "../../../Mots_Glisses/Annexes/Test/Plateau/invalide1.csv";
            Plateau board1 = new Plateau(path);
            if (board1.Board != null) Assert.Fail();

            path = "../../../Mots_Glisses/Annexes/Test/Plateau/invalide2.csv";
            Plateau board2 = new Plateau(path);
            if (board2.Board != null) Assert.Fail();

            path = "../../../Mots_Glisses/Annexes/Test/Plateau/invalide3.csv";
            Plateau board3 = new Plateau(path);
            if (board3.Board != null) Assert.Fail();

            path = "../../../Mots_Glisses/Annexes/Test/Plateau/invalide4.csv";
            Plateau board4 = new Plateau(path);
            if (board4.Board != null) Assert.Fail();

            path = "../../../Mots_Glisses/Annexes/Test/Plateau/valide1.csv";
            Plateau board5 = new Plateau(path);
            char[,] compare = new char[,]
            {
                { 'a', 'b', 'c' },
                { 'e', 'j', 's'},
                { 'p', 't', 'c'}
            };
            Tools.print_matrix_chars(compare);
            Tools.print_matrix_chars(board5.Board);
            if (!Tools.char_matrixs_equals(board5.Board, compare)) Assert.Fail();
        }

        [TestMethod()]
        public void searchTest()
        {
            bool found;
            bool[,] correct_path;
            string racine = "../../../Mots_Glisses/Annexes/Test/Plateau/search/";
            // check that we can't go twice on the same case
            Plateau board = new Plateau(racine+"avoid_repetitions.csv", true);
            (found, correct_path) = board.search("aaa");
            if (!found) Assert.Fail();
            (found, correct_path) = board.search("aaaa");
            if (found) Assert.Fail();
            // check that if the word exist but do not start in the last row it isn't found
            board = new Plateau(racine + "word_not_last_row.csv");
            (found, correct_path) = board.search("ain");
            if (found) Assert.Fail();
            (found, correct_path) = board.search("bon");
            if (found) Assert.Fail();
            // chech that if the word has multiple letters in last row doesn't impact search 
            (found, correct_path) = board.search("trs");
            if (!found) Assert.Fail();
            // check that if the word has multiple occurences in the board doesn't impact search
            board = new Plateau(racine + "twice_same_board.csv");
            (found, correct_path) = board.search("cou");
            if (!found) Assert.Fail();
            // check that diagonal reserach is working
            board.disable_diagonale_search();
            board = new Plateau(racine + "valide3.csv");
            (found, correct_path) = board.search("Romain");
            if (!found) Assert.Fail();
            (found, correct_path) = board.search("thOmas");
            if(found) Assert.Fail();
            board.enable_diagonale_search();
            (found, correct_path) = board.search("thOmas");
            if(!found) Assert.Fail();
        }
    }
}