using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    public static GameManager Instance { get; private set; }

    public event EventHandler OnStateChanged;
    public event EventHandler OnGamePaused;
    public event EventHandler OnGameUnPaused;
    public event EventHandler OnLocalPlayerReadyChanged;

    private enum State
    {
        WaitingToStart,
        CountDownToStart,
        GamePlaying,
        GameOver
    }

    private NetworkVariable<State> state = new NetworkVariable<State>(State.WaitingToStart);
    private bool isLocalPlayerReady;
    private NetworkVariable<float> coundownToStartTimer = new NetworkVariable<float>(3f);
    private NetworkVariable<float> gamePlayingTimer = new NetworkVariable<float>(0f);
    private float gamePlayingTimerMax = 60f;
    private bool isGamePaused = false;
    private Dictionary<ulong, bool> playerReadyDictionary;

    private void Awake()
    {
        Instance = this;

        playerReadyDictionary = new Dictionary<ulong, bool>();
    }

    private void Start()
    {
        PlayerInputManager.Instance.OnPauseAction += InputManager_OnPauseAction;
        PlayerInputManager.Instance.OnInteractAction += InputManager_OnInteractAction;
    }

    public override void OnNetworkSpawn()
    {
        state.OnValueChanged += State_OnValueChanged;
        PlayerInputManager.Instance.OnPauseAction += InputManager_OnPauseAction;
        PlayerInputManager.Instance.OnInteractAction += InputManager_OnInteractAction;
    }

    private void State_OnValueChanged(State previousValue, State newValue)
    {
        OnStateChanged?.Invoke(this, EventArgs.Empty);
    }

    private void InputManager_OnInteractAction(object sender, EventArgs e)
    {
        if(state.Value == State.WaitingToStart)
        {
            isLocalPlayerReady = true;
            SetPlayerReadyServerRpc();
            OnLocalPlayerReadyChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void SetPlayerReadyServerRpc(ServerRpcParams serverRpcParams = default)
    {
        playerReadyDictionary[serverRpcParams.Receive.SenderClientId] = true;

        bool allClientsReady = true;
        foreach (ulong clientId in NetworkManager.Singleton.ConnectedClientsIds)
        {
            if(!playerReadyDictionary.ContainsKey(clientId) || !playerReadyDictionary[clientId])
            {
                allClientsReady = false;
                break;
            }
        }

        if (allClientsReady)
        {
            state.Value = State.CountDownToStart;
        }
    }

    private void InputManager_OnPauseAction(object sender, EventArgs e)
    {
        TogglePauseGame();
    }
    private void Update()
    {
        if (!IsServer)
        {
            return;
        }
        switch (state.Value)
        {
            case State.WaitingToStart:
                break;
            case State.CountDownToStart:
                coundownToStartTimer.Value -= Time.deltaTime;
                if (coundownToStartTimer.Value < 0f)
                {
                    gamePlayingTimer.Value = gamePlayingTimerMax;
                    state.Value = State.GamePlaying;
                }
                break;
            case State.GamePlaying:
                gamePlayingTimer.Value -= Time.deltaTime;
                if (gamePlayingTimer.Value < 0f)
                {
                    state.Value = State.GameOver;
                }
                break;
            case State.GameOver:
                break;
        }
    }
    public bool IsGamePlaying()
    {
        return state.Value == State.GamePlaying;
    }

    public bool IsCountdownToStartActive()
    {
        return state.Value == State.CountDownToStart;
    }

    public float GetCountDownToStartTimer()
    {
        return coundownToStartTimer.Value;
    }
    public bool IsGameOver()
    {
        return state.Value == State.GameOver;
    }

    public bool IsLocalPlayerReady()
    {
        return isLocalPlayerReady;
    }

    public float GetGamePlayingTimerNormalized()
    {
        return 1 - (gamePlayingTimer.Value / gamePlayingTimerMax);
    }

    public void TogglePauseGame()
    {
        isGamePaused = !isGamePaused;

        if (isGamePaused)
        {
            OnGamePaused?.Invoke(this, EventArgs.Empty);
            Time.timeScale = 0f;
        }
        else
        {
            OnGameUnPaused?.Invoke(this, EventArgs.Empty);
            Time.timeScale = 1f;
        }
    }

}
