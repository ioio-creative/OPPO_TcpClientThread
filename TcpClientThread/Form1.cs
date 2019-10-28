using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TcpClientThread
{
    public partial class fmTcpClientThread : Form
    {
        private const int ReconnectionIntervalInMillis = 2000;
        private const int TimerIntervalInMillis = 20;
        private const int IsWaitingForConnectionCounterMax = ReconnectionIntervalInMillis / TimerIntervalInMillis;


        private delegate void SetTextCallback(string str);

        private ClientThread ct;
        //private bool isSend;
        //private bool isReceive;
        private int isWaitingForConnectionCounter = 0;
        private bool _isWaitingForConnection;
        private bool isWaitingForConnection
        {
            get { return _isWaitingForConnection; }
            set
            {
                _isWaitingForConnection = value;
                if (!value)
                {
                    isWaitingForConnectionCounter = 0;
                }
            }
        }

        private readonly Timer timer = new Timer();

        public fmTcpClientThread()
        {
            InitializeComponent();
        }

        private void fmTcpClientThread_Load(object sender, EventArgs e)
        {
            StartClient();

            timer.Tick += new EventHandler(Update);
            timer.Interval = TimerIntervalInMillis;
            timer.Enabled = true;
        }

        private void fmTcpClientThread_FormClosed(object sender, FormClosedEventArgs e)
        {            
            timer.Dispose();
            ct.StopConnect();             
        }

        // This method demonstrates a pattern for making thread-safe
        // calls on a Windows Forms control.
        //
        // If the calling thread is different from the thread that
        // created the TextBox control, this method creates a
        // SetTextCallback and calls itself asynchronously using the
        // Invoke method.
        //
        // If the calling thread is the same as the thread that created
        // the TextBox control, the Text property is set directly.

        private void SetText(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.txtReceived.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetText);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                // https://stackoverflow.com/questions/13318561/adding-new-line-of-data-to-textbox
                this.txtReceived.AppendText(text);
                this.txtReceived.AppendText(Environment.NewLine);
            }
        }

        private void StartClient()
        {
            ct = new ClientThread(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp, "127.0.0.1", 12580);
            ct.StartConnect();

            //isSend = true;
            isWaitingForConnection = true;
        }

        private void Update(object sender, EventArgs e)
        {
            if (ct.receiveMessage != null)
            {
                SetText("Server: " + ct.receiveMessage);
                ct.receiveMessage = null;
            }

            if (ct.IsConnected)
            {
                isWaitingForConnection = false;
                ct.Receive();
            }
            else
            {
                SetText("Server: Disconnected!");
                if (!isWaitingForConnection || isWaitingForConnectionCounter > IsWaitingForConnectionCounterMax)
                {
                    ct.StopConnect();
                    ct.StartConnect();
                    isWaitingForConnection = true;
                    isWaitingForConnectionCounter = 0;
                }
            }

            if (isWaitingForConnection)
            {
                isWaitingForConnectionCounter++;
            }
        }

        //private IEnumerator delaySend()
        //{
        //    isSend = false;
        //    yield return new WaitForSeconds(1);
        //    ct.Send("Hello~ My name is Client");
        //    isSend = true;
        //}
    }
}
