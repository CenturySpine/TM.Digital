using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using Newtonsoft.Json;
using TM.Digital.Model;
using TM.Digital.Model.Cards;
using TM.Digital.Model.Corporations;
using TM.Digital.Ui.Resources.ViewModelCore;

namespace TM.Digital.Editor
{
    public class MainViewModel : NotifierBase
    {
        private const string packs = "packs.json";

        private ObservableCollection<PackPresenter> _packs;

        public ObservableCollection<PackPresenter> Packs
        {
            get => _packs;
            set
            {
                _packs = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand SaveCommand { get; set; }

        //SelectedObject = all;


        private void SaveObject_OnClick(object sender, RoutedEventArgs e)
        {

        }
        public MainViewModel()
        {
            SaveCommand = new RelayCommand(ExecuteSave);
            CardReferencesHolder all;

            DirectoryInfo di = new DirectoryInfo(Environment.CurrentDirectory);
            var files = di.GetFiles("*_packs.json").OrderBy(fi => fi.CreationTime).Reverse();
            var TheOne = files.FirstOrDefault();


            if (TheOne != null && File.Exists(TheOne.FullName))
            {
                all = JsonConvert.DeserializeObject<CardReferencesHolder>(File.ReadAllText(TheOne.FullName));
            }
            else
            {
                all = new CardReferencesHolder()
                {
                    Packs = new List<ExtensionPack>()
                    {
                        new ExtensionPack()
                        {
                            Name = Extensions.Base,
                            Corporations = new List<Corporation>(),
                            Patents = new List<Patent>(),
                            Preludes = new List<Prelude>()

                        },
                        new ExtensionPack()
                        {
                            Name = Extensions.Prelude,
                            Corporations = new List<Corporation>(),
                            Patents = new List<Patent>(),
                            Preludes = new List<Prelude>()

                        },
                        new ExtensionPack()
                        {
                            Name = Extensions.Colonies,
                            Corporations = new List<Corporation>(),
                            Patents = new List<Patent>(),
                            Preludes = new List<Prelude>()

                        },
                        new ExtensionPack()
                        {
                            Name = Extensions.Turnmoil,
                            Corporations = new List<Corporation>(),
                            Patents = new List<Patent>(),
                            Preludes = new List<Prelude>()

                        }
                    }
                };
            }

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

                streamWriter.Write(JsonConvert.SerializeObject(h));
            }
        }
    }
}