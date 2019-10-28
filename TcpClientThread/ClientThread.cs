﻿using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

class ClientThread
{
    public struct Struct_Internet
    {
        public string ip;
        public int port;
    }

    private Socket clientSocket;//連線使用的Socket
    private Struct_Internet internet;
    public string receiveMessage;
    private string sendMessage;

    private Thread threadReceive;
    private Thread threadConnect;

    private readonly Func<Socket> SocketFactory;

    public bool IsConnected
    {
        get
        {
            if (clientSocket == null || !clientSocket.Connected)
            {
                return false;
            }

            try
            {
                return !(clientSocket.Poll(1, SelectMode.SelectRead) && clientSocket.Available == 0);
            }
            catch (SocketException)
            {
                return false;
            }
        }
    }

    public ClientThread(AddressFamily family, SocketType socketType, ProtocolType protocolType, string ip, int port)
    {
        SocketFactory = () => new Socket(family, socketType, protocolType);
        internet.ip = ip;
        internet.port = port;
        receiveMessage = null;
        //nowConnectCount = 0;
    }

    public void StartConnect()
    {
        threadConnect = new Thread(Accept);
        threadConnect.Start();
    }

    public void StopConnect()
    {
        try
        {
            clientSocket.Close();
        }
        catch (Exception ex)
        {
            Log(ex.ToString());
        }
    }

    public void Send(string message)
    {
        if (message == null)
            throw new NullReferenceException("message不可為Null");
        else
            sendMessage = message;
        SendMessage();
    }

    public void Receive()
    {
        if (threadReceive != null && threadReceive.IsAlive)
            return;
        threadReceive = new Thread(ReceiveMessage);
        threadReceive.IsBackground = true;
        threadReceive.Start();
    }

    private void Accept()
    {
        try
        {
            clientSocket = SocketFactory();
            clientSocket.Connect(IPAddress.Parse(internet.ip), internet.port);//等待連線，若未連線則會停在這行
            Log("Connected!");
        }
        catch (Exception ex)
        {
            Log(ex.ToString());
        }
    }

    private void SendMessage()
    {
        try
        {
            if (clientSocket.Connected)
            {
                clientSocket.Send(Encoding.ASCII.GetBytes(sendMessage));
            }
        }
        catch (Exception ex)
        {
            Log(ex.ToString());
        }
    }

    private void ReceiveMessage()
    {
        try
        {
            if (clientSocket.Connected)
            {
                byte[] bytes = new byte[256];
                long dataLength = clientSocket.Receive(bytes);

                if (dataLength == 0)
                {
                    receiveMessage = null;
                }
                else
                {
                    receiveMessage = Encoding.ASCII.GetString(bytes);
                }
            }
        }
        catch (Exception ex)
        {
            Log(ex.ToString());
        }
    }

    private void Log(string str)
    {
        Console.WriteLine(str);
    }
}