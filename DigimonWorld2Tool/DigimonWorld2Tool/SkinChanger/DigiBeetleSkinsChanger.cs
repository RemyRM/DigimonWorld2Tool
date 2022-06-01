using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using DigimonWorld2Tool.Views;

namespace DigimonWorld2Tool.SkinChanger
{
    class DigiBeetleSkinsChanger
    {
        private const int DIGIBEETLE_TEXTURE_LENGTH = 0x7770;
        private const int DIGITBEETLE_TYRE_COUNT = 6;
        private readonly int[] STEEL_BODY_OFFSETS = new int[6] { 0x18C71268, 0x18C878B8, 0x18C9DF08, 0x18C5AC18, 0x18CBBCC8, 0x18CD2318 };
        private readonly int[] TITANIUM_BODY_OFFSETS = new int[6] { 0x18C789D8, 0x18C8F028, 0x18C62388, 0x18CACDE8, 0x18CC3438, 0x18CD9A88 };
        private readonly int[] ADAMANT_BODY_OFFSETS = new int[6] { 0x18C80148, 0x18C96798, 0x18CA5678, 0x18CB4558, 0x18CCABA8, 0x18C69AF8 };

        public readonly string SteelBodySkinsRelativeDirectory = "Resources\\DigiBeetleSkins\\Steel\\";
        public readonly string TitaniumBodySkinsRelativeDirectory = "Resources\\DigiBeetleSkins\\Titanium\\";
        public readonly string AdamantBodySkinsRelativeDirectory = "Resources\\DigiBeetleSkins\\Adamant\\";
        public readonly string SkinPreviewErrorImage = @"Resources\Images\ErrorPreview.png";

        public void UpdateSteelBodyPreview(string skinPath, string skinName)
        {
            string fileName = $"Steel_{skinName}_PREVIEW.png";
            string filePath = Path.Combine(skinPath, fileName);

            if (!File.Exists(filePath))
            {
                MessageBox.Show($"Failed to load steel skin preview. \nPreview file not loaded\nCouldn't find preview file:\n{filePath}.", "File not found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                filePath = SkinPreviewErrorImage;
            }
            SkinsWindow.Instance.DigiBeetleSteelPanel.BackgroundImage = Image.FromFile(filePath);
        }

        public void UpdateTitaniumBodyPreview(string skinPath, string skinName)
        {
            string fileName = $"Titanium_{skinName}_PREVIEW.png";
            string filePath = Path.Combine(skinPath, fileName);

            if (!File.Exists(filePath))
            {
                MessageBox.Show($"Failed to load titanium skin preview. \nPreview file not loaded\nCouldn't find preview file:\n{filePath}.", "File not found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                filePath = SkinPreviewErrorImage;
            }
            SkinsWindow.Instance.DigiBeetleTitaniumPanel.BackgroundImage = Image.FromFile(filePath);
        }

        public void UpdateAdamantBodyPreview(string skinPath, string skinName)
        {
            string fileName = $"Adamant_{skinName}_PREVIEW.png";
            string filePath = Path.Combine(skinPath, fileName);

            if (!File.Exists(filePath))
            {
                MessageBox.Show($"Failed to load adamant skin preview. \nPreview file not loaded\nCouldn't find preview file:\n{filePath}.", "File not found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                filePath = SkinPreviewErrorImage;
            }
            SkinsWindow.Instance.DigiBeetleAdamantPanel.BackgroundImage = Image.FromFile(filePath);
        }

        public void UpdateDigiBeetleBinary(string steelSkinName, string titaniumSkinName, string adamantSkinName)
        {
            SkinsWindow.Instance.DigiBeetleBackgroundWorker.ReportProgress(0, "Updating steel body binary...");
            UpdateSteelBodyBinary(steelSkinName);

            SkinsWindow.Instance.DigiBeetleBackgroundWorker.ReportProgress(1, "Updating titanium body binary...");
            UpdateTitaniumBodyBinary(titaniumSkinName);

            SkinsWindow.Instance.DigiBeetleBackgroundWorker.ReportProgress(2, "Updating adamant body binary...");
            UpdateAdamantBodyBinary(adamantSkinName);

            SkinsWindow.Instance.DigiBeetleBackgroundWorker.ReportProgress(3, "Saving changes to binary...");
            SkinsWindow.Instance.SaveEditedDW2Bin();

            SkinsWindow.Instance.DigiBeetleBackgroundWorker.ReportProgress(4, "Saving completed, Digibeetle skins have been updated!");
        }

        private void UpdateSteelBodyBinary(string steelSkinName)
        {
            for (int i = 0; i < DIGITBEETLE_TYRE_COUNT; i++)
            {
                string skinName = $"Steel_{steelSkinName}_{i}.BIN";
                string filePath = Path.Combine(SkinsWindow.Instance.BaseDirectory, SteelBodySkinsRelativeDirectory, steelSkinName, skinName);
                if (!File.Exists(filePath))
                    continue;

                byte[] textureData = File.ReadAllBytes(filePath);
                Array.Copy(textureData, 0, SkinsWindow.Instance.DW2Binary, STEEL_BODY_OFFSETS[i], DIGIBEETLE_TEXTURE_LENGTH);
            }
        }

        private void UpdateTitaniumBodyBinary(string titaniumSkinName)
        {
            for (int i = 0; i < DIGITBEETLE_TYRE_COUNT; i++)
            {
                string skinName = $"Titanium_{titaniumSkinName}_{i}.BIN";
                string filePath = Path.Combine(SkinsWindow.Instance.BaseDirectory, TitaniumBodySkinsRelativeDirectory, titaniumSkinName, skinName);
                if (!File.Exists(filePath))
                    continue;

                byte[] textureData = File.ReadAllBytes(filePath);
                Array.Copy(textureData, 0, SkinsWindow.Instance.DW2Binary, TITANIUM_BODY_OFFSETS[i], DIGIBEETLE_TEXTURE_LENGTH);
            }
        }

        private void UpdateAdamantBodyBinary(string adamantSkinName)
        {
            for (int i = 0; i < DIGITBEETLE_TYRE_COUNT; i++)
            {
                string skinName = $"Adamant_{adamantSkinName }_{i}.BIN";
                string filePath = Path.Combine(SkinsWindow.Instance.BaseDirectory, AdamantBodySkinsRelativeDirectory, adamantSkinName, skinName);
                if (!File.Exists(filePath))
                    continue;

                byte[] textureData = File.ReadAllBytes(filePath);
                Array.Copy(textureData, 0, SkinsWindow.Instance.DW2Binary, ADAMANT_BODY_OFFSETS[i], DIGIBEETLE_TEXTURE_LENGTH);
            }
        }
    }
}
