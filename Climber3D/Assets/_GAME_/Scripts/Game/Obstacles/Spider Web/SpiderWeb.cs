using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderWeb : MonoBehaviour
{
    [SerializeField] List<GameObject> spiderPFs = new List<GameObject>();
    [SerializeField] Transform spiderSpawnPosition;

    Spider _mySpider;

    private void Start()
    {
        GameObject spiderObj = Instantiate(spiderPFs[Random.Range(0, spiderPFs.Count)], spiderSpawnPosition.position, spiderSpawnPosition.rotation, transform);
        _mySpider = spiderObj.GetComponent<Spider>();
    }

    public void OnPlayerEntered(Vector3 _pos)
    {
        Debug.Log("Player entered to WEB!");
        GameManager.Instance.Player.PlayerMovement.CanClimb = false;
        _mySpider.Attack(_pos);
    }


}
