using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections.Generic;

public class LevelDesignManager : MonoBehaviour
{
    [SerializeField] GameObject climbPointPF;

    
    bool _setup = false;

    Transform _root;
    LevelFacade _levelFacade;

    Transform _climbPointHolder;
    List<ClimbPoint> _climbPointsList;

    Transform _levelWall;

#if UNITY_EDITOR
    [PropertySpace]
    [Button][DisableIf("_setup")]
    public void FirstSetup()
    {
        _root = transform.root;
        _levelFacade = _root.GetComponent<LevelFacade>();

        _climbPointHolder = _root.Find("Environment/Climb Points Holder");

        _climbPointsList = new List<ClimbPoint>();
        _climbPointHolder.GetComponentsInChildren<ClimbPoint>(_climbPointsList);

        _levelWall = _root.Find("Environment/Level Wall");

        levelHeight = _levelFacade.LevelHeight;

        _setup = true;
    }
    [PropertySpace] [EnableIf("_setup")] [OnValueChanged("SetLevelHeight")]
    [SerializeField] float levelHeight;

    [PropertySpace]
    [Button][EnableIf("_setup")]
    void AddClimbPoints()
    {
        float maxY = 0f;

        for (int i = 0; i < _climbPointsList.Count; i++)
        {
            if (_climbPointsList[i] == null)
            {
                _climbPointsList = new List<ClimbPoint>();
                _climbPointHolder.GetComponentsInChildren<ClimbPoint>(_climbPointsList);

                AddClimbPoints();

                return;
            }

            float yVal = _climbPointsList[i].transform.position.y;
            if (yVal > maxY)
            {
                maxY = yVal;
            }
        }

        for (int i = 0; i < 5; i++)
        {
            GameObject obj = Instantiate(climbPointPF, _climbPointHolder);

            var newPos = obj.transform.position;
            newPos.y = maxY + 1f + i;
            obj.transform.position = newPos;

            ClimbPoint climbPoint = obj.GetComponent<ClimbPoint>();
            _climbPointsList.Add(climbPoint);
        }        
    }

    [Button][EnableIf("_setup")]
    [PropertySpace]
    [ExecuteInEditMode]
    void CheckClimbPoints()
    {
        bool error;


        for (int i = 0; i < _climbPointsList.Count; i++)
        {
            /*

            */


            /*
            RaycastHit hit;

            if(_climbPointsList[i].gameObject.scene.GetPhysicsScene().SphereCast(_climbPointsList[i].transform.position, 25f, Vector3.zero, out hit))
            {
                Debug.Log("name: " + _climbPointsList[i].name + " hit: " + hit.collider.name);
            }
            else
            {
                Debug.Log("name: " + _climbPointsList[i].name + " ...");
            }
            */

            /*
            RaycastHit hit;

            if (Physics.SphereCast(_climbPointsList[i].transform.position,250f, Vector3.zero, out hit, 250f, LayerMask.GetMask("Default")))
            {
                Debug.Log("name: " + _climbPointsList[i].name + " hit: " + hit.collider.name);
            }
            else
            {
                Debug.Log("name: " + _climbPointsList[i].name + " ...");
            }
            */

            Collider[] cols = new Collider[] {};
            int count = gameObject.scene.GetPhysicsScene().OverlapSphere(_climbPointsList[i].transform.position, 25f, cols, LayerMask.GetMask("Default"), QueryTriggerInteraction.Collide);

            Debug.Log("count: "+ count);
            Debug.Log("cols count: " + cols.Length);

            if (cols.Length <= 0)
            {
                error = true;
            }

            /*
            RaycastHit hit;
            if (gameObject.scene.GetPhysicsScene().SphereCast(_climbPointsList[i].transform.position, 2.5f, Vector3.one, out hit, 2.5f, LayerMask.GetMask("Touchable")))
            {
                Debug.Log("name: " + _climbPointsList[i].name + " hit: " + hit.collider.name);
            }
            else
            {
                Debug.Log("name: " + _climbPointsList[i].name + " ...");
            }
            */
        }
    }

    void SetLevelHeight()
    {
        _levelFacade.LevelHeight = levelHeight;

        var newScale = _levelWall.transform.localScale;
        newScale.y = levelHeight;
        _levelWall.transform.localScale = newScale;
    }

    //visualize the overlapsphere
    private void OnDrawGizmosSelected()
    {
        if (!_setup) return;
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(_levelFacade.FirstJumpPoint.transform.position, 2.5f);
    }
#endif


}
