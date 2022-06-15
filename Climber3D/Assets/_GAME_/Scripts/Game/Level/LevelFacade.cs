using UnityEngine;

/// <summary>
/// This class holds all the references required for a level to be instantiated
/// </summary>
public class LevelFacade : MonoBehaviour
{
	[SerializeField] Transform playerSpawnPosition;
	[SerializeField] ClimbPoint firstJumpPoint;

	public Transform PlayerSpawnPosition => playerSpawnPosition;
	public ClimbPoint FirstJumpPoint => firstJumpPoint;
}