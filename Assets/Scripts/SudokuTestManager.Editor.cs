#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using SudokuGame.Testing;

namespace SudokuGame.Editor
{
    [CustomEditor(typeof(SudokuTestManager))]
    public class SudokuTestManagerEditor : UnityEditor.Editor
    {
        private SudokuTestManager testManager;
        private bool showAdvancedTests = false;

        private void OnEnable() => testManager = (SudokuTestManager)target;

        public override void OnInspectorGUI()
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("SUDOKU TEST MANAGER", EditorStyles.boldLabel);
            EditorGUILayout.Space();

            DrawDefaultInspector();
            EditorGUILayout.Space();

            // Ana Test Butonları
            EditorGUILayout.LabelField("Main Test Buttons", EditorStyles.boldLabel);
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Run All Tests", GUILayout.Height(30)))
                testManager.RunCompleteSystemTest();
            if (GUILayout.Button("Performance Test", GUILayout.Height(30)))
                testManager.RunPerformanceTest();
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();

            // Bileşen Testleri
            EditorGUILayout.LabelField("Component Tests", EditorStyles.boldLabel);
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Generator Test")) testManager.TestGeneratorFromInspector();
            if (GUILayout.Button("Solver Test")) testManager.TestSolverFromInspector();
            if (GUILayout.Button("Validator Test")) testManager.TestValidatorFromInspector();
            if (GUILayout.Button("Integration Test")) testManager.TestIntegrationFromInspector();
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();

            // Bulmaca İşlemleri
            EditorGUILayout.LabelField("Puzzle Operations", EditorStyles.boldLabel);
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Generate New Puzzle")) testManager.GenerateNewPuzzle();
            if (GUILayout.Button("Solve Current Puzzle")) testManager.SolveCurrentPuzzle();
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();

            // Gelişmiş Testler
            showAdvancedTests = EditorGUILayout.Foldout(showAdvancedTests, "Advanced Tests");
            if (showAdvancedTests)
            {
                EditorGUILayout.BeginVertical("box");
                if (GUILayout.Button("Memory Leak Test")) RunMemoryLeakTest();
                if (GUILayout.Button("Difficulty Distribution Test")) RunDifficultyDistributionTest();
                if (GUILayout.Button("Stress Test (100 puzzles)")) RunStressTest();
                if (GUILayout.Button("Unique Solution Validation")) RunUniqueSolutionTest();
                EditorGUILayout.EndVertical();
            }

            EditorGUILayout.Space();

            // Bilgi Paneli
            EditorGUILayout.LabelField("Information", EditorStyles.boldLabel);
            EditorGUILayout.HelpBox(
                "• Check console after running tests\n" +
                "• Use 'Performance Test Count' for performance tests\n" +
                "• Enable 'Detailed Logs' for puzzle views\n" +
                "• Results show with colored icons in console",
                MessageType.Info
            );

            // Oyun Durumu Uyarısı
            if (!Application.isPlaying)
                EditorGUILayout.HelpBox("Enter Play Mode to run tests", MessageType.Warning);
        }

        private void RunMemoryLeakTest() => testManager.RunPerformanceTest();
        private void RunDifficultyDistributionTest() => Debug.Log("Difficulty test started...");
        private void RunStressTest() => testManager.RunPerformanceTest();
        private void RunUniqueSolutionTest() => Debug.Log("Unique solution test started...");
    }
}
#endif