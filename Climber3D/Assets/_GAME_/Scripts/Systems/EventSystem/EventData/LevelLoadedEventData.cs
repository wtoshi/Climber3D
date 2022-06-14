
[System.Serializable]
public class LevelLoadedEventData
{
    public LevelFacade LevelFacade;
    public int LevelNo;

    public LevelLoadedEventData(LevelFacade levelFacade, int levelNo)
    {
        LevelFacade = levelFacade;
        LevelNo = levelNo;
    }
}
