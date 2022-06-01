using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;
using DigimonWorld2Tool.Views;

namespace DigimonWorld2Tool.SkinChanger
{
    class DigiBeetlePresetFileProcessor
    {
        public static readonly string DigiBeetlePresetsCfgRelativeDirectory = "Resources\\DigiBeetleSkins\\";
        public List<DigiBeetleSKinPreset> DigiBeetleSkinPresets { get; private set; } = new List<DigiBeetleSKinPreset>();

        public void ReadPresetCfgFile()
        {
            string filePath = Path.Combine(SkinsWindow.Instance.BaseDirectory, DigiBeetlePresetsCfgRelativeDirectory, "Presets.cfg");
            if (!File.Exists(filePath))
            {
                MessageBox.Show($"Presets.cfg could not be found in the expected location!\nPlease make sure this file is not deleted and the name has not been changed.\nExpected location: {filePath}",
                                 "Presets.cfg not found",
                                 MessageBoxButtons.OK,
                                 MessageBoxIcon.Error);
                return;
            }

            string[] lines = File.ReadAllLines(filePath);
            foreach (var item in lines)
            {
                if (item.StartsWith("#"))
                    continue;


                var preset = new DigiBeetleSKinPreset(item, out bool hasErrors);
                if (hasErrors)
                    continue;

                DigiBeetleSkinPresets.Add(preset);
            }
        }

        public string[] GetDigiBeetlePresetNames()
        {
            string[] presetNames = new string[DigiBeetleSkinPresets.Count];
            for (int i = 0; i < presetNames.Length; i++)
                presetNames[i] = DigiBeetleSkinPresets[i].PresetName;
            return presetNames;
        }
    }

    class DigiBeetleSKinPreset
    {
        public string PresetName { get; private set; }
        public string SteelBodySkinName { get; private set; }
        public string TitaniumBodySkinName { get; private set; }
        public string AdamanBodySkinName { get; private set; }

        public DigiBeetleSKinPreset(string data, out bool hasErrors)
        {
            if (!data.Contains(':') || !data.Contains(","))
            {
                MessageBox.Show($"Config file has an illegal entry that does not contain a \":\" or \",\" and doesn't start with a #.\n" +
                                $"Please check your config file at {Path.Combine(SkinsWindow.Instance.BaseDirectory, DigiBeetlePresetFileProcessor.DigiBeetlePresetsCfgRelativeDirectory, "Presets.cfg")}\n" +
                                $"Preset will not be loaded.",
                                 "Error in Presets.cfg",
                                 MessageBoxButtons.OK,
                                 MessageBoxIcon.Error);
                hasErrors = true;
                return;
            }

            PresetName = data.Substring(0, data.IndexOf(':'));
            data = data.Remove(0, data.IndexOf(':') + 1);
            string[] skins = data.Split(',');
            if (skins.Length != 3)
            {
                MessageBox.Show($"Config file has an illegal entry that has more or less than 3 skins assigned to it." +
                                $"Please check your config file at {Path.Combine(SkinsWindow.Instance.BaseDirectory, DigiBeetlePresetFileProcessor.DigiBeetlePresetsCfgRelativeDirectory, "Presets.cfg")}\n" +
                                $"Preset will not be loaded.",
                                 "Error in Presets.cfg",
                                 MessageBoxButtons.OK,
                                 MessageBoxIcon.Error);
                hasErrors = true;
                return;
            }
            SteelBodySkinName = skins[0];
            TitaniumBodySkinName = skins[1];
            AdamanBodySkinName = skins[2];

            hasErrors = false;
        }
    }
}