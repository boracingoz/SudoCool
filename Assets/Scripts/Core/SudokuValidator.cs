using System.Collections.Generic;
using UnityEngine;

public class SudokuValidator
{
    /// <summary>
    /// Sudoku tahtasının geçerli olup olmadığını kontrol eder
    /// </summary>
    /// <param name="board">9x9 Sudoku tahtası</param>
    /// <returns>Geçerli ise true, değilse false</returns>
    public static bool IsValidSudoku(int[,] board)
    {
        // Satır kontrolü
        for (int row = 0; row < 9; row++)
        {
            if (!IsValidUnit(GetRow(board, row)))
            {
                Debug.Log($"Invalid row: {row}");
                return false;
            }
        }

        // Sütun kontrolü  
        for (int col = 0; col < 9; col++)
        {
            if (!IsValidUnit(GetColumn(board, col)))
            {
                Debug.Log($"Invalid column: {col}");
                return false;
            }
        }

        // 3x3 kutu kontrolü
        for (int boxRow = 0; boxRow < 3; boxRow++)
        {
            for (int boxCol = 0; boxCol < 3; boxCol++)
            {
                if (!IsValidUnit(GetBox(board, boxRow, boxCol)))
                {
                    Debug.Log($"Invalid box: ({boxRow}, {boxCol})");
                    return false;
                }
            }
        }

        return true;
    }

    /// <summary>
    /// Sudoku tahtasının tamamen çözüldüğünü kontrol eder (boş hücre yok + geçerli)
    /// </summary>
    public static bool IsCompleteSudoku(int[,] board)
    {
        // Önce boş hücre var mı kontrol et
        for (int row = 0; row < 9; row++)
        {
            for (int col = 0; col < 9; col++)
            {
                if (board[row, col] == 0)
                {
                    return false; // Boş hücre var
                }
            }
        }

        // Sonra geçerli mi kontrol et
        return IsValidSudoku(board);
    }

    /// <summary>
    /// Belirli bir pozisyona sayı yerleştirilebilir mi kontrol eder
    /// </summary>
    public static bool CanPlaceNumber(int[,] board, int row, int col, int number)
    {
        if (number < 1 || number > 9) return false;
        if (board[row, col] != 0) return false; // Zaten dolu

        // Satırda var mı?
        for (int c = 0; c < 9; c++)
        {
            if (board[row, c] == number) return false;
        }

        // Sütunda var mı?
        for (int r = 0; r < 9; r++)
        {
            if (board[r, col] == number) return false;
        }

        // 3x3 kutuda var mı?
        int boxStartRow = (row / 3) * 3;
        int boxStartCol = (col / 3) * 3;

        for (int r = boxStartRow; r < boxStartRow + 3; r++)
        {
            for (int c = boxStartCol; c < boxStartCol + 3; c++)
            {
                if (board[r, c] == number) return false;
            }
        }

        return true;
    }

    // === YARDIMCI FONKSİYONLAR ===

    /// <summary>
    /// 9 elemanlı bir birimde (satır/sütun/kutu) tekrar eden sayı var mı kontrol eder
    /// </summary>
    private static bool IsValidUnit(int[] unit)
    {
        HashSet<int> seen = new HashSet<int>();

        foreach (int value in unit)
        {
            if (value != 0) // Boş hücreler (0) göz ardı edilir
            {
                if (seen.Contains(value))
                {
                    return false; // Tekrar eden sayı bulundu
                }
                seen.Add(value);
            }
        }

        return true;
    }

    /// <summary>
    /// Belirli bir satırı array olarak döndürür
    /// </summary>
    private static int[] GetRow(int[,] board, int row)
    {
        int[] result = new int[9];
        for (int col = 0; col < 9; col++)
        {
            result[col] = board[row, col];
        }
        return result;
    }

    /// <summary>
    /// Belirli bir sütunu array olarak döndürür
    /// </summary>
    private static int[] GetColumn(int[,] board, int col)
    {
        int[] result = new int[9];
        for (int row = 0; row < 9; row++)
        {
            result[row] = board[row, col];
        }
        return result;
    }

    /// <summary>
    /// Belirli bir 3x3 kutuyu array olarak döndürür
    /// </summary>
    private static int[] GetBox(int[,] board, int boxRow, int boxCol)
    {
        int[] result = new int[9];
        int index = 0;

        int startRow = boxRow * 3;
        int startCol = boxCol * 3;

        for (int row = startRow; row < startRow + 3; row++)
        {
            for (int col = startCol; col < startCol + 3; col++)
            {
                result[index++] = board[row, col];
            }
        }

        return result;
    }

    // === TEST FONKSİYONLARI ===

    /// <summary>
    /// Test amaçlı - sudoku tahtasını konsola yazdırır
    /// </summary>
    public static void PrintBoard(int[,] board)
    {
        string result = "Sudoku Board:\n";
        for (int row = 0; row < 9; row++)
        {
            if (row % 3 == 0) result += "+---------+---------+---------+\n";

            for (int col = 0; col < 9; col++)
            {
                if (col % 3 == 0) result += "| ";
                result += (board[row, col] == 0 ? "." : board[row, col].ToString()) + " ";
            }
            result += "|\n";
        }
        result += "+---------+---------+---------+";

        Debug.Log(result);
    }
}