using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bats : MonoBehaviour
{
    public ParticleSystem batsParticle;

    void Start()
    {
        batsParticle = GetComponent<ParticleSystem>();
    }

    void OnParticleCollision(GameObject other)
    {
        if (GameManager.Instance.GameState != GameManager.GameStates.Started)
            return;

        if (other.tag == Consts.Tags.PlayerRagdoll)
        {
            ParticleSystem.CollisionModule colModule = batsParticle.collision;
            colModule.enabled = false;

            GameManager.Instance.Player.GetHit();

            GameManager.Instance.FinishLevel(false);
        }
            
    }

}
