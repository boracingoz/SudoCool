using Assets.Scripts.Core;
using UnityEngine;

public class SudokuTestRunner : MonoBehaviour
{
    void Start()
    {
        int[,] puzzle = SudokuGenerator.GeneratePuzzle(SudokuGenerator.LevelDificulty.Easy);

        PrintBoard(puzzle);
    }

    void PrintBoard(int[,] board)
    {
        string output = "";
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                output += board[i, j] + " ";
            }
            output += "\n";
        }

        Debug.Log(output);
    }
}
