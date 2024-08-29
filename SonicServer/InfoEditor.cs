using System.Net;
using System.Net.Sockets;
using System.Security.Permissions;
using System.Windows.Forms;
using Newtonsoft.Json;
using SonicServer;
using SonicServer.JsonClasses;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

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

        public Image? DownloadFromURL(string url)
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

            if(activeHandler != null)
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
                if(MessageBox.Show($"Are you sure you want to overwrite {comboBox2.Text}?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes)
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
    }
}
