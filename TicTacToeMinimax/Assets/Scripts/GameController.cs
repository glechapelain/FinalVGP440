using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Player
{
    public Image panel;
    public Text text;
    public Button button;
}

[System.Serializable]
public class PlayerColor
{
    public Color panelColor;
    public Color textColor;
}

public class GameController : MonoBehaviour
{
    [SerializeField] Text[] buttonList;
    string[] copiedBoard;
    string playerSide;
    string computerSide;
    public bool playerMove;
    int score;
    int position;
    int bestPosition;

    [SerializeField] GameObject gameOverPanel;
    [SerializeField] Text gameOverText;

    [SerializeField] GameObject restartButton;

    [SerializeField] Player playerX;
    [SerializeField] Player playerO;
    [SerializeField] PlayerColor activePlayerColor;
    [SerializeField] PlayerColor inactivePlayerColor;

    int moveCount;
    int minimaxMoveCount;

    void Awake()
    {
        gameOverPanel.SetActive(false);
        SetGameControllerReferenceOnButtons();
        moveCount = 0;
        restartButton.SetActive(false);
        playerMove = true;
    }

    void Update()
    {
        while (playerMove == false)
        {
            bestPosition = BestPosition(true);

            if (buttonList[bestPosition].GetComponentInParent<Button>().interactable == true)
            {
                buttonList[bestPosition].text = GetComputerSide();
                buttonList[bestPosition].GetComponentInParent<Button>().interactable = false;
                EndTurn();
            }
        }
    }

    void SetGameControllerReferenceOnButtons()
    {
        for (int i = 0; i < buttonList.Length; ++i)
        {
            buttonList[i].GetComponentInParent<GridSpace>().SetGameControllerReference(this);
        }
    }

    public void SetStartingSide(string startingSide)
    {
        playerSide = startingSide;
        if(playerSide == "X")
        {
            playerMove = true;
            computerSide = "O";
            SetPlayerColors(playerX, playerO);
        }
        else
        {
            playerMove = false;
            computerSide = "X";
            SetPlayerColors(playerO, playerX);
        }

        StartGame();
    }

    void StartGame()
    {
        SetBoardInteractable(true);
        SetPlayerButtons(false);
    }

    public string GetPlayerSide()
    {
        return playerSide;
    }

    public string GetComputerSide()
    {
        return computerSide;
    }

    public void EndTurn()
    {
        moveCount++;

        if (buttonList[0].text == playerSide && buttonList[1].text == playerSide && buttonList[2].text == playerSide ||
            buttonList[3].text == playerSide && buttonList[4].text == playerSide && buttonList[5].text == playerSide ||
            buttonList[6].text == playerSide && buttonList[7].text == playerSide && buttonList[8].text == playerSide    )
        {
            GameOver(playerSide);
        }
        else if (buttonList[0].text == playerSide && buttonList[3].text == playerSide && buttonList[6].text == playerSide ||
            buttonList[1].text == playerSide && buttonList[4].text == playerSide && buttonList[7].text == playerSide ||
            buttonList[2].text == playerSide && buttonList[5].text == playerSide && buttonList[8].text == playerSide)
        {
            GameOver(playerSide);
        }
        else if (buttonList[0].text == playerSide && buttonList[4].text == playerSide && buttonList[8].text == playerSide ||
            buttonList[2].text == playerSide && buttonList[4].text == playerSide && buttonList[6].text == playerSide)
        {
            GameOver(playerSide);
        }
        else if (buttonList[0].text == computerSide && buttonList[1].text == computerSide && buttonList[2].text == computerSide ||
            buttonList[3].text == computerSide && buttonList[4].text == computerSide && buttonList[5].text == computerSide ||
            buttonList[6].text == computerSide && buttonList[7].text == computerSide && buttonList[8].text == computerSide)
        {
            GameOver(computerSide);
        }
        else if (buttonList[0].text == computerSide && buttonList[3].text == computerSide && buttonList[6].text == computerSide ||
            buttonList[1].text == computerSide && buttonList[4].text == computerSide && buttonList[7].text == computerSide ||
            buttonList[2].text == computerSide && buttonList[5].text == computerSide && buttonList[8].text == computerSide)
        {
            GameOver(computerSide);
        }
        else if (buttonList[0].text == computerSide && buttonList[4].text == computerSide && buttonList[8].text == computerSide ||
            buttonList[2].text == computerSide && buttonList[4].text == computerSide && buttonList[6].text == computerSide)
        {
            GameOver(computerSide);
        }
        else if (moveCount >= 9)
        {
            GameOver("draw");
        }
        else
        {
           ChangeSides();
        }
    }

    void SetPlayerColors(Player newPlayer, Player oldPlayer)
    {
        newPlayer.panel.color = activePlayerColor.panelColor;
        newPlayer.text.color = activePlayerColor.textColor;
        oldPlayer.panel.color = inactivePlayerColor.panelColor;
        oldPlayer.text.color = inactivePlayerColor.textColor;
    }

    void GameOver(string winPlayer)
    {
        SetBoardInteractable(false);
        playerMove = true;

        if (winPlayer == "draw")
        {
            SetGameOverText("It's a draw!");
            SetPlayerColorsInactive();
        }
        else
        {
            SetGameOverText(winPlayer + " Wins!");
        }

        restartButton.SetActive(true);
    }

    void ChangeSides()
    {
        playerMove = (playerMove == true) ? false : true;

        if(playerMove == true)
        {
            SetPlayerColors(playerX, playerO);
        }
        else
        {
            SetPlayerColors(playerO, playerX);
        }
    }

