using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gameplay : MonoBehaviour
{
    private int[][] _board;
    private bool _gameOver = false;
    private Vector2Int _aiPosition;

    // Start is called before the first frame update
    void Start()
    {
        PlayerMove();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void AIMove ()
    {
        int bestScore = -1000;


        for (int i = 0; i < 3; ++i)
        {
            for (int j = 0; j < 3; ++j)
            {
                if (_board[i][j] == 0)
                {
                    //_board[i][j] = 1;
                    int score = Minimax(_board);

                    if(score > bestScore)
                    {
                        bestScore = score;
                        //_aiPosition = { i; j };
                    }
                }
            }
        }

        if (!_gameOver)
        {
            PlayerMove();
        }
    }

    private void PlayerMove()
    {
        if(!_gameOver)
        {
            AIMove();
        }
    }

    private int Minimax(int[][] board)
    {
        return 1;
    }
}
