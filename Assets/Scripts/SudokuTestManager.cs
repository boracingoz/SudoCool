using UnityEngine;
using SudokuGame.Core;

namespace SudokuGame.Testing
{
    /// <summary>
    /// Sudoku sisteminin tüm bileşenlerini test etmek için kullanılan manager
    /// Unity Editor'da Inspector'dan test butonları ile kullanılabilir
    /// </summary>
    public class SudokuTestManager : MonoBehaviour
    {
        [Header("Test Settings")]
        [SerializeField] private SudokuGenerator.LevelDifficulty testDifficulty = SudokuGenerator.LevelDifficulty.Medium;
        [SerializeField] private bool enableDetailedLogs = true;
        [SerializeField] private bool printBoards = true;

        [Header("Performance Test")]
        [SerializeField] private int performanceTestCount = 10;

        [Header("Current Puzzle State")]
        [SerializeField] private int[,] currentPuzzle;
        [SerializeField] private int[,] currentSolution;

        private void Start()
        {
            if (enableDetailedLogs)
            {
                Debug.Log("Sudoku Test Manager başlatıldı!");
                Debug.Log("Test butonlarını Inspector'dan kullanabilirsiniz.");
            }
        }

        #region Test Buttons (Inspector'da görünür)

        [ContextMenu("Complete System Test")]
        public void RunCompleteSystemTest()
        {
            Debug.Log("=== COMPLETE SUDOKU SYSTEM TEST BAŞLADI ===");

            bool allTestsPassed = true;

            // Test 1: Generator Test
            if (!TestGenerator())
            {
                allTestsPassed = false;
                Debug.LogError("Generator testi başarısız!");
            }

            // Test 2: Solver Test
            if (!TestSolver())
            {
                allTestsPassed = false;
                Debug.LogError("Solver testi başarısız!");
            }

            // Test 3: Validator Test
            if (!TestValidator())
            {
                allTestsPassed = false;
                Debug.LogError("Validator testi başarısız!");
            }

            // Test 4: Integration Test
            if (!TestIntegration())
            {
                allTestsPassed = false;
                Debug.LogError("Integration testi başarısız!");
            }

            if (allTestsPassed)
            {
                Debug.Log(" === TÜM TESTLER BAŞARILI! SISTEM HAZIR! ===");
            }
            else
            {
                Debug.LogError(" === BAZI TESTLER BAŞARISIZ! KONTROL EDİN! ===");
            }
        }

        [ContextMenu("Test Generator")]
        public void TestGeneratorFromInspector()
        {
            TestGenerator();
        }

        [ContextMenu("Test Solver")]
        public void TestSolverFromInspector()
        {
            TestSolver();
        }

        [ContextMenu("Test Validator")]
        public void TestValidatorFromInspector()
        {
            TestValidator();
        }

        [ContextMenu("Test Integration")]
        public void TestIntegrationFromInspector()
        {
            TestIntegration();
        }

        [ContextMenu("Performance Test")]
        public void RunPerformanceTest()
        {
            TestPerformance();
        }

        [ContextMenu("Generate New Puzzle")]
        public void GenerateNewPuzzle()
        {
            currentPuzzle = SudokuGenerator.GenerateValidatedPuzzle(testDifficulty);
            Debug.Log($"Yeni {testDifficulty} seviye bulmaca oluşturuldu!");

            if (printBoards)
            {
                SudokuValidator.PrintBoard(currentPuzzle);
            }
        }

        [ContextMenu("Solve Current Puzzle")]
        public void SolveCurrentPuzzle()
        {
            if (currentPuzzle == null)
            {
                Debug.LogWarning("Önce bir bulmaca oluşturun!");
                return;
            }

            currentSolution = SudokuSolver.CloneBoard(currentPuzzle);

            System.Diagnostics.Stopwatch sw = System.Diagnostics.Stopwatch.StartNew();
            bool solved = SudokuSolver.SolveSudoku(currentSolution);
            sw.Stop();

            if (solved)
            {
                Debug.Log($" Bulmaca {sw.ElapsedMilliseconds}ms'de çözüldü!");

                if (printBoards)
                {
                    Debug.Log("--- ÇÖZÜM ---");
                    SudokuValidator.PrintBoard(currentSolution);
                }
            }
            else
            {
                Debug.LogError(" Bulmaca çözülemedi!");
            }
        }

        #endregion

        #region Test Methods

