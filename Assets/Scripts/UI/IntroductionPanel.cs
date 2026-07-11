using UnityEngine;

public class IntroductionPanel : MonoBehaviour
{
    [SerializeField] private GameObject panel;

    private void Start()
    {
        panel.SetActive(!GameManager.IsRestarting);
    }
}
