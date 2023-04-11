using Photon.Pun;

using TMPro;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Poton_CreateOrJoin : MonoBehaviourPunCallbacks
{
    [SerializeField] TMP_InputField _roomNameTMP;
    [SerializeField] GameObject _roomLoading;
    public InputField RoomNameInputField;
    #region Methods
    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }
    public void OnCreateRoomButtonClicked()
    {
        string roomName = RoomNameInputField.text;

        PhotonNetwork.CreateRoom(roomName);
    }

    public void OnJoinRandomRoomButtonClicked()
    {
        PhotonNetwork.JoinRandomRoom();
    }
    public override void OnConnectedToMaster()
    {
        Debug.Log("connected");
        PhotonNetwork.JoinLobby();
    }
    public override void OnJoinedLobby()
    {
        // whenever this joins a new lobby, clear any previous room lists
        _roomLoading.SetActive(true);
    }
    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("CoreGame");
       
    }
    public void starrtGame()
    {
        SceneManager.LoadScene(1);
    }

    #region Fails
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        _roomLoading.SetActive(false);
    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        _roomLoading.SetActive(false);
    }
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        _roomLoading.SetActive(false);
    }
    #endregion
    #endregion
}
