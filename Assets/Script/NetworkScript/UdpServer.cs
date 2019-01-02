﻿using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

public class UdpServer : MonoBehaviour
{

    //以下默认都是私有的成员 
    private int bcPort = 8001;//广播端口  
    Socket socket; //目标socket 
    EndPoint clientEnd; //客户端 
    IPEndPoint ipEnd; //侦听端口 
    string recvStr; //接收的字符串 
    string sendStr; //发送的字符串 
    byte[] recvData = new byte[1024]; //接收的数据，必须为字节 
    byte[] sendData = new byte[1024]; //发送的数据，必须为字节 
    int recvLen; //接收的数据长度 
    Thread connectThread; //连接线程

    //初始化
    public void InitSocket()
    {
        //定义侦听端口,侦听任何IP
        ipEnd = new IPEndPoint(IPAddress.Any, bcPort);
        //定义套接字类型,在主线程中定义
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        //服务端需要绑定ip
        socket.Bind(ipEnd);
        //定义客户端
        IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);
        clientEnd = (EndPoint)sender;
        sendStr = PlayerPrefs.GetString("PlayerName");
        //开启一个线程连接，必须的，否则主线程卡死
        connectThread = new Thread(new ThreadStart(SocketReceive));
        connectThread.Start();
    }

    void Update()
    {
        if (socketColdTime < 0.5) socketColdTime += Time.deltaTime;
    }

    void SocketSend()
    {
        print("发送房间消息给客户端：" + clientEnd);
        //清空发送缓存
        sendData = new byte[1024];
        //数据类型转换
        sendData = Encoding.ASCII.GetBytes(sendStr);
        //发送给指定客户端
        socket.SendTo(sendData, sendData.Length, SocketFlags.None, clientEnd);
    }

    float socketColdTime = 0;
    //服务器接收
    void SocketReceive()
    {
        //进入接收循环
        while (true)
        {
            //对data清零
            recvData = new byte[1024];
            //获取客户端，获取客户端数据，用引用给客户端赋值
            recvLen = socket.ReceiveFrom(recvData, ref clientEnd);
            //输出接收到的数据
            recvStr = Encoding.ASCII.GetString(recvData, 0, recvLen);
            print("房主-检测到玩家：" + recvStr + "\nIP：" + clientEnd.ToString());
            //将接收到的数据经过处理再发送出去
            SocketSend();
            socketColdTime = 0;
        }
    }

    //连接关闭
    public void SocketQuit()
    {
        //关闭线程
        if (connectThread != null)
        {
            connectThread.Interrupt();
            connectThread.Abort();
            print("关闭UDP广播");
        }
        //最后关闭socket
        if (socket != null)
            socket.Close();
    }

    void OnApplicationQuit()
    {
        SocketQuit();
    }
}
