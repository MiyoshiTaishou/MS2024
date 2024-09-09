using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class Server : MonoBehaviour
{
    private TcpListener server;
    private TcpClient client;

    void Start()
    {
        StartServer();
    }

    void StartServer()
    {
        // 任意のポート番号を指定 (例: 7777)
        int port = 7777;
        server = new TcpListener(IPAddress.Any, port);
        server.Start();
        Debug.Log("サーバーが起動しました。");

        server.BeginAcceptTcpClient(new AsyncCallback(OnClientConnected), null);
    }

    void OnClientConnected(IAsyncResult result)
    {
        client = server.EndAcceptTcpClient(result);
        Debug.Log("クライアントが接続しました。");
        // ここでメッセージの送受信処理を行う
        byte[] buffer = new byte[1024];
        client.GetStream().BeginRead(buffer, 0, buffer.Length, new AsyncCallback(OnDataReceived), buffer);
    }

    void OnDataReceived(IAsyncResult result)
    {
        byte[] buffer = (byte[])result.AsyncState;
        string message = Encoding.UTF8.GetString(buffer);
        Debug.Log("受信したメッセージ: " + message);
    }

    void OnApplicationQuit()
    {
        client.Close();
        server.Stop();
    }
}
