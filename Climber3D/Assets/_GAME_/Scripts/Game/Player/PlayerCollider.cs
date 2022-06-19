using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollider : MonoBehaviour
{
    [SerializeField] PlayerController playerController;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag(Consts.Tags.Ground))
        {
            if (GameManager.Instance.GameState != GameManager.GameStates.Finished)
                return;

            //collision.collider.enabled = false;
            playerController.OnHitGround(collision.contacts[0].point);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Consts.Tags.LevelEnd))
        {
            if (GameManager.Instance.GameState != GameManager.GameStates.Started)
                return;

            other.enabled = false;
            playerController.OnLevelEnd();

            GameManager.Instance.FinishLevel(true);
        }
    }
}
