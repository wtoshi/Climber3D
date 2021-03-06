using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class SpawnController : PersistentSingleton<SpawnController>
{
    [FoldoutGroup("Bat Spawn Settings")]
    [SerializeField] GameObject batsPF;
    [FoldoutGroup("Bat Spawn Settings")] [InfoBox("X: Left , Y: Right")]
    [SerializeField] Vector2 batsSpawnPositions;
    [FoldoutGroup("Bat Spawn Settings")]
    [SerializeField] float yMaxDistanceToPlayer;

    [FoldoutGroup("Rock Spawn Settings")]
    [SerializeField] GameObject rockPF;
    [FoldoutGroup("Rock Spawn Settings")][InfoBox("X:: Min X , Y: Max Y")]
    [SerializeField] Vector2 rockSpawnPositions;
    [FoldoutGroup("Rock Spawn Settings")]
    [SerializeField] float yDistanceToPlayer;

    [FoldoutGroup("Alert UI Settings")]
    [SerializeField] GameObject alertUIPF;
    [FoldoutGroup("Alert UI Settings")]
    [SerializeField] Transform alertUIHolder;
    [FoldoutGroup("Alert UI Settings")]
    [SerializeField] float alertUIScreenGap;

    public float AlertUIScreenGap => alertUIScreenGap;

    Coroutine _spawnBatsCO;
    Coroutine _spawnRockCO;

    public enum ObstacleTypes
    {
        Bats, Rock, Spider
    }

    private void OnEnable()
    {
        EventManager.LevelResetEvent.AddListener(ResetHolder);
    }
    private void OnDisable()
    {
        EventManager.LevelResetEvent.RemoveListener(ResetHolder);
    }

    public void SpawnRock(bool _mode)
    {
        if (_spawnRockCO != null)
            StopCoroutine(_spawnRockCO);

        if (_mode)
        {
            _spawnRockCO = StartCoroutine(doSpawnRock());
        }
    }

    IEnumerator doSpawnRock()
    {
        while (true)
        {
            var random = Random.Range(GameManager.Instance.GameSettings.BaseSpawnGap, GameManager.Instance.GameSettings.BaseSpawnGap + 5f);
            yield return new WaitForSeconds(random);

            var playerPos = GameManager.Instance.Player.PlayerMovement.pelvis.transform.position;

            bool fmLeft = Random.Range(0, 2) == 0;

            Vector3 spawnPos = new Vector3();

            if ((playerPos.y + yDistanceToPlayer) > LevelController.Instance.LevelFacade.LevelHeight)
            {
                yield break;
            }

            spawnPos.z = playerPos.z;
            spawnPos.y = playerPos.y + yDistanceToPlayer;
            spawnPos.x = Random.Range(rockSpawnPositions.x, rockSpawnPositions.y);

            // Add Alert Icon firstly
            AlertUI alert = AddAlertIcon(spawnPos, ObstacleTypes.Rock);

            yield return new WaitForSeconds(2f);
            Destroy(alert.gameObject);

            // Spawn BATS
            GameObject rockObj = Instantiate(rockPF, spawnPos, rockPF.transform.rotation, LevelController.Instance.SpawnsHolder);
            RockObstacle rock = rockObj.GetComponent<RockObstacle>();
            rock.Set();

            yield return null;
        }

    }

    public void SpawnBats(bool _mode)
    {
        if (_spawnBatsCO != null)
            StopCoroutine(_spawnBatsCO);

        if (_mode)
        {
            _spawnBatsCO = StartCoroutine(doSpawnBats());
        }
    }

    IEnumerator doSpawnBats()
    {
        while (true)
        {
            var random = Random.Range(GameManager.Instance.GameSettings.BaseSpawnGap, GameManager.Instance.GameSettings.BaseSpawnGap + 5f);
            yield return new WaitForSeconds(random);

            var playerPos = GameManager.Instance.Player.PlayerMovement.pelvis.transform.position;
           
            bool fmLeft = Random.Range(0, 2) == 0;

            Vector3 spawnPos = new Vector3();

            float minY = (playerPos.y + yMaxDistanceToPlayer) > LevelController.Instance.LevelFacade.LevelHeight ? playerPos.y - yMaxDistanceToPlayer / 2 : playerPos.y;
            float maxY = (playerPos.y + yMaxDistanceToPlayer) > LevelController.Instance.LevelFacade.LevelHeight ? LevelController.Instance.LevelFacade.LevelHeight : (playerPos.y + yMaxDistanceToPlayer);
            spawnPos.z = playerPos.z;
            spawnPos.y = Random.Range(minY, maxY);
            spawnPos.x = fmLeft ? batsSpawnPositions.x : batsSpawnPositions.y;

            // Add Alert Icon firstly
            AlertUI alert = AddAlertIcon(spawnPos, ObstacleTypes.Bats);

            yield return new WaitForSeconds(2f);
            Destroy(alert.gameObject);

            // Spawn BATS
            GameObject bats = Instantiate(batsPF, spawnPos, batsPF.transform.rotation, LevelController.Instance.SpawnsHolder);

            var newRot = bats.transform.eulerAngles;
            newRot.y = fmLeft ? 90f : -90f;
            bats.transform.eulerAngles = newRot;

           

            yield return null;
        }

    }

    AlertUI AddAlertIcon(Vector3 _worldPos, ObstacleTypes _obstacleType)
    {
        AlertUI alertUI = Instantiate(alertUIPF, alertUIHolder).GetComponent<AlertUI>();

        alertUI.Set(_worldPos, _obstacleType);

        return alertUI;
    }

    void ResetHolder()
    {
        alertUIHolder.DestroyChildren();
    }

}
