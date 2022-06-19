using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelContent", menuName = "Settings/LevelContent")]
public class LevelContent : ScriptableObject
{
	[SerializeField] LevelFacade _levelFacade;

	public LevelFacade LevelFacade { get => _levelFacade; set { _levelFacade = value; } }

}

