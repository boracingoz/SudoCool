using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts.Core
{
    public class SudokuGenerator
    {
        public enum LevelDificulty { Easy, Medium, Hard }

        public static int[,] GeneratePuzzle(LevelDificulty levelDificulty)
        {
            int[,] full = GenerateFullSolution();
            int[,] puzzle = (int[,])full.Clone();

            int removeCount;
            switch (levelDificulty)
            {
                case LevelDificulty.Easy:
                    removeCount = 30;
                    break;
                case LevelDificulty.Medium:
                    removeCount = 40;
                    break;
                case LevelDificulty.Hard:
                    removeCount = 50;
                    break;
                default:
                    removeCount = 40;
                    break;
            }

            RemoveCells(puzzle, removeCount);

            return puzzle;
        }

        public static int[,] GenerateFullSolution()
        {
            int[,] board = new int[9, 9]; // Fix the array size  
            FillDiagonalBoxes(board);
            SudokuSolver.SolveSudoku(board);

            return board;
        }

        private static void FillDiagonalBoxes(int[,] board)
        {
            for (int i = 0; i < 9; i += 3) // Fix the loop increment to fill diagonal boxes  
            {
                FillBox(board, i, i);
            }
        }

        private static void FillBox(int[,] board, int startRow, int startCol)
        {
            List<int> numbers = Enumerable.Range(1, 9).OrderBy(x => UnityEngine.Random.value).ToList(); // Use UnityEngine.Random  
            int index = 0;

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    board[startRow + i, startCol + j] = numbers[index++];
                }
            }
        }

        private static void RemoveCells(int[,] board, int removeCount)
        {
            int removed = 0;

            while (removed < removeCount)
            {
                int row = UnityEngine.Random.Range(0, 9); // Use UnityEngine.Random  
                int col = UnityEngine.Random.Range(0, 9); // Use UnityEngine.Random  

                if (board[row, col] != 0)
                {
                    board[row, col] = 0;
                    removed++;
                }
            }
        }
    }
}