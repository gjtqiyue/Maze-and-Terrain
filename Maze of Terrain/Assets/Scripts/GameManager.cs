using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public static GameManager instance = null;

    public GameObject player;       // player reference
    public Text winText;            
    public GameObject startButton;
    public GameObject quitButton;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }


    // Use this for initialization
    void Start () {
        ShowMenu();
	}

    // Show the game menu and initilize related values
    private void ShowMenu()
    {
        winText.enabled = true;
        winText.text = "Maze";

        startButton.SetActive(true);
        startButton.GetComponentInChildren<Text>().text = "Start Game";

        quitButton.SetActive(true);
        quitButton.GetComponentInChildren<Text>().text = "Quit Game";
    }

    // Show win text to the player, providing options to restart or quit
    public void Win()
    {
        winText.enabled = true;
        winText.text = "You win!";

        startButton.SetActive(true);
        startButton.GetComponentInChildren<Text>().text = "Restart";

        quitButton.SetActive(true);

        // prevent player from moving freely
        FirstPersonPlayer.instance.lockMovement();
    }

    // restart the game
    public void Initialize()
    {
        winText.enabled = false;
        startButton.SetActive(false);
        quitButton.SetActive(false);

        // initialize the player
        FirstPersonPlayer.instance.Initialize();

        // initialize the maze
        MazeGenerator.instance.InitializeMaze();

        // initialize the senser
        SenserScript.instance.InitializePosition();
    }

    // quit game
    public void Quit()
    {
        Application.Quit();
    }


}
