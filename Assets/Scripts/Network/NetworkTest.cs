using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class NetworkTest : MonoBehaviour
{
    private string status = "Network Offline";

    private void Start()
    {
        NetworkManager networkManager = NetworkManager.Singleton;

        networkManager.OnClientConnectedCallback += OnClientConnected;
        networkManager.OnClientDisconnectCallback += OnClientDisconnected;
    }

    private void OnDestroy()
    {
        if (NetworkManager.Singleton == null)
            return;

        NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
        NetworkManager.Singleton.OnClientDisconnectCallback -= OnClientDisconnected;
    }

    private void Update()
    {
        NetworkManager networkManager = NetworkManager.Singleton;

        if (Input.GetKeyDown(KeyCode.G))
        {
            if (!networkManager.IsListening)
            {
                networkManager.StartHost();
            }
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            if (!networkManager.IsListening)
            {
                networkManager.StartClient();
            }
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            if (networkManager.IsListening)
            {
                networkManager.Shutdown();
                status = "Network Offline";
            }
        }
    }

    private void OnClientConnected(ulong clientId)
    {
        status = $"Connected - ClientId: {clientId}";
        Debug.Log($"Client connected: {clientId}");
    }

    private void OnClientDisconnected(ulong clientId)
    {
        status = $"Disconnected - ClientId: {clientId}";
        Debug.Log($"Client disconnected: {clientId}");
    }

    private void OnGUI()
    {
        GUIStyle style = new GUIStyle(GUI.skin.label);
        style.fontSize = 24;

        NetworkManager networkManager = NetworkManager.Singleton;

        string role = "Offline";

        if (networkManager.IsHost)
        {
            role = "HOST";
        }
        else if (networkManager.IsServer)
        {
            role = "SERVER";
        }
        else if (networkManager.IsClient)
        {
            role = "CLIENT";
        }

        GUI.Label(
            new Rect(20, 20, 500, 40),
            $"Role: {role}",
            style
        );

        GUI.Label(
            new Rect(20, 60, 700, 40),
            $"Status: {status}",
            style
        );

        GUI.Label(
            new Rect(20, 100, 700, 40),
            "G = Host    H = Client    J = Shutdown",
            style
        );
    }
}
