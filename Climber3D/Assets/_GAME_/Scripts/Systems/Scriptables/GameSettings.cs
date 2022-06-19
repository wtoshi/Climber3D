using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "GameSettings", menuName = "Settings/GameSettings")]
public class GameSettings : ScriptableObject
{
	[SerializeField] float _baseSpawnGap;

	public float BaseSpawnGap => _baseSpawnGap;

}

