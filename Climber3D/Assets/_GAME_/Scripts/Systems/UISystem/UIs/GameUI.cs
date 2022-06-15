using TMPro;
using UnityEngine;

public class GameUI : _UI
{
	[SerializeField] private TextMeshProUGUI levelText;
	[SerializeField] GameObject playButton;

	private void OnEnable()
	{
		EventManager.LevelLoadedEvent.AddListener(OnLevelLoaded);
		EventManager.LevelSuccessEvent.AddListener(OnLevelSuccess);
		EventManager.LevelFailEvent.AddListener(OnLevelFail);
		EventManager.LevelStartEvent.AddListener(OnGameStartedEvent);
	}

	private void OnDisable()
	{
		EventManager.LevelLoadedEvent.RemoveListener(OnLevelLoaded);
		EventManager.LevelSuccessEvent.RemoveListener(OnLevelSuccess);
		EventManager.LevelFailEvent.RemoveListener(OnLevelFail);
		EventManager.LevelStartEvent.RemoveListener(OnGameStartedEvent);
	}

	private void OnLevelLoaded(LevelLoadedEventData eventData)
	{
		levelText.text = "LEVEL " + eventData.LevelNo.ToString("0");
		SetShown();

        if (!playButton.activeSelf)
        {
			playButton.SetActive(true);
		}
	}

	private void OnLevelFail()
	{
		SetHidden();
	}

	private void OnLevelSuccess()
	{
		SetHidden();
	}

	void OnGameStartedEvent()
    {
		playButton.SetActive(false);
	}

}
