using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public Material[] PreloadedMaterials { get { return _PreloadedMaterials; } set { _PreloadedMaterials = value; } }
    public Player CurrentPlayer { get; private set; }
    public UIManager CurrentInterface { get; private set; }

    [SerializeField]
    private SceneField[] _GameScenes = null;

    [SerializeField]
    private Texture2D _CustomCursorTex = null;

    [SerializeField]
    [Tooltip("THis will help with stuttering and lag when loading new objects for the first time, but will increase the memory usage right away")]
    private bool _PreloadMaterialsInEditor = false;

    [SerializeField]
    private Material[] _PreloadedMaterials = null;

    public void Quit()
    {
        Application.Quit();
    }

    public void StartGame(int index = -1)
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        //Load the game scene
        if (index == -1)
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
        else
            SceneManager.LoadSceneAsync(_GameScenes[index].SceneName, LoadSceneMode.Single);
    }

    public void SetPlayerPosition()
    {
        //Set the position and rotation with the random spawn point transform
        CurrentPlayer.transform.position = GetSpawnPoint();
        CurrentPlayer.transform.rotation = GetSpawnRotation();
    }

    private Vector3 GetSpawnPoint()
    {
        //Get a random spawn point
        var spawnPoints = FindObjectOfType<PlayerSpawnPoints>();
        Vector3 spawnPoint = CurrentPlayer.transform.position;

        if (spawnPoints != null)
        {
            var newSpawnPoint = spawnPoints.GetRandomSpawnPoint();

            if (newSpawnPoint != Vector3.zero)
                spawnPoint = newSpawnPoint;
        }

        return spawnPoint;
    }

    private Quaternion GetSpawnRotation()
    {
        var spawnPoints = FindObjectOfType<PlayerSpawnPoints>();
        Quaternion spawnRotation = CurrentPlayer.transform.rotation;

        if (spawnPoints != null)
        {
            spawnRotation = spawnPoints.GetRandomRotation();
        }
        return spawnRotation;
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        if(Application.isEditor && _PreloadMaterialsInEditor)
        {
            List<GameObject> preloadObjects = new List<GameObject>();

            Camera camera = new GameObject("Material Preload Camera", typeof(Camera)).GetComponent<Camera>();
            camera.orthographic = true;
            camera.orthographicSize = 100f;
            camera.farClipPlane = 100f;
            camera.depth = 999;
            camera.renderingPath = RenderingPath.Forward;
            camera.useOcclusionCulling = camera.allowHDR = camera.allowMSAA = camera.allowDynamicResolution = false;

            preloadObjects.Add(camera.gameObject);

            foreach (var mat in _PreloadedMaterials)
            {
                if (mat == null)
                    continue;

                var quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
                quad.transform.position = camera.transform.position + camera.transform.forward * 50f + camera.transform.right * UnityEngine.Random.Range(-100f, 100f) + camera.transform.up * UnityEngine.Random.Range(-100f, 100f);
                quad.transform.localScale = Vector3.one * 0.01f;

                quad.GetComponent<Renderer>().sharedMaterial = mat;

                preloadObjects.Add(quad);
            }

            camera.Render();

            foreach (var obj in preloadObjects)
                Destroy(obj);

            preloadObjects.Clear();
        }

        if (_CustomCursorTex != null)
            Cursor.SetCursor(_CustomCursorTex, Vector2.zero, CursorMode.Auto);

        DontDestroyOnLoad(gameObject);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        CurrentPlayer = FindObjectOfType<Player>();
        CurrentInterface = FindObjectOfType<UIManager>();

        CurrentInterface.AttachToPlayer(CurrentPlayer);
    }

    private void Start()
    {
        Shader.WarmupAllShaders();
        GC.Collect();
    }
}