        private bool TestGenerator()
        {
            Debug.Log(" Generator testi başlıyor...");

            try
            {
                // Her zorluk seviyesi için test
                foreach (SudokuGenerator.LevelDifficulty difficulty in System.Enum.GetValues(typeof(SudokuGenerator.LevelDifficulty)))
                {
                    Debug.Log($"{difficulty} seviyesi test ediliyor...");

                    int[,] puzzle = SudokuGenerator.GeneratePuzzle(difficulty);

                    if (puzzle == null)
                    {
                        Debug.LogError($"{difficulty} için null bulmaca oluşturuldu!");
                        return false;
                    }

                    if (!SudokuValidator.IsValidSudoku(puzzle))
                    {
                        Debug.LogError($"{difficulty} için geçersiz bulmaca oluşturuldu!");
                        return false;
                    }

                    int emptyCells = CountEmptyCells(puzzle);
                    Debug.Log($"{difficulty}: {emptyCells} boş hücre");
                }

                Debug.Log("Generator testi başarılı!");
                return true;
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"Generator testi hatası: {ex.Message}");
                return false;
            }
        }

        private bool TestSolver()
        {
            Debug.Log("Solver testi başlıyor...");

            try
            {
                int[,] puzzle = SudokuGenerator.GeneratePuzzle(SudokuGenerator.LevelDifficulty.Easy);
                int[,] solution = SudokuSolver.CloneBoard(puzzle);

                // Çözüm testi
                System.Diagnostics.Stopwatch sw = System.Diagnostics.Stopwatch.StartNew();
                bool solved = SudokuSolver.SolveSudoku(solution);
                sw.Stop();

                if (!solved)
                {
                    Debug.LogError("Bulmaca çözülemedi!");
                    return false;
                }

                if (!SudokuValidator.IsCompleteSudoku(solution))
                {
                    Debug.LogError(" Çözüm geçersiz!");
                    return false;
                }

                Debug.Log($"Bulmaca {sw.ElapsedMilliseconds}ms'de çözüldü");

                // Optimized solver testi
                int[,] solution2 = SudokuSolver.CloneBoard(puzzle);
                sw.Restart();
                bool solvedOptimized = SudokuSolver.SolveSudokuOptimized(solution2);
                sw.Stop();

                if (solvedOptimized)
                {
                    Debug.Log($"Optimized solver: {sw.ElapsedMilliseconds}ms");
                }

                // Unique solution testi
                bool hasUnique = SudokuSolver.HasUniqueSolution(puzzle);
                Debug.Log($" Benzersiz çözüm: {hasUnique}");

                Debug.Log("Solver testi başarılı!");
                return true;
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"Solver testi hatası: {ex.Message}");
                return false;
            }
        }

        private bool TestValidator()
        {
            Debug.Log("Validator testi başlıyor...");

            try
            {
                // Geçerli bulmaca testi
                int[,] validPuzzle = SudokuGenerator.GeneratePuzzle(SudokuGenerator.LevelDifficulty.Easy);

                if (!SudokuValidator.IsValidSudoku(validPuzzle))
                {
                    Debug.LogError("Geçerli bulmaca geçersiz olarak işaretlendi!");
                    return false;
                }

                // Geçersiz bulmaca testi
                int[,] invalidPuzzle = CreateInvalidPuzzle();

                if (SudokuValidator.IsValidSudoku(invalidPuzzle))
                {
                    Debug.LogError("Geçersiz bulmaca geçerli olarak işaretlendi!");
                    return false;
                }

                // CanPlaceNumber testi
                int[,] testBoard = new int[9, 9];
                testBoard[0, 0] = 1;

                if (SudokuValidator.CanPlaceNumber(testBoard, 0, 1, 1))
                {
                    Debug.LogError("Aynı satırda aynı sayı yerleştirilebildi!");
                    return false;
                }

                if (!SudokuValidator.CanPlaceNumber(testBoard, 0, 1, 2))
                {
                    Debug.LogError("Geçerli sayı yerleştirilemedi!");
                    return false;
                }

                Debug.Log("Validator testi başarılı!");
                return true;
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"Validator testi hatası: {ex.Message}");
                return false;
            }
        }

        private bool TestIntegration()
        {
            Debug.Log("Integration testi başlıyor...");

            try
            {
                // Tam entegrasyon testi
                Debug.Log("Validasyonlu bulmaca oluşturuluyor...");
                int[,] puzzle = SudokuGenerator.GenerateValidatedPuzzle(testDifficulty);

                Debug.Log("Bulmaca doğrulanıyor...");
                if (!SudokuValidator.IsValidSudoku(puzzle))
                {
                    Debug.LogError("Oluşturulan bulmaca geçersiz!");
                    return false;
                }

                Debug.Log("Bulmaca çözülüyor...");
                int[,] solution = SudokuSolver.CloneBoard(puzzle);
                if (!SudokuSolver.SolveSudoku(solution))
                {
                    Debug.LogError("Bulmaca çözülemedi!");
                    return false;
                }

                Debug.Log("Çözüm doğrulanıyor...");
                if (!SudokuValidator.IsCompleteSudoku(solution))
                {
                    Debug.LogError("Çözüm geçersiz!");
                    return false;
                }

                Debug.Log("Zorluk analizi yapılıyor...");
                int difficulty = SudokuGenerator.AnalyzePuzzleDifficulty(puzzle);
                Debug.Log($"Zorluk skoru: {difficulty}");

                Debug.Log("Integration testi başarılı!");
                return true;
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"Integration testi hatası: {ex.Message}");
                return false;
            }
        }

        private void TestPerformance()
        {
            Debug.Log($"Performance testi başlıyor... ({performanceTestCount} bulmaca)");

            System.Diagnostics.Stopwatch totalTime = System.Diagnostics.Stopwatch.StartNew();

            float totalGenerationTime = 0f;
            float totalSolvingTime = 0f;
            int successfulPuzzles = 0;

            for (int i = 0; i < performanceTestCount; i++)
            {
                try
                {
                    // Generation time
                    System.Diagnostics.Stopwatch sw = System.Diagnostics.Stopwatch.StartNew();
                    int[,] puzzle = SudokuGenerator.GeneratePuzzle(testDifficulty);
                    sw.Stop();
                    totalGenerationTime += sw.ElapsedMilliseconds;

                    // Solving time
                    int[,] solution = SudokuSolver.CloneBoard(puzzle);
                    sw.Restart();
                    bool solved = SudokuSolver.SolveSudoku(solution);
                    sw.Stop();
                    totalSolvingTime += sw.ElapsedMilliseconds;

                    if (solved && SudokuValidator.IsCompleteSudoku(solution))
                    {
                        successfulPuzzles++;
                    }
                }
                catch (System.Exception ex)
                {
                    Debug.LogError($"Performance test hatası: {ex.Message}");
                }
            }

            totalTime.Stop();

            Debug.Log(" === PERFORMANCE TEST SONUÇLARI ===");
            Debug.Log($"   Başarılı bulmaca: {successfulPuzzles}/{performanceTestCount}");
            Debug.Log($"   Toplam süre: {totalTime.ElapsedMilliseconds}ms");
            Debug.Log($"   Ortalama oluşturma: {totalGenerationTime / performanceTestCount:F2}ms");
            Debug.Log($"   Ortalama çözme: {totalSolvingTime / performanceTestCount:F2}ms");
            Debug.Log($"   Başarı oranı: {(successfulPuzzles * 100f / performanceTestCount):F1}%");
        }

        #endregion

        #region Helper Methods

        private int CountEmptyCells(int[,] board)
        {
            int count = 0;
            for (int row = 0; row < 9; row++)
            {
                for (int col = 0; col < 9; col++)
                {
                    if (board[row, col] == 0)
                        count++;
                }
            }
            return count;
        }

        private int[,] CreateInvalidPuzzle()
        {
            int[,] invalid = new int[9, 9];
            // Aynı satırda aynı sayı koy (geçersiz)
            invalid[0, 0] = 1;
            invalid[0, 1] = 1;
            return invalid;
        }

        #endregion

        #region Public API Methods (Oyun için kullanım)

        /// <summary>
        /// Oyun için yeni bulmaca oluşturur
        /// </summary>
        public int[,] CreateGamePuzzle(SudokuGenerator.LevelDifficulty difficulty)
        {
            return SudokuGenerator.GenerateValidatedPuzzle(difficulty);
        }

        /// <summary>
        /// Oyuncu hamlesi geçerli mi kontrol eder
        /// </summary>
        public bool IsValidMove(int[,] board, int row, int col, int number)
        {
            return SudokuValidator.CanPlaceNumber(board, row, col, number);
        }

        /// <summary>
        /// Oyun tamamlandı mı kontrol eder
        /// </summary>
        public bool IsGameComplete(int[,] board)
        {
            return SudokuValidator.IsCompleteSudoku(board);
        }

        /// <summary>
        /// İpucu için bir hücreyi çözer
        /// </summary>
        public int GetHint(int[,] board, int row, int col)
        {
            return SudokuSolver.GetFirstValidNumber(board, row, col);
        }

        /// <summary>
        /// Tüm bulmacayı çözer (cheat/test için)
        /// </summary>
        public int[,] SolvePuzzle(int[,] puzzle)
        {
            int[,] solution = SudokuSolver.CloneBoard(puzzle);
            SudokuSolver.SolveSudoku(solution);
            return solution;
        }

        #endregion
    }
}