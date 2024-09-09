using System.Net;
using UnityEngine;

public class ShowLocalIPAddress : MonoBehaviour
{
    private void Start()
    {
        // ローカルホストのIPアドレスを取得
        string localIP = GetLocalIPAddress();
        Debug.Log("ローカルIPアドレス: " + localIP);
    }

    // ローカルIPアドレスを取得するメソッド
    private string GetLocalIPAddress()
    {
        string localIP = "";
        foreach (var ip in Dns.GetHostAddresses(Dns.GetHostName()))
        {
            if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork) // IPv4アドレスのみを取得
            {
                localIP = ip.ToString();
                break;
            }
        }
        return localIP;
    }
}
