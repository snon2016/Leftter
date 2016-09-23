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
        ObservableCollection<ListItem> listItems = new ObservableCollection<ListItem>();
        // 動的にList<ListItem>を追加するのに使う
        int n = 0;
        const int cellAmount = 25;

        public MainPage()
        {
            InitializeComponent();
            AddListItem(n);
            BindingContext = listItems;

            // ListViewの各Itemが表示された時にイベントが発生する
            list.ItemAppearing += async (object sender, ItemVisibilityEventArgs e) =>
            {
                // ObservableCollectionの最後がListViewのItemと一致したときにObservableCollectionにデータを追加するなどの処理を行う
                if(listItems.Last() == e.Item as ListItem)
                {
                    stack.IsVisible = true;
                    await Task.Delay(2000);

                    n++;
                    AddListItem(cellAmount * n);
                    stack.IsVisible = false;
                }
            };
        }

        void AddListItem(int i)
        {
            foreach(var j in Enumerable.Range(i, cellAmount))
            {
                listItems.Add(new ListItem { TextItem = "TextData " + j, DetailItem = "DetailData " + j });
            }
        }
    }
}
