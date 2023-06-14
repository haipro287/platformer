using System;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using Cinemachine;
using Platformer;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using Player = Photon.Realtime.Player;

public class PhotonPlaying : MonoBehaviourPunCallbacks
{
    public static PhotonPlaying instance;
    public string photonPlayerName = "PhotonPlayer";
    public List<PlayerProfile> players = new List<PlayerProfile>();

    public static event Action<Transform> OnCreateMyPlayer;
    
    protected void Awake()
    {
        PhotonPlaying.instance = this;//Dont do this in your game
    }

    public override void OnEnable()
    {
        base.OnEnable();
        LoadRoomPlayers();
        SpawnPlayer();
    }
    
    protected virtual void SpawnPlayer()
    {
        Debug.Log("SpawnPlayer");
        if (PhotonNetwork.NetworkClientState != ClientState.Joined)
        {
            Invoke(nameof(SpawnPlayer), 1f);
            return;
        }
    
        var playerObj = PhotonNetwork.Instantiate("PhotonPlayer", Vector3.zero, Quaternion.identity);
        var photonView = playerObj.GetComponent<PhotonView>();
        if (photonView.IsMine)
        {
            var photonPlayer = playerObj.GetComponent<PhotonPlayer>();
            PhotonPlayer.Me = photonPlayer;

            var textObj = GameObject.Find("Cherry Number");
            var text = textObj.GetComponent<TextMeshProUGUI>();

            if (text)
            {
                photonPlayer.cherryText = text;
            }

            if (OnCreateMyPlayer != null) OnCreateMyPlayer(photonPlayer.transform);
        }
    }


    protected virtual void LoadRoomPlayers()
    {
        Debug.Log("LoadRoomPlayers");
        if (PhotonNetwork.NetworkClientState != ClientState.Joined)
        {
            Invoke("LoadRoomPlayers", 1f);
            return;
        }
    
        PlayerProfile playerProfile;
        foreach (KeyValuePair<int, Player> playerData in PhotonNetwork.CurrentRoom.Players)
        {
            //Debug.Log(playerData.Value.NickName);
            playerProfile = new PlayerProfile
            {
                nickName = playerData.Value.NickName
            };
            this.players.Add(playerProfile);
        }
    }

    public virtual void Leave()
    {
        Debug.Log(transform.name + ": Leave Room");
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        Debug.Log(transform.name + ": OnLeftRoom");
        PhotonNetwork.LoadLevel("1_PhotonRoom");
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("OnPlayerEnteredRoom: " + newPlayer.NickName);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log("OnPlayerLeftRoom: " + otherPlayer.NickName);
    }
}