using UnityEngine;
using UnityEngine.UI;
using RouteForge;

public class AllSingleton : MonoBehaviour
{
    [Header("Controllers")] 
    [SerializeField] private GameManager gameManager;
    [SerializeField] private GridController gridController;
    [SerializeField] private CubeController cubeController;
    [SerializeField] private GameOver gameOver;

    [Header("UI")] 
    [SerializeField] private Button resetGameButton;

    [Header("Scene")]
    [SerializeField] private Camera mainCamera;

    private GameCompositionRoot _compositionRoot;

    private void Awake()
    {
        if (!ValidateReferences())
        {
            enabled = false;
            return;
        }

        _compositionRoot = new GameCompositionRoot(cubeController.CubeCount);
        gameManager.Construct(_compositionRoot.Session, gameOver);
        gridController.Construct(cubeController, mainCamera);
        cubeController.Construct(gameManager, gridController, mainCamera);
        gameManager.BeginPlanning();
    }

    private bool ValidateReferences()
    {
        bool isValid = gameManager != null
            && gridController != null
            && cubeController != null
            && gameOver != null
            && resetGameButton != null
            && mainCamera != null;

        if (!isValid)
        {
            Debug.LogError("Scene composition is missing required references.", this);
        }

        return isValid;
    }
}
