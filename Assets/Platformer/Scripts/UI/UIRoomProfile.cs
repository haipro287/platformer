using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIRoomProfile : MonoBehaviour
{
    [SerializeField] protected TextMeshProUGUI roomName;
    [SerializeField] protected RoomProfile roomProfile;

    public virtual void SetRoomProfile(RoomProfile roomProfile)
    {
        this.roomProfile = roomProfile;
        Debug.Log(roomName.text);
        Debug.Log(roomProfile.name);
        this.roomName.text = this.roomProfile.name;
    }

    public virtual void OnClick()
    {
        Debug.Log("OnClick: " + this.roomProfile.name);
        PhotonRoom.instance.input.text = this.roomProfile.name;
    }
}