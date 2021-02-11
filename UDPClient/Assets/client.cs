using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Lecture 4
using System;
using System.Text;
using System.Net;
using System.Net.Sockets;


public class client : MonoBehaviour
{

    public GameObject myCube;

    private static byte[] outBuffer = new byte[512];
    private static IPEndPoint remoteEP;
    private static Socket client_socket;
    
    public static void RunClient()
    {
        IPAddress ip = IPAddress.Parse("192.168.68.107");
        remoteEP = new IPEndPoint(ip, 11111);

        client_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

        //try

        outBuffer = Encoding.ASCII.GetBytes("Testing.......... INFR3830");
        client_socket.SendTo(outBuffer, remoteEP);

        //client_socket.Shutdown(SocketShutdown.Both);
        //client_socket.Close();
    }

    // Start is called before the first frame update
    void Start()
    {
        myCube = GameObject.Find("Cube");
        
        RunClient();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Cube X: " + myCube.transform.position.x + "  Cube Z: " + myCube.transform.position.z);

        //outBuffer = Encoding.ASCII.GetBytes(myCube.transform.position.x.ToString());

        //outBuffer = BitConverter.GetBytes(myCube.transform.position.x);


        //outBuffer = Encoding.ASCII.GetBytes(myCube.transform.position.z.ToString());
        //client_socket.SendTo(outBuffer, remoteEP);

        // From https://answers.unity.com/questions/683693/converting-vector3-to-byte.html,
        // there's this nifty Buffer.BlockCopy trick.

        Buffer.BlockCopy(BitConverter.GetBytes(myCube.transform.position.x), 0, outBuffer, 0 * sizeof(float), sizeof(float));
        Buffer.BlockCopy(BitConverter.GetBytes(myCube.transform.position.z), 0, outBuffer, 1 * sizeof(float), sizeof(float));

        client_socket.SendTo(outBuffer, remoteEP);

    }
}
