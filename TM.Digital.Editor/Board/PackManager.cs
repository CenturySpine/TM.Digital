using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using TM.Digital.Model;
using TM.Digital.Services.Common;

namespace TM.Digital.Editor.Board
{
    public class PackManager
    {
        private const string packs = "packs.json";

        private EditorConfig _config;

        private string _packageLocation;

        private string _savePackageLocation;

        private CardReferencesHolder _all;

        public CardReferencesHolder AllPack => _all;

        public string PackageLocation
        {
            get => _packageLocation;
            set
            {
                _packageLocation = value;
                _config.PackageLocation = value;
                //OnPropertyChanged();
                SaveConfig();
            }
        }
        public string SavePackageLocation
        {
            get => _savePackageLocation;
            set
            {
                _savePackageLocation = value;
                _config.SavePackageLocation = value;
                //OnPropertyChanged();
                SaveConfig();
            }
        }

        public async Task Initialize()
        {
            try
            {
                if (File.Exists("editorConfig.json"))
                {
                    _config = System.Text.Json.JsonSerializer.Deserialize<EditorConfig>(
                        await File.ReadAllTextAsync("editorConfig.json"));
                }
                else
                {
                    _config = new EditorConfig { PackageLocation = Environment.CurrentDirectory };
                    SaveConfig();
                }

                _packageLocation = _config.PackageLocation;
                _savePackageLocation = _config.SavePackageLocation;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Load Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        protected internal void Save()
        {
            if (string.IsNullOrEmpty(SavePackageLocation))
            {
                MessageBox.Show("Invalid save location");
                return;
            }
            try
            {
                string path = $"{SavePackageLocation}\\{DateTime.Now.Ticks}_{packs}";
                using (var streamWriter = new StreamWriter(path, false))
                {
                    streamWriter.Write(System.Text.Json.JsonSerializer.Serialize(_all));
                }
                MessageBox.Show($"File saved : {path}", "Save successful", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Save Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        protected internal async Task Load()
        {
            string directory = _config.PackageLocation;

            _all = await PackSerializer.GetPacks(directory);
        }

        protected void SaveConfig()
        {
            var json = System.Text.Json.JsonSerializer.Serialize(_config);
            using (var sw = new StreamWriter("editorConfig.json", false))
            {
                sw.Write(json);
            }
        }
    }
}