using UnityEngine;

public class LevelController : _LevelController
{
	[SerializeField] GameObject playerPF;
	[SerializeField] Transform spawnsHolder;

	LevelFacade _levelFacade;

	public LevelFacade LevelFacade => _levelFacade;
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
}
