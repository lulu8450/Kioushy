using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;

public class InterfaceController : MonoBehaviour
{

    public static InterfaceController instance;
    public TextMeshProUGUI gameStateText;
    public TextMeshProUGUI scoreText;
    public int score = 0;
    public int targetScore = 10;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
        AssignUIReferences();
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        AssignUIReferences();
    }

    void AssignUIReferences()
    {
        scoreText = GameObject.Find("ScoreText")?.GetComponent<TextMeshProUGUI>();
        // gameStateText = GameObject.Find("Win/LoseText")?.GetComponent<TextMeshProUGUI>();
        // Find Win/LoseText by searching all TextMeshProUGUI components
        gameStateText = null;
        foreach (var tmp in Resources.FindObjectsOfTypeAll<TextMeshProUGUI>())
        {
            if (tmp.gameObject.name == "Win/LoseText")
            {
                gameStateText = tmp;
                break;
            }
        }

        if (scoreText == null)
        {
            Debug.LogWarning("scoreText is not assigned in InterfaceController!");
        }
        else
        {
            scoreText.text = "Score: " + score.ToString() + " / " + targetScore.ToString();
        }

        if (gameStateText == null)
        {
            Debug.LogWarning("gameStateText is not assigned in InterfaceController!");
        }
        else
        {
            gameStateText.text = "";
        }
    }

    public void UpdateScoreText()
    {
        scoreText.text = "Score: " + score.ToString() + " / " + targetScore.ToString();
    }

    public void ResetGame()
    {
        gameStateText.gameObject.SetActive(false);
        score = 0;
        UpdateScoreText();
        gameStateText.text = "";
    }
    public void SetGameOver()
    {
        gameStateText.gameObject.SetActive(true);
        gameStateText.text = "Game Over! \n You can't Recycle yourself! \n Press R to Restart";
    }
}
