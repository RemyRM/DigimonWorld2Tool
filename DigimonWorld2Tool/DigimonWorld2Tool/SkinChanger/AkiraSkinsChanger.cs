using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using DigimonWorld2Tool.Views;

namespace DigimonWorld2Tool.SkinChanger
{
    class AkiraSkinsChanger
    {
        private const int AKIRA_PORTRAIT_OFFSET = 0x147B88C8;
        private const int AKIRA_PORTRAIT_LENGTH = 0xD60;

        private const int AKIRA_MODEL_TEXTURE_OFFSET = 0x18C52B78;
        private const int AKIRA_MODEL_TEXTURE_LENGTH = 0x80A0;

        public readonly string AkiraSkinsRelativeDirectory = "Resources\\AkiraSkins\\";

        internal void UpdateAkiraPortraitPreview(string skinPath, string skinName)
        {
            string fileName = $"Akira_{skinName}_Portrait_PREVIEW.png";
            string filePath = Path.Combine(skinPath, fileName);

            if (!File.Exists(filePath))
            {
                MessageBox.Show($"Couldn't find skin file:\n{filePath}.", "File not found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            SkinsWindow.Instance.AkiraPortraitPreviewPanel.BackgroundImage = Image.FromFile(filePath);
        }

        internal void UpdateAkiraModelPrevieW(string skinPath, string skinName)
        {
            string fileName = $"Akira_{skinName}_PREVIEW.png";
            string filePath = Path.Combine(skinPath, fileName);

            if (!File.Exists(filePath))
            {
                MessageBox.Show($"Couldn't find skin file:\n{filePath}.", "File not found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            SkinsWindow.Instance.AkiraSkinPreviewPanel.BackgroundImage = Image.FromFile(filePath);
        }

        internal void UpdateAkiraBinary(string skinPath, string skinName)
        {
            SkinsWindow.Instance.AkiraBackgroundWorker.ReportProgress(0, "Updating portrait binary (if set)...");
            UpdateAkiraPortraitInBinary(skinPath, skinName);

            SkinsWindow.Instance.AkiraBackgroundWorker.ReportProgress(1, "Updating texture binary...");
            UpdateAkiraModelInBinary(skinPath, skinName);

            SkinsWindow.Instance.AkiraBackgroundWorker.ReportProgress(2, "Saving changes to binary...");
            SkinsWindow.Instance.SaveEditedDW2Bin();

            SkinsWindow.Instance.AkiraBackgroundWorker.ReportProgress(3, "Saving completed, akira skin has been updated!");
        }

        private void UpdateAkiraPortraitInBinary(string skinPath, string skinName)
        {
            if (!SkinsWindow.Instance.ChangePortrait)
                return;

            string fileName = $"Akira_{skinName}_Portrait.BIN";
            string filePath = Path.Combine(skinPath, fileName);

            if (!File.Exists(filePath))
            {
                MessageBox.Show($"Couldn't find skin file:\n{filePath}.", "File not found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            byte[] akiraPortraitData = File.ReadAllBytes(filePath);
            Array.Copy(akiraPortraitData, 0, SkinsWindow.Instance.DW2Binary, AKIRA_PORTRAIT_OFFSET, AKIRA_PORTRAIT_LENGTH);
        }

        private void UpdateAkiraModelInBinary(string skinPath, string skinName)
        {
            string fileName = $"Akira_{skinName}.BIN";
            string filePath = Path.Combine(skinPath, fileName);

            if (!File.Exists(filePath))
            {
                MessageBox.Show($"Couldn't find skin file:\n{filePath}.", "File not found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            byte[] akiraPortraitData = File.ReadAllBytes(filePath);
            Array.Copy(akiraPortraitData, 0, SkinsWindow.Instance.DW2Binary, AKIRA_MODEL_TEXTURE_OFFSET, AKIRA_MODEL_TEXTURE_LENGTH);
        }
    }
}
