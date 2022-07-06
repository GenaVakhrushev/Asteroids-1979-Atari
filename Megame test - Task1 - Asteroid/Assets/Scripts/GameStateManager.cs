using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameStateManager : MonoBehaviour
{
    Ship ship;

    static GameStateManager instance;

    public GameObject MenuPanel;

    public static UnityEvent OnGameStart = new UnityEvent();

    private void Start()
    {
        instance = this;
        ship = GameObject.FindGameObjectWithTag("Player").GetComponent<Ship>();
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            Pause();
        }
    }

    public static void Pause()
    {
        instance.MenuPanel.SetActive(true);
        Time.timeScale = 0;
        instance.ship.enabled = false;
    }

    public static void Lose()
    {
        Pause();
        Menu.Instance.ResumeButton.interactable = false;
    }
}
