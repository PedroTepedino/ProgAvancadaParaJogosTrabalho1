using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.AI;
using Debug = UnityEngine.Debug;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    private StateMachine _stateMachine;
    public Action<IState> OnGameStateChanged;

    public bool RaceStarted { get; private set; }
    public bool RaceFinished { get; private set; }

    public Dictionary<int, FloorCharacteristics> FloorTypes { get; private set; }  

    public static GameManager Instance
    {
        get
        {
            if (_instance == null) CreateGameManager();
            
            return _instance;
        }  
    }

    private void Awake()
    {
        CreateSingleTon();

        CreateStateMachine();

        CreateFloorTypesDictionary();
    }

    private void OnEnable()
    {
        _stateMachine.OnStateChanged += CallOnStateChange;
    }

    private void OnDisable()
    {
        _stateMachine.OnStateChanged -= CallOnStateChange;
    }

    private void Update()
    {
        _stateMachine.Tick();
    }

    public static void InitializeGameManager()
    {
        if (_instance == null) 
            CreateGameManager();
    }

    private static void CreateGameManager()
    {
        var gameManager = new GameObject("[GAME MANAGER]");
        _instance = gameManager.AddComponent<GameManager>();
    }

    private void CreateSingleTon()
    {
        if (_instance != null) Destroy(this.gameObject);
        else _instance = this;
        
        DontDestroyOnLoad(this.gameObject);
    }

    private void CreateStateMachine()
    {
         _stateMachine = new StateMachine();
         
         var menu = new MenuState();
         var load = new LoadLevelState();
         var countDown = new CountDownState(this);
         var play = new PlayState();
         var pause = new PauseState();
         var finishRace = new FinishRace();
         var exit = new ExitState();
         
         _stateMachine.SetState(menu);
         
         _stateMachine.AddTransition(menu, load, () => LoadLevelState.LevelToLoad != null); 
         _stateMachine.AddTransition(menu, exit,() => ExitButton.Pressed);
         
         _stateMachine.AddTransition(load, countDown, load.FinishLoading); 
         
         _stateMachine.AddTransition(countDown, play,() => RaceStarted);
         _stateMachine.AddTransition(countDown, pause,() => PlayerInput.Instance.Pause);
         
         _stateMachine.AddTransition(play, finishRace,() => RaceFinished);
         _stateMachine.AddTransition(play, pause, () => PlayerInput.Instance.Pause);
         
         _stateMachine.AddTransition(finishRace, menu, () => MenuButton.Pressed);
         _stateMachine.AddTransition(finishRace, load, () => LoadLevelState.LevelToLoad != null);
         _stateMachine.AddTransition(finishRace, exit, () => ExitButton.Pressed);
         
         _stateMachine.AddTransition(pause, countDown, () => PauseButton.Pressed && !RaceStarted);
         _stateMachine.AddTransition(pause, countDown, () =>  PlayerInput.Instance.Pause && !RaceStarted);
         _stateMachine.AddTransition(pause, play, () => PauseButton.Pressed && RaceStarted);
         _stateMachine.AddTransition(pause, play, () => PlayerInput.Instance.Pause && RaceStarted);
         _stateMachine.AddTransition(pause, load, () => LoadLevelState.LevelToLoad != null);
         _stateMachine.AddTransition(pause, menu, () => MenuButton.Pressed);
         _stateMachine.AddTransition(pause, exit, () => ExitButton.Pressed);
    }

    private void CreateFloorTypesDictionary()
    {
        var floorTypes = Resources.LoadAll<FloorCharacteristics>("FloorTypes");

        FloorTypes = new Dictionary<int, FloorCharacteristics>();
        
        foreach (var type in floorTypes)
        {
            FloorTypes.Add(NavMesh.GetAreaFromName(type.FloorTag), type);
        }
    }

    private void CallOnStateChange(IState state)
    {
        OnGameStateChanged?.Invoke(state);
    }

    public void ResetRaceStatus()
    {
        RaceStarted = false;
        RaceFinished = false;
    }

    public void CountDownFinished()
    {
        RaceStarted = true;
    }
}