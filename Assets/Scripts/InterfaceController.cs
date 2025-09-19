using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine;
using TMPro;

public class InterfaceController : MonoBehaviour
{

    public static InterfaceController instance;
    public TextMeshProUGUI gameStateText;
    // public TextMeshProUGUI 
    public TextMeshProUGUI scoreText;
    public int score = 0;
    public int targetScore = 10;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // gameStateText.text = "";
        DontDestroyOnLoad(this.gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
        AssignUIReferences();
    }

    // void Update()
    // {
    //     if (score == targetScore)
    //     {
    //         // Show win text for 3 seconds
    //         StartCoroutine(ShowWinText());
    //     } 
    // }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        AssignUIReferences();
    }

    void AssignUIReferences()
    {
        scoreText = GameObject.Find("ScoreText")?.GetComponent<TextMeshProUGUI>();
        // gameStateText = GameObject.Find("Win/LoseText")?.GetComponent<TextMeshProUGUI>();
        // Find Win/LoseText by searching all TextMeshProUGUI components
        // gameStateText = null;
        // foreach (var tmp in Resources.FindObjectsOfTypeAll<TextMeshProUGUI>())
        // {
        //     if (tmp.gameObject.name == "Win/LoseText")
        //     {
        //         gameStateText = tmp;
        //         break;
        //     }
        // }

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
        if (gameStateText != null)
            gameStateText.gameObject.SetActive(false);
        score = 0;
        UpdateScoreText();
        if (gameStateText != null)
            gameStateText.text = "";
    }
    public void SetGameOver()
    {
        Debug.Log("Game Over set in InterfaceController");
        gameStateText.gameObject.SetActive(true);
        gameStateText.text = "Game Over! \n You can't Recycle yourself! \n Press R to Restart";
    }

    public void ShowText()
    {
        Debug.Log("ShowText called");
        StartCoroutine(ShowTextCoroutine());
    }

    // coroutine to show win text for 3 seconds
    public IEnumerator ShowTextCoroutine()
    {
        Debug.Log("Coroutine started");
        //augment the size of the text gameObject
        if (gameStateText == null)
        {
            Debug.LogWarning("gameStateText is not assigned in InterfaceController!");
            yield break;
        }
        // gameStateText.gameObject.scale = Vector3.one * 1.5f;
        gameStateText.gameObject.SetActive(true);
        // gameStateText.text = "You finished! \n Go to the exit!";
        yield return new WaitForSeconds(2f);

        // gameStateText.gameObject.scale = Vector3.one;
        gameStateText.gameObject.SetActive(false);
        gameStateText.text = "";
        Debug.Log("Coroutine ended");
    }


}
