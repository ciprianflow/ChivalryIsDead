using UnityEngine.UI;

public class ScoreTextHandler : ScoreHandler
{
    private Text ScoreText;

    void Awake()
    {
        ScoreText = gameObject.GetComponent<Text>();
    }

    void Update()
    {
        ScoreText.text = Score.ToString();
    }
}
