using System;
using System.Collections;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows;
using DevExpress.Mvvm.Native;
using TM.Digital.Cards;

namespace TM.Digital.Editor
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {

        public MainWindow(MainViewModel vm)
        {
            DataContext = vm;
            InitializeComponent();


        }



        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void EditorGrid_OnSelectionChanged(object sender, RoutedEventArgs e)
        {
            ((MainViewModel)DataContext).Packs.ForEach(p =>
                p.Content.Refresh.Execute(null));
        }
    }
}






