using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Threading;
namespace p2pchat_sinf
{
    public partial class Form1 : Form
    {
        delegate void AddMessage(string message);

        string userName;

        const int port = 54545;
        const string broadcastAddress = "255.255.255.255";

        UdpClient receivingClient;
        UdpClient sendingClient;

        Thread receivingThread;
        public Form1()
        {
            InitializeComponent();
            this.Load += new EventHandler(ChatForm_Load);
        }
        void ChatForm_Load(object sender, EventArgs e)
        {
            this.Hide();

            using (fLogin loginForm = new fLogin())
            {
                loginForm.ShowDialog();

                if (loginForm.UserName == "")
                    this.Close();
                else
                {
                    userName = loginForm.UserName;
                    this.Show();
                }
            }

            tbMensaje.Focus();

            InitializeSender();
            InitializeReceiver();
        }

        private void InitializeSender()
        {
            sendingClient = new UdpClient(broadcastAddress, port);
            sendingClient.EnableBroadcast = true;
        }

        private void InitializeReceiver()
        {
            receivingClient = new UdpClient(port);

            ThreadStart start = new ThreadStart(Receiver);
            receivingThread = new Thread(start);
            receivingThread.IsBackground = true;
            receivingThread.Start();
        }

        private void btnEnviar_Click(object sender, EventArgs e)
        {
            tbMensaje.Text = tbMensaje.Text.TrimEnd();

            if (!string.IsNullOrEmpty(tbMensaje.Text))
            {
                string toSend = userName + ":\n" + tbMensaje.Text;
                byte[] data = Encoding.ASCII.GetBytes(toSend);
                sendingClient.Send(data, data.Length);
                tbMensaje.Text = "";
            }

            tbMensaje.Focus();
        }
        private void Receiver()
        {
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, port);
            AddMessage messageDelegate = MessageReceived;

            while (true)
            {
                byte[] data = receivingClient.Receive(ref endPoint);
                string message = Encoding.ASCII.GetString(data);
                Invoke(messageDelegate, message);
            }
        }

        private void MessageReceived(string message)
        {
            rtbChat.Text += message + "\n";
        }
    }
}
