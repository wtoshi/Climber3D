using System.Collections.Generic;
using Sirenix.OdinInspector;

[System.Serializable]
public class LevelSaveController : _SaveController<LevelSaveData>
{

	protected override JsonSerializer<LevelSaveData> Serializer
	{
		get
		{
			if (_serializer == null)
			{
				Init(Consts.FileNames.LEVELDATA);
			}
			return _serializer;
		}
	}

	public int CurrentLevelNo => Serializer.Data.currentLevelNo;

	[Button("Set CurrentLevelNo", ButtonSizes.Medium)]
	public void SetCurrentLevelNo(int currentLevelNo, bool save = true)
	{
		Serializer.Data.currentLevelNo = currentLevelNo;
		if (save)
		{
			SaveData();
		}
	}

}