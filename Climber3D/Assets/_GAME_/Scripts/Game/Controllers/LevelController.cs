using UnityEngine;

public class LevelController : _LevelController
{
	[SerializeField] GameObject playerPF;

    private void Start()
    {
		LoadLevel();
	}

    protected override void LoadLevel()
	{
		base.LoadLevel();
		PrepareLevel();
	}

	private void PrepareLevel()
	{
		LevelFacade levelFacade = Instantiate<LevelFacade>(LevelContent.LevelFacade, LevelParent);

		GameObject player = Instantiate(playerPF, levelFacade.transform);

		Transform target = player.transform;

		if (target != null)
		{
			CameraManager.Instance.Init(target);
		}

		SendLevelLoadedEvent(levelFacade);
	}
}
