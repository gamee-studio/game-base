using CodeStage.AdvancedFPSCounter;
using DG.Tweening;
using Pancake.GameService;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public LevelController LevelController;
    public GameState GameState;

    public AFPSCounter AFPSCounter => GetComponent<AFPSCounter>();

    void Awake()
    {
        Application.targetFrameRate = 80;
    }
    
    void Start()
    {
        DontDestroyOnLoad(this);
        ReturnHome();
        EventController.CurrentLevelChanged += UpdateScore;
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
        if (GameState == GameState.PlayingGame)
        {
            AdsManager.TotalTimesPlay += Time.deltaTime;
        }
    }

    public void PrepareLevel()
    {
        GameState = GameState.PrepareGame;
        LevelController.PrepareLevel();
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
        FirebaseManager.OnStartLevel(Data.CurrentLevel,LevelController.Instance.CurrentLevel.gameObject.name);
        
        GameState = GameState.PlayingGame;
        
        PopupController.Instance.HideAll();
        PopupController.Instance.Show<PopupInGame>();
        LevelController.Instance.CurrentLevel.gameObject.SetActive(true);
    }

    public void OnWinGame(float delayPopupShowTime = 2.5f)
    {
        if (GameState == GameState.LoseGame || GameState == GameState.WinGame) return;
        GameState = GameState.WinGame;
        EventController.OnWinLevel?.Invoke();
        // Data setup
        FirebaseManager.OnWinGame(Data.CurrentLevel,LevelController.Instance.CurrentLevel.gameObject.name);
        AdsManager.TotalLevelWinLose++;
        Data.CurrentLevel++;
        // Effect and sounds
        SoundController.Instance.PlayFX(SoundType.WinGame);
        // Event invoke
        LevelController.OnWinGame();
        DOTween.Sequence().AppendInterval(delayPopupShowTime).AppendCallback(() =>
        {
            PopupController.Instance.HideAll();
            PopupWin popupWin = PopupController.Instance.Get<PopupWin>() as PopupWin;
            popupWin.SetupMoneyWin(LevelController.CurrentLevel.BonusMoney);
            popupWin.Show();
        });
    }
    
    public void OnLoseGame(float delayPopupShowTime = 2.5f)
    {
        if (GameState == GameState.LoseGame || GameState == GameState.WinGame) return;
        GameState = GameState.LoseGame;
        EventController.OnLoseLevel?.Invoke();
        // Data setup
        FirebaseManager.OnLoseGame(Data.CurrentLevel,LevelController.Instance.CurrentLevel.gameObject.name);
        AdsManager.TotalLevelWinLose++;
        // Effect and sounds
        SoundController.Instance.PlayFX(SoundType.LoseGame);
        // Event invoke
        LevelController.OnLoseGame();
        DOTween.Sequence().AppendInterval(delayPopupShowTime).AppendCallback(() =>
        {
            PopupController.Instance.Hide<PopupInGame>();
            PopupController.Instance.Show<PopupLose>();
        });
    }

    public void ChangeAFPSState()
    {
        if (Data.IsTesting)
        {
            AFPSCounter.enabled = !AFPSCounter.isActiveAndEnabled;
        }
    }
}

public enum GameState
{
    PrepareGame,
    PlayingGame,
    LoseGame,
    WinGame,
}