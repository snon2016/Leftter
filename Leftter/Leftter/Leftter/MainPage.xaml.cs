using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using Firebase.Xamarin.Database;

namespace Leftter
{
    public partial class MainPage : ContentPage
    {
        // 動的にList<ListItem>を追加するのに使う
        ObservableCollection<ListItem> listItems = new ObservableCollection<ListItem>();
        Position position;

        public MainPage()
        {
            InitializeComponent();

            BindingContext = listItems;

            sendButton.Clicked += async delegate
            {
                AddListItem(setEntry.Text);
                setEntry.Text = string.Empty;

                IGeolocator locator = CrossGeolocator.Current;
                locator.DesiredAccuracy = 50;

                if(locator.IsGeolocationAvailable)
                    if(locator.IsGeolocationEnabled)
                    {
                        position = await locator.GetPositionAsync(timeoutMilliseconds: 100);
                        AddListItem("Latitude:\t" + position.Latitude);
                        AddListItem("Longitude:\t" + position.Longitude);
                    }

                list.ScrollTo(listItems.Last(), ScrollToPosition.End, true);
            };
			getButton.Clicked += delegate {
				var firebase = new FirebaseClient("https://test-da012.firebaseio.com/");
				//var items = await firebase.Child("messages")
					//.OrderByKey();
				  //.LimitToFirst(2)
				  //.OnceAsync<YourObject>();

				var child = firebase.Child("messages");
				var orderedChild = Firebase.Xamarin.Database.Query.QueryFactoryExtensions.OrderByKey(child);
				var limttedChild = Firebase.Xamarin.Database.Query.QueryExtensions.LimitToFirst(orderedChild, 2);
				var items = limttedChild.OnceAsync<ListItem>();


				foreach (var item in items)
				{
					AddListItem($"{item.Key} name is {item.Object.Name}");
				}
			};
        }

        void AddListItem(string text)
        {
            listItems.Add(new ListItem { TextItem = text , DetailItem = "text"});
        }
    }
}
