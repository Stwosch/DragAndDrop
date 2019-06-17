using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace DragAndDrop
{
    public static class ListBoxExtensions
    {
        public static ListBoxItem GetItemAt(this ListBox listBox, Point position)
        {
            var item = VisualTreeHelper.HitTest(listBox, position).VisualHit;
            while (item != null && !(item is ListBoxItem))
            {
                item = VisualTreeHelper.GetParent(item);
            }
            return item as ListBoxItem;
        }

        public static int IndexFromPoint(this ListBox listBox, Point position)
        {
            var item = listBox.GetItemAt(position);
            return listBox.Items.IndexOf(item);
        }
    }
}
