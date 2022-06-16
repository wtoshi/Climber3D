using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebCol : MonoBehaviour
{
    [SerializeField] SpiderWeb myWeb;
    [SerializeField] MeshCollider myCol;

    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Consts.Tags.PlayerRagdoll))
        {
            var collisionPoint =  myCol.ClosestPoint(other.transform.position);
            collisionPoint.z = other.transform.position.z;
            myCol.enabled = false;
            myWeb.OnPlayerEntered(collisionPoint);
        }
    }
    

}
