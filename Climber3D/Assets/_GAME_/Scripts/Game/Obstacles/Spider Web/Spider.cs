using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spider : MonoBehaviour
{
    [SerializeField] Animator myAnim;
    float _moveSpeed = 3f;

    int MovingHash = Animator.StringToHash("IsMoving");
    int AttackHash = Animator.StringToHash("Attack");

    private void Start()
    {
        var distance = GameManager.Instance.Player.transform.position - transform.position;
        transform.rotation = Quaternion.LookRotation(distance.normalized, transform.up);
    }

    public void Attack(Vector3 _movePos)
    {
        if (GameManager.Instance.GameState != GameManager.GameStates.Started)
            return;

        StartCoroutine(doAttack(_movePos));
    }

    IEnumerator doAttack(Vector3 _movePos)
    {
        var distance = _movePos - transform.position;

        SetMovingState(true);

        while (distance.magnitude >= .1f)
        {
            distance = _movePos - transform.position;

            transform.rotation = Quaternion.LookRotation(distance.normalized, transform.up);
            
            transform.position += distance.normalized * _moveSpeed * Time.deltaTime;

            yield return new WaitForEndOfFrame();
            yield return null;
        }

        Bite();
    }

    public void SetMovingState(bool val)
    {
        myAnim.SetBool(MovingHash, val);
    }

    public void Bite()
    {
        SetMovingState(false);
        myAnim.SetTrigger(AttackHash);

        GameManager.Instance.Player.GetHit();

        this.Run(.2f, ()=> {
            GameManager.Instance.FinishLevel(false);
        });
    }

}
