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
    string playerSide;
    string computerSide;
    public bool playerMove;
    int eval;
    int value;
    int score;
    int position;

    [SerializeField] GameObject gameOverPanel;
    [SerializeField] Text gameOverText;

    [SerializeField] GameObject restartButton;

    [SerializeField] Player playerX;
    [SerializeField] Player playerO;
    [SerializeField] PlayerColor activePlayerColor;
    [SerializeField] PlayerColor inactivePlayerColor;

    int moveCount;
    int moveCountMinimax;

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
        if (playerMove == false)
        {
            moveCountMinimax = moveCount;
            value = Minimax(buttonList, buttonList.Length - moveCount, true);

            if (buttonList[value].GetComponentInParent<Button>().interactable == true)
            {
                buttonList[value].text = GetComputerSide();
                buttonList[value].GetComponentInParent<Button>().interactable = false;
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
            computerSide = "O";
            SetPlayerColors(playerX, playerO);
        }
        else
        {
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

    int Minimax(Text[] list, int depth, bool maximizingPlayer)
    {
        Text[] tempBoard = new Text[9];

        for (int i = 0; i < tempBoard.Length; ++i)
        {
            tempBoard[i].GetComponentInParent<GridSpace>().SetGameControllerReference(this);
        }

        for (int i = 0; i < buttonList.Length; ++i)
        {
            tempBoard[i].text = list[i].text;
        }

        if (depth == 0)
        {
            return Random.Range(0, 8);
        }
        else if(maximizingPlayer)
        {
            for (int i = 0; i < tempBoard.Length; ++i)
            {
                if(tempBoard[i].text == "X" || tempBoard[i].text == "O")
                {
                    continue;
                }
                else
                {
                    tempBoard[i].text = computerSide;
                    score = TurnMinimax();

                    if (score == 2)
                    {
                        return i;
                    }
                    else if (score == 1)
                    {
                        position = i;
                    }

                    eval = Minimax(tempBoard, depth - 1, false);
                    tempBoard[i].text = "";
                }
            }
        }
        else
        {
            for (int i = 0; i < tempBoard.Length; ++i)
            {
                if (tempBoard[i].text == "X" || tempBoard[i].text == "O")
                {
                    continue;
                }
                else
                {
                    tempBoard[i].text = computerSide;
                    score = TurnMinimax();

                    if (score == 2)
                    {
                        return i;
                    }
                    else if(score == 1)
                    {
                        position = i;
                    }

                    eval = Minimax(tempBoard, depth - 1, true);
                    tempBoard[i].text = "";
                }
            }
        }

        return position;
    }

    int TurnMinimax()
    {
        moveCountMinimax++;

        if (buttonList[0].text == playerSide && buttonList[1].text == playerSide && buttonList[2].text == playerSide ||
            buttonList[3].text == playerSide && buttonList[4].text == playerSide && buttonList[5].text == playerSide ||
            buttonList[6].text == playerSide && buttonList[7].text == playerSide && buttonList[8].text == playerSide)
        {
            return -1;
        }
        else if (buttonList[0].text == playerSide && buttonList[3].text == playerSide && buttonList[6].text == playerSide ||
            buttonList[1].text == playerSide && buttonList[4].text == playerSide && buttonList[7].text == playerSide ||
            buttonList[2].text == playerSide && buttonList[5].text == playerSide && buttonList[8].text == playerSide)
        {
            return -1;
        }
        else if (buttonList[0].text == playerSide && buttonList[4].text == playerSide && buttonList[8].text == playerSide ||
            buttonList[2].text == playerSide && buttonList[4].text == playerSide && buttonList[6].text == playerSide)
        {
            return -1;
        }
        else if (buttonList[0].text == computerSide && buttonList[1].text == computerSide && buttonList[2].text == computerSide ||
            buttonList[3].text == computerSide && buttonList[4].text == computerSide && buttonList[5].text == computerSide ||
            buttonList[6].text == computerSide && buttonList[7].text == computerSide && buttonList[8].text == computerSide)
        {
            return 2;
        }
        else if (buttonList[0].text == computerSide && buttonList[3].text == computerSide && buttonList[6].text == computerSide ||
            buttonList[1].text == computerSide && buttonList[4].text == computerSide && buttonList[7].text == computerSide ||
            buttonList[2].text == computerSide && buttonList[5].text == computerSide && buttonList[8].text == computerSide)
        {
            return 2;
        }
        else if (buttonList[0].text == computerSide && buttonList[4].text == computerSide && buttonList[8].text == computerSide ||
            buttonList[2].text == computerSide && buttonList[4].text == computerSide && buttonList[6].text == computerSide)
        {
            return 2;
        }
        else if (moveCountMinimax >= 9)
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }
}
