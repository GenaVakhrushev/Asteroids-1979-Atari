using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    Ship ship;

    public Text InputModeText;
    public Button ResumeButton;

    public static Menu Instance;

    private void Start()
    {
        Instance = this;
        ship = GameObject.FindGameObjectWithTag("Player").GetComponent<Ship>();

        if (ship.CurrentInputType == InputType.KeyboardMouse)
        {
            InputModeText.text = "����������: ���������� + ����";
        }
        else
        {
            InputModeText.text = "����������: ����������";
        }
    }

    public void Resume()
    {
        Time.timeScale = 1;
        gameObject.SetActive(false);
        ship.enabled = true;
    }

    public void NewGame()
    {
        GameStateManager.OnGameStart.Invoke();
        ResumeButton.interactable = true;
        Resume();
        Statistics.Reset();
    }

    public void ChangeInputMode()
    {
        if (ship.CurrentInputType == InputType.Keyboard)
        {
            InputModeText.text = "����������: ���������� + ����";
            ship.CurrentInputType = InputType.KeyboardMouse;
        }
        else
        {
            InputModeText.text = "����������: ����������";
            ship.CurrentInputType = InputType.Keyboard;
        }
    }

    public void Exit()
    {
        Application.Quit();
    }
}
