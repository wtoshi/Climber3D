using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RockObstacle : MonoBehaviour
{
    [SerializeField] Transform modelTransform;
    [SerializeField] float fallingSpeed;

    bool _isActive;

    private void Update()
    {
        if (!_isActive)
            return;

        var newPos = transform.position;
        newPos.y -= fallingSpeed * Time.deltaTime;
        transform.position = newPos;
    }

    public void Set()
    {
        _isActive = true;
        Animate();
    }

    public void HitToPlayer(Vector3 _hitPoint)
    {
        if (GameManager.Instance.GameState != GameManager.GameStates.Started)
            return;

        _isActive = false;
        modelTransform.gameObject.SetActive(false);

        // Add Rock Hit FX
        FXController.Instance.AddParticle(Consts.Particles.RockHit, _hitPoint, 2f, LevelController.Instance.SpawnsHolder);

        // Player GetHit
        GameManager.Instance.Player.GetHit();

        // Fail LEVEL
        GameManager.Instance.FinishLevel(false);
    }

    void Animate()
    {
        var toRotate = new Vector3(360f,0,0);
        modelTransform.DORotate(toRotate, .5f, RotateMode.FastBeyond360).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
    }
}
