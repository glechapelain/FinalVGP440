using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridSpace : MonoBehaviour
{
    [SerializeField] Button _button;
    [SerializeField] Text _buttonText;

    private GameController gameController;

    public void SetSpace()
    {
        if(!gameController.playerMove)
        {
            return;
        }

        _buttonText.text = gameController.GetPlayerSide();
        _button.interactable = false;
        gameController.EndTurn();
    }

    public void SetGameControllerReference(GameController controller)
    {
        gameController = controller;
    }
}
