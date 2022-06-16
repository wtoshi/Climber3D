using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class AlertUI : MonoBehaviour
{
    [SerializeField] RectTransform rectTransform;

    SpawnController.ObstacleTypes _alertType;
    Vector3 _spawnedWorldPos;
    bool _isActive;

    private void Update()
    {
        if (_isActive)
        {
            UpdatePos(_alertType);
        }   
    }

    public void Set(Vector3 _worldPos, SpawnController.ObstacleTypes _obstacleType)
    {
        _spawnedWorldPos = _worldPos;
        _alertType = _obstacleType;

        var screenPoint = Camera.main.WorldToScreenPoint(_spawnedWorldPos);
        screenPoint.z = 0f;

        if (_obstacleType == SpawnController.ObstacleTypes.Bats)
        {
            bool fmLeft = _spawnedWorldPos.x <= 0;

            if (fmLeft)
            {
                rectTransform.pivot = new Vector2(0,.5f);
                screenPoint.x = SpawnController.Instance.AlertUIScreenGap;
            }
            else
            {
                rectTransform.pivot = new Vector2(1f, .5f);
                screenPoint.x = Screen.width - SpawnController.Instance.AlertUIScreenGap;
            }
        }
        else if (_obstacleType == SpawnController.ObstacleTypes.Rock)
        {
            rectTransform.pivot = new Vector2(0.5f, 1f);
            screenPoint.y = Screen.height - SpawnController.Instance.AlertUIScreenGap;
        }
        rectTransform.position = screenPoint;
        Animate();

        _isActive = true;
    }

    void UpdatePos(SpawnController.ObstacleTypes _obstacleType)
    {
        var myScreenPos = rectTransform.position;

        switch (_obstacleType)
        {
            case SpawnController.ObstacleTypes.Bats:
                myScreenPos.y = Camera.main.WorldToScreenPoint(_spawnedWorldPos).y;
                break;
            case SpawnController.ObstacleTypes.Rock:
                myScreenPos.x = Camera.main.WorldToScreenPoint(_spawnedWorldPos).x;
                break;
        }


        rectTransform.position = myScreenPos;
    }

    void Animate()
    {
        transform.DOScale(1.1f, .3f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo).From(1f);
    }
}
