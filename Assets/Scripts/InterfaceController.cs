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
        scoreText.text = "Score: " + score.ToString() + " / " + targetScore.ToString();
        gameStateText.text = "";
        DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        // if (score >= targetScore)
        // {
        //     gameStateText.text = "You finished! \n Go to the exit!";
        // }
    }
    public void UpdateScoreText()
    {
        scoreText.text = "Score: " + score.ToString() + " / " + targetScore.ToString();
    }

    public void ResetGame()
    {
        score = 0;
        UpdateScoreText();
        gameStateText.text = "";
    }
    public void SetGameOver()
    {
        gameStateText.text = "Game Over! \n You can't Recycle yourself! \n Press R to Restart";
    }
}
