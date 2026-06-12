using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AfterGameUI : MonoBehaviour
{
    public TextMeshProUGUI timeResultText;
    public int rating = -1;
    public TextMeshProUGUI resultText;

    public Image star1;
    public Image star2;
    public Image star3;

    private Color brightColor;
    private Color darkColor;

    void OnEnable()
    {
        float t = GameManager.ElapsedTime;
        int min = Mathf.FloorToInt(t / 60f);
        int sec = Mathf.FloorToInt(t % 60f);
        timeResultText.text = $"{min:00}:{sec:00}";

        if (resultText != null)
            resultText.text = GameManager.BattleResult;

        rating = GameManager.BattleRating;

  
        ColorUtility.TryParseHtmlString("#F7FEFF", out brightColor);
        ColorUtility.TryParseHtmlString("#033941", out darkColor);

     
        star1.color = darkColor;
        star2.color = darkColor;
        star3.color = darkColor;

      
        if (rating >= 1)
            star1.color = brightColor;

        if (rating >= 2)
            star2.color = brightColor;

        if (rating >= 3)
            star3.color = brightColor;
    }
}