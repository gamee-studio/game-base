using CodeStage.AdvancedFPSCounter;
using DG.Tweening;
using Pancake.GameService;
using UnityEngine;

public class GameManager : SingletonDontDestroy<GameManager>
{
    public LevelController levelController;
    public GameState gameState;

    public AFPSCounter AFpsCounter => GetComponent<AFPSCounter>();

    protected override void Awake()
    {
        base.Awake();
        Application.targetFrameRate = 80;
    }
    
    void Start()
    {
        ReturnHome();
        Observer.CurrentLevelChanged += UpdateScore;
    }

    public void PlayCurrentLevel()
    {
        PrepareLevel();
        StartGame();
    }
    
    public void UpdateScore()
    {
        if (AuthService.Instance.isLoggedIn && AuthService.Instance.IsCompleteSetupName)
        {
            AuthService.UpdatePlayerStatistics("RANK_LEVEL", Data.CurrentLevel);
        }
    }
    private void FixedUpdate()
    {
        if (gameState == GameState.PlayingGame)
        {
            AdsManager.TotalTimesPlay += Time.deltaTime;
        }
    }

    public void PrepareLevel()
    {
        gameState = GameState.PrepareGame;
        levelController.PrepareLevel();
    }

    public void ReturnHome()
    {
        PrepareLevel();
        
        PopupController.Instance.HideAll();
        PopupController.Instance.Show<PopupBackground>();
        PopupController.Instance.Show<PopupHome>();
    }

    public void ReplayGame()
    {
        PrepareLevel();
        StartGame();
    }

    public void BackLevel()
    {
        Data.CurrentLevel--;
        
        PrepareLevel();
        StartGame();
    }

    public void NextLevel()
    {
        Data.CurrentLevel++;

        PrepareLevel();
        StartGame();
    }
    
    public void StartGame()
    {
        FirebaseManager.OnStartLevel(Data.CurrentLevel,levelController.currentLevel.gameObject.name);
        
        gameState = GameState.PlayingGame;
        
        PopupController.Instance.HideAll();
        PopupController.Instance.Show<PopupInGame>();
        levelController.currentLevel.gameObject.SetActive(true);
    }

    public void OnWinGame(float delayPopupShowTime = 2.5f)
    {
        if (gameState == GameState.LoseGame || gameState == GameState.WinGame) return;
        gameState = GameState.WinGame;
        Observer.OnWinLevel?.Invoke();
        // Data setup
        FirebaseManager.OnWinGame(Data.CurrentLevel,levelController.currentLevel.gameObject.name);
        AdsManager.TotalLevelWinLose++;
        Data.CurrentLevel++;
        // Effect and sounds
        SoundController.Instance.PlayFX(SoundType.WinGame);
        // Event invoke
        levelController.OnWinGame();
        DOTween.Sequence().AppendInterval(delayPopupShowTime).AppendCallback(() =>
        {
            PopupController.Instance.HideAll();
            PopupWin popupWin = PopupController.Instance.Get<PopupWin>() as PopupWin;
            popupWin.SetupMoneyWin(levelController.currentLevel.BonusMoney);
            popupWin.Show();
        });
    }
    
    public void OnLoseGame(float delayPopupShowTime = 2.5f)
    {
        if (gameState == GameState.LoseGame || gameState == GameState.WinGame) return;
        gameState = GameState.LoseGame;
        Observer.OnLoseLevel?.Invoke();
        // Data setup
        FirebaseManager.OnLoseGame(Data.CurrentLevel,levelController.currentLevel.gameObject.name);
        AdsManager.TotalLevelWinLose++;
        // Effect and sounds
        SoundController.Instance.PlayFX(SoundType.LoseGame);
        // Event invoke
        levelController.OnLoseGame();
        DOTween.Sequence().AppendInterval(delayPopupShowTime).AppendCallback(() =>
        {
            PopupController.Instance.Hide<PopupInGame>();
            PopupController.Instance.Show<PopupLose>();
        });
    }

    public void ChangeAFpsState()
    {
        if (Data.IsTesting)
        {
            AFpsCounter.enabled = !AFpsCounter.isActiveAndEnabled;
        }
    }
}

public enum GameState
{
    PrepareGame,
    PlayingGame,
    WaitingResult,
    LoseGame,
    WinGame,
}