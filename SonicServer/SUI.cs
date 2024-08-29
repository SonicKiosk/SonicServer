using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;
using SonicServer;
using SonicServer.JsonClasses;

namespace ServerUI
{
    public partial class SUI : Form
    {
        Logger logger = new Logger("ServerUI", Color.Aquamarine, 2f);

        private TcpListener listener;
        private string host;
        private int port;

        private ClientHandler? activeHandler;

        public SUI(TcpListener server, string host, int port)
        {
            listener = server;
            this.host = host;
            this.port = port;
            InitializeComponent();
            logger.Info("Initialized All Components");
        }

        public void UpdateClients()
        {
            clientList.Items.Clear();
            for (int i = 0; i < SServer._activeConnections.Count; i++)
            {
                ClientHandler handler = SServer._activeConnections[i];
                clientList.Items.Add(i.ToString());
            }
        }

        public void SUI_Load(object sender, EventArgs e)
        {
            IPLabel.Text = $"{host}:{port}";
        }
        public static string GetClientString(string guid) => $"Client {{{guid}}}";
        public static string GetInfoString(string info) => $"Info: {info}"; // this is so stupid
        public void UpdateInfo(ClientHandler? targetHandler = null)
        {
            ClientHandler client = targetHandler ?? SServer._activeConnections[clientList.SelectedIndex];
            activeClientID.Text = GetClientString(client.id.ToString());
            infoIndicatorLbl.Text = client.clientInfo.Equals(default(CustomerInfo)) ?
                GetInfoString("Nothing")
                : GetInfoString($"Exists ({client.clientInfo.ID})");
        }
        private void clientList_DoubleClick(object sender, EventArgs e)
        {
            //activeClientID.Text = clientList.SelectedIndex != -1 ?
            //    GetClientString(SServer._activeConnections[clientList.SelectedIndex].id.ToString())
            //    : GetClientString("N/A");

            if (clientList.SelectedIndex != -1)
            {
                ClientHandler client = SServer._activeConnections[clientList.SelectedIndex];
                //activeClientID.Text = GetClientString(client.id.ToString());
                activeHandler = client;

                if (instance != null)
                    instance.activeHandler = client;

                UpdateInfo(client);
                //infoIndicatorLbl.Text = client.clientInfo.Equals(default(CustomerInfo)) ? GetInfoString("Nothing") : GetInfoString("Exists");
            }
            else
            {
                activeClientID.Text = GetClientString("N/A");
            }
        }

        private void updatelist_Tick(object sender, EventArgs e)
        {
            if (clientList.Items.Count != SServer._activeConnections.Count) UpdateClients();
        }

        private void checkInButton_Click(object sender, EventArgs e)
        {
            //RetailEventUtils.Checkin(activeHandler, new Customer() { CustomerInfo = new CustomerInfo() { } })
        }

        InfoEditor? instance;
        private void infoEditorBtn_Click(object sender, EventArgs e)
        {
            if (instance == null || instance.IsDisposed)
                instance = new InfoEditor(this, activeHandler);

            instance.Show();
        }

        private void checkInBtn_Click(object sender, EventArgs e)
        {
            if (activeHandler != null)
                RetailEventUtils.Checkin(activeHandler, new Customer() { CustomerInfo = activeHandler.clientInfo });
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (activeHandler != null)
                //RetailEventUtils.CrashClient(activeHandler, RetailEventUtils.CrashMethod);
                RetailEventUtils.Checkout(activeHandler, new Customer() { CustomerInfo = activeHandler.clientInfo });
        }

        private void SUI_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
        }

        private void createOrderBtn_Click(object sender, EventArgs e)
        {
            if (activeHandler != null)
                RetailEventUtils.Ticket(activeHandler, new Ticket()
                {
                    EmployeeFirstName = "Joe",
                    EmployeeLastName = "Swagson",
                    State = "ACTIVE",
                    SubTicketList = new List<SubTicket>(),
                    Tax = "0.00",
                    Total = "0.00"
                });
        }
        [STAThread]
        private void button2_Click(object sender, EventArgs e)
        {
            string fp = string.Concat(Enumerable.Repeat("../", 0)) + "gq8zp9lznddd1.jpeg";
            logger.TestStyles(fp);
            if (activeHandler != null)
                activeHandler.AddItem(lastnametxt.Text.Trim(), "Snack Popcorn Chicken Combo LRG", "shit", "1.00", 1, fp);
            if (openFileDialog1.ShowDialog() != DialogResult.OK)
                return;

            if (!openFileDialog1.FileName.EndsWith("productcatalog.json"))
            {
                logger.Warn("Filename didn't end with \"productcatalog.json\"");
                return;
            }

            using(FileStream fs = (FileStream) openFileDialog1.OpenFile())
            {
                byte[] rawData = new byte[fs.Length];
                fs.Read(rawData, 0, rawData.Length);

                string strjson = Encoding.UTF8.GetString(rawData);

            }
        }
    }
}
