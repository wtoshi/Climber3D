using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEnd : MonoBehaviour
{
    [SerializeField] Transform landingPoint;
    [SerializeField] List<ParticleSystem> particles = new List<ParticleSystem>();

    public Transform LandingPoint => landingPoint;

    private void OnEnable()
    {
        EventManager.LevelSuccessEvent.AddListener(RunParticles);
    }

    private void OnDisable()
    {
        EventManager.LevelSuccessEvent.RemoveListener(RunParticles);
    }

    public void RunParticles()
    {
        foreach (var item in particles)
        {
            item.Play();
        }
    }
}
