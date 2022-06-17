using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FXController : PersistentSingleton<FXController>
{
    public GameObject AddParticle(string _fxName, Vector3 _pos, float _destroyTime = -1f, Transform _holder = null, float _scale = 0)
    {
        GameObject particle = Resources.Load<GameObject>(_fxName);

        if (particle == null)
            return null;

        if (_holder)
        {
            particle = Instantiate(particle, _pos, particle.transform.rotation, _holder);
        }
        else
        {
            particle = Instantiate(particle, _pos, particle.transform.rotation);
        }

        if (_destroyTime == -1f)
        {
            return particle;
        }
        else
        {
            Destroy(particle, _destroyTime);
            return null;
        }

    }

}