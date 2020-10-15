using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.AI;
using Debug = UnityEngine.Debug;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    private StateMachine _stateMachine;

    public Action<IState> OnGameStateChanged;
    private AbstractRunner[] _runners;
    private AbstractRunner[] _finalRunnersPositions;
    private int _runnersThatFinished = 0;
    public event Action<bool> OnHandleRunnerLockState;
    public event Action OnResetRunnerStatus;

    public AbstractRunner[] FinalRunnersPositions => _finalRunnersPositions.Where(runner => runner != null).ToArray();

    public event Action OnResetRace;

    public bool RaceStarted { get; private set; }
    public bool RaceFinished { get; private set; }

    public Dictionary<int, FloorCharacteristics> FloorTypes { get; private set; }

    public bool HasPlayerWon => RaceFinished && (_finalRunnersPositions[0] is Player);

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
        AbstractRunner.OnLap += ListenOnPlayerLap;
    }

    private void OnDisable()
    {
        _stateMachine.OnStateChanged -= CallOnStateChange;
        AbstractRunner.OnLap -= ListenOnPlayerLap;
    }

    private void Update()
    {
        _stateMachine.Tick();
        
        if (RaceStarted)
            CalculatePlayerPositions();
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
         var load = new LoadLevelState(this);
         var countDown = new CountDownState(this);
         var play = new PlayState(this);
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
            FloorTypes.Add(1 << NavMesh.GetAreaFromName(type.FloorTag), type);
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
        OnResetRace?.Invoke();
    }

    public void CountDownFinished()
    {
        RaceStarted = true;
    }

    public void LockPlayers(bool lockState) => OnHandleRunnerLockState?.Invoke(lockState);

    public void ResetPlayersWayPoints() => OnResetRunnerStatus?.Invoke();
    
    public void GetAllPlayers()
    {
        _runners = FindObjectsOfType<AbstractRunner>();
        _finalRunnersPositions = new AbstractRunner[_runners.Length];
        _runnersThatFinished = 0;
    }
    
    private void ListenOnPlayerLap(AbstractRunner runner, int lapCount)
    {    
        Debug.Log($"{runner} -> {lapCount}", runner.transform);
        if (lapCount >= 3)
        {
            runner.FinishRace(_runnersThatFinished);
            _finalRunnersPositions[_runnersThatFinished] = runner;

            _runnersThatFinished++;
        }


        if ((runner is Player && _finalRunnersPositions.Contains(runner)) || _runnersThatFinished >= _runners.Length - 1)
        {
            RaceFinished = true;
            
            
            //TODO: Remove this
            // string aux = "Final Race Status -> ";
            //
            // foreach (var finalRunners in _finalRunnersPositions)
            // {
            //     aux += finalRunners + "  -  ";
            // }
            // Debug.LogError(aux);
        }
    }

    private void CalculatePlayerPositions()
    {
        if (_runners == null) return;
        if (!(_stateMachine.CurrentState is PlayState)) return;
        
        _runners = _runners.OrderBy(runner => runner.PositionIndex).ToArray();
        // TODO: Remove this
        // string aux = null;
        //
        // foreach (var runner in _runners)
        // {
        //     aux += runner + "  -  ";
        // }
        // Debug.Log(aux);
    }
}