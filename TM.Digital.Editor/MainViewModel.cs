using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using TM.Digital.Model;
using TM.Digital.Model.Cards;
using TM.Digital.Model.Corporations;
using TM.Digital.Services.Common;
using TM.Digital.Ui.Resources.ViewModelCore;

namespace TM.Digital.Editor
{
    public class MainViewModel : NotifierBase
    {
        private const string packs = "packs.json";

        private ObservableCollection<PackPresenter> _packs;
        private PackPresenter _destinationPack;

        public ObservableCollection<PackPresenter> Packs
        {
            get => _packs;
            set
            {
                _packs = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand MoveToCommand { get; set; }

        public PackPresenter DestinationPack
        {
            get => _destinationPack;
            set { _destinationPack = value; OnPropertyChanged(); }
        }

        public RelayCommand SaveCommand { get; set; }

        //SelectedObject = all;

        private void SaveObject_OnClick(object sender, RoutedEventArgs e)
        {
        }

        private void ExecuteMoveTo(object obj)
        {
            if (obj != null && DestinationPack != null)
            {
                if (obj is Patent patent)
                {
                    var currentPack = Packs.FirstOrDefault(r => r.Content.Patents.Contains(patent));
                    if (currentPack != null)
                    {
                        currentPack.Content.Patents.Remove(patent);
                        DestinationPack.Content.Patents.Add(patent);
                    }
                }
                if (obj is Corporation corporation)
                {
                    var currentPack = Packs.FirstOrDefault(r => r.Content.Corporations.Contains(corporation));
                    if (currentPack != null)
                    {
                        currentPack.Content.Corporations.Remove(corporation);
                        DestinationPack.Content.Corporations.Add(corporation);
                    }
                }
                if (obj is Prelude prelude)
                {
                    var currentPack = Packs.FirstOrDefault(r => r.Content.Preludes.Contains(prelude));
                    if (currentPack != null)
                    {
                        currentPack.Content.Preludes.Remove(prelude);
                        DestinationPack.Content.Preludes.Add(prelude);
                    }
                }
            }
        }

        public MainViewModel()
        {
            MoveToCommand = new RelayCommand(ExecuteMoveTo);

            SaveCommand = new RelayCommand(ExecuteSave);
            
        }

        public async Task Initialize()
        {
            CardReferencesHolder all;
            string directory = Environment.CurrentDirectory;

            all = await PackSerializer.GtPacks(directory);
            Packs = new ObservableCollection<PackPresenter>(all.Packs.Select(r => new PackPresenter()
            {
                Content = new PackViewModel(r),
                Name = r.Name.ToString(),
            }));
        }

        private void ExecuteSave(object obj)
        {
            using (var streamWriter = new StreamWriter($"{DateTime.Now.Ticks}_{packs}", false))
            {
                var h = new CardReferencesHolder()
                {
                    Packs = Packs.Select(r => new ExtensionPack()
                    {
                        Name = (Extensions)Enum.Parse(typeof(Extensions), r.Name),
                        Patents = r.Content.Patents.ToList(),
                        Corporations = r.Content.Corporations.ToList(),
                        Preludes = r.Content.Preludes.ToList(),
                    }).ToList()
                };

                streamWriter.Write(System.Text.Json.JsonSerializer.Serialize(h));
            }
        }
    }
}