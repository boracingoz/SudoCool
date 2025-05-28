using UnityEngine;

public class SudokuValidatorTest : MonoBehaviour
{
    void Start()
    {
        Debug.Log("=== SUDOKU VALIDATOR TEST BAŞLADI ===");

        TestValidBoard();
        TestInvalidRowBoard();
        TestInvalidColumnBoard();
        TestInvalidBoxBoard();
        TestCanPlaceNumber();
        TestCompleteSudoku();

        Debug.Log("=== SUDOKU VALIDATOR TEST BİTTİ ===");
    }

    void TestValidBoard()
    {
        Debug.Log("\n--- Test 1: Geçerli Board ---");

        // Kısmen dolu, geçerli bir sudoku
        int[,] validBoard = {
            {5, 3, 0, 0, 7, 0, 0, 0, 0},
            {6, 0, 0, 1, 9, 5, 0, 0, 0},
            {0, 9, 8, 0, 0, 0, 0, 6, 0},
            {8, 0, 0, 0, 6, 0, 0, 0, 3},
            {4, 0, 0, 8, 0, 3, 0, 0, 1},
            {7, 0, 0, 0, 2, 0, 0, 0, 6},
            {0, 6, 0, 0, 0, 0, 2, 8, 0},
            {0, 0, 0, 4, 1, 9, 0, 0, 5},
            {0, 0, 0, 0, 8, 0, 0, 7, 9}
        };

        SudokuValidator.PrintBoard(validBoard);
        bool result = SudokuValidator.IsValidSudoku(validBoard);
        Debug.Log($"Sonuç: {result} (Beklenen: true)");
    }

    void TestInvalidRowBoard()
    {
        Debug.Log("\n--- Test 2: Geçersiz Satır ---");

        // İlk satırda 5 sayısı iki kez var
        int[,] invalidRowBoard = {
            {5, 3, 5, 0, 7, 0, 0, 0, 0}, // Hatalı satır
            {6, 0, 0, 1, 9, 0, 0, 0, 0},
            {0, 9, 8, 0, 0, 0, 0, 6, 0},
            {8, 0, 0, 0, 6, 0, 0, 0, 3},
            {4, 0, 0, 8, 0, 3, 0, 0, 1},
            {7, 0, 0, 0, 2, 0, 0, 0, 6},
            {0, 6, 0, 0, 0, 0, 2, 8, 0},
            {0, 0, 0, 4, 1, 9, 0, 0, 0},
            {0, 0, 0, 0, 8, 0, 0, 7, 9}
        };

        bool result = SudokuValidator.IsValidSudoku(invalidRowBoard);
        Debug.Log($"Sonuç: {result} (Beklenen: false)");
    }

    void TestInvalidColumnBoard()
    {
        Debug.Log("\n--- Test 3: Geçersiz Sütun ---");

        // İlk sütunda 6 sayısı iki kez var
        int[,] invalidColBoard = {
            {5, 3, 0, 0, 7, 0, 0, 0, 0},
            {6, 0, 0, 1, 9, 5, 0, 0, 0},
            {6, 9, 8, 0, 0, 0, 0, 0, 0}, // Hatalı hücre
            {8, 0, 0, 0, 0, 0, 0, 0, 3},
            {4, 0, 0, 8, 0, 3, 0, 0, 1},
            {7, 0, 0, 0, 2, 0, 0, 0, 0},
            {0, 6, 0, 0, 0, 0, 2, 8, 0},
            {0, 0, 0, 4, 1, 9, 0, 0, 5},
            {0, 0, 0, 0, 8, 0, 0, 7, 9}
        };

        bool result = SudokuValidator.IsValidSudoku(invalidColBoard);
        Debug.Log($"Sonuç: {result} (Beklenen: false)");
    }

    void TestInvalidBoxBoard()
    {
        Debug.Log("\n--- Test 4: Geçersiz 3x3 Kutu ---");

        // Sol üst kutuda 3 sayısı iki kez var
        int[,] invalidBoxBoard = {
            {5, 3, 0, 0, 7, 0, 0, 0, 0},
            {6, 0, 3, 1, 9, 5, 0, 0, 0}, // Hatalı hücre
            {0, 9, 8, 0, 0, 0, 0, 6, 0},
            {8, 0, 0, 0, 6, 0, 0, 0, 3},
            {4, 0, 0, 8, 0, 3, 0, 0, 1},
            {7, 0, 0, 0, 2, 0, 0, 0, 6},
            {0, 6, 0, 0, 0, 0, 2, 8, 0},
            {0, 0, 0, 4, 1, 9, 0, 0, 5},
            {0, 0, 0, 0, 8, 0, 0, 7, 9}
        };

        bool result = SudokuValidator.IsValidSudoku(invalidBoxBoard);
        Debug.Log($"Sonuç: {result} (Beklenen: false)");
    }

    void TestCanPlaceNumber()
    {
        Debug.Log("\n--- Test 5: CanPlaceNumber ---");

        int[,] board = {
            {5, 3, 0, 0, 7, 0, 0, 0, 0},
            {6, 0, 0, 1, 9, 5, 0, 0, 0},
            {0, 9, 8, 0, 0, 0, 0, 6, 0},
            {8, 0, 0, 0, 6, 0, 0, 0, 3},
            {4, 0, 0, 8, 0, 3, 0, 0, 1},
            {7, 0, 0, 0, 2, 0, 0, 0, 6},
            {0, 6, 0, 0, 0, 0, 2, 8, 0},
            {0, 0, 0, 4, 1, 9, 0, 0, 5},
            {0, 0, 0, 0, 8, 0, 0, 7, 9}
        };

        // (0,2) pozisyonuna 4 yerleştirilebilir mi?
        bool canPlace4 = SudokuValidator.CanPlaceNumber(board, 0, 2, 4);
        Debug.Log($"(0,2) pozisyonuna 4 yerleştirilebilir: {canPlace4} (Beklenen: true)");

        // (0,2) pozisyonuna 5 yerleştirilebilir mi? (aynı satırda 5 var)
        bool canPlace5 = SudokuValidator.CanPlaceNumber(board, 0, 2, 5);
        Debug.Log($"(0,2) pozisyonuna 5 yerleştirilebilir: {canPlace5} (Beklenen: false)");
    }

    void TestCompleteSudoku()
    {
        Debug.Log("\n--- Test 6: Complete Sudoku ---");

        // Tam çözülmüş sudoku
        int[,] completeSudoku = {
            {5, 3, 4, 6, 7, 8, 9, 1, 2},
            {6, 7, 2, 1, 9, 5, 3, 4, 8},
            {1, 9, 8, 3, 4, 2, 5, 6, 7},
            {8, 5, 9, 7, 6, 1, 4, 2, 3},
            {4, 2, 6, 8, 5, 3, 7, 9, 1},
            {7, 1, 3, 9, 2, 4, 8, 5, 6},
            {9, 6, 1, 5, 3, 7, 2, 8, 4},
            {2, 8, 7, 4, 1, 9, 6, 3, 5},
            {3, 4, 5, 2, 8, 6, 1, 7, 9}
        };

        bool isComplete = SudokuValidator.IsCompleteSudoku(completeSudoku);
        Debug.Log($"Sudoku tamamen çözülmüş: {isComplete} (Beklenen: true)");
    }
}