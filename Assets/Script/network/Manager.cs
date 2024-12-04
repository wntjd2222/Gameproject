using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System;
using System.Threading;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Text;

public class Manager
{
    public static TcpClient client;
    public static NetworkStream stream;
    public static int my_socket = 0;
    public static int byte_spend;
    public enum ConnectionState
    {
        ConnectTry, ConnectSuccess, Disconnect
    }
    static public ConnectionState connection_state = ConnectionState.Disconnect;
    static public string connection_state_string = "click";

    public static Thread recv_thread;
    public static Thread send_thread;
    public static SafeQueue<Packet> packet_recv_queue = new SafeQueue<Packet>();
    public static SafeQueue<Packet> packet_send_queue = new SafeQueue<Packet>();

    private static string server_ip;
    private static int server_port;

    public static Mutex mutex = new Mutex();

    public static int SigninSuccess = 0;

    static public void ConnectServer(string ip, int port)
    {
        if (connection_state == ConnectionState.ConnectSuccess)
        {
            
            UnityEngine.Debug.Log("연결 준비 안됨");
            return;
        }

        if (connection_state == ConnectionState.ConnectTry)
            return;

        connection_state = ConnectionState.ConnectTry;
        connection_state_string = "연결 시도중";

        server_ip = ip;
        server_port = port;

        Thread accept_thread = new Thread(new ThreadStart(ConnectThread));
        accept_thread.Start();
    }

    static void ConnectThread()
    {
        try
        {
            IPEndPoint client_address = new IPEndPoint(0, 0);
            IPEndPoint server_address = new IPEndPoint(IPAddress.Parse(server_ip), server_port);

            client = new TcpClient(client_address);
            client.Connect(server_address);
            stream = client.GetStream();
            client.NoDelay = true;
            client.Client.NoDelay = true;

            connection_state_string = "connection success";
            connection_state = ConnectionState.ConnectSuccess;

            recv_thread = new Thread(new ThreadStart(RecvThread));
            recv_thread.Start();

            send_thread = new Thread(new ThreadStart(SendThread));
            send_thread.Start();
        }
        catch (Exception ex)
        {
            connection_state = ConnectionState.Disconnect;
            connection_state_string = ex.ToString();
            UnityEngine.Debug.Log(ex);
        }

        return;
    }

    static void SendThread()
    {
        while (true)
        {
            if (connection_state == Manager.ConnectionState.Disconnect)
                return;

            int count = packet_send_queue.Count;
            if (count != 0)
            {
                for (int i = 0; i < count; i++)
                {
                    Packet packet = packet_send_queue.Dequeue();
                    Header header = new Header
                    {
                        number = packet.number,
                        size = packet.data != null ? packet.data.Length : 0
                    };

                    byte[] headerBytes = StructureToByte(header);
                    byte[] resultBytes;

                    if (packet.data != null)
                    {
                        resultBytes = new byte[headerBytes.Length + packet.data.Length];
                        Array.Copy(headerBytes, 0, resultBytes, 0, headerBytes.Length);
                        Array.Copy(packet.data, 0, resultBytes, headerBytes.Length, packet.data.Length);
                    }
                    else
                    {
                        resultBytes = headerBytes;
                    }

                    stream.Write(resultBytes, 0, resultBytes.Length);
                    stream.Flush();
                    byte_spend += resultBytes.Length;
                }
            }
            Thread.Sleep(1);
        }
    }



    static void RecvThread()
    {
        while (true)
        {
            if (connection_state == Manager.ConnectionState.Disconnect)
                return;

            try
            {
                byte[] lengthBytes = new byte[sizeof(int)];
                int bytesRead = stream.Read(lengthBytes, 0, sizeof(int));
                if (bytesRead == 0)
                    continue;

                int size = BitConverter.ToInt32(lengthBytes, 0);

                byte[] numberBytes = new byte[sizeof(int)];
                stream.Read(numberBytes, 0, sizeof(int));
                int number = BitConverter.ToInt32(numberBytes, 0);

                Packet packet = new Packet();
                packet.number = number;

                if (size > 0)
                {
                    packet.data = new byte[size];
                    stream.Read(packet.data, 0, size);
                }

                byte_spend += sizeof(int) * 2 + size;

                packet_recv_queue.Enqueue(packet);

                ProcessReceivePacket(packet);
            }
            catch (ObjectDisposedException ex)
            {
                UnityEngine.Debug.LogError(ex);
                connection_state = Manager.ConnectionState.Disconnect;
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.LogError("RecvThread Error: " + ex);
                connection_state = Manager.ConnectionState.Disconnect;
            }
        }
    }

    static void ProcessReceivePacket(Packet packet)
    {
        switch (packet.number)
        {
            case PacketNumber.CHAT:
                string receivedMessage = Encoding.UTF8.GetString(packet.data);
                UnityEngine.Debug.Log("Received Chat: " + receivedMessage);
                ClientManager.instance.ClientChat(receivedMessage);
                break;

            case PacketNumber.CONNECTSUCCESS:
                UnityEngine.Debug.Log("Connection Successful");
                break;

            case PacketNumber.SIGNIN:
                SigninSuccess = 1;
                UnityEngine.Debug.Log("Sign in Successful");
                break;

            default:
                UnityEngine.Debug.LogWarning("Unknown packet received: " + packet.number);
                break;
        }
    }

    public static void Close()
    {
        if (stream != null)
            stream.Close();
        if (client != null)
            client.Close();

        connection_state = Manager.ConnectionState.Disconnect;

        if (send_thread != null && send_thread.IsAlive)
            send_thread.Join();

        if (recv_thread != null && recv_thread.IsAlive)
        {
            recv_thread.Interrupt();
            recv_thread.Abort();
        }
    }

    public static byte[] StructureToByte(object obj)
    {
        int datasize = Marshal.SizeOf(obj);
        byte[] data = new byte[datasize];
        IntPtr buff = Marshal.AllocHGlobal(datasize);

        try
        {
            Marshal.StructureToPtr(obj, buff, false);
            Marshal.Copy(buff, data, 0, datasize);
        }
        finally
        {
            Marshal.FreeHGlobal(buff);
        }

        return data;
    }

    static void DisconnectClient()
    {
        if (connection_state == ConnectionState.Disconnect)
            return;

        UnityEngine.Debug.LogWarning("연결 종료 중");
        connection_state = ConnectionState.Disconnect;

        try
        {
            if (stream != null) stream.Close();
            if (client != null) client.Close();
        }
        catch (Exception ex)
        {
            UnityEngine.Debug.LogError("연결 종료 중 오류 발생 : " + ex.Message);
        }

        if (recv_thread != null && recv_thread.IsAlive)
        {
            recv_thread.Interrupt();
            recv_thread.Abort();
        }
        if (send_thread != null && send_thread.IsAlive)
        {
            send_thread.Interrupt();
            send_thread.Abort();
        }

        UnityEngine.Debug.Log("연결 종료 완료");
    }

}