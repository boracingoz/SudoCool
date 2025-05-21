using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SudokuValidator
{
   public static bool IsValidSudoku(int[,] board)
   {
        for (int i = 0; i < 9; i++)
        {
            HashSet<int> rowSet = new HashSet<int>();
            HashSet<int> colSet = new HashSet<int>();
            HashSet<int> boxSet = new HashSet<int>();

            for (global::System.Int32 j = 0; j < 9; j++)
            {
                int rowVal = board[i, j];
                if (rowVal != 0)
                {
                    if (rowSet.Contains(rowVal))
                    {
                        return false;
                    }
                    rowSet.Add(rowVal);
                }

                int colVal = board[j,i];
                if (colVal != 0)
                {
                    if (colSet.Contains(colVal))
                    {
                        return false;
                    }
                    colSet.Add(colVal);
                }

                int rowIndex = 3 * (i / 3) + j / 3;
                int colIndex = 3 * (i % 3) + j % 3;
                int boxVal = board[rowIndex, colIndex];
                if (boxVal != 0)
                {
                    if (boxSet.Contains(boxVal))
                    {
                        return false;
                    }
                    boxSet.Add(boxVal);
                }
            }
        }

        return true;
   }

}
