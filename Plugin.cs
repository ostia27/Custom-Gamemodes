using BepInEx;
using CustomGameModes.Controllers;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using WKLib.Core;

namespace CustomGameModes;

public class TextLoading: MonoBehaviour
{
    public static GameObject LoadingInstance;

    void Awake()
    {
        if (LoadingInstance) return;
        GameObject canvas = GameObject.Find("Canvas/Loading");
        if (canvas)
        {
            LoadingInstance = Instantiate(canvas);
            LoadingInstance.name = "LoadingPrefab";
            LoadingInstance.SetActive(false);
            LoadingInstance.GetComponent<Image>().color += new Color(0f, 0f, 0f, 0.4f);
            DontDestroyOnLoad(LoadingInstance);
        }
    }
}

[BepInIncompatibility("com.validaq.loadintolevel")]
[BepInDependency("WKLib")]
[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin
{
    public static Plugin Instance;
    public ModContext Context;

    public GameObject stuffHolder;


    private bool _isObjectInitialized;
    private bool _isGameModeManagerInitialized;
    private bool _isCustomItemsInitialized;

    private void Awake()
    {
        Instance = this;

        // Register to WKLib
        Context = ModRegistry.Register(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_VERSION);

        // Plugin startup logic
        LogManager.Init(Logger);
        LogManager.Info($"Plugin {MyPluginInfo.PLUGIN_GUID} v{MyPluginInfo.PLUGIN_VERSION} is loaded!");

        SceneManager.sceneLoaded += OnSceneLoaded;
    }


    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SetupMainObject();

        switch (scene.name)
        {
            case "Game-Main":
                SetupCustomItemHolder();
                break;
            case "Intro":
                SetupGameModeController();
                break;
        }
    }

    private void SetupMainObject()
    {
        if (_isObjectInitialized) return;
        _isObjectInitialized = true;

        stuffHolder = new GameObject("CustomGamemodesManager");
        DontDestroyOnLoad(stuffHolder);
    }

    private void SetupCustomItemHolder()
    {
        if (_isCustomItemsInitialized) return;
        _isCustomItemsInitialized = true;

        stuffHolder.AddComponent<SpawnController>();
    }

    private void SetupGameModeController()
    {
        if (_isGameModeManagerInitialized) return;
        _isGameModeManagerInitialized = true;

        stuffHolder.AddComponent<GameModeController>();
        stuffHolder.AddComponent<TextLoading>();
    }
}