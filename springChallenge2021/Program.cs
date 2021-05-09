using System;
using System.Collections.Generic;
using System.Linq;

namespace springChallenge2021
{
    class Program
    {
        static void Main(string[] args)
        {
            var numberOfCells = int.Parse(Console.ReadLine());
            var forest = new List<Cell>();
            for (int i = 0; i < numberOfCells; i++)
            {
                forest.Add(new Cell());
            }

            // game loop
            while (true)
            {
                var gameTurn = new GameTurn(forest);

                int numberOfPossibleActions = int.Parse(Console.ReadLine()); // all legal actions
                for (int i = 0; i < numberOfPossibleActions; i++)
                {
                    string possibleAction = Console.ReadLine(); // try printing something from here to start with
                }

                var mostRichTree = gameTurn.TreeList
                    .Where(tree => tree.IsMine && !tree.IsDormant)
                    .OrderByDescending(tree => tree.Size)
                    .ThenByDescending(tree => tree.Richness)
                    .FirstOrDefault();

                // GROW cellIdx | SEED sourceIdx targetIdx | COMPLETE cellIdx | WAIT <message>
                if (mostRichTree == default(Tree)) {
                    Console.WriteLine("WAIT");
                } else if(mostRichTree.Size == TreeSize.Tall) {
                    Console.WriteLine($"COMPLETE {mostRichTree.Index}");
                } else {
                    Console.WriteLine($"GROW {mostRichTree.Index}");
                }
            }
        }
    }

    public class GameTurn {
        public int Day { get; private set; }

        public int Nutrients { get; private set; }

        public Player Me { get; private set; }

        public Player Opponent { get; private set; }

        public IList<Tree> TreeList { get; private set; }

        public GameTurn(IList<Cell> forest) {
            Day = int.Parse(Console.ReadLine()); // the game lasts 24 days: 0-23
            Nutrients = int.Parse(Console.ReadLine()); // the base score you gain from the next COMPLETE action
            Me = new Player(false);
            Opponent = new Player();
            var numberOfTrees = int.Parse(Console.ReadLine()); // the current amount of trees
            TreeList = new List<Tree>();
            for (int i = 0; i < numberOfTrees; i++) {
                TreeList.Add(new Tree(forest));
            }
        }
    }

    public class Tree {

        public int Index { get; private set; }

        public TreeSize Size { get; private set; }

        public Boolean IsMine { get; private set; }

        public Boolean IsDormant { get; private set; }

        public CellRichness Richness { get; private set; }

        public Tree(IList<Cell> forest) {
            var inputs = Console.ReadLine().Split(' ');
            Index = int.Parse(inputs[0]);
            Size = (TreeSize) Enum.Parse(typeof(TreeSize), inputs[1]);
            IsMine = inputs[2] == "1";
            IsDormant = inputs[3] == "1";
            Richness = forest.First(tree => tree.Index == Index).Richness;
        }
    }

    public class Player {

        public int SunPoint { get; private set; }

        public int Score { get; private set; }

        public Boolean IsWaiting { get; private set; }

        public Player(Boolean isWaiting) {
            var inputs = Console.ReadLine().Split(' ');
            SunPoint = int.Parse(inputs[0]);
            Score = int.Parse(inputs[1]);
            IsWaiting = isWaiting;
        }

        public Player() {
            var inputs = Console.ReadLine().Split(' ');
            SunPoint = int.Parse(inputs[0]);
            Score = int.Parse(inputs[1]);
            IsWaiting = inputs[2] != "0"; // whether your opponent is asleep until the next day
        }
    }

    public class Cell {
        public int Index { get; private set; }

        public CellRichness Richness { get; private set; }

        public Cell() {
            var inputs = Console.ReadLine().Split(' ');
            Index = int.Parse(inputs[0]); // 0 is the center cell, the next cells spiral outwards
            Richness = (CellRichness) Enum.Parse(typeof(CellRichness), inputs[1]);
            int neigh0 = int.Parse(inputs[2]); // the index of the neighbouring cell for each direction
            int neigh1 = int.Parse(inputs[3]);
            int neigh2 = int.Parse(inputs[4]);
            int neigh3 = int.Parse(inputs[5]);
            int neigh4 = int.Parse(inputs[6]);
            int neigh5 = int.Parse(inputs[7]);
        }
    }

    public enum CellRichness {
        Unusable,
        Bad,
        Medium,
        Good
    }

    public enum TreeSize {
        Seed,
        Small,
        Medium,
        Tall
    }
}