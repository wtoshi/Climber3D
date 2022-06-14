using Sirenix.OdinInspector;

public class DataManager : PersistentSingleton<DataManager>
{

	public readonly LevelSaveController LevelSaveController = new LevelSaveController();

    private void Start()
    {
		LevelSaveController.Init(Consts.FileNames.LEVELDATA);

	}

	[Button(ButtonSizes.Medium)]
	public void ClearAllData()
	{
		LevelSaveController.ClearData();
	}

}