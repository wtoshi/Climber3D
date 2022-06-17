using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using Sirenix.OdinInspector;

public class LevelEditor : EditorWindow
{
    [SerializeField] GameObject baseLevel;
    [SerializeField] GameObject climbPointPF;
    [SerializeField] GameObject spiderObstacle;
    List<GameObject> levelRocks = new List<GameObject>();


    List<GameObject> levelsList = new List<GameObject>();

    int windowId = 0;

    float _maxYClimbPoint;

    Scene lastScene;

    LevelFacade levelFacade;
    int levelNameIndex;

    bool _levelCreated = false;
    bool _levelEditMode = false;


    // LEVELFACADE
    Transform _obstacleHolder;
    Transform _climbPointHolder;
    List<ClimbPoint> _climbPointsList;
    Transform _levelWall;
    //

    Vector2 scrollPosition = Vector2.zero;


    [MenuItem("Settings/Level Design")]
    public static void PlatformPanel()
    {
        EditorWindow window = (LevelEditor)EditorWindow.GetWindowWithRect(typeof(LevelEditor), new Rect(0, 0, 700, 500));
    }

    private void OnEnable()
    {
        GetObjectsFmDirectory();
    }


    void OnGUI()
    {
        switch (windowId)
        {
            //MAIN MENU
            case 0:
                GUILayout.Label("Main Menu", EditorStyles.boldLabel);

                if (GUI.Button(new Rect(30, 40 + EditorGUIUtility.singleLineHeight, 200, 200), "LEVELS"))
                {
                    windowId = 1;
                }
                break;

            // Create Platform
            case 1:
                GUILayout.Label("LEVELS", EditorStyles.boldLabel);
                GUILayout.Space(20f);

                int sutunSayisi = 4;
                for (int i = 0; i < levelsList.Count; i++)
                {
                    if (GUI.Button(new Rect(150 * ((i) % sutunSayisi) + 20, 50 + EditorGUIUtility.singleLineHeight + 50 * ((i % sutunSayisi) == 0 ? i / sutunSayisi : 0 + (int)(i / sutunSayisi)), 150, 40), levelsList[i].name))
                    {
                        OpenEditorScene();

                        levelFacade = SpawnLevel(levelsList[i]);
                        _levelEditMode = true;

                        windowId = 11;
                    }
                }

                if (GUI.Button(new Rect(10, 430 + EditorGUIUtility.singleLineHeight, 100, 40), "Back"))
                {
                    windowId = 0;
                }

                if (GUI.Button(new Rect(115, 430 + EditorGUIUtility.singleLineHeight, 100, 40), "Create New"))
                {
                    OpenEditorScene();
                    levelFacade = SpawnLevel();

                    windowId = 11;
                }

                break;

            case 11:
                GUILayout.Label("Create Platform", EditorStyles.boldLabel);
                GUILayout.Space(20f);

                FirstSetup();

                levelFacade.LevelHeight = EditorGUILayout.FloatField("Level Height", levelFacade.LevelHeight);

                if (GUI.changed)
                {
                    SetLevelHeight();
                }

                // ADD CLIMB POINT
                if (GUI.Button(new Rect(10, 200 + EditorGUIUtility.singleLineHeight, 150, 40), "Add Spider Obstacle"))
                {
                    AddSpiderObstacle();
                }

                // ADD CLIMB POINT
                if (GUI.Button(new Rect(10, 250 + EditorGUIUtility.singleLineHeight, 150, 40), "Add Climb Points"))
                {
                    AddClimbPoints();
                }

                // CHECK FREE POINTS
                if (GUI.Button(new Rect(200, 250 + EditorGUIUtility.singleLineHeight, 150, 40), "CHECK POINTS"))
                {
                    CheckClimbPoints();
                }

                // CREATE/SAVE
                string baslik = _levelEditMode ? "Save" : "Create";
                if (GUI.Button(new Rect(10, 380 + EditorGUIUtility.singleLineHeight, 100, 40), baslik))
                {
                    if (_levelEditMode)
                    {
                        PrefabUtility.ApplyPrefabInstance(levelFacade.gameObject, InteractionMode.AutomatedAction);
                        EditorUtility.FocusProjectWindow();
                        Selection.activeObject = levelFacade;
                    }
                    else
                    {
                        GameObject prefab = PrefabUtility.SaveAsPrefabAssetAndConnect(levelFacade.gameObject, "Assets/_GAME_/Prefabs/Levels/" + "Level_" + levelNameIndex + ".prefab", InteractionMode.AutomatedAction);

                        levelsList.Add(prefab);

                        EditorUtility.FocusProjectWindow();
                        Selection.activeObject = levelFacade;
                    }

                    GetObjectsFmDirectory();

                    levelFacade = null;
                    _levelEditMode = false;

                    windowId = 1;
                    OpenMainScene();
                }

                if (GUI.Button(new Rect(115, 380 + EditorGUIUtility.singleLineHeight, 100, 40), "Cancel"))
                {
                    string asset = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(levelFacade.gameObject);

                    if (!string.IsNullOrEmpty(asset) && !_levelEditMode)
                    {
                        AssetDatabase.DeleteAsset(asset);
                    }

                    if (levelFacade != null)
                    {
                        DestroyImmediate(levelFacade.gameObject);
                    }

                    GetObjectsFmDirectory();

                    levelFacade = null;
                    _levelEditMode = false;

                    windowId = 1;

                    OpenMainScene();
                }

                break;



        }

        if (GUI.changed)  //If there have been changes
        {
            if (levelFacade != null)
            {
                EditorUtility.SetDirty(levelFacade.gameObject); //Save changes
            }
        }
    }

