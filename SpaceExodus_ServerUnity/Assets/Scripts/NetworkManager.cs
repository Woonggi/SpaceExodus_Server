using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NetworkManager : MonoBehaviour
{
    public static NetworkManager instance;
    public GameObject playerPrefab;
    public GameObject[] asteroidPrefabs;
    public Text HostName;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.Log("Instance already exists, destroying object!");
            Destroy(this);
        }
    }

    private void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 30;
        HostName.text = "Host Name : " + Dns.GetHostName();
        // Server.Start(50, 26950);
    }

    private void OnApplicationQuit()
    {
        Server.Stop(); 
    }

    public Player InstantiatePlayer(int id)
    {
        Player p = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity).GetComponent<Player>();
        return p;
    }

    public void GameOver()
    {
        Server.Stop();
        Application.Quit();
    }
}
