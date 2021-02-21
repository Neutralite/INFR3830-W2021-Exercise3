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
    private static IPAddress ip;
    private static IPEndPoint remoteEP;
    private static Socket client_socket;
    private static float[] pos = new float[3];
    private static float elapsedTime;
    private static float delayTime = 1f;


    public static void RunClient()
    {
        //ip = IPAddress.Parse("192.168.68.107");
        ip = IPAddress.Parse("127.0.0.1"); 

        remoteEP = new IPEndPoint(ip, 11111);

        client_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

        //try

        //outBuffer = Encoding.ASCII.GetBytes("Testing.......... INFR3830");
        //client_socket.SendTo(outBuffer, remoteEP);

        //client_socket.Shutdown(SocketShutdown.Both);
        //client_socket.Close();
    }

    // Start is called before the first frame update
    void Start()
    {
        myCube = GameObject.Find("Cube");
        
        RunClient();

        // From https://answers.unity.com/questions/17131/execute-code-every-x-seconds-with-update.html
        // InvokeRepeating's also nifty function
        //InvokeRepeating("UpdatePosition", 0.0f, 1.0f);
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("Cube X: " + myCube.transform.position.x + "  Cube Y: " + myCube.transform.position.y + "  Cube Z: " + myCube.transform.position.z);

        //outBuffer = Encoding.ASCII.GetBytes(myCube.transform.position.x.ToString());

        //outBuffer = BitConverter.GetBytes(myCube.transform.position.x);


        //outBuffer = Encoding.ASCII.GetBytes(myCube.transform.position.z.ToString());
        //client_socket.SendTo(outBuffer, remoteEP);

        elapsedTime += Time.deltaTime;
        if (elapsedTime >= delayTime)
        {
            UpdatePosition();
            elapsedTime = 0;
        }

    }

    void UpdatePosition()
    {
        if (pos[0] != myCube.transform.position.x || pos[1] != myCube.transform.position.y || pos[2] != myCube.transform.position.z)
        {
            pos[0] = myCube.transform.position.x;
            pos[1] = myCube.transform.position.y;
            pos[2] = myCube.transform.position.z;
            byte[] bpos = new byte[pos.Length * 4];

            // From https://answers.unity.com/questions/683693/converting-vector3-to-byte.html,
            // there's this nifty Buffer.BlockCopy trick.

            //Buffer.BlockCopy(BitConverter.GetBytes(myCube.transform.position.x), 0, outBuffer, 0 * sizeof(float), sizeof(float));
            //Buffer.BlockCopy(BitConverter.GetBytes(myCube.transform.position.y), 0, outBuffer, 1 * sizeof(float), sizeof(float));
            //Buffer.BlockCopy(BitConverter.GetBytes(myCube.transform.position.z), 0, outBuffer, 2 * sizeof(float), sizeof(float));
            
            Buffer.BlockCopy(pos, 0, bpos, 0, bpos.Length);

            client_socket.SendTo(bpos, remoteEP);

            Debug.Log("Sent Coordinates!");
        }
    }
}
