using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockCollider : MonoBehaviour
{
    [SerializeField] RockObstacle myRock;
    [SerializeField] MeshCollider myCol;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Consts.Tags.PlayerRagdoll))
        {
            var collisionPoint = myCol.ClosestPoint(other.transform.position);
            collisionPoint.z = other.transform.position.z;

            myCol.enabled = false;

            myRock.HitToPlayer(collisionPoint);
        }
    }
}
