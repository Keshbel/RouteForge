using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using RouteForge;

public class GameManager : MonoBehaviour
{
    private GameSession _session;
    private GameOver _gameOver;
    private GameTextLocalizer _textLocalizer;

    public static bool IsRestarting { get; private set; }

    public void Construct(GameSession session, GameOver gameOver, GameTextLocalizer textLocalizer)
    {
        _session = session;
        _gameOver = gameOver;
        _textLocalizer = textLocalizer;
        _session.SessionCompleted += ShowResult;
    }

    private void OnDestroy()
    {
        if (_session != null)
        {
            _session.SessionCompleted -= ShowResult;
        }
    }

    public void BeginPlanning()
    {
        _session?.BeginPlanning();
    }

    public bool StartRunning()
    {
        return _session != null && _session.StartRunning();
    }

    public void EndGame(int countGoal)
    {
        _session?.CompleteSession(countGoal);
    }

    public void Restart()
    {
        IsRestarting = true;
        SceneManager.LoadSceneAsync(0);
    }

    private void ShowResult(ScoreResult result)
    {
        string score = _textLocalizer != null
            ? _textLocalizer.FormatScore(result.Score)
            : String.Concat("Score: ", result.Score);
        string resultText = _textLocalizer != null
            ? _textLocalizer.LocalizeResult(result.ResultText)
            : result.ResultText;

        _gameOver.SetText(score, resultText);
        _gameOver.SetVisible(true);
    }
}
