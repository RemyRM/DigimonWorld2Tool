using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using DigimonWorld2Tool.Interfaces;
using DigimonWorld2Tool.Utility;
using DigimonWorld2Tool.SkinChanger;

namespace DigimonWorld2Tool.Views
{
    public partial class SkinsWindow : UserControl, IHostWindow
    {
        private static SkinsWindow instance;
        public static SkinsWindow Instance { get { return instance; } }

        private AkiraSkinsChanger AkiraSkinChanger { get; set; }
        private DigiBeetleSkinsChanger DigiBeetleSkinChanger { get; set; }
        private DigiBeetlePresetFileProcessor DigiBeetlePresetCfgFileProcessor { get; set; }

        public string BaseDirectory { get { return AppDomain.CurrentDomain.BaseDirectory; } }

        private string IsoFilePath { get; set; }
        private string AkiraSkinName { get; set; }
        private string SteelSkinName { get; set; }
        private string TitaniumSkinName { get; set; }
        private string AdamantSkinName { get; set; }

        public byte[] DW2Binary { get; private set; }

        public bool ChangePortrait { get; private set; } = true;

        public SkinsWindow()
        {
            InitializeComponent();

            instance = this;
            AkiraSkinChanger = new AkiraSkinsChanger();
            DigiBeetleSkinChanger = new DigiBeetleSkinsChanger();
            DigiBeetlePresetCfgFileProcessor = new DigiBeetlePresetFileProcessor();
            ColourTheme.SetColourScheme(this.Controls);
            ColourTheme.SetColourScheme(this.MainTabControl.Controls);
            //ColourTheme.SetColourScheme(this.AikiraTab.Controls);
            //ColourTheme.SetColourScheme(this.DigiBeetleTab.Controls);

            PopulateComboBoxes();
        }

        private void PopulateComboBoxes()
        {
            //Populate the Akira skins dropdown based on the direcotry names in the Akira skins folder, where every folder is a skin
            string akiraSkinsPath = Path.Combine(BaseDirectory, AkiraSkinChanger.AkiraSkinsRelativeDirectory);
            if (Directory.Exists(akiraSkinsPath))
            {
                string[] akiraSkins = Directory.GetDirectories(akiraSkinsPath);
                for (int i = 0; i < akiraSkins.Length; i++)
                    akiraSkins[i] = akiraSkins[i].Replace(akiraSkinsPath, "");

                AkiraStyleComboBox.Items.AddRange(akiraSkins);
                AkiraStyleComboBox.SelectedIndex = AkiraStyleComboBox.Items.IndexOf("Default");
            }

            //Populate the Steel digibeetle skins dropdown based on the direcotry names in the steel digibeetle skins folder, where every folder is a skin
            string digibeetleSteelSkinsPath = Path.Combine(BaseDirectory, DigiBeetleSkinChanger.SteelBodySkinsRelativeDirectory);
            if (Directory.Exists(digibeetleSteelSkinsPath))
            {
                string[] digiBeetleSteelSkins = Directory.GetDirectories(digibeetleSteelSkinsPath);
                for (int i = 0; i < digiBeetleSteelSkins.Length; i++)
                    digiBeetleSteelSkins[i] = digiBeetleSteelSkins[i].Replace(digibeetleSteelSkinsPath, "");

                DigiBeetleSteelBodyComboBox.Items.AddRange(digiBeetleSteelSkins);
                DigiBeetleSteelBodyComboBox.SelectedIndex = 0;
            }

            //Populate the titanium digibeetle skins dropdown based on the direcotry names in the titanium digibeetle skins folder, where every folder is a skin
            string digibeetleTitaniumSkinsPath = Path.Combine(BaseDirectory, DigiBeetleSkinChanger.TitaniumBodySkinsRelativeDirectory);
            if (Directory.Exists(digibeetleTitaniumSkinsPath))
            {
                string[] digiBeetleTitaniumSkins = Directory.GetDirectories(digibeetleTitaniumSkinsPath);
                for (int i = 0; i < digiBeetleTitaniumSkins.Length; i++)
                    digiBeetleTitaniumSkins[i] = digiBeetleTitaniumSkins[i].Replace(digibeetleTitaniumSkinsPath, "");

                DigiBeetleTitaniumBodyComboBox.Items.AddRange(digiBeetleTitaniumSkins);
                DigiBeetleTitaniumBodyComboBox.SelectedIndex = 0;
            }

            //Populate the adamant digibeetle skins dropdown based on the direcotry names in the adamant digibeetle skins folder, where every folder is a skin
            string digibeetleAdamantSkinsPath = Path.Combine(BaseDirectory, DigiBeetleSkinChanger.AdamantBodySkinsRelativeDirectory);
            if (Directory.Exists(digibeetleAdamantSkinsPath))
            {
                string[] digiBeetleAdamanSkins = Directory.GetDirectories(digibeetleAdamantSkinsPath);
                for (int i = 0; i < digiBeetleAdamanSkins.Length; i++)
                    digiBeetleAdamanSkins[i] = digiBeetleAdamanSkins[i].Replace(digibeetleAdamantSkinsPath, "");

                DigiBeetleAdamantBodyComboBox.Items.AddRange(digiBeetleAdamanSkins);
                DigiBeetleAdamantBodyComboBox.SelectedIndex = 0;
            }

            DigiBeetlePresetCfgFileProcessor.ReadPresetCfgFile();
            DigiBeetlePresetComboBox.Items.AddRange(DigiBeetlePresetCfgFileProcessor.GetDigiBeetlePresetNames());
            DigiBeetlePresetComboBox.SelectedIndex = DigiBeetlePresetComboBox.Items.IndexOf("Default");
        }

