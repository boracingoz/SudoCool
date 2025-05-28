using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SudokuGame.Core
{
    /// <summary>
    /// Advanced Sudoku solver with multiple algorithms and optimization strategies
    /// Works seamlessly with SudokuValidator for placement validation
    /// </summary>
    public static class SudokuSolver
    {
        #region Public Solving Methods

        /// <summary>
        /// Solves sudoku puzzle using backtracking algorithm
        /// </summary>
        /// <param name="board">9x9 sudoku board to solve</param>
        /// <returns>True if solved successfully, false if unsolvable</returns>
        public static bool SolveSudoku(int[,] board)
        {
            if (board == null || board.GetLength(0) != 9 || board.GetLength(1) != 9)
            {
                Debug.LogError("Invalid board size - must be 9x9");
                return false;
            }

            return SolveBacktracking(board);
        }

        /// <summary>
        /// Solves sudoku with step counting for performance analysis
        /// </summary>
        /// <param name="board">9x9 sudoku board to solve</param>
        /// <param name="steps">Number of steps taken to solve</param>
        /// <returns>True if solved successfully, false if unsolvable</returns>
        public static bool SolveSudokuWithSteps(int[,] board, out int steps)
        {
            steps = 0;
            if (board == null || board.GetLength(0) != 9 || board.GetLength(1) != 9)
            {
                Debug.LogError("Invalid board size - must be 9x9");
                return false;
            }

            return SolveBacktrackingWithSteps(board, ref steps);
        }

        /// <summary>
        /// Solves sudoku using optimized backtracking with MRV (Most Constraining Variable) heuristic
        /// </summary>
        /// <param name="board">9x9 sudoku board to solve</param>
        /// <returns>True if solved successfully, false if unsolvable</returns>
        public static bool SolveSudokuOptimized(int[,] board)
        {
            if (board == null || board.GetLength(0) != 9 || board.GetLength(1) != 9)
            {
                Debug.LogError("Invalid board size - must be 9x9");
                return false;
            }

            return SolveOptimizedBacktracking(board);
        }

        /// <summary>
        /// Checks if a sudoku puzzle has a unique solution
        /// </summary>
        /// <param name="board">9x9 sudoku board to check</param>
        /// <returns>True if unique solution exists, false otherwise</returns>
        public static bool HasUniqueSolution(int[,] board)
        {
            if (board == null || board.GetLength(0) != 9 || board.GetLength(1) != 9)
            {
                return false;
            }

            // Create a copy to avoid modifying original
            int[,] copy = (int[,])board.Clone();
            return CountSolutions(copy, 2) == 1; // Stop counting after 2 solutions found
        }

        /// <summary>
        /// Gets the first valid number that can be placed at a position
        /// </summary>
        /// <param name="board">9x9 sudoku board</param>
        /// <param name="row">Row index (0-8)</param>
        /// <param name="col">Column index (0-8)</param>
        /// <returns>First valid number (1-9) or 0 if none valid</returns>
        public static int GetFirstValidNumber(int[,] board, int row, int col)
        {
            for (int num = 1; num <= 9; num++)
            {
                if (SudokuValidator.CanPlaceNumber(board, row, col, num))
                {
                    return num;
                }
            }
            return 0;
        }

        /// <summary>
        /// Gets all valid numbers that can be placed at a position
        /// </summary>
        /// <param name="board">9x9 sudoku board</param>
        /// <param name="row">Row index (0-8)</param>
        /// <param name="col">Column index (0-8)</param>
        /// <returns>List of valid numbers (1-9)</returns>
        public static List<int> GetValidNumbers(int[,] board, int row, int col)
        {
            var validNumbers = new List<int>();

            for (int num = 1; num <= 9; num++)
            {
                if (SudokuValidator.CanPlaceNumber(board, row, col, num))
                {
                    validNumbers.Add(num);
                }
            }

            return validNumbers;
        }

        #endregion

        #region Private Solving Algorithms

        /// <summary>
        /// Basic backtracking algorithm
        /// </summary>
        private static bool SolveBacktracking(int[,] board)
        {
            // Find first empty cell
            for (int row = 0; row < 9; row++)
            {
                for (int col = 0; col < 9; col++)
                {
                    if (board[row, col] == 0)
                    {
                        // Try numbers 1-9
                        for (int num = 1; num <= 9; num++)
                        {
                            if (SudokuValidator.CanPlaceNumber(board, row, col, num))
                            {
                                board[row, col] = num;

                                // Recursively solve
                                if (SolveBacktracking(board))
                                {
                                    return true;
                                }

                                // Backtrack
                                board[row, col] = 0;
                            }
                        }
                        return false; // No valid number found
                    }
                }
            }
            return true; // Board is complete
        }

        /// <summary>
        /// Backtracking with step counting
        /// </summary>
        private static bool SolveBacktrackingWithSteps(int[,] board, ref int steps)
        {
            steps++;

            // Find first empty cell
            for (int row = 0; row < 9; row++)
            {
                for (int col = 0; col < 9; col++)
                {
                    if (board[row, col] == 0)
                    {
                        // Try numbers 1-9
                        for (int num = 1; num <= 9; num++)
                        {
                            if (SudokuValidator.CanPlaceNumber(board, row, col, num))
                            {
                                board[row, col] = num;

                                // Recursively solve
                                if (SolveBacktrackingWithSteps(board, ref steps))
                                {
                                    return true;
                                }

                                // Backtrack
                                board[row, col] = 0;
                            }
                        }
                        return false; // No valid number found
                    }
                }
            }
            return true; // Board is complete
        }

        /// <summary>
        /// Optimized backtracking using MRV (Most Constraining Variable) heuristic
        /// </summary>
        private static bool SolveOptimizedBacktracking(int[,] board)
        {
            // Find empty cell with minimum remaining values (MRV heuristic)
            var emptyCell = FindBestEmptyCell(board);

            if (emptyCell == null)
            {
                return true; // Board is complete
            }

            int row = emptyCell.Value.row;
            int col = emptyCell.Value.col;

            // Get valid numbers for this cell
            var validNumbers = GetValidNumbers(board, row, col);

            // Try each valid number
            foreach (int num in validNumbers)
            {
                board[row, col] = num;

                // Recursively solve
                if (SolveOptimizedBacktracking(board))
                {
                    return true;
                }

                // Backtrack
                board[row, col] = 0;
            }

            return false; // No solution found
        }

        /// <summary>
        /// Counts the number of solutions (up to maxSolutions)
        /// </summary>
        private static int CountSolutions(int[,] board, int maxSolutions)
        {
            return CountSolutionsRecursive(board, 0, maxSolutions);
        }

        private static int CountSolutionsRecursive(int[,] board, int currentSolutions, int maxSolutions)
        {
            if (currentSolutions >= maxSolutions)
            {
                return currentSolutions; // Early termination
            }

            // Find first empty cell
            for (int row = 0; row < 9; row++)
            {
                for (int col = 0; col < 9; col++)
                {
                    if (board[row, col] == 0)
                    {
                        int solutionCount = 0;

                        // Try numbers 1-9
                        for (int num = 1; num <= 9; num++)
                        {
                            if (SudokuValidator.CanPlaceNumber(board, row, col, num))
                            {
                                board[row, col] = num;

                                // Recursively count solutions
                                solutionCount = CountSolutionsRecursive(board, solutionCount, maxSolutions);

                                // Backtrack
                                board[row, col] = 0;

                                if (solutionCount >= maxSolutions)
                                {
                                    return solutionCount;
                                }
                            }
                        }
                        return solutionCount;
                    }
                }
            }
            return currentSolutions + 1; // Found a complete solution
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Finds the empty cell with minimum remaining values (MRV heuristic)
        /// </summary>
        private static (int row, int col)? FindBestEmptyCell(int[,] board)
        {
            (int row, int col)? bestCell = null;
            int minOptions = 10; // More than maximum possible (9)

            for (int row = 0; row < 9; row++)
            {
                for (int col = 0; col < 9; col++)
                {
                    if (board[row, col] == 0)
                    {
                        int optionCount = GetValidNumbers(board, row, col).Count;

                        if (optionCount == 0)
                        {
                            return null; // No valid options - unsolvable state
                        }

                        if (optionCount < minOptions)
                        {
                            minOptions = optionCount;
                            bestCell = (row, col);

                            if (minOptions == 1)
                            {
                                return bestCell; // Found cell with only one option
                            }
                        }
                    }
                }
            }

            return bestCell;
        }

        /// <summary>
        /// Checks if the current board state is solvable
        /// </summary>
        /// <param name="board">9x9 sudoku board</param>
        /// <returns>True if potentially solvable, false if definitely unsolvable</returns>
        public static bool IsPotentiallySolvable(int[,] board)
        {
            // First check if current state is valid
            if (!SudokuValidator.IsValidSudoku(board))
            {
                return false;
            }

            // Check if any empty cell has no valid options
            for (int row = 0; row < 9; row++)
            {
                for (int col = 0; col < 9; col++)
                {
                    if (board[row, col] == 0)
                    {
                        bool hasValidOption = false;
                        for (int num = 1; num <= 9; num++)
                        {
                            if (SudokuValidator.CanPlaceNumber(board, row, col, num))
                            {
                                hasValidOption = true;
                                break;
                            }
                        }

                        if (!hasValidOption)
                        {
                            return false; // Found empty cell with no valid options
                        }
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Estimates the difficulty of a sudoku puzzle
        /// </summary>
        /// <param name="board">9x9 sudoku board</param>
        /// <returns>Difficulty score (higher = more difficult)</returns>
        public static int EstimateDifficulty(int[,] board)
        {
            int[,] copy = (int[,])board.Clone();
            int steps = 0;

            SolveSudokuWithSteps(copy, out steps);

            // Calculate difficulty based on:
            // - Number of empty cells
            // - Steps required to solve
            // - Minimum options available

            int emptyCells = 0;
            int minOptionsSum = 0;

            for (int row = 0; row < 9; row++)
            {
                for (int col = 0; col < 9; col++)
                {
                    if (board[row, col] == 0)
                    {
                        emptyCells++;
                        minOptionsSum += GetValidNumbers(board, row, col).Count;
                    }
                }
            }

            // Difficulty formula (can be tweaked)
            int difficulty = steps + (emptyCells * 10) + (minOptionsSum > 0 ? (emptyCells * 81) / minOptionsSum : 0);

            return difficulty;
        }

        #endregion

        #region Public Utility Methods

        /// <summary>
        /// Creates a deep copy of the sudoku board
        /// </summary>
        /// <param name="board">Original board</param>
        /// <returns>Deep copy of the board</returns>
        public static int[,] CloneBoard(int[,] board)
        {
            if (board == null) return null;

            int[,] clone = new int[9, 9];
            for (int row = 0; row < 9; row++)
            {
                for (int col = 0; col < 9; col++)
                {
                    clone[row, col] = board[row, col];
                }
            }
            return clone;
        }

        /// <summary>
        /// Fills obvious cells (cells with only one possible value)
        /// </summary>
        /// <param name="board">9x9 sudoku board</param>
        /// <returns>Number of cells filled</returns>
        public static int FillObviousCells(int[,] board)
        {
            int filled = 0;
            bool foundAny = true;

            while (foundAny)
            {
                foundAny = false;

                for (int row = 0; row < 9; row++)
                {
                    for (int col = 0; col < 9; col++)
                    {
                        if (board[row, col] == 0)
                        {
                            var validNumbers = GetValidNumbers(board, row, col);
                            if (validNumbers.Count == 1)
                            {
                                board[row, col] = validNumbers[0];
                                filled++;
                                foundAny = true;
                            }
                        }
                    }
                }
            }

            return filled;
        }

        #endregion
    }
}