using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.IO;
using MDPModel;
using System.Windows.Threading;
namespace MDPSimulator
{
    public class WifiConnector
    {
        private TcpClient clientSocket;
        private string IpAddr { set; get; }
        public delegate void ReceivingData(string s);
        public delegate void UpdatingInfo(string s);
        public delegate void UpdatingConnectionStatus(bool isConnected);
        public event UpdatingConnectionStatus UpdatingConnectionHandler;
        public event ReceivingData ReceivingDataHandler;
        public event UpdatingInfo UpdatingConsoleHandler;
        public string shortestPath;
        public bool isShortestPathSent;
        private DispatcherTimer timer;
        private Socket testSocket;
        private byte[] byteData = new byte[1024];
        public WifiConnector()
        {
            clientSocket = new TcpClient();
            IpAddr = "192.168.9.9";
            this.timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 10);
            //timer.Tick += new EventHandler(timer_Tick);
        }

        //not implemented yet
        private void timer_Tick(object sender, EventArgs e)
        {
            connect();
            listen();
        }

        public bool connect()
        {
            OnUpdatingConsole("Connecting to RPI");
            IPEndPoint serverEndPoint = new IPEndPoint(IPAddress.Parse(IpAddr), 3000);
            bool isConnected = false;
            //try 3 times
            int trial = 3;
            while (!isConnected && trial > 0)
            {
                trial--;
                try
                {
                    clientSocket.Connect(serverEndPoint);
                    if (clientSocket.Connected)
                    {
                        isConnected = true;
                        OnStatusUpdating(true);
                        OnUpdatingConsole("Connected to rpi");
                    }
                }
                catch (Exception)
                {
                    OnStatusUpdating(false);
                }
            }
            if (!isConnected && trial == 0)
            {
                Console.WriteLine("Cannot connect to server!");
                OnUpdatingConsole("Fail to establish connection!");
                OnStatusUpdating(false);
                return false;
            }
            else
            {
                return true;
            }
        }
        protected virtual void OnUpdatingConsole(string s)
        {
            if (UpdatingConsoleHandler != null)
            {
                UpdatingConsoleHandler(s);
            }
            else
            {
                Console.WriteLine(s);
            }
        }
        public void send(string s)
        {
            if (clientSocket.Connected)
            {
                OnUpdatingConsole("Sending " + s + "!");
                NetworkStream stream = clientSocket.GetStream();
                StreamWriter writer = new StreamWriter(stream);
                StreamReader reader = new StreamReader(stream);
                writer.WriteLine(s);
                writer.Flush();
            }
            else
            {
                Console.WriteLine("No connection!");
            }
        }
        public void listen()
        {
            try
            {
                NetworkStream nw = clientSocket.GetStream();
                clientSocket.ReceiveBufferSize = 400;
                string desc = "";
                byte[] data;
                //while (this.checkConnectionStatus)
                while (this.clientSocket.Connected)
                {
                    data = new byte[clientSocket.ReceiveBufferSize];
                    int bytesRead = nw.Read(data, 0, clientSocket.ReceiveBufferSize);
                    desc = Encoding.ASCII.GetString(data, 0, bytesRead);
                    if (desc.Length == 0)
                    {
                        continue;
                    }
                    //message is not zero
                    OnReceivingData(desc);
                    //exploration finished, compute shortest path
                    //if (desc[desc.Length - 1] == 'F')
                    //{
                    //    OnUpdatingConsole("Exploration finish, computing shortest path");
                    //    Robot robot = new Robot();
                    //    string path = robot.realTimeShortestPath(desc);
                    //    this.shortestPath = "A";
                    //    if (path != null)
                    //    {
                    //        this.shortestPath += path;
                    //    }
                    //    else
                    //    {
                    //        //no solution
                    //        this.shortestPath = "B";
                    //    }
                    //    send(this.shortestPath);
                    //}
                }
                OnStatusUpdating(false);
                OnUpdatingConsole("Connection lost");
            }
            catch (Exception e)
            {
                Console.WriteLine("Error in listening: " + e.Message);
                OnStatusUpdating(false);
            }

        }
        public bool checkConnectionStatus()
        {
            if (this.clientSocket.Connected && clientSocket.Client.Poll(01, SelectMode.SelectRead))
            {
                byte [] buff = new byte[1];
                if (clientSocket.Client.Receive(buff, SocketFlags.Peek) == 0)
                {
                    return false;
                }
            }
            return true;
        }

        public void run()
        {
            this.connect();
            if (clientSocket.Connected)
                this.listen();

            //a new connection, not tested
            //try
            //{
            //    testSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //    IPAddress ipAddress = IPAddress.Parse(IpAddr);
            //    IPEndPoint ipEndPoint = new IPEndPoint(ipAddress, 3000);
            //    //Connect to the server
            //    testSocket.BeginConnect(ipEndPoint, new AsyncCallback(OnConnect), null);
            //    testSocket.BeginReceive(byteData, 0, byteData.Length, SocketFlags.None,
            //        new AsyncCallback(OnReceive), testSocket);
            //}
            //catch (Exception ex)
            //{
            //    OnUpdatingConsole("Failed to set up connection");
            //    Console.WriteLine(ex.Message);
            //}
        }

        private void OnReceive(IAsyncResult ar)
        {
            var socket = (Socket)ar.AsyncState;
            int received = testSocket.EndReceive(ar);
            var receivedData = new byte[received];
            Array.Copy(byteData, receivedData, received);
            string desc = Encoding.ASCII.GetString(receivedData, 0, received);
            if (desc[desc.Length - 1] == 'F')
            {
                Robot robot = new Robot();
                string path = robot.realTimeShortestPath(desc);
                this.shortestPath = "A";
                if (path != null)
                {
                    this.shortestPath += path;
                }
                else
                {
                    //no solution
                    this.shortestPath = "B";
                }
                sendShortestPath(this.shortestPath);
            }
            if (desc.Length != 0) 
            {
                OnReceivingData(desc);
            }
            testSocket.BeginReceive(byteData, 0, byteData.Length, SocketFlags.None, OnReceive, testSocket);
        }
        private void OnSend(IAsyncResult ar)
        {
            var socket = (Socket)ar.AsyncState;
            OnUpdatingConsole("Shortest path sent");
            socket.EndSend(ar);
        }
        private void OnConnect(IAsyncResult ar)
        {
            OnStatusUpdating(true);
            OnUpdatingConsole("Connected to rpi");
        }
        private void sendShortestPath(string s)
        {
            try
            {
                System.Buffer.BlockCopy(s.ToCharArray(), 0, byteData, 0, byteData.Length);
                //Send it to the server
                testSocket.BeginSend(byteData, 0, byteData.Length, SocketFlags.None, new AsyncCallback(OnSend), null);
            }
            catch (Exception)
            {
                Console.WriteLine("Cannot send shortest path");
            }
        }
        protected virtual void OnReceivingData(string s)
        {
            if (ReceivingDataHandler != null)
            {
                ReceivingDataHandler(s);
            }
            else
            {
                Console.WriteLine("Receiving data but no event subscriber!");
            }
        }
        protected virtual void OnStatusUpdating(bool b)
        {
            if (UpdatingConnectionHandler != null)
            {
                UpdatingConnectionHandler(b);
            }
            else
            {
                Console.WriteLine("No handler for connection status update!");
            }
        }

    }
}