    void SetGameOverText(string value)
    {
        gameOverPanel.SetActive(true);
        gameOverText.text = value;
    }

    public void RestartGame()
    {
        moveCount = 0;
        gameOverPanel.SetActive(false);
        restartButton.SetActive(false);
        SetPlayerButtons(true);
        SetPlayerColorsInactive();
        playerMove = true;

        for (int i = 0; i < buttonList.Length; ++i)
        {
            buttonList[i].text = "";
        }
    }

    void SetBoardInteractable(bool toggle)
    {
        for (int i = 0; i < buttonList.Length; ++i)
        {
            buttonList[i].GetComponentInParent<Button>().interactable = toggle;
        }
    }

    void SetPlayerButtons(bool toggle)
    {
        playerX.button.interactable = toggle;
        playerO.button.interactable = toggle;
    }

    void SetPlayerColorsInactive()
    {
        playerX.panel.color = inactivePlayerColor.panelColor;
        playerX.text.color = inactivePlayerColor.textColor;
        playerO.panel.color = inactivePlayerColor.panelColor;
        playerO.text.color = inactivePlayerColor.textColor;
    }

    int BestPosition(bool useMinimax)
    {
        if(!useMinimax)
        {
            position = Random.Range(0, 8);
            return position;
        }

        CopyBoardstate();
        int bestScore = int.MinValue;
        for (int i = 0; i < buttonList.Length; ++i)
        {
            if (buttonList[i].text == "X" || buttonList[i].text == "O")
            {
                continue;
            }

            copiedBoard[i] = computerSide;
            score = Minimax(0, false);
            copiedBoard[i] = "";

            if (score > bestScore)
            {
                bestScore = score;
                position = i;
            }

            if(score == bestScore)
            {
                Debug.Log($"{i} - {score}");
            }
        }

        return position;
    }

    int Minimax(int depth, bool maximizingPlayer)
    {
        int checkedResult = CheckGameResult();
        int bestScore = 0;

        if (checkedResult != 0)
        {
            minimaxMoveCount = moveCount;
            return checkedResult;
        }
        else if (maximizingPlayer)
        {
            bestScore = int.MinValue;
            for (int i = 0; i < copiedBoard.Length; ++i)
            {
                if (copiedBoard[i] == "X" || copiedBoard[i] == "O")
                {
                    continue;
                }

                copiedBoard[i] = computerSide;
                score = Minimax(depth + 1, !maximizingPlayer);
                minimaxMoveCount--;
                copiedBoard[i] = "";

                bestScore = Mathf.Max(score, bestScore);
            }
        }
        else
        {
            bestScore = int.MaxValue;
            for (int i = 0; i < copiedBoard.Length; ++i)
            {
                if (copiedBoard[i] == "X" || copiedBoard[i] == "O")
                {
                    continue;
                }

                copiedBoard[i] = playerSide;
                score = Minimax(depth + 1, !maximizingPlayer);
                minimaxMoveCount--;
                copiedBoard[i] = "";

                bestScore = Mathf.Min(score, bestScore);
            }
        }

        return bestScore;
    }

    void CopyBoardstate()
    {
        copiedBoard = new string[9];
        for (int i = 0; i < buttonList.Length; ++i)
        {
            copiedBoard[i] = buttonList[i].text;
        }

        minimaxMoveCount = moveCount;
    }

    int CheckGameResult()
    {
        minimaxMoveCount++;

        if (copiedBoard[0] == playerSide && copiedBoard[1] == playerSide && copiedBoard[2] == playerSide ||
            copiedBoard[3] == playerSide && copiedBoard[4] == playerSide && copiedBoard[5] == playerSide ||
            copiedBoard[6] == playerSide && copiedBoard[7] == playerSide && copiedBoard[8] == playerSide)
        {
            return -1;
        }
        else if (copiedBoard[0] == playerSide && copiedBoard[3] == playerSide && copiedBoard[6] == playerSide ||
                 copiedBoard[1] == playerSide && copiedBoard[4] == playerSide && copiedBoard[7] == playerSide ||
                 copiedBoard[2] == playerSide && copiedBoard[5] == playerSide && copiedBoard[8] == playerSide)
        {
            return -1;
        }
        else if (copiedBoard[0] == playerSide && copiedBoard[4] == playerSide && copiedBoard[8] == playerSide ||
                 copiedBoard[2] == playerSide && copiedBoard[4] == playerSide && copiedBoard[6] == playerSide)
        {
            return -1;
        }
        else if (copiedBoard[0] == computerSide && copiedBoard[1] == computerSide && copiedBoard[2] == computerSide ||
                 copiedBoard[3] == computerSide && copiedBoard[4] == computerSide && copiedBoard[5] == computerSide ||
                 copiedBoard[6] == computerSide && copiedBoard[7] == computerSide && copiedBoard[8] == computerSide)
        {
            return 2;
        }
        else if (copiedBoard[0] == computerSide && copiedBoard[3] == computerSide && copiedBoard[6] == computerSide ||
                 copiedBoard[1] == computerSide && copiedBoard[4] == computerSide && copiedBoard[7] == computerSide ||
                 copiedBoard[2] == computerSide && copiedBoard[5] == computerSide && copiedBoard[8] == computerSide)
        {
            return 2;
        }
        else if (copiedBoard[0] == computerSide && copiedBoard[4] == computerSide && copiedBoard[8] == computerSide ||
                 copiedBoard[2] == computerSide && copiedBoard[4] == computerSide && copiedBoard[6] == computerSide)
        {
            return 2;
        }
        else if (minimaxMoveCount >= 9)
        {
            return 1;
        }

        return 0;
    }
}
