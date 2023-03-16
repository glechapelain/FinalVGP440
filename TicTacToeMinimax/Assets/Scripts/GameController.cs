using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [SerializeField] Text[] buttonList;
    private string playerSide;
    private string computerSide;
    private bool playerMove;

    [SerializeField] GameObject gameOverPanel;
    [SerializeField] Text gameOverText;

    [SerializeField] GameObject restartButton;

    int moveCount;

    void Awake()
    {
        gameOverPanel.SetActive(false);
        SetGameControllerReferenceOnButtons();
        playerSide = "X";
        moveCount = 0;
        restartButton.SetActive(false);
    }

    void SetGameControllerReferenceOnButtons()
    {
        for (int i = 0; i < buttonList.Length; ++i)
        {
            buttonList[i].GetComponentInParent<GridSpace>().SetGameControllerReference(this);
        }
    }

    public string GetPlayerSide()
    {
        return playerSide;
    }

    public void EndTurn()
    {
        moveCount++;

        if(buttonList[0].text == playerSide && buttonList[1].text == playerSide && buttonList[2].text == playerSide ||
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
        else
        {
            if(moveCount >= 9)
            {
                GameOver("draw");
            }

            ChangeSides();
        }
    }

    void GameOver(string winPlayer)
    {
        SetBoardInteractable(false);

        if (winPlayer == "draw")
        {
            SetGameOverText("It's a draw!");
        }
        else
        {
            SetGameOverText(winPlayer + " Wins!");
        }

        restartButton.SetActive(true);
    }

    void ChangeSides()
    {
        playerSide = (playerSide == "X") ? "O" : "X";
    }

    void SetGameOverText(string value)
    {
        gameOverPanel.SetActive(true);
        gameOverText.text = value;
    }

    public void RestartGame()
    {
        playerSide = "X";
        moveCount = 0;
        gameOverPanel.SetActive(false);

        SetBoardInteractable(true);

        for (int i = 0; i < buttonList.Length; ++i)
        {
            buttonList[i].text = "";
        }

        restartButton.SetActive(false);
    }

    void SetBoardInteractable(bool toggle)
    {
        for (int i = 0; i < buttonList.Length; ++i)
        {
            buttonList[i].GetComponentInParent<Button>().interactable = toggle;
        }
    }
}
