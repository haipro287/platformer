using Photon.Pun;
using TMPro;
using UnityEngine;

namespace PhotonUtil
{
    public class PhotonLogin:MonoBehaviourPunCallbacks
    {
        public TMP_InputField inputUsername;
        [SerializeField] private string nickName;

        void Start()
        {
            nickName = "Sai";
            inputUsername.text = nickName;
        }

        public virtual void OnChangeName()
        {
            nickName = inputUsername.text;
        }

        public virtual void Login()
        {
            string name = nickName;
            Debug.Log(transform.name + ": Login " + name);

            //PhotonNetwork.SendRate = 20;
            //PhotonNetwork.SerializationRate = 5;

            PhotonNetwork.AutomaticallySyncScene = true;
            PhotonNetwork.LocalPlayer.NickName = name;
            PhotonNetwork.ConnectUsingSettings();
        }

        public override void OnConnectedToMaster()
        {
            Debug.Log(transform.name + ": OnConnectedToMaster");
            PhotonNetwork.JoinLobby();
        }

        public override void OnJoinedLobby()
        {
            Debug.Log("OnJoinedLobby");
        }
    }
}