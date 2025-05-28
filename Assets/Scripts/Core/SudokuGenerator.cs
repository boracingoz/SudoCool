using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SudokuGame.Core
{
    public class SudokuGenerator
    {
        public enum LevelDifficulty { Easy, Medium, Hard }

        /// <summary>
        /// Belirtilen zorluk seviyesinde bir Sudoku bulmacası oluşturur
        /// </summary>
        /// <param name="levelDifficulty">Zorluk seviyesi</param>
        /// <returns>9x9 Sudoku bulmacası (0 = boş hücre)</returns>
        public static int[,] GeneratePuzzle(LevelDifficulty levelDifficulty)
        {
            int[,] full = GenerateFullSolution();
            int[,] puzzle = (int[,])full.Clone();

            int removeCount;
            switch (levelDifficulty)
            {
                case LevelDifficulty.Easy:
                    removeCount = 30;
                    break;
                case LevelDifficulty.Medium:
                    removeCount = 40;
                    break;
                case LevelDifficulty.Hard:
                    removeCount = 50;
                    break;
                default:
                    removeCount = 40;
                    break;
            }

            RemoveCellsWithValidation(puzzle, removeCount);

            return puzzle;
        }

        /// <summary>
        /// Tamamen dolu ve geçerli bir Sudoku çözümü oluşturur
        /// </summary>
        /// <returns>9x9 tam çözülmüş Sudoku tahtası</returns>
        public static int[,] GenerateFullSolution()
        {
            int[,] board = new int[9, 9];

            // Diagonal kutları doldur (birbirini etkilemezler)
            FillDiagonalBoxes(board);

            // Geri kalan hücreleri çöz
            if (!SudokuSolver.SolveSudoku(board))
            {
                Debug.LogError("Sudoku çözümü oluşturulamadı!");
                return GenerateFullSolution(); // Tekrar dene
            }

            // Çözümün geçerli olduğunu doğrula
            if (!SudokuValidator.IsCompleteSudoku(board))
            {
                Debug.LogError("Geçersiz Sudoku çözümü oluşturuldu!");
                return GenerateFullSolution(); // Tekrar dene
            }

            return board;
        }

        /// <summary>
        /// Diagonal 3x3 kutları rastgele sayılarla doldurur
        /// Bu kutlar birbirini etkilemediği için güvenle doldurulabilir
        /// </summary>
        /// <param name="board">9x9 Sudoku tahtası</param>
        private static void FillDiagonalBoxes(int[,] board)
        {
            for (int i = 0; i < 9; i += 3)
            {
                FillBox(board, i, i);
            }
        }

        /// <summary>
        /// Belirtilen pozisyondaki 3x3 kutuyu rastgele sayılarla doldurur
        /// </summary>
        /// <param name="board">9x9 Sudoku tahtası</param>
        /// <param name="startRow">Kutunun başlangıç satırı</param>
        /// <param name="startCol">Kutunun başlangıç sütunu</param>
        private static void FillBox(int[,] board, int startRow, int startCol)
        {
            List<int> numbers = Enumerable.Range(1, 9).OrderBy(x => UnityEngine.Random.value).ToList();
            int index = 0;

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    board[startRow + i, startCol + j] = numbers[index++];
                }
            }
        }

        /// <summary>
        /// Bulmacadan hücreleri kaldırır ve benzersiz çözüm olduğunu doğrular
        /// </summary>
        /// <param name="board">9x9 Sudoku tahtası</param>
        /// <param name="removeCount">Kaldırılacak hücre sayısı</param>
        private static void RemoveCellsWithValidation(int[,] board, int removeCount)
        {
            int removed = 0;
            int maxAttempts = removeCount * 3; // Sonsuz döngüyü önlemek için
            int attempts = 0;

            while (removed < removeCount && attempts < maxAttempts)
            {
                attempts++;

                int row = UnityEngine.Random.Range(0, 9);
                int col = UnityEngine.Random.Range(0, 9);

                if (board[row, col] != 0)
                {
                    int originalValue = board[row, col];
                    board[row, col] = 0; // Hücreyi geçici olarak boşalt

                    // Hala benzersiz çözüm var mı kontrol et
                    if (SudokuSolver.HasUniqueSolution(board))
                    {
                        removed++; // Başarılı - hücre kaldırıldı
                    }
                    else
                    {
                        board[row, col] = originalValue; // Geri al
                    }
                }
            }

            // Eğer yeterince hücre kaldırılamadıysa, validation olmadan kaldır
            if (removed < removeCount)
            {
                Debug.LogWarning($"Sadece {removed}/{removeCount} hücre benzersiz çözüm korunarak kaldırılabildi.");
                RemoveCellsSimple(board, removeCount - removed);
            }
        }

        /// <summary>
        /// Basit hücre kaldırma (validation olmadan)
        /// </summary>
        /// <param name="board">9x9 Sudoku tahtası</param>
        /// <param name="removeCount">Kaldırılacak hücre sayısı</param>
        private static void RemoveCellsSimple(int[,] board, int removeCount)
        {
            int removed = 0;

            while (removed < removeCount)
            {
                int row = UnityEngine.Random.Range(0, 9);
                int col = UnityEngine.Random.Range(0, 9);

                if (board[row, col] != 0)
                {
                    board[row, col] = 0;
                    removed++;
                }
            }
        }

        /// <summary>
        /// Bulmaca oluşturma işlemini analiz eder ve zorluk seviyesini tahmin eder
        /// </summary>
        /// <param name="puzzle">9x9 Sudoku bulmacası</param>
        /// <returns>Tahmini zorluk skoru</returns>
        public static int AnalyzePuzzleDifficulty(int[,] puzzle)
        {
            return SudokuSolver.EstimateDifficulty(puzzle);
        }

        /// <summary>
        /// Belirtilen zorluk seviyesine uygun bulmaca oluşturur ve doğrular
        /// </summary>
        /// <param name="levelDifficulty">Hedef zorluk seviyesi</param>
        /// <param name="maxAttempts">Maksimum deneme sayısı</param>
        /// <returns>Uygun zorlukta Sudoku bulmacası</returns>
        public static int[,] GenerateValidatedPuzzle(LevelDifficulty levelDifficulty, int maxAttempts = 5)
        {
            for (int attempt = 0; attempt < maxAttempts; attempt++)
            {
                int[,] puzzle = GeneratePuzzle(levelDifficulty);

                // Bulmacayı doğrula
                if (SudokuValidator.IsValidSudoku(puzzle) && SudokuSolver.HasUniqueSolution(puzzle))
                {
                    int difficulty = AnalyzePuzzleDifficulty(puzzle);
                    Debug.Log($"Bulmaca oluşturuldu - Zorluk: {levelDifficulty}, Skor: {difficulty}");
                    return puzzle;
                }

                Debug.LogWarning($"Geçersiz bulmaca oluşturuldu, tekrar deneniyor... ({attempt + 1}/{maxAttempts})");
            }

            Debug.LogError("Geçerli bulmaca oluşturulamadı, basit bulmaca döndürülüyor.");
            return GeneratePuzzle(levelDifficulty); // Fallback
        }

        /// <summary>
        /// Test amaçlı - oluşturulan bulmacayı konsola yazdırır
        /// </summary>
        /// <param name="puzzle">9x9 Sudoku bulmacası</param>
        public static void PrintPuzzle(int[,] puzzle)
        {
            SudokuValidator.PrintBoard(puzzle);
        }
    }
}