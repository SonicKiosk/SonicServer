using System.Net;
using System.Net.Sockets;
using System.Security.Permissions;
using System.Windows.Forms;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SonicServer;
using SonicServer.JsonClasses;

namespace ServerUI
{
    public partial class InfoEditor : Form
    {
        public ProfileRegistryClass ProfileRegistryInstance = new ProfileRegistryClass();
        public class ProfileRegistryClass : AbstractGenericConfig<ProfileRegistryClass>
        {
            [JsonProperty("Default")]
            public CustomerInfo DefaultProfile = Default;

            public Dictionary<string, CustomerInfo> CustomProfiles = new Dictionary<string, CustomerInfo>()
            {
                { "JoeSwanson", new CustomerInfo() {
                    ID = "joe",
                    FirstName="Joe",
                    LastName="Swanson",
                    ProfilePictureUrl="https://static.wikia.nocookie.net/familyguy/images/9/9c/190px-Joe_Swanson.png/"
                } },
            };

            public override ProfileRegistryClass DefaultValue()
            {
                return new ProfileRegistryClass();
            }
        }

        public void LoadProfileRegistryFromClassInstance(ProfileRegistryClass instance)
        {
            ProfileRegistry.Clear();

            ProfileRegistry.Add("Default", instance.DefaultProfile);

            foreach (KeyValuePair<string, CustomerInfo> profile in instance.CustomProfiles)
            {
                ProfileRegistry.Add(profile.Key, profile.Value);
            }
        }
        public static Dictionary<string, CustomerInfo> ProfileRegistry = new Dictionary<string, CustomerInfo>()
        {
            { "Default", Default }
        };
        public static readonly CustomerInfo Default = new CustomerInfo()
        {
            ID = "guest",
            FirstName = "Guest",
            LastName = "",
            ProfilePictureUrl = ""
        };
        public CustomerInfo CurrentProfile = Default;
        private ClientHandler? _activeHandler;
        private SUI parent;
        public ClientHandler? activeHandler
        {
            get
            {
                return _activeHandler;
            }
            set
            {
                _activeHandler = value;
                activeClientID.Text = SUI.GetClientString(_activeHandler?.id.ToString() ?? "Error");
            }
        }
        public InfoEditor(SUI source, ClientHandler? activeHandler)
        {
            InitializeComponent();
            this.activeHandler = activeHandler;
            parent = source;
        }

        public static IEnumerable<T> Descendants<T>(Control control) where T : class // SAME AS THE OTHER THING
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
        public void LoadTheme(Theme theme) // REPETITION. BAD. FIX THIS LATER!
        {
            foreach (Control c in Descendants<Control>(this))
            {
                switch (c.GetType())
                {
                    case Type t when t == typeof(Label):
                        Label l = (Label)c;
                        l.ForeColor = theme.TextColor;
                        l.BackColor = Color.Transparent;
                        if (c.Parent != null)
                            c.BackColor = c.Parent.BackColor;
                        break;
                    case Type t when t.BaseType == typeof(TextBoxBase) || t == typeof(TextBox) || t == typeof(RichTextBox):
                        TextBoxBase tb = (TextBoxBase)c;
                        tb.ForeColor = theme.TextColor;
                        tb.BackColor = theme.ElementColor;
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
            ForeColor = theme.TextColor;
            BackColor = theme.BackgroundColor;
        }
        public static Image? DownloadFromURL(string url)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    Task<byte[]> webTask = client.GetByteArrayAsync(url);
                    webTask.Wait();
                    byte[] imageData = webTask.Result;
                    using (MemoryStream ms = new MemoryStream(imageData))
                        return Image.FromStream(ms);
                }
            }
            catch
            {
                return null;
            }
        }

        private void pfpSection_Click(object sender, EventArgs e)
        {
        }

        private void mainContent_SelectedIndexChanged(object sender, EventArgs e)
        {
            //test
            //if (mainContent.SelectedTab?.Text == "Profile Picture")
            //    //Console.WriteLine("updating image");
            //    pfpActualImg.Image = DownloadFromURL("https://placehold.co/256.png");

            if (mainContent.SelectedTab?.Text == "Profile Picture" && activeHandler != null)
                //Console.WriteLine("updating image");
                pfpActualImg.Image = DownloadFromURL(activeHandler.clientInfo.ProfilePictureUrl);
        }
        private string targetUrl = "";
        private bool CheckForChanges(CustomerInfo currentProfile)
        {
            applyProfileBtn.Text = "*Apply Info";
            if (targetUrl != currentProfile.ProfilePictureUrl)
                return true;
            else if (idTxt.Text != currentProfile.ID)
                return true;
            else if (firstnametxt.Text != currentProfile.FirstName)
                return true;
            else if (lastnametxt.Text != currentProfile.LastName)
                return true;
            else
            {
                applyProfileBtn.Text = "Apply Info";
                return false;
            }
        }
        private void previewBtn_Click(object sender, EventArgs e)
        {
            targetUrl = previewUrlTxt.Text;
            downloadedPreview.Image = DownloadFromURL(previewUrlTxt.Text);
            if (previewLargeCbx.Checked)
                pfpActualImg.Image = DownloadFromURL(previewUrlTxt.Text);
        }

