using System;
using System.IO;
using System.Windows.Forms;

using DigimonWorld2Tool.Interfaces;
using DigimonWorld2Tool.Utility;
using DigimonWorld2Tool.FileFormats;

namespace DigimonWorld2Tool.Views
{
    public partial class ModelWindow : UserControl, IHostWindow
    {
        private static ModelWindow instance;
        public static ModelWindow Instance { get { return instance; } }

        private string LoadedModelFilePath { get; set; }
        private ModelFile LoadedModelFile { get; set; }

        public ModelWindow()
        {
            InitializeComponent();

            instance = this;

            ColourTheme.SetColourScheme(this.Controls);
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
    }
}
