using UnityEngine;
using Sirenix.OdinInspector;

/// <summary>
/// This class holds all the references required for a level to be instantiated
/// </summary>
public class LevelFacade : MonoBehaviour
{
	[SerializeField] Transform playerSpawnPosition;
	[SerializeField] ClimbPoint firstJumpPoint;

	[ReadOnly]
	[SerializeField] float levelHeight;

	public Transform PlayerSpawnPosition => playerSpawnPosition;
	public ClimbPoint FirstJumpPoint => firstJumpPoint;
	public float LevelHeight { get => levelHeight; set { levelHeight = value; } }
}