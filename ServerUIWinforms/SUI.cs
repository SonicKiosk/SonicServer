using System.Net.Sockets;

namespace ServerUI
{
    public partial class SUI : Form
    {
        private TcpListener listener;
        private string host;
        private int port;
        public SUI(TcpListener server, string host, int port)
        {
            listener = server;
            this.host = host;
            this.port = port;
            InitializeComponent();
        }

        public void SUI_Load(object sender, EventArgs e)
        {
            IPLabel.Text = $"{host}:{port}";
        }
    }
}
