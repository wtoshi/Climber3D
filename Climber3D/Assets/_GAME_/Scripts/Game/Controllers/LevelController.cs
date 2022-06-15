using UnityEngine;

public class LevelController : _LevelController
{
	[SerializeField] GameObject playerPF;

	LevelFacade _levelFacade;

	public LevelFacade LevelFacade => _levelFacade;

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
		_levelFacade = Instantiate<LevelFacade>(LevelContent.LevelFacade, LevelParent);

		GameObject player = Instantiate(playerPF, _levelFacade.transform);

		Transform target = player.transform;

		if (target != null)
		{
			CameraManager.Instance.Init(target);
		}

		SendLevelLoadedEvent(_levelFacade);
	}
}
