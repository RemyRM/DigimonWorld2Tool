using System;
using System.IO;
using System.Windows.Forms;
using System.Linq;

using DigimonWorld2Tool.Interfaces;
using DigimonWorld2Tool.Utility;
using DigimonWorld2Tool.FileFormats;
using System.Drawing;
using System.Diagnostics;
using System.ComponentModel;

namespace DigimonWorld2Tool.Views
{
    public partial class ModelWindow : UserControl, IHostWindow
    {
        private static ModelWindow instance;
        public static ModelWindow Instance { get { return instance; } }

        private string LoadedModelFilePath { get; set; }
        private ModelFile LoadedModelFile { get; set; }
        private string LoadedAnimationFilePath { get; set; }
        private AnimationFile LoadedAnimationFile { get; set; }

        public ModelWindow()
        {
            InitializeComponent();

            instance = this;

            ColourTheme.SetColourScheme(this.Controls);
            DigimonModelsComboBox.Items.AddRange(Settings.Settings.MODELDT0File.DigimonModelMappings.Select(digimon => digimon.GetDigimonName()).ToArray());
            DigimonModelsComboBox.SelectedIndex = 0;

            InitDataGridView();
        }

        private void InitDataGridView()
        {
            MODELDT0GridView.DefaultCellStyle.BackColor = (Color)Settings.Settings.BackgroundColour;
            MODELDT0GridView.DefaultCellStyle.ForeColor = (Color)Settings.Settings.TextColour;

            MODELDT0GridView.DefaultCellStyle.BackColor = (Color)Settings.Settings.BackgroundColour;
            MODELDT0GridView.DefaultCellStyle.ForeColor = (Color)Settings.Settings.TextColour;


            PopulateMODELDT0GridView();
        }

        public void OnWindowResizeEnded() { }

        private void LoadModelButton_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog fileDialog = new OpenFileDialog())
            {
                fileDialog.Filter = "Bin files (*.bin)|*.bin|All files (*.*)|*.*";
                fileDialog.FilterIndex = 0;
                fileDialog.RestoreDirectory = true;

                if (fileDialog.ShowDialog() == DialogResult.OK)
                {
                    LoadedModelFilePath = fileDialog.FileName;
                    byte[] loadedModelData = File.ReadAllBytes(LoadedModelFilePath);
                    LoadModelFile(loadedModelData);
                }
            }
        }

        private void LoadModelFile(byte[] loadedModelData)
        {
            LoadedModelFile = new ModelFile(loadedModelData);
        }

        private void ConvertToFbxButton_Click(object sender, EventArgs e)
        {
            LoadedModelFilePath = @"D:\Dev\C#\DigimonWorld2Tool\Documentation\MODELS\RAW_AGUM\M003AGUM.BIN";
            byte[] loadedModelData = File.ReadAllBytes(LoadedModelFilePath);
            LoadModelFile(loadedModelData);
            //if (LoadedModelFile == null)
            //    return;

            FBX export = new FBX(LoadedModelFile, "", @"D:\Dev\C#\DigimonWorld2Tool\Documentation\MODELS\FBX");
            File.WriteAllText(@"D:\Dev\C#\DigimonWorld2Tool\Documentation\MODELS\FBX\Test.fbx", export.FBXFileInfo.ToString());
        }

        private void LoadAnimationButton_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog fileDialog = new OpenFileDialog())
            {
                fileDialog.Filter = "Bin files (*.bin)|*.bin|All files (*.*)|*.*";
                fileDialog.FilterIndex = 0;
                fileDialog.RestoreDirectory = true;

                if (fileDialog.ShowDialog() == DialogResult.OK)
                {
                    LoadedAnimationFilePath = fileDialog.FileName;
                    byte[] loadedAnimationData = File.ReadAllBytes(LoadedAnimationFilePath);
                    LoadAnimationFile(loadedAnimationData);
                }
            }
        }

        private void LoadAnimationFile(byte[] loadedAnimationdata)
        {
            LoadedAnimationFile = new AnimationFile(loadedAnimationdata);
        }

        private void DigimonModelsComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string baseDir = @"D:\Program Files (x86)\EmulatorsAndRomhacking\DigimonWorld2\AAA\4.AAA\MODEL\";
        }

        private void PopulateMODELDT0GridView()
        {
            MODELDT0GridView.Columns.Add(new DataGridViewButtonColumn());

            BindingList<DigimonModelFilesMapping> binding = new BindingList<DigimonModelFilesMapping>();
            foreach (var item in Settings.Settings.MODELDT0File.DigimonModelMappings)
            {
                binding.Add(item);
            }
            MODELDT0GridView.DataSource = binding;
        }


        private void MODELDT0GridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            Debug.WriteLine("Editing cell");
        }

        private void dataGridView1_CellValueChanged_1(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView cell = (DataGridView)sender;
            var a = Settings.Settings.MODELDT0File.DigimonModelMappings;
            Debug.WriteLine("Editing cell");
        }

        private void MODELDT0GridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {

            if (e.ColumnIndex == 0)
                e.Value = Settings.Settings.MODELDT0File.DigimonModelMappings[e.RowIndex].GetDigimonName();

            if (Settings.Settings.ValueTextFormat == "X2")
                e.Value = $"{e.Value:X4}";
        }

        private void MODELDT0GridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView view = (DataGridView)sender;

            if (view.Columns[e.ColumnIndex] is DataGridViewButtonColumn /*&& e.RowIndex > 0*/)
            {
                Debug.WriteLine($"ID: {view.Rows[e.RowIndex].Cells[2].Value}");
            }
        }

        private void SelectMainModelDirButton_Click(object sender, EventArgs e)
        {
            FolderPicker folderPicker = new FolderPicker();
            folderPicker.InputPath = @"C:\";
            if(folderPicker.ShowDialog(this.Handle) == true)
            {
                string selectedPath = folderPicker.ResultPath;
            }
        }
    }
}
