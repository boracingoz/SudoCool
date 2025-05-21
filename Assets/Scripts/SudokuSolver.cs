using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    public class SudokuSolver
    {
       public static bool SolveSudoku(int[,] board)
        {
            for (int row = 0; row < 9; row++)
            {
                for (global::System.Int32 col = 0; col < 9; col++)
                {
                    if (board[row, col] == 0)
                    {
                        for (global::System.Int32 num = 1; num <= 9; num++)
                        {
                            if (IsSafe(board, row, col, num))
                            {
                                board[row, col] = num;

                                if (SolveSudoku(board))
                                {
                                    return true;
                                }
                                board[row, col] = 0;
                            }
                        }
                        return false;
                    }
                }
            }
            return true;
        }

        public static bool IsSafe(int[,] board, int row, int col, int num)
        {
            for (int i = 0; i < 9; i++)
            {
                if (board[row,i] == num || board[i, col] == num)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