        private void applyPreviewBtn_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(targetUrl) && downloadedPreview.Image != null && activeHandler != null)
                activeHandler.clientInfo.ProfilePictureUrl = targetUrl;
        }
        public void LoadProfileAsPreview(CustomerInfo info)
        {
            previewIdLbl.Text = info.ID;
            previewFullNameLbl.Text = $"{info.FirstName} {info.LastName}";
            pictureBox1.Image = DownloadFromURL(info.ProfilePictureUrl);
        }
        public void LoadProfile(CustomerInfo info)
        {
            previewIdLbl.Text = info.ID;
            previewFullNameLbl.Text = $"{info.FirstName} {info.LastName}";

            idTxt.Text = info.ID;
            firstnametxt.Text = info.FirstName;
            lastnametxt.Text = info.LastName;
            pfpActualImg.Image = DownloadFromURL(info.ProfilePictureUrl);
            pictureBox1.Image = pfpActualImg.Image;

            targetUrl = info.ProfilePictureUrl;
        }
        private void InfoEditor_Load(object sender, EventArgs e)
        {
            LoadTheme(ThemeRegistry.DefaultDark);

            ProfileRegistryInstance = ProfileRegistryClass.ReadFromFileStatic("profiles") ?? new ProfileRegistryClass();
            LoadProfileRegistryFromClassInstance(ProfileRegistryInstance);

            comboBox1.Items.Clear();
            foreach (string key in ProfileRegistry.Keys)
            {
                comboBox1.Items.Add(key);
                comboBox2.Items.Add(key);
            }

            comboBox1.SelectedItem = comboBox1.Items.Count > 0 ? comboBox1.Items[0] : null;
            comboBox2.SelectedItem = comboBox2.Items.Count > 0 ? comboBox2.Items[0] : null;

            if (activeHandler != null)
                CurrentProfile = activeHandler.clientInfo;
            else
                CurrentProfile = ProfileRegistry.TryGetValue(comboBox1.SelectedItem?.ToString()!, out CustomerInfo profile) ? profile : Default;

            LoadProfile(CurrentProfile);
            CheckForChanges(CurrentProfile);

        }

        private void idTxt_TextChanged(object sender, EventArgs e)
        {
            CheckForChanges(CurrentProfile);
        }

        private void fullNameTxt_TextChanged(object sender, EventArgs e)
        {
            CheckForChanges(CurrentProfile);
        }

        private void applyProfileBtn_Click(object sender, EventArgs e)
        {
            CurrentProfile.ID = previewIdLbl.Text;
            CurrentProfile.FirstName = firstnametxt.Text;
            CurrentProfile.LastName = lastnametxt.Text;
            if (activeHandler != null)
            {
                activeHandler.clientInfo = CurrentProfile;
                parent.UpdateInfo();
            }

            CheckForChanges(CurrentProfile);
        }

        private void lastnametxt_TextChanged(object sender, EventArgs e)
        {
            CheckForChanges(CurrentProfile);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (ProfileRegistryInstance.CustomProfiles.TryGetValue(comboBox2.Text, out CustomerInfo _))
                if (MessageBox.Show($"Are you sure you want to overwrite {comboBox2.Text}?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    ProfileRegistryInstance.CustomProfiles[comboBox2.Text] = CurrentProfile;
                else
                    ProfileRegistryInstance.CustomProfiles.Add(comboBox2.Text, CurrentProfile);
            ProfileRegistryInstance.SaveToFile("profiles");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            CurrentProfile = ProfileRegistry.TryGetValue(comboBox1.SelectedItem?.ToString()!, out CustomerInfo profile) ? profile : Default;
            LoadProfile(CurrentProfile);
            comboBox2.Text = comboBox1.SelectedItem?.ToString();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadProfileAsPreview(ProfileRegistry.TryGetValue(comboBox1.SelectedItem?.ToString()!, out CustomerInfo profile) ? profile : Default);
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
    }
}
