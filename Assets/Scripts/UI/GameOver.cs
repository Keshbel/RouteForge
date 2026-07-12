using TMPro;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    
    [SerializeField] private TMP_Text textScore;
    [SerializeField] private TMP_Text textResult;

    public void SetText(string score, string result)
    {
        if (textScore != null)
        {
            textScore.text = score;
        }

        if (textResult != null)
        {
            textResult.text = result;
        }
    }

    public void SetVisible(bool isVisible)
    {
        if (panel != null)
        {
            panel.SetActive(isVisible);
        }
    }
}
