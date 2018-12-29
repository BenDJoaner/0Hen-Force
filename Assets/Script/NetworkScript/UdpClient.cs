using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

public class UdpClient : MonoBehaviour
{
    //以下默认都是私有的成员
    private int bcPort = 8001;//广播端口  
    Socket socket; //目标socket
    EndPoint serverEnd; //服务端
    IPEndPoint ipEnd; //服务端端口
    string recvStr; //接收的字符串
    string sendStr; //发送的字符串
    byte[] recvData = new byte[1024]; //接收的数据，必须为字节
    byte[] sendData = new byte[1024]; //发送的数据，必须为字节
    int recvLen; //接收的数据长度
    Thread connectThread; //连接线程
    LHLobbyManager manager;
    IPAddress ipv4;
    bool dataInFlag;
    //初始化
    public void InitSocket()
    {
        IPAddress[] ips = Dns.GetHostAddresses(Dns.GetHostName());   //Dns.GetHostName()获取本机名Dns.GetHostAddresses()根据本机名获取ip地址组
        foreach (IPAddress ip in ips)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                ipv4 = ip;
            }
        }
        //定义连接的服务器ip和端口，可以是本机ip，局域网，互联网
        ipEnd = new IPEndPoint(ipv4, bcPort);
        //定义套接字类型,在主线程中定义
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        //定义服务端
        IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);
        serverEnd = (EndPoint)sender;
        //建立初始连接，这句非常重要，第一次连接初始化了serverEnd后面才能收到消息
        SocketSend("Client:" + ipv4.ToString());
        //开启一个线程连接，必须的，否则主线程卡死
        connectThread = new Thread(new ThreadStart(SocketReceive));
        connectThread.Start();
        print("启动UDP客户端监听");
        manager = GetComponent<LHLobbyManager>();
    }

    void SocketSend(string sendStr)
    {
        //清空发送缓存
        sendData = new byte[1024];
        //数据类型转换
        sendData = Encoding.ASCII.GetBytes(sendStr);
        //发送给指定服务端
        socket.SendTo(sendData, sendData.Length, SocketFlags.None, ipEnd);
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
            //获取客户端，获取服务端端数据，用引用给服务端赋值，实际上服务端已经定义好并不需要赋值
            recvLen = socket.ReceiveFrom(recvData, ref serverEnd);
            //输出接收到的数据
            recvStr = Encoding.ASCII.GetString(recvData, 0, recvLen);
            print("玩家-检测到房间：" + recvStr + "\nIP：" + serverEnd.ToString());
            dataInFlag = true;
            ipEnd = new IPEndPoint(IPAddress.Parse(serverEnd.ToString()), bcPort); ;
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
            print("关闭UDP接收");
        }
        //最后关闭socket
        if (socket != null)
            socket.Close();
    }

    private void Update()
    {
        if (dataInFlag)
        {
            string ip = serverEnd.ToString().Substring(0, serverEnd.ToString().Length - 5);
            manager.joinPanel.OnLoadHostList(recvStr, ip);
            dataInFlag = false;
        }
        if (socketColdTime < 1) socketColdTime += Time.deltaTime;
    }

    void OnApplicationQuit()
    {
        SocketQuit();
    }
}
