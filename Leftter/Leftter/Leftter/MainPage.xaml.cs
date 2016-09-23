using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Leftter
{
    public partial class MainPage : ContentPage
    {
        // 動的にList<ListItem>を追加するのに使う
        ObservableCollection<ListItem> listItems = new ObservableCollection<ListItem>();

        public MainPage()
        {
            InitializeComponent();

            BindingContext = listItems;

            sendButton.Clicked += delegate
            {
                AddListItem(setEntry.Text);
                setEntry.Text = string.Empty;

                list.ScrollTo(listItems.Last(), ScrollToPosition.End, true);
            };            
        }

        void AddListItem(string text)
        {
            listItems.Add(new ListItem { TextItem = text , DetailItem = "text"});
        }
    }
}
