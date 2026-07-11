using TMPro;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    
    [SerializeField] private TMP_Text textScore;
    [SerializeField] private TMP_Text textResult;

    public void SetText(string score, string result)
    {
        textScore.text = score;
        textResult.text = result;
    }

    public void SetVisible(bool isVisible)
    {
        panel.SetActive(isVisible);
    }
}
