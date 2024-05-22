using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Services.Relay;
using Unity.Services.Lobbies;
using Unity.Services.Relay.Models;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Lobbies.Models;
using UnityEngine.UI;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using WebSocketSharp;
using TMPro;
using Newtonsoft.Json.Linq;

public class RelayLobby : MonoBehaviour
{
    private List<Lobby> lobbyList;
    [SerializeField] GameObject lobbyButton, contentView, LobbyCreationPanel, InLobbyPenel, LobbyListPenel, gameStartButton, lobbyPWUI, clientLobbyPWUI;
    [SerializeField] Toggle lobbyToggle;
    [SerializeField] TMP_InputField lobbyNameInput, passwordInput, clientPasswordInput;
    private string myLobbyId;
    private bool isReady;
    private Lobby lobbyTryToEnter;

    void Start()
    {
        lobbyPWUI.SetActive(false);
        clientLobbyPWUI.SetActive(false);
        lobbyToggle.onValueChanged.AddListener((bool isPrivate) => {
            if (isPrivate)
            {
                lobbyPWUI.SetActive(true);
            }
            else {
                lobbyPWUI.SetActive(false);
            }
        });
        init();
    }

    private async void init() { 
        await UnityServices.InitializeAsync();
        AuthenticationService.Instance.ClearSessionToken();
        await AuthenticationService.Instance.SignInAnonymouslyAsync();

        Debug.Log(AuthenticationService.Instance.PlayerId);
    }


    public async void relayAllocation() { 
        Allocation allocatoin = await Unity.Services.Relay.RelayService.Instance.CreateAllocationAsync(2);
        string joinCode = await Unity.Services.Relay.RelayService.Instance.GetJoinCodeAsync(allocatoin.AllocationId);
        NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(new RelayServerData(allocatoin, "dtls"));

        createLobby(joinCode);
    }

    private async void createLobby(string joincode) {

        string lobbyName = lobbyNameInput.text;
        int maxPlayer = 2;

        CreateLobbyOptions options = new CreateLobbyOptions();

        if (lobbyToggle.isOn)
        {
            options.IsPrivate = true;
            options.Password = passwordInput.text; //8~64자여야함
        }
        else {
            options.IsPrivate = false;
        }

        options.Data = new Dictionary<string, DataObject>() {
            { 
                "relayJoinCode", new DataObject(
                        visibility: DataObject.VisibilityOptions.Public,
                        value: joincode
                    )
            }
        };

        Lobby lobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, maxPlayer, options);
        myLobbyId = lobby.Id;
        isReady = false;
        StartCoroutine(HeartbeatLobbyCoroutine(lobby.Id, 15));
        Debug.Log(lobby.LobbyCode);

        gameStartButton.GetComponent<Button>().interactable = false;
        NetworkManager.Singleton.StartHost();
        LobbyCreationPanel.SetActive(false);
        InLobbyPenel.SetActive(true);
        lobbyNameInput.text = "";
        passwordInput.text = "";
        lobbyToggle.isOn = false;
    }

    public async void joinLobby(string joinCode, Lobby lobby) {
        JoinAllocation allocation = await Unity.Services.Relay.Relay.Instance.JoinAllocationAsync(joinCode);
        NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(new RelayServerData(allocation, "dtls"));

        try {

            if (lobby.IsPrivate)
            {
                var idOptions = new JoinLobbyByIdOptions { Password = clientPasswordInput.text };
                await LobbyService.Instance.JoinLobbyByIdAsync(lobby.Id, idOptions);
            }
            else {
                await LobbyService.Instance.JoinLobbyByIdAsync(lobby.Id);
            }
            myLobbyId = lobby.Id;
            gameStartButton.GetComponent<Button>().interactable = true;
            gameStartButton.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = "Ready";
            NetworkManager.Singleton.StartClient();
            LobbyCreationPanel.SetActive(false);
            InLobbyPenel.SetActive(true);
        }
        catch (LobbyServiceException e) {
            Debug.Log(e);
        }
    }

    public async void refreshLobbyList()
    {
        try
        {
            QueryLobbiesOptions options = new QueryLobbiesOptions();
            options.Count = 25;

            options.Filters = new List<QueryFilter>() {
            new QueryFilter(
                field: QueryFilter.FieldOptions.AvailableSlots,
                op: QueryFilter.OpOptions.GT,
                value: "0"
                )
        };

            options.Order = new List<QueryOrder>() {
            new QueryOrder(
                asc: false,
                field: QueryOrder.FieldOptions.Created
                )
        };

            QueryResponse lobbies = await Lobbies.Instance.QueryLobbiesAsync(options);
            lobbyList = lobbies.Results;

            int tempNum = 0;

            if (lobbyList.Count == 0) Debug.Log("존재하는 로비가 없습니다.");

            for (int i = 0; i < lobbyList.Count; i++) {
                tempNum = i;
                GameObject newBtn = Instantiate(lobbyButton);
                newBtn.transform.SetParent(contentView.transform);
                newBtn.GetComponent<Button>().onClick.AddListener(() => {
                    Debug.Log(lobbyList[tempNum].Id);
                    if (lobbyList[tempNum].IsPrivate)
                    {
                        clientLobbyPWUI.SetActive(true);
                        lobbyTryToEnter = lobbyList[tempNum];
                    }
                    else {
                        joinLobby(lobbyList[tempNum].Data["relayJoinCode"].Value, lobbyList[tempNum]);
                    }
                });
            }

        }
        catch (LobbyServiceException e) {
            Debug.Log(e);
        }
    }

    public void checkPassword() {
        joinLobby(lobbyTryToEnter.Data["relayJoinCode"].Value, lobbyTryToEnter);
        clientPasswordInput.text = "";
        clientLobbyPWUI.SetActive(false);
        lobbyTryToEnter = null;
    }

    public async void leaveLobby()
    {
        try
        {
            if (NetworkManager.Singleton.IsHost)
            {
                string playerId = AuthenticationService.Instance.PlayerId;
                await LobbyService.Instance.RemovePlayerAsync(myLobbyId, playerId);
                //await LobbyService.Instance.DeleteLobbyAsync(myLobbyId);
            }
            else {
                //var lobbyId = await LobbyService.Instance.GetJoinedLobbiesAsync();
                string playerId = AuthenticationService.Instance.PlayerId;
                await LobbyService.Instance.RemovePlayerAsync(myLobbyId, playerId);
            }
            InLobbyPenel.SetActive(false);
            LobbyListPenel.SetActive(true);
            myLobbyId = "";
        }
        catch (LobbyServiceException e) {
            Debug.Log(e);
        }
    }

    IEnumerator HeartbeatLobbyCoroutine(string lobbyId, float waitTimeSeconds) {

        var delay = new WaitForSeconds(waitTimeSeconds);

        while (true) {
            LobbyService.Instance.SendHeartbeatPingAsync(lobbyId);
            yield return delay;
        }
    }

    //ConcurrentQueue<string> createdLobbyIds = new ConcurrentQueue<string>();

    private void OnApplicationQuit()
    {
        if (!myLobbyId.IsNullOrEmpty() /*&& NetworkManager.Singleton.IsHost*/)
        {
            Debug.Log("로비 삭제 중");
            LobbyService.Instance.DeleteLobbyAsync(myLobbyId);
            myLobbyId = "";
        }
    }

    public void closePanel() { 
        clientLobbyPWUI.SetActive(false);
        lobbyTryToEnter = null;
    }
}
