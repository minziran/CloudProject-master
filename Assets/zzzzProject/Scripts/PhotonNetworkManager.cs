using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhotonNetworkManager : MonoBehaviour {    

    public GameObject lobbyCamera;
    public GameObject light;
    public GameObject player;

    [SerializeField] public Transform[] spawnPoint;

	public InputField new_room;
	public InputField nameField;



    private const string GAME_VERSION = "0.1";
	private bool isConnecting;

    void Awake () {
        Debug.Log ("Connect to network");
        PhotonNetwork.autoJoinLobby = false;
		PhotonNetwork.automaticallySyncScene = true;
        //PhotonNetwork.ConnectUsingSettings (GAME_VERSION);
    }

	void Start(){
		Connect();
		SetPlayerName();
	}

	public void Connect(){
	
		isConnecting = true;
		if (PhotonNetwork.connected) {

			Debug.Log ("Connected");

		} else {
			PhotonNetwork.ConnectUsingSettings (GAME_VERSION);
		
		}
	}


	public void CreateARoom(){
		if (PhotonNetwork.connected) {
			if (PhotonNetwork.CreateRoom (new_room.text, new RoomOptions{ MaxPlayers = 4 }, null)) {
				Debug.Log ("Already create a room");
				PhotonNetwork.LoadLevel ("ROOM");
			
			}
		}
	}



	public void SetPlayerName(){
		if (string.IsNullOrEmpty (nameField.text)) {
			if (PlayerPrefs.HasKey ("PlayerName")) {
				
				nameField.text = PlayerPrefs.GetString ("PkayerName");
			}
		}
		PhotonNetwork.playerName = nameField.text;
		PlayerPrefs.SetString ("PlayerName", nameField.text);
	
	}

	public void GetStart(){
	
		PhotonNetwork.LoadLevel("Main");
	
	}

    public virtual void OnConnectedToMaster()
    {
        Debug.Log("Connected to master");
        //PhotonNetwork.JoinOrCreateRoom("Room0", null, null);
    }

	public virtual void OnCreatedRoom(){
        Debug.Log("Created room");
		lobbyCamera.SetActive (false);
		PhotonNetwork.JoinOrCreateRoom (new_room.text, null, TypedLobby.Default);

    }


	public virtual void OnJoinedRoom() {
        Debug.Log ("Joined room");
		PhotonNetwork.automaticallySyncScene = true;
        lobbyCamera.SetActive (false);
		light.SetActive (false);
        PhotonNetwork.Instantiate (player.name, spawnPoint[0].position, spawnPoint[0].rotation, 0);
    }


    public virtual void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
    {
        Debug.Log("Player connected");
    }

    public virtual void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)
    {
        Debug.Log("Player disconnected");
    }


    public virtual void OnConnectionFail(DisconnectCause cause)
    {
        Debug.Log("Connection failed to the Photon network");
    }
//
    public virtual void OnDisconnectedFromPhoton()
    {
        Debug.Log("We got disconnected form the Photon network");
    }
}

