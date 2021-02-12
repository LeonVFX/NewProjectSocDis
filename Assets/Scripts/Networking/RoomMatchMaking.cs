using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RoomMatchMaking : MonoBehaviourPunCallbacks, IInRoomCallbacks
{
    // Room info
    public static RoomMatchMaking room;
    private PhotonView phoView;

    public bool isGameLoaded;
    public int currentScene;

    // Player info
    public Photon.Realtime.Player[] players;
    public int playersInRoom;
    public int myNumberInRoom;

    public int playersInGame;

    // Delayed start
    private bool readyToCount;
    private bool readyToStart;
    public float startingTime;
    private float lessThanMaxPlayers;
    private float atMaxPlayers;
    private float timeToStart;

    public GameObject lobbyGO;
    public GameObject roomGO;
    public Transform playersPanel;
    public GameObject playerListingPrefab;
    public GameObject startButton;

    private PlayerStgs playerSettings;
    [SerializeField] private SpawnPoints spawnPoints = null;
    [SerializeField] private Transform creatureSpawnPoint = null;

    private void Awake()
    {
        // Singleton
        if (room == null)
        {
            room = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public override void OnEnable()
    {
        // subscribe to functions
        base.OnEnable();
        PhotonNetwork.AddCallbackTarget(this);
        SceneManager.sceneLoaded += OnSceneFinishedLoading;
    }

    public override void OnDisable()
    {
        base.OnDisable();
        PhotonNetwork.RemoveCallbackTarget(this);
        SceneManager.sceneLoaded -= OnSceneFinishedLoading;
    }

    private void Start()
    {
        // set private variables
        phoView = GetComponent<PhotonView>();
        playerSettings = GetComponent<PlayerStgs>();
        readyToCount = false;
        readyToStart = false;
        lessThanMaxPlayers = startingTime;
        atMaxPlayers = 6;
        timeToStart = startingTime;
    }

    private void Update()
    {
        // for delay start onlt, count down to start
        if (MultiplayerSettings.multiplayerSettings.delayStart)
        {
            if (playersInRoom == 1)
            {
                RestartTimer();
            }
            if (!isGameLoaded)
            {
                if (readyToStart)
                {
                    atMaxPlayers -= Time.deltaTime;
                    lessThanMaxPlayers = atMaxPlayers;
                    timeToStart = atMaxPlayers;
                }
                else if (readyToCount)
                {
                    lessThanMaxPlayers -= Time.deltaTime;
                    timeToStart = lessThanMaxPlayers;
                }
                Debug.Log("Display time to start to the players " + timeToStart);
                if (timeToStart <= 0)
                {
                    StartGame();
                }
            }
        }
    }

    public override void OnJoinedRoom()
    {
        // sets player data when we join the room
        base.OnJoinedRoom();
        Debug.Log("We are now in a room");

        lobbyGO.SetActive(false);
        roomGO.SetActive(true);
        if (PhotonNetwork.IsMasterClient)
            startButton.SetActive(true);

        ClearPlayerListing();
        ListPlayers();

        players = PhotonNetwork.PlayerList;
        playersInRoom = players.Length;
        myNumberInRoom = playersInRoom;
        
        // for delay start only
        if (MultiplayerSettings.multiplayerSettings.delayStart)
        {
            Debug.Log("Displayer players in room out of max players possible (" + playersInRoom);
            if (playersInRoom > 1)
            {
                readyToCount = true;
            }
            if (playersInRoom == MultiplayerSettings.multiplayerSettings.maxPlayers)
            {
                readyToStart = true;
                if (!PhotonNetwork.IsMasterClient)
                    return;
                PhotonNetwork.CurrentRoom.IsOpen = false;
            }
        }
    }

    private void ClearPlayerListing()
    {
        // only if in Menu
        if (SceneManager.GetActiveScene().buildIndex == MultiplayerSettings.multiplayerSettings.menuScene)
        {
            for (int i = playersPanel.childCount - 1; i >= 0; --i)
            {
                if (playersPanel.GetChild(i).gameObject != null)
                    Destroy(playersPanel.GetChild(i).gameObject);
            }
        }
    }

    private void ListPlayers()
    {
        if (PhotonNetwork.InRoom)
        {
            foreach (Photon.Realtime.Player player in PhotonNetwork.PlayerList)
            {
                GameObject tempListing = Instantiate(playerListingPrefab, playersPanel);
                Text tempText = tempListing.transform.GetChild(0).GetComponent<Text>();
                tempText.text = player.NickName;
            }
        }
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        // updates player data when a new player joins
        base.OnPlayerEnteredRoom(newPlayer);
        Debug.Log("A new player has joined the room");

        ClearPlayerListing();
        ListPlayers();

        players = PhotonNetwork.PlayerList;
        playersInRoom++;
        // delay start only
        if (MultiplayerSettings.multiplayerSettings.delayStart)
        {
            Debug.Log("Displayer players in room out of max players possible (" + playersInRoom);
            if (playersInRoom > 1)
            {
                readyToCount = true;
            }
            if (playersInRoom == MultiplayerSettings.multiplayerSettings.maxPlayers)
            {
                readyToStart = true;
                if (!PhotonNetwork.IsMasterClient)
                    return;
                PhotonNetwork.CurrentRoom.IsOpen = false;
            }
        }
    }

    public void StartGame()
    {
        // loads the multiplayer scene for all players
        isGameLoaded = true;
        if (!PhotonNetwork.IsMasterClient)
            return;
        if (MultiplayerSettings.multiplayerSettings.delayStart)
            PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.LoadLevel(MultiplayerSettings.multiplayerSettings.loadingScene);

        // Randomize player settings
        playerSettings.RandomizePlayers(playersInRoom);
    }

    private void RestartTimer()
    {
        // restarts the time for when players leave the room (DelayStart)
        lessThanMaxPlayers = startingTime;
        timeToStart = startingTime;
        atMaxPlayers = 6;
        readyToCount = false;
        readyToStart = false;
    }

    #region Game Scene Creation
    private void OnSceneFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        // called when multiplayer scene is loaded
        currentScene = scene.buildIndex;
        if (currentScene == MultiplayerSettings.multiplayerSettings.multiplayerScene)
        {
            isGameLoaded = true;
            // for delay start game
            if (MultiplayerSettings.multiplayerSettings.delayStart)
            {
                phoView.RPC("RPC_LoadedGameScene", RpcTarget.MasterClient);
            }
        }
    }

    [PunRPC]
    private void RPC_LoadedGameScene()
    {
        playersInGame++;
        if (playersInGame == PhotonNetwork.PlayerList.Length)
        {
            phoView.RPC("RPC_CreateGameManager", RpcTarget.MasterClient);
            phoView.RPC("RPC_CreatePlayer", RpcTarget.All);
            phoView.RPC("RPC_CreateLevel", RpcTarget.MasterClient);
            phoView.RPC("RPC_CreateTasks", RpcTarget.MasterClient);
        }
    }

    [PunRPC]
    private void RPC_CreateGameManager()
    {
        // creates a Game Manager at the host
        PhotonNetwork.InstantiateRoomObject(System.IO.Path.Combine("GamePrefabs", "GameManager"), Vector3.zero, Quaternion.identity);
    }

    [PunRPC]
    private void RPC_CreateLevel()
    {
        // creates level network controller but not player character
        PhotonNetwork.InstantiateRoomObject(System.IO.Path.Combine("LevelPrefabs", "Level"), Vector3.zero, Quaternion.identity);
    }

    [PunRPC]
    private void RPC_CreateTasks()
    {
        PhotonNetwork.InstantiateRoomObject(System.IO.Path.Combine("GamePrefabs", "TaskManager"), Vector3.zero, Quaternion.identity);
    }

    [PunRPC]
    private void RPC_CreatePlayer()
    {
        // creates player network controller but not player character
        if (playerSettings.roleList.ElementAt(myNumberInRoom - 1) == PlayerStgs.PlayerRole.Creature)
        {
            GameObject creature = PhotonNetwork.Instantiate(System.IO.Path.Combine("PlayerPrefabs", "Creature"), creatureSpawnPoint.position, Quaternion.identity);
            creature.GetComponent<Creature>().PlayerNumber = myNumberInRoom;
        }
        else
        {
            GameObject researcher = PhotonNetwork.Instantiate(System.IO.Path.Combine("PlayerPrefabs", "Researcher"), spawnPoints.GetPosition(myNumberInRoom), Quaternion.identity);
            researcher.GetComponent<Researcher>().PlayerNumber = myNumberInRoom;
        }
        // TODO: Properly set player's numbers for everybody
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
        Debug.Log(otherPlayer.NickName + " has left the game");
        playersInRoom--;

        ClearPlayerListing();
        ListPlayers();
    }
    #endregion
}