using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ConnectToServer : MonoBehaviourPunCallbacks
{
    [SerializeField] private InputField _nickName;
    [SerializeField] private Text _connectButtonText;
    bool fullscreen;
    void Start()
    {
        Resolution[] resolutions = Screen.resolutions;
        fullscreen = !fullscreen;
        if (fullscreen)
            Screen.SetResolution(resolutions[resolutions.Length - 1].width, resolutions[resolutions.Length - 1].height, fullscreen);
        else
            Screen.SetResolution(800, 600, fullscreen);
    }
    public void ConnectUserToServer()
    {
        if (_nickName.text.Length > 0)
        {
            Time.timeScale = 1;
            PhotonNetwork.NickName = _nickName.text;
            _connectButtonText.text = "Connecting...";
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        SceneManager.LoadScene("Lobby");
    }
}
