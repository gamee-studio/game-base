using UnityEngine;
using Pancake;
public class LevelController : Singleton<LevelController>
{
    [ReadOnly] public Level CurrentLevel;
    private GameConfig Game => ConfigController.Game;
    public void PrepareLevel()
    {
        GenerateLevel(Data.CurrentLevel);
    }
    
    public void GenerateLevel(int indexLevel)
    {
        if (CurrentLevel != null)
        {
            Destroy(CurrentLevel.gameObject);
        }

        if (indexLevel > ConfigController.Game.MaxLevel)
        {
            indexLevel = (indexLevel-Game.StartLoopLevel) % (Game.MaxLevel - Game.StartLoopLevel + 1) + Game.StartLoopLevel;
        }
        else
        {
            if (Game.LevelLoopType == LevelLoopType.NormalLoop)
            {
                indexLevel = (indexLevel-1) % ConfigController.Game.MaxLevel + 1;
            }
            else if (Game.LevelLoopType == LevelLoopType.RandomLoop)
            {
                indexLevel = UnityEngine.Random.Range(Game.StartLoopLevel, Game.MaxLevel);
            }
        }

        Level level = GetLevelByIndex(indexLevel);
        CurrentLevel = Instantiate(level);
        CurrentLevel.gameObject.SetActive(false);
    }

    public Level GetLevelByIndex(int indexLevel)
    {
        GameObject levelGO;
        levelGO = Resources.Load($"Levels/Level {indexLevel}") as GameObject;
        return levelGO.GetComponent<Level>();
    }
    
    public void OnLoseGame()
    {
        CurrentLevel.OnLoseGame();
    }

    public void OnWinGame()
    {
        CurrentLevel.OnWinGame();
    }
}