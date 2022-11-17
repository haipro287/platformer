using UnityEngine;
using Photon.Pun;
using TMPro;

public class PhotonStatus : MonoBehaviour
{
    [field: SerializeField] public string Status { get; private set; }
    public TextMeshProUGUI textStatus;

    // Update is called once per frame
    void Update()
    {
        this.Status = PhotonNetwork.NetworkClientState.ToString();
        this.textStatus.text = this.Status;
    }
}
