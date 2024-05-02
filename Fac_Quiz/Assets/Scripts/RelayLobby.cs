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

public class RelayLobby : MonoBehaviour
{
    private List<Lobby> lobbyList;
    [SerializeField] GameObject lobbyButton, contentView;

    void Start()
    {
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

        string lobbyName = "new Lobby";
        int maxPlayer = 2;

        CreateLobbyOptions options = new CreateLobbyOptions();
        options.IsPrivate = false;
        options.Data = new Dictionary<string, DataObject>() {
            { 
                "relayJoinCode", new DataObject(
                        visibility: DataObject.VisibilityOptions.Public,
                        value: joincode
                    )
            }
        };

        Lobby lobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, maxPlayer, options);
        Debug.Log(lobby.LobbyCode);

        NetworkManager.Singleton.StartHost();
        gameObject.SetActive(false);
    }

    public async void joinLobby(string joinCode, string lobbyId) {
        JoinAllocation allocation = await Unity.Services.Relay.Relay.Instance.JoinAllocationAsync(joinCode);
        NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(new RelayServerData(allocation, "dtls"));

        try {
            await LobbyService.Instance.JoinLobbyByIdAsync(lobbyId);
            NetworkManager.Singleton.StartClient();
            gameObject.SetActive(false);
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

            for (int i = 0; i < lobbyList.Count; i++) {
                tempNum = i;
                GameObject newBtn = Instantiate(lobbyButton);
                newBtn.transform.SetParent(contentView.transform);
                newBtn.GetComponent<Button>().onClick.AddListener(() => {
                    Debug.Log(lobbyList[tempNum].Id);
                    joinLobby(lobbyList[tempNum].Data["relayJoinCode"].Value, lobbyList[tempNum].Id);
                });
            }

        }
        catch (LobbyServiceException e) {
            Debug.Log(e);
        }
    }
}
