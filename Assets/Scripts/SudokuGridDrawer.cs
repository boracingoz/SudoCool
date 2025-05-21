using Assets.Scripts;
using UnityEngine;

public class SudokuGridDrawer : MonoBehaviour
{
    public int[,] sudokuBoard = new int[9, 9];
    public Color gridColor = Color.black;
    public Color thickGridColor = Color.red;
    public float cellSize = 1f;
    public float lineWidth = 0.05f;
    public float thickLineWidth = 0.1f;

    void Start()
    {

        sudokuBoard = new int[9, 9] {
        {5,3,0, 0,7,0, 0,0,0},
        {6,0,0, 1,9,5, 0,0,0},
        {0,9,8, 0,0,0, 0,6,0},

        {8,0,0, 0,6,0, 0,0,3},
        {4,0,0, 8,0,3, 0,0,1},
        {7,0,0, 0,2,0, 0,0,6},

        {0,6,0, 0,0,0, 2,8,0},
        {0,0,0, 4,1,9, 0,0,5}, 
        {0,0,0, 0,8,0, 0,7,9}
        };
        InitializeEmptyBoard();


        bool solved = SudokuSolver.SolveSudoku(sudokuBoard);

        if (solved)
        {
            Debug.Log("is solved!");
        }
        else
        {
            Debug.Log("Not solved!");
        }

        TestSudokuValidator();
    }

    void InitializeEmptyBoard()
    {
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                sudokuBoard[i, j] = 0;
            }
        }
    }

    void OnDrawGizmos()
    {
        Vector3 gridCenter = transform.position;
        Gizmos.matrix = Matrix4x4.TRS(gridCenter, Quaternion.identity, Vector3.one);

        Gizmos.color = gridColor;
        for (int i = 0; i <= 9; i++)
        {
            // Dikey çizgiler
            Gizmos.DrawCube(
                new Vector3(i * cellSize - cellSize * 4.5f, 0, 0),
                new Vector3(lineWidth, cellSize * 9, lineWidth)
            );

            // Yatay çizgiler
            Gizmos.DrawCube(
                new Vector3(0, i * cellSize - cellSize * 4.5f, 0),
                new Vector3(cellSize * 9, lineWidth, lineWidth)
            );
        }

        Gizmos.color = thickGridColor;
        for (int i = 0; i <= 3; i++)
        {
            Gizmos.DrawCube(
                new Vector3(i * 3 * cellSize - cellSize * 4.5f, 0, 0),
                new Vector3(thickLineWidth, cellSize * 9, thickLineWidth)
            );

            Gizmos.DrawCube(
                new Vector3(0, i * 3 * cellSize - cellSize * 4.5f, 0),
                new Vector3(cellSize * 9, thickLineWidth, thickLineWidth)
            );
        }

        GUIStyle style = new GUIStyle();
        style.normal.textColor = Color.black;
        style.fontSize = 20;
        style.alignment = TextAnchor.MiddleCenter;

        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                if (sudokuBoard[i, j] != 0)
                {
                    Vector3 pos = new Vector3(
                        j * cellSize - cellSize * 4 + cellSize / 2,
                        -i * cellSize + cellSize * 4 - cellSize / 2,
                        0
                    );
                    UnityEditor.Handles.Label(pos, sudokuBoard[i, j].ToString(), style);
                }
            }
        }
    }



    public void TestSudokuValidator()
    {
        bool isValid = SudokuValidator.IsValidSudoku(sudokuBoard);
        Debug.Log("Sudoku Geçerli mi? " + isValid);
    }
}