    void OpenMainScene()
    {
        lastScene = EditorSceneManager.GetActiveScene();

        if (lastScene.name == "GameScene")
            return;

        EditorSceneManager.OpenScene("Assets/_GAME_/Scenes/GameScene.unity", OpenSceneMode.Single);
    }
    
    void OpenEditorScene() //Create a new scene
    {
        lastScene = EditorSceneManager.GetActiveScene();

        if (lastScene.name == "LevelDesign")
            return;


        EditorSceneManager.SaveScene(lastScene, "Assets/_GAME_/Scenes/" + lastScene.name + ".unity", false);
        EditorSceneManager.OpenScene("Assets/_GAME_/Scenes/LevelDesign.unity", OpenSceneMode.Single);
    }

    LevelFacade SpawnLevel(GameObject _object = null)
    {
        if (levelFacade != null)
            return null;

        string levelName = "Level_";
        bool unpack = false;

        if (_object == null)
        {
            _object = baseLevel;
            unpack = true;
            levelName += levelNameIndex.ToString();
        }
        else
        {
            unpack = false;
            levelName = _object.name;
        }


        GameObject go = PrefabUtility.InstantiatePrefab(_object) as GameObject;
        go.name = levelName;
        //go.GetComponent<Platform>().Set();

        if (unpack)
            PrefabUtility.UnpackPrefabInstance(go, PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);

        UnityEditor.Selection.activeObject = go; //Selecting new object

        return go.GetComponent<LevelFacade>();
    }

    LevelFacade SpawnBaseLevel()
    {
        if (levelFacade != null)
            return null;

        string platformName = "Level"+ levelNameIndex;

        GameObject go = PrefabUtility.InstantiatePrefab(baseLevel) as GameObject;
        go.name = platformName;

        PrefabUtility.UnpackPrefabInstance(go, PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);

        UnityEditor.Selection.activeObject = go; //Selecting new object

        return go.GetComponent<LevelFacade>();
    }

