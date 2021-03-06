using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class _LevelController : PersistentSingleton<LevelController>
{
    [SerializeField]
    protected Transform LevelParent;
    [SerializeField] protected Transform spawnsHolder;

    protected LevelContent LevelContent;
    protected int LevelNo;

    #region Levels
    [SerializeField, FoldoutGroup("Levels")]
    public List<LevelContent> allLevels = new List<LevelContent>();
    [SerializeField, FoldoutGroup("Levels")]
    public List<LevelContent> levelsToRepeat = new List<LevelContent>();
    #endregion

    protected virtual void OnEnable()
    {
        EventManager.LevelSuccessEvent.AddListener(OnLevelSuccess);
    }

    protected virtual void OnDisable()
    {
        EventManager.LevelSuccessEvent.RemoveListener(OnLevelSuccess);
    }

    protected virtual void LoadLevel()
    {
        LevelContent = GetLevelContent();
    }

    private LevelContent GetLevelContent()
    {
        
        LevelSaveController levelSaveController = DataManager.Instance.LevelSaveController;
        LevelNo = levelSaveController.CurrentLevelNo;

        int index = LevelNo - 1;

        if (index < allLevels.Count)
        {
            return allLevels[index];
        }
        else
        {
            if (levelsToRepeat.Count >= 0)
            {
                return levelsToRepeat[Random.Range(0, levelsToRepeat.Count)];
            }
            else
            {
                Debug.LogError("NO LEVEL!");
                return null;
            }
        }
    
    }

    private void IncreaseLevelNo()
    {

        LevelSaveController levelSaveController = DataManager.Instance.LevelSaveController;

        levelSaveController.SetCurrentLevelNo(levelSaveController.CurrentLevelNo + 1, false);
        levelSaveController.SaveData();
        
    }
    protected virtual void OnLevelSuccess()
    {
        IncreaseLevelNo();
    }

    protected void SendLevelLoadedEvent(LevelFacade facade)
    {
        EventManager.LevelLoadedEvent.Invoke(new LevelLoadedEventData(
            facade,
            LevelNo));
    }

    public virtual void RestartLevel()
    {
        ResetLevel();
        EventManager.LevelResetEvent.Invoke();
        LoadLevel();
    }

    protected virtual void ResetLevel()
    {
        LevelParent.DestroyChildren();
        spawnsHolder.DestroyChildren();
        //TODO Clear destroyList or DestroyTransform
    }

    public void FillLists(LevelContent _level, bool _randomToo)
    {
        allLevels.Add(_level);

        if (_randomToo)
        {
            levelsToRepeat.Add(_level);
        }
    }

}
