using UnityEngine;

public class LevelController : _LevelController
{
	[SerializeField] GameObject playerPF;
	[SerializeField] GameObject levelEndPF;

	LevelFacade _levelFacade;
	LevelEnd _levelEnd;

	public LevelFacade LevelFacade => _levelFacade;
	public LevelEnd LevelEnd => _levelEnd;
	public Transform SpawnsHolder => spawnsHolder;

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

		AddLevelEnd();

		GameObject player = Instantiate(playerPF, _levelFacade.transform);

        if (player != null)
        {
			PlayerController playerController = player.GetComponent<PlayerController>();

			CameraManager.Instance.Init(playerController.PlayerMovement.pelvis);

			if (playerController != null)
			{
				GameManager.Instance.Player = playerController;

				SendLevelLoadedEvent(_levelFacade);

				return;
			}
		}

		Debug.LogError("Can't Instantiate Player!");
	}

	void AddLevelEnd()
    {
		GameObject levelEndObj = Instantiate(levelEndPF, spawnsHolder);

		var newPos = levelEndObj.transform.position;
		newPos.y = LevelFacade.LevelHeight;
		levelEndObj.transform.position = newPos;

		_levelEnd = levelEndObj.GetComponent<LevelEnd>();
	}
}
