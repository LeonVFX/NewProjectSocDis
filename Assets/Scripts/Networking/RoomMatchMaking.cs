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

    public GameObject lobbyGO;
    public GameObject roomGO;
    public Transform playersPanel;
    public GameObject playerListingPrefab;
    public GameObject startGamePrefab;
    private GameObject startGameButton;
    public float countdownGameStart;
    public GameObject countdownPrefab;

    private PlayerStgs playerSettings;
    [SerializeField] private SpawnPoints spawnPoints = null;

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
    }

    private void Update()
    {
        // Set Stage 1
        // If every player is loaded, proceed to the creation of items and go to Next Stage
        if (!PhotonNetwork.IsMasterClient)
            return;

        if (GameManager.gm)
        {
            if (PlayerManager.pm.playerLoaded == playersInRoom &&
                GameManager.gm.currentStage == GameManager.GameStage.Setup)
            phoView.RPC("RPC_CreateInteractions", RpcTarget.MasterClient);
        }
    }

    public override void OnJoinedRoom()
    {
        // sets player data when we join the room
        base.OnJoinedRoom();
        Debug.Log("We are now in a room");

        players = PhotonNetwork.PlayerList;
        playersInRoom = players.Length;
        myNumberInRoom = playersInRoom;

        if (PhotonNetwork.IsMasterClient)
            PhotonNetwork.InstantiateRoomObject(System.IO.Path.Combine("GamePrefabs", "GameManager"), Vector3.zero, Quaternion.identity);

        PhotonNetwork.LoadLevel(MultiplayerSettings.multiplayerSettings.waitingRoom);

        IEnumerator loadPlayerContent = LoadPlayerContents();
        StartCoroutine(loadPlayerContent);
    }

    private IEnumerator LoadPlayerContents()
    {
        while (currentScene != MultiplayerSettings.multiplayerSettings.waitingRoom)
            yield return null;

        while (!GameManager.gm)
            yield return null;

        if (PhotonNetwork.IsMasterClient)
        {
            startGameButton = Instantiate(startGamePrefab);
            startGameButton.transform.SetParent(GameObject.Find("Canvas").transform, false);
            startGameButton.GetComponent<Button>().onClick.AddListener(StartGameOnClick);
        }

        GameObject playerActor = PhotonNetwork.Instantiate(System.IO.Path.Combine("PlayerPrefabs", "Player"), Vector3.zero, Quaternion.identity);
        yield return null;
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        // updates player data when a new player joins
        base.OnPlayerEnteredRoom(newPlayer);
        Debug.Log("A new player has joined the room");

        players = PhotonNetwork.PlayerList;
        playersInRoom++;
    }

    public void StartGameOnClick()
    {
        // loads the multiplayer scene for all players
        isGameLoaded = true;
        if (!PhotonNetwork.IsMasterClient)
            return;
        if (MultiplayerSettings.multiplayerSettings.delayStart)
            PhotonNetwork.CurrentRoom.IsOpen = false;

        startGameButton.GetComponent<Button>().interactable = false;

        // Randomize Roles
        int isCreature = Random.Range(1, playersInRoom);
        phoView.RPC("RPC_RandomizeRole", RpcTarget.All, isCreature);
        IEnumerator startGame = StartGame();
        StartCoroutine(startGame);
    }

    private IEnumerator StartGame()
    {
        yield return new WaitForSeconds(countdownGameStart);

        PhotonNetwork.LoadLevel(MultiplayerSettings.multiplayerSettings.loadingScene);
        // Randomize player settings
        playerSettings.RandomizePlayers(playersInRoom);

        yield return null;
    }

    #region Game Scene Creation
    private void OnSceneFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        // called when multiplayer scene is loaded
        currentScene = scene.buildIndex;
        if (currentScene == MultiplayerSettings.multiplayerSettings.multiplayerScene)
        {
            isGameLoaded = true;

            if (!PhotonNetwork.IsMasterClient)
                return;

            StartCoroutine(CreateInteractions());
        }
    }

    private IEnumerator CreateInteractions()
    {
        while (GameManager.gm.currentStage != GameManager.GameStage.Stage1)
            yield return null;
        
        phoView.RPC("RPC_CreateInteractions", RpcTarget.MasterClient);
        yield return null;
    }

    [PunRPC]
    private void RPC_CreateInteractions()
    {
        PhotonNetwork.InstantiateRoomObject(System.IO.Path.Combine("LevelPrefabs", "Items"), Vector3.zero, Quaternion.Euler(0f, 225f, 0f));
        PhotonNetwork.InstantiateRoomObject(System.IO.Path.Combine("LevelPrefabs", "EscapePods"), Vector3.zero, Quaternion.Euler(0f, 225f, 0f));
        PhotonNetwork.InstantiateRoomObject(System.IO.Path.Combine("LevelPrefabs", "VoteButton"), Vector3.zero, Quaternion.Euler(0f, 225f, 0f));
        ItemManager.im.SetupItemsToPlayers();
    }

    [PunRPC]
    private void RPC_RandomizeRole(int isCreature)
    {
        // Initiate Timer
        GameObject countdownTimer = Instantiate(countdownPrefab);
        countdownTimer.transform.SetParent(GameObject.Find("Canvas").transform, false);
        IEnumerator countdown = Countdown(countdownTimer);
        StartCoroutine(countdown);

        // Check if creature
        PlayerManager.pm.PickRole(isCreature, myNumberInRoom);
    }

    private IEnumerator Countdown(GameObject cd)
    {
        Text cdText = cd.GetComponent<Text>();

        int totalTime;
        int.TryParse(cdText.text, out totalTime);
        float elapsedTime = (float)totalTime;

        while (totalTime > 0)
        {
            elapsedTime -= Time.deltaTime;
            totalTime = (int)elapsedTime;
            cdText.text = totalTime.ToString();

            yield return null;
        }
        yield return null;
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
        Debug.Log(otherPlayer.NickName + " has left the game");
        playersInRoom--;
    }
    #endregion
}