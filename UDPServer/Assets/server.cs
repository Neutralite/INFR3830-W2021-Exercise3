using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Lecture 4
using System;
using System.Text;
using System.Net;
using System.Net.Sockets;


public class server : MonoBehaviour
{

    public GameObject myCube;

    private static byte[] outBuffer = new byte[512];
    private static Socket client_socket;

    private static byte[] buffer = new byte[512];
    private static IPHostEntry host;
    private static IPAddress ip;
    private static IPEndPoint localEP;
    private static Socket server_socket;
    private static IPEndPoint remoteEP;
    private static EndPoint remoteClient;

    //public static void RunClient()
    //{
    //    IPAddress ip = IPAddress.Parse("192.168.68.107");
    //    remoteEP = new IPEndPoint(ip, 11111);

    //    client_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

    //    //try

    //    outBuffer = Encoding.ASCII.GetBytes("Testing.......... INFR3830");
    //    client_socket.SendTo(outBuffer, remoteEP);

    //    //client_socket.Shutdown(SocketShutdown.Both);
    //    //client_socket.Close();
    //}

    public static void RunServer()
    {

        host = Dns.GetHostEntry(Dns.GetHostName());
        ip = host.AddressList[1];
        //ip = IPAddress.Parse("192.168.68.107"); 

        Debug.Log("Server name: " + host.HostName + "  IP: " + ip);

        localEP = new IPEndPoint(ip, 11111);

        server_socket = new Socket(ip.AddressFamily, SocketType.Dgram, ProtocolType.Udp);

        remoteEP = new IPEndPoint(IPAddress.Any, 0); // 0 represents any available port
        remoteClient = (EndPoint)remoteEP;

        server_socket.Bind(localEP);

        Debug.Log("Waiting for data...");
    }

    // Start is called before the first frame update
    void Start()
    {
        myCube = GameObject.Find("Cube");

        RunServer();
    }

    // Update is called once per frame
    void Update()
    {
        try
        {
            int rec = server_socket.ReceiveFrom(buffer, ref remoteClient);

            //Debug.Log("Received x: " + Encoding.ASCII.GetString(buffer, 0, rec) + "  from Client: " + remoteClient.ToString());

            //rec = server_socket.ReceiveFrom(buffer, ref remoteClient);

            //Debug.Log("Received y: " + Encoding.ASCII.GetString(buffer, 0, rec) + "  from Client: " + remoteClient.ToString());

            Debug.Log("Received: X: " + BitConverter.ToSingle(buffer, 0 * sizeof(float)) + 
                              "  Z: " + BitConverter.ToSingle(buffer, 1 * sizeof(float)) + 
                              "  from Client: " + remoteClient.ToString());

            myCube.transform.position = new Vector3(BitConverter.ToSingle(buffer, 0), myCube.transform.position.y, BitConverter.ToSingle(buffer, 1 * sizeof(float)));

        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }
    }







}