    void GetObjectsFmDirectory()
    {
        //Get Base Level
        string baseLevelDir = "Assets/_GAME_/Prefabs/Levels/Base/";
        DirectoryInfo dir = new DirectoryInfo(baseLevelDir);
        FileInfo[] files = dir.GetFiles("*.prefab");

        GameObject levelPF = (GameObject)AssetDatabase.LoadAssetAtPath(baseLevelDir + files[0].Name, typeof(GameObject));
        baseLevel = levelPF;

        // Fill levelsList
        string levelsDir = "Assets/_GAME_/Prefabs/Levels/";

        DirectoryInfo dir2 = new DirectoryInfo(levelsDir);
        FileInfo[] files2 = dir2.GetFiles("*.prefab");

        levelsList.Clear();
      
        foreach (FileInfo fileInfo in files2)
        {
            GameObject prefab = (GameObject)AssetDatabase.LoadAssetAtPath(levelsDir + fileInfo.Name, typeof(GameObject));
            levelsList.Add(prefab);
        }

        //Get new Level Index
        levelNameIndex = levelsList.Count + 1;

        // Fill RockGroups
        string rocksDir = "Assets/_GAME_/Prefabs/Game/Level/RockGroups/";

        DirectoryInfo dir3 = new DirectoryInfo(rocksDir);
        FileInfo[] files3 = dir3.GetFiles("*.prefab");

        levelRocks.Clear();

        foreach (FileInfo fileInfo in files3)
        {
            GameObject prefab = (GameObject)AssetDatabase.LoadAssetAtPath(rocksDir + fileInfo.Name, typeof(GameObject));
            levelRocks.Add(prefab);
        }
    }

    void FillRocks()
    {
        StartCoroutine(doFillRocks());
    }

    IEnumerator doFillRocks()
    {
        for (int y = 0; y < levelFacade.LevelHeight; y++)
        {

            yield return null;
        }
    }

    void SetLevelHeight()
    {
        if (levelFacade == null)
            return;

        Transform levelWall = levelFacade.transform.Find("Environment/Level Wall");

        var newScale = levelWall.transform.localScale;
        newScale.y = levelFacade.LevelHeight;
        levelWall.transform.localScale = newScale;
    }

    public void FirstSetup()
    {
        if (levelFacade == null)
            return;

        _climbPointHolder = levelFacade.transform.Find("Environment/Climb Points Holder");
        _obstacleHolder = levelFacade.transform.Find("Obstacles");

        _climbPointsList = new List<ClimbPoint>();
        _climbPointHolder.GetComponentsInChildren<ClimbPoint>(_climbPointsList);

        _levelWall = levelFacade.transform.Find("Environment/Level Wall");
    }

    void AddClimbPoints()
    {
        _maxYClimbPoint = 0f;

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
            if (yVal > _maxYClimbPoint)
            {
                _maxYClimbPoint = yVal;
            }
        }

        for (int i = 0; i < 5; i++)
        {
            GameObject obj = Instantiate(climbPointPF, _climbPointHolder);

            var newPos = obj.transform.position;
            newPos.y = _maxYClimbPoint + 1f + i;
            obj.transform.position = newPos;

            obj.name = "Point_"+ _climbPointsList.Count.ToString();

            ClimbPoint climbPoint = obj.GetComponent<ClimbPoint>();
            _climbPointsList.Add(climbPoint);
        }
    }

    void AddSpiderObstacle()
    {
        GameObject obj = Instantiate(spiderObstacle, _obstacleHolder);

        var newPos = obj.transform.position;
        newPos.y = _maxYClimbPoint;
        newPos.z = -1f;
        obj.transform.position = newPos;

        Selection.activeGameObject = obj;
    }

    void CheckClimbPoints()
    {
        bool error = false;

        List<string> freePointNames = new List<string>();

        for (int i = 0; i < _climbPointsList.Count; i++)
        {
            _climbPointsList[i].GetComponent<Collider>().enabled = false;

            Collider[] cols = Physics.OverlapSphere(_climbPointsList[i].transform.position, 2.5f, LayerMask.GetMask("Touchable"));
            
            if (cols.Length <= 0)
            {
                error = true;

                freePointNames.Add(_climbPointsList[i].name);
            }

            _climbPointsList[i].GetComponent<Collider>().enabled = true;
        }

        if (error)
        {
            string names = "";

            for (int x = 0; x < freePointNames.Count; x++)
            {
                names += ","+freePointNames[x] + " ";
            }
            Debug.LogError("Free Points: "+ names);
        }
    }


}
