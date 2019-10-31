using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    enum GameState { START, RULES, IN_GAME, GAME_OVER };
    private GameState gameState;
    [SerializeField] private GameState startState = GameState.START; // Exists to enable individual level testing


    [SerializeField] private GameObject gameOverPrefab;



    static private GameManager instance = null;

    // Lets other scripts find the instane of the game manager
    public static GameManager Instance
    {
        get
        {
            return instance;
        }
    }

    // Ensure there is only one instance of this object in the game
    void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else
            instance = this;
    }

    void Start()
    {
        gameState = startState;
    }

    void OnChangeState(GameState newState)
    {
        if (gameState != newState)
        {
            switch (newState)
            {
                case GameState.START:
                    Debug.Log("WORKS");
                    SceneManager.LoadScene("MainMenu");
                    break;
                case GameState.RULES:
                    SceneManager.LoadScene("Rules");
                    break;
                case GameState.IN_GAME:

                    Time.timeScale = 1; // Set timescale to be a normal rate 
                    SceneManager.LoadScene("Game"); // Load the 'Game' scene

                    break;
                case GameState.GAME_OVER:

                    //Cursor.lockState = CursorLockMode.None; // unlock the cursor for the menu
                    //Cursor.visible = true;

                    //EnableInput(false); // disable character controls

                    //Time.timeScale = 0; // Pause the game by setting timescale to 0 to stop AI behaviour
                    //Transform instance = Instantiate(gameOverPrefab).transform; // Instantiate the GameOver menu prefab

                    //// Find the button component in the child 'RestartButton' object.
                    //Button restartButton = instance.Find("RestartButton").GetComponent<Button>();
                    //// Add a callback to the button, when the button is clicked the PlayGame() function will be called.
                    //restartButton.onClick.AddListener(() => PlayGame());

                    //// Find the button component in the child 'QuitButton' object.
                    //Button quitButton = instance.Find("QuitButton").GetComponent<Button>();

                    //// Add a callback to the button, when the button is clicked the QuitGame() function will be called.
                    //quitButton.onClick.AddListener(() => QuitGame());

                    break;
            }

            gameState = newState;
        }
    }

    private void EnableInput(bool input)
    {
        // Find the player object
        GameObject player = PlayerController.instance.gameObject;

    }

    public void PlayGame()
    {
        OnChangeState(GameState.IN_GAME);
    }

    public void GameOver()
    {
        OnChangeState(GameState.GAME_OVER);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ReturnToStart()
    {
        OnChangeState(GameState.START);
    }

    public void ViewRules()
    {
        OnChangeState(GameState.RULES);
    }

}
