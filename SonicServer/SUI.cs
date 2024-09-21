using System.CodeDom;
using System.Drawing.Imaging;
using System.Net.Sockets;
using System.Runtime.InteropServices.Marshalling;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using Newtonsoft.Json;
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
        public static IEnumerable<T> Descendants<T>(Control control) where T : class
        {
            foreach (Control child in control.Controls)
            {

                T childOfT = child as T;
                if (childOfT != null)
                {
                    yield return (T)childOfT;
                }

                if (child.HasChildren)
                {
                    foreach (T descendant in Descendants<T>(child))
                    {
                        yield return descendant;
                    }
                }
            }
        }
        public void LoadTheme(Theme theme)
        {
            foreach (Control c in Descendants<Control>(this))
            {
                switch (c.GetType())
                {
                    case Type t when t == typeof(Label):
                        Label l = (Label)c;
                        l.ForeColor = theme.TextColor;
                        l.BackColor = theme.BackgroundColor;
                        if (c.Parent != null)
                            c.BackColor = c.Parent.BackColor;
                        break;
                    case Type t when t.BaseType == typeof(TextBoxBase) || t == typeof(TextBox) || t == typeof(RichTextBox):
                        TextBoxBase tb = (TextBoxBase)c;
                        tb.ForeColor = theme.TextColor;
                        tb.BackColor = theme.TextBoxColor;
                        tb.BorderStyle = theme.BorderStyle;
                        break;
                    case Type t when t == typeof(TabPage):
                        TabPage tp = (TabPage)c;
                        tp.ForeColor = theme.TextColor;
                        tp.BackColor = theme.PageColor;
                        break;
                    case Type t when t == typeof(CheckBox):
                        CheckBox cb = (CheckBox)c;
                        cb.ForeColor = theme.TextColor;
                        cb.BackColor = Color.Transparent;
                        if (c.Parent != null)
                            c.BackColor = c.Parent.BackColor;
                        break;
                    case Type t when t == typeof(Button):
                        Button b = (Button)c;
                        b.ForeColor = theme.TextColor;
                        b.BackColor = theme.ElementColor;
                        b.FlatStyle = theme.FlatStyle;
                        break;
                    case Type t when t == typeof(Panel):
                        Panel p = (Panel)c;
                        p.ForeColor = theme.TextColor;
                        p.BackColor = Color.Transparent;
                        if (c.Parent != null)
                            c.BackColor = c.Parent.BackColor;
                        break;
                    default:
                        c.ForeColor = theme.TextColor;
                        c.BackColor = theme.ElementColor;
                        break;
                }
            }
            logger.Debug(theme.TextColor, theme.BackgroundColor);
            ForeColor = theme.TextColor;
            BackColor = theme.BackgroundColor;

            checkInBtn.ForeColor = theme.DataBoxColorGuide.CheckIn;
            button1.ForeColor = theme.DataBoxColorGuide.CheckOut;
            createOrderBtn.ForeColor = theme.DataBoxColorGuide.OrderBtn;
            infoEditorBtn.ForeColor = theme.DataBoxColorGuide.InfoBtn;
            activeClientID.ForeColor = theme.DataBoxColorGuide.ClientLbl;
            infoIndicatorLbl.ForeColor = theme.DataBoxColorGuide.InfoLbl;
        }

        public void SUI_Load(object sender, EventArgs e)
        {
            IPLabel.Text = $"{host}:{port}";

            mainContent.DrawMode = TabDrawMode.OwnerDrawFixed;

            LoadTheme(ThemeRegistry.DefaultDark);
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
                //infoIndicatorLbl.Text = client.clientInfo.Equals(default(CustomerInfo))
                //    ? GetInfoString("Nothing")
                //    : GetInfoString("Exists");
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
            {
                activeHandler.ActiveTicket.Clear();
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
        }
        [STAThread]
        private void button2_Click(object sender, EventArgs e)
        {
            //string fp = string.Concat(Enumerable.Repeat("../", 0)) + "gq8zp9lznddd1.jpeg";
            //logger.TestStyles(fp);
            //if (activeHandler != null)
            //    activeHandler.AddItem(lastnametxt.Text.Trim(), "Snack Popcorn Chicken Combo LRG", "shit", "1.00", 1, fp);
            if (openFileDialog1.ShowDialog() != DialogResult.OK)
                return;

            //if (!openFileDialog1.FileName.EndsWith("productcatalog.json"))
            //{
            //    logger.Warn("Filename didn't end with \"productcatalog.json\"");
            //    //return;
            //}

            using (FileStream fs = (FileStream)openFileDialog1.OpenFile())
            {
                byte[] rawData = new byte[fs.Length];
                fs.Read(rawData, 0, rawData.Length);

                string strjson = Encoding.UTF8.GetString(rawData);
                ProductCatalog catalog = default;
                //logger.Debug(strjson);
                try
                {
                    catalog = JsonConvert.DeserializeObject<ProductCatalog>(strjson);
                    // not needed for debugging, so just log filename
                    logger.Info($"Deserialized {Path.GetFileName(openFileDialog1.FileName)} successfully.");
                }
                catch
                {
                    logger.Error($"Failed to deserialize {openFileDialog1.FileName} to struct `ProductCatalog`");
                    return;
                }

                bool show_all = MessageBox.Show(
                    "Show every item? (this includes items without pictures or items that might not supposed to be visible)",
                    "ProductCatalog Parser",
                    MessageBoxButtons.YesNo) == DialogResult.Yes;

                UpdateCatalogList(catalog);
            }
        }

        List<ListViewCatalogItem> catalogItems = new List<ListViewCatalogItem>();
        public class ListViewCatalogItem
        {
            public Image? image;
            public ProductCatalog.Item item;

            public ListViewCatalogItem(string rootFolder, ProductCatalog.Item item)
            {
                this.item = item;
                if (item.Images.Count > 0)
                    if (!string.IsNullOrWhiteSpace(item.Images[0].Path))
                        image = Image.FromFile(Path.Combine(rootFolder, "data/Stall/marketing", item.Images[0].Path));
            }
        }
        public static string GenerateRandomString(int length)
        {
            const string chars = "abcdef0123456789";
            StringBuilder result = new StringBuilder(length);
            Random random = new Random();

            for (int i = 0; i < length; i++)
            {
                result.Append(chars[random.Next(chars.Length)]);
            }

            return result.ToString();
        }
        public static string GetImageHash(Image image)
        {
            return GenerateRandomString(32); // totally a hash trust
            //using (MemoryStream ms = new MemoryStream())
            //{
            //    image.Save(ms, ImageFormat.Bmp);

            //    byte[] imageBytes = ms.ToArray();

            //    using (SHA256 sha256 = SHA256.Create())
            //    {
            //        byte[] hashBytes = sha256.ComputeHash(imageBytes);

            //        StringBuilder sb = new StringBuilder();
            //        foreach (byte b in hashBytes)
            //        {
            //            sb.Append(b.ToString("X2"));
            //        }
            //        return sb.ToString();
            //    }
            //}
        }
        public string PromptRootFolder(bool notify = false)
        {
            if (notify)
                MessageBox.Show(
                    "SyncInfo wasnt loaded. Please locate and select C:/Sonic or the Sonic folder you are primarily using",
                    "SyncInfo not loaded",
                    MessageBoxButtons.OK);

            //folderBrowserDialog1.SelectedPath = "C:/";
            if (string.IsNullOrWhiteSpace(folderBrowserDialog1.SelectedPath) && Directory.Exists("C:/Sonic"))
                folderBrowserDialog1.SelectedPath = "C:/Sonic";

            return folderBrowserDialog1.ShowDialog() == DialogResult.OK
                ? folderBrowserDialog1.SelectedPath
                : "C:/Sonic";
        }
        string? rootFolder = null;
        public void UpdateCatalogList(ProductCatalog catalog, bool show_all = false, List<ListViewCatalogItem>? override_list = null)
        {
            List<ListViewCatalogItem> target = override_list ?? catalogItems;
            target.Clear();
            //string rootFolder;

            if (MessageBox.Show("Manually specify root folder? (Only select yes if you installed the Sonic folder outside of C:/ or the apps config points to the wrong RootFolder location)",
                "Root Folder Selection",
                MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                rootFolder = PromptRootFolder();
            }
            else
            {
                logger.Debug("App config RootFolder:", activeHandler?.syncInfo.Software.CurrentConfig.App.RootFolder);
                rootFolder = activeHandler != null ? activeHandler.syncInfo.Software.CurrentConfig.App.RootFolder : PromptRootFolder(true);
            }
            foreach (ProductCatalog.Item item in catalog.Items)
                target.Add(new ListViewCatalogItem(rootFolder, item));

            DrawCatalogList(show_all, target);
        }

        // shitty. this took so much longer than the new method by loading all the images first then adding the items
        //public void DrawCatalogList(bool show_all = false, List<ListViewCatalogItem>? override_list = null)
        //{
        //    listView1.Clear();
        //    List<ListViewCatalogItem> target = override_list ?? catalogItems;
        //    ImageList images = new ImageList();
        //    listView1.LargeImageList = images;
        //    new Thread(() =>
        //    {
        //        foreach (ListViewCatalogItem catalogItem in target)
        //        {
        //            foreach (ProductCatalog.Item.DescriptionProperty description in catalogItem.item.Descriptions.Long)
        //                if (description.Language == "en-US"
        //                    && !string.IsNullOrWhiteSpace(description.Text)
        //                    && (catalogItem.image != null || show_all))
        //                {
        //                    ListViewItem? item = null;
        //                    Invoke(() =>
        //                    {
        //                        item = listView1.Items.Add(description.Text);
        //                    });

        //                    if (item == null)
        //                        continue;

        //                    if (catalogItem.image != null)
        //                    {
        //                        string imageHash = GetImageHash(catalogItem.image);
        //                        Invoke(() =>
        //                        {
        //                            images.Images.Add(imageHash, catalogItem.image);
        //                            item.ImageKey = imageHash;
        //                        });
        //                    }
        //                    else if (show_all)
        //                        continue;
        //                }
        //        }
        //        logger.Info("Finished drawing listview.");
        //    }).Start();
        //}
        public void DrawCatalogList(bool show_all = false, List<ListViewCatalogItem>? override_list = null)
        {
            listView1.Clear();
            List<ListViewCatalogItem> target = override_list ?? catalogItems;
            ImageList images = new ImageList();
            listView1.LargeImageList = images;

            Dictionary<Image, string> imageKeys = new Dictionary<Image, string>();

            foreach (ListViewCatalogItem catalogItem in target)
            {
                if (catalogItem.image != null)
                {
                    string imageHash = GetImageHash(catalogItem.image);
                    images.Images.Add(imageHash, catalogItem.image);
                    imageKeys[catalogItem.image] = imageHash;
                }
            }

            new Thread(() =>
            {
                foreach (ListViewCatalogItem catalogItem in target)
                {
                    foreach (ProductCatalog.Item.DescriptionProperty description in catalogItem.item.Descriptions.Long)
                    {
                        if (description.Language == "en-US"
                            && !string.IsNullOrWhiteSpace(description.Text)
                            && (catalogItem.image != null || show_all))
                        {
                            ListViewItem? item = null;
                            Invoke(() =>
                            {
                                item = listView1.Items.Add(description.Text);
                            });

                            if (item == null)
                                continue;

                            if (catalogItem.image != null || show_all)
                            {
                                if (catalogItem.image != null && !imageKeys.ContainsKey(catalogItem.image))
                                    return;
                                // Use the stored image key
                                string imageHash = imageKeys[catalogItem.image!];
                                Invoke(() =>
                                {
                                    item.ImageKey = imageHash;
                                });
                            }
                            else if (show_all)
                            {
                                continue;
                            }
                        }
                    }
                }
                logger.Info("Finished drawing listview.");
            }).Start();
        }

        // TODO: change `category` from "who fucking knows bro" because SOMEONE has to know and it isnt me (yet)
        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            foreach (ListViewItem selectedItem in listView1.SelectedItems)
            {
                IEnumerable<ListViewCatalogItem> items = catalogItems.Where(x => x.item.Descriptions.Long.Where(y => y.Text == selectedItem.Text).Count() > 0);
                foreach (ListViewCatalogItem item in items)
                {
                    if (activeHandler != null)
                        activeHandler.AddItem(item.item.PLU, selectedItem.Text, "who fucking knows bro", PricingScraper.GetPriceForCatalogItem(item.item, SServer.Prices), 1, Path.GetRelativePath(rootFolder ?? "C:/Sonic", Path.Combine(rootFolder ?? "C:/Sonic", "data/Stall/marketing", item.item.Images[0].Path)));
                }
            }
        }

        private void SUI_Paint(object sender, PaintEventArgs e)
        {

        }

        private void mainContent_DrawItem(object sender, DrawItemEventArgs e) // garbage and thrown together super quick if you hate it make a pull request
        {
            TabPage tabPage = mainContent.TabPages[e.Index];
            Rectangle tabRect = mainContent.GetTabRect(e.Index);

            using (SolidBrush foreBrush = new SolidBrush(Color.FromArgb(175, 175, 175)))
            using (SolidBrush backBrush = new SolidBrush(Color.FromArgb(20, 20, 20)))
            {
                e.Graphics.FillRectangle(backBrush, new Rectangle(Point.Empty, mainContent.Size));
                foreach (TabPage page in mainContent.TabPages)
                {
                    Rectangle tabRect2 = mainContent.GetTabRect(mainContent.TabPages.IndexOf(page));
                    tabRect2.Offset(new Point(-1, -1));
                    tabRect2.Inflate(new Size(2, 2));
                    e.Graphics.FillRectangle(foreBrush, tabRect2);
                    tabRect2.Offset(new Point(1, 1));
                    tabRect2.Inflate(new Size(-2, -2));
                    e.Graphics.FillRectangle(backBrush, tabRect2);
                    TextRenderer.DrawText(e.Graphics, page.Text, page.Font, tabRect2, foreBrush.Color, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
                }
                e.Graphics.FillRectangle(backBrush, tabRect);
            }

            TextRenderer.DrawText(e.Graphics, tabPage.Text, tabPage.Font, tabRect, Color.White, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
        }

        private void dataBox_Paint(object sender, PaintEventArgs e)
        {
            GroupBox box = (sender as GroupBox)!;
            DrawGroupBox(box!, e.Graphics, Color.White, Color.FromArgb(155, 155, 155));
        }


        private void DrawGroupBox(GroupBox box, Graphics g, Color textColor, Color borderColor)
        {
            if (box != null)
            {
                Brush textBrush = new SolidBrush(textColor);
                Brush borderBrush = new SolidBrush(borderColor);
                Pen borderPen = new Pen(borderBrush);
                SizeF strSize = g.MeasureString(box.Text, box.Font);
                Rectangle rect = new Rectangle(box.ClientRectangle.X,
                                               box.ClientRectangle.Y + (int)(strSize.Height / 2),
                                               box.ClientRectangle.Width - 1,
                                               box.ClientRectangle.Height - (int)(strSize.Height / 2) - 1);

                // Clear text and border
                g.Clear(this.BackColor);

                // Draw text
                g.DrawString(box.Text, box.Font, textBrush, box.Padding.Left, 0);

                // Drawing Border
                //Left
                g.DrawLine(borderPen, rect.Location, new Point(rect.X, rect.Y + rect.Height));
                //Right
                g.DrawLine(borderPen, new Point(rect.X + rect.Width, rect.Y), new Point(rect.X + rect.Width, rect.Y + rect.Height));
                //Bottom
                g.DrawLine(borderPen, new Point(rect.X, rect.Y + rect.Height), new Point(rect.X + rect.Width, rect.Y + rect.Height));
                //Top1
                g.DrawLine(borderPen, new Point(rect.X, rect.Y), new Point(rect.X + box.Padding.Left, rect.Y));
                //Top2
                g.DrawLine(borderPen, new Point(rect.X + box.Padding.Left + (int)(strSize.Width), rect.Y), new Point(rect.X + rect.Width, rect.Y));
            }
        }
    }
}
