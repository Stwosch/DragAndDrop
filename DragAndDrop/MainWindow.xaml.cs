using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DragAndDrop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Point? startedPosition = null;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void ListBox_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            startedPosition = e.GetPosition(this);
        }

        private void ListBox_DragEnter(object sender, DragEventArgs e)
        {
            e.Effects = e.KeyStates.HasFlag(DragDropKeyStates.ControlKey) ? DragDropEffects.Copy : DragDropEffects.Move;
        }

        private void ListBox_Drop(object sender, DragEventArgs e)
        {
            var listBoxSender = sender as ListBox;
            var index = listBoxSender.IndexFromPoint(e.GetPosition(listBoxSender));
            var movedItemsLabels = (e.Data.GetData(DataFormats.StringFormat) as string).TrimEnd('\n').Split('\n'); 
            foreach (var movedItemLabel in movedItemsLabels)
            {
                var movedItem = new ListBoxItem { Content = movedItemLabel };
                if (index < 0)
                {
                    listBoxSender.Items.Add(movedItem);
                }
                else
                {
                    listBoxSender.Items.Insert(index, movedItem);
                }
            }
        }

        private void ListBox_MouseMove(object sender, MouseEventArgs e)
        {
            var currentPosition = e.GetPosition(this);
            if (e.LeftButton == MouseButtonState.Released || !startedPosition.HasValue)
            {
                return;
            }
            var shift = currentPosition - startedPosition.Value;
            if (Math.Abs(shift.X) > SystemParameters.MinimumHorizontalDragDistance ||
                Math.Abs(shift.Y) > SystemParameters.MinimumVerticalDragDistance)
            {
                var listBoxSender = sender as ListBox;
                if (listBoxSender.SelectedItem is ListBoxItem movedItem)
                {
                    var movedItemsLabels = string.Join("\n", listBoxSender.SelectedItems);
                    var dataObject = new DataObject(DataFormats.StringFormat, movedItemsLabels);
                    DragDrop.DoDragDrop(listBoxSender, dataObject, DragDropEffects.Copy | DragDropEffects.Move);
                    startedPosition = null;
                    if (!Keyboard.Modifiers.HasFlag(ModifierKeys.Control))
                    {
                        for (var i = listBoxSender.SelectedItems.Count - 1; i >= 0; i--)
                        {
                            listBoxSender.Items.Remove(listBoxSender.SelectedItems[i]);
                        }
                    }
                }
            }
        }
    }
}