        #region ButtonClicks

        /// <summary>
        /// Opens a file dialogue from which the user can select the binary to load
        /// Start the background worker that loads the binary and reports its progress to the progress bar
        /// </summary>
        private void SelectISOButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Bin files (*.bin)|*.bin|All files (*.*)|*.*";
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                IsoFilePath = openFileDialog.FileName;
                SelectedISOPath.Text = IsoFilePath;
                SetupLoadingBar(1, 1);
                BinaryLoaderBackgroundWorker.ReportProgress(0, "Loading binary...");
                BinaryLoaderBackgroundWorker.RunWorkerAsync();
            }
        }

        /// <summary>
        /// Get the full path to the selected skin and start a background worker to save it to the selected binary
        /// </summary>
        private void SaveAkiraSkinButton_Click(object sender, EventArgs e)
        {
            if (DW2Binary == null || DW2Binary.Length == 0)
            {
                MessageBox.Show("No Digimon World 2 binary was loaded, please select your complete Digimon World2.bin before saving.", "Select binary", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string skinPath = Path.Combine(BaseDirectory, AkiraSkinChanger.AkiraSkinsRelativeDirectory, AkiraSkinName);

            SetupLoadingBar(3, 1);
            AkiraBackgroundWorker.RunWorkerAsync(skinPath);
        }

        /// <summary>
        /// Start a background worker to save the selected digibeetle skins to the selected binary
        /// /// </summary>
        private void SaveDigiBeetleSkinButton_Click(object sender, EventArgs e)
        {
            SkinsWindow.Instance.SetupLoadingBar(4, 1);

            if (DW2Binary == null || DW2Binary.Length == 0)
            {
                MessageBox.Show("No Digimon World 2 binary was loaded, please select your complete Digimon World2.bin before saving.", "Select binary", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DigiBeetleBackgroundWorker.RunWorkerAsync();
        }
        #endregion

        #region DW2Binary
        /// <summary>
        /// Check if the <see cref="IsoFilePath"/> exists and read all bytes from it.
        /// Creates a backup of the read file if none already exists.
        /// </summary>
        /// <returns>The bytes in <see cref="IsoFilePath"/></returns>
        private byte[] GetDW2BinaryFromISOPath()
        {
            if (!File.Exists(IsoFilePath))
            {
                MessageBox.Show($"No file found at {IsoFilePath}", "File not found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }

            CreateBackupOfDW2Bin();

            byte[] data = File.ReadAllBytes(IsoFilePath);
            BinaryLoaderBackgroundWorker.ReportProgress(1, "Completed loading binary!");
            return data;
        }

        /// <summary>
        /// Create a backup of the file found at <see cref="IsoFilePath"/> and append it with "_backup"
        /// </summary>
        private void CreateBackupOfDW2Bin()
        {
            if (!File.Exists(IsoFilePath + "_backup"))
                File.Copy(IsoFilePath, IsoFilePath + "_backup");
        }

        /// <summary>
        /// Write all bytes in <see cref="DW2Binary"/> back to the file given at <see cref="IsoFilePath"/>
        /// </summary>
        public void SaveEditedDW2Bin()
        {
            try
            {
                File.WriteAllBytes(IsoFilePath, DW2Binary);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }
        #endregion

        private void ChangePortraitCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            ChangePortrait = ((CheckBox)sender).Checked;
        }

        private void AkiraStyleComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox akiraBox = (ComboBox)sender;
            AkiraSkinName = akiraBox.Text;
            string skinPath = Path.Combine(BaseDirectory, AkiraSkinChanger.AkiraSkinsRelativeDirectory, AkiraSkinName);

            AkiraSkinChanger.UpdateAkiraPortraitPreview(skinPath, AkiraSkinName);
            AkiraSkinChanger.UpdateAkiraModelPrevieW(skinPath, AkiraSkinName);
        }

        private void DigiBeetleSteelBodyComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox steelBox = (ComboBox)sender;
            SteelSkinName = steelBox.Text;
            string skinPath = Path.Combine(BaseDirectory, DigiBeetleSkinChanger.SteelBodySkinsRelativeDirectory, SteelSkinName);

            DigiBeetleSkinChanger.UpdateSteelBodyPreview(skinPath, SteelSkinName);
        }

        private void DigiBeetleTitaniumBodyComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox titaniumBox = (ComboBox)sender;
            TitaniumSkinName = titaniumBox.Text;
            string skinPath = Path.Combine(BaseDirectory, DigiBeetleSkinChanger.TitaniumBodySkinsRelativeDirectory, TitaniumSkinName);

            DigiBeetleSkinChanger.UpdateTitaniumBodyPreview(skinPath, TitaniumSkinName);
        }

        private void DigiBeetleAdamantBodyComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox adamantBox = (ComboBox)sender;
            AdamantSkinName = adamantBox.Text;
            string skinPath = Path.Combine(BaseDirectory, DigiBeetleSkinChanger.AdamantBodySkinsRelativeDirectory, AdamantSkinName);

            DigiBeetleSkinChanger.UpdateAdamantBodyPreview(skinPath, AdamantSkinName);
        }

        private void DigiBeetlePresetComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox presetBox = (ComboBox)sender;
            string selectedPresetName = presetBox.Text;

            if (selectedPresetName == "None")
                return;

            DigiBeetleSKinPreset selectedPreset = DigiBeetlePresetCfgFileProcessor.DigiBeetleSkinPresets.FirstOrDefault(o => o.PresetName == selectedPresetName);

            if (selectedPreset == null)
            {
                MessageBox.Show($"Error loading preset.\nPreset {selectedPresetName} could not be found.", "Error loading preset", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            SteelSkinName = selectedPreset.SteelBodySkinName;
            DigiBeetleSteelBodyComboBox.SelectedIndex = DigiBeetleSteelBodyComboBox.Items.IndexOf(SteelSkinName);

            TitaniumSkinName = selectedPreset.TitaniumBodySkinName;
            DigiBeetleTitaniumBodyComboBox.SelectedIndex = DigiBeetleTitaniumBodyComboBox.Items.IndexOf(TitaniumSkinName);

            AdamantSkinName = selectedPreset.AdamanBodySkinName;
            DigiBeetleAdamantBodyComboBox.SelectedIndex = DigiBeetleAdamantBodyComboBox.Items.IndexOf(AdamantSkinName);
        }

        public void SetupLoadingBar(int maxStep, int stepIncrement)
        {
            pbar.Minimum = 0;
            pbar.Maximum = maxStep;
            pbar.Value = 0;
            pbar.Step = stepIncrement;
            pbar.Visible = true;
        }

        #region BackgroundWorkers
        #region DigiBeetleWorker
        private void DigiBeetleBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            DigiBeetleSkinChanger.UpdateDigiBeetleBinary(SteelSkinName, TitaniumSkinName, AdamantSkinName);
        }

        private void DigiBeetleBackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            pbar.Value = e.ProgressPercentage;
            pbarLabel.Text = e.UserState.ToString();
        }

        private void DigiBeetleBackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e) { }
        #endregion

        #region AkiraWorker
        private void AkiraBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            string skinPath = e.Argument.ToString();
            AkiraSkinChanger.UpdateAkiraBinary(skinPath, AkiraSkinName);
        }

        private void AkiraBackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            pbar.Value = e.ProgressPercentage;
            pbarLabel.Text = e.UserState.ToString();
        }

        private void AkiraBackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e) { }
        #endregion

        #region BinaryLoaderWorker
        private void BinaryLoaderBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            DW2Binary = GetDW2BinaryFromISOPath();
        }

        private void BinaryLoaderBackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            pbar.Value = e.ProgressPercentage;
            pbarLabel.Text = e.UserState.ToString();
        }

        private void BinaryLoaderBackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            SaveAkiraSkinButton.Enabled = true;
            SaveDigiBeetleSkinButton.Enabled = true;
        }

        #endregion

        #endregion

        public void OnWindowResizeEnded()
        {
            //throw new NotImplementedException();
        }
    }
}
