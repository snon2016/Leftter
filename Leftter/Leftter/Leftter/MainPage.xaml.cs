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
using static System.Math;

namespace Leftter
{
    public partial class MainPage : ContentPage
    {
        // 動的にList<ListItem>を追加するのに使う
        ObservableCollection<SendCell> listItems = new ObservableCollection<SendCell>();

        public MainPage()
        {
            InitializeComponent();

            BindingContext = listItems;
        }

        private void SendButtonClicked(object sender, EventArgs e)
        {
            sendButton.IsEnabled = false;
            string detailText = DateTime.Now.ToString("yyyy/MM/dd HH:mm");
            AddListItem(setEditor.Text, detailText);
            SendData(setEditor.Text, detailText);

            setEditor.Text = string.Empty;
        }

        private async void GetButtonClicked(object sender, EventArgs e)
        {
            var firebase = new FirebaseClient("https://test-da012.firebaseio.com/");

            var child = firebase.Child("messages");
            var orderedChild = Firebase.Xamarin.Database.Query.QueryFactoryExtensions.OrderByKey(child);
            var limttedChild = Firebase.Xamarin.Database.Query.QueryExtensions.LimitToFirst(orderedChild, 100);
            var items = await limttedChild.OnceAsync<SendCell>();
            Position gps = await GetGPS();

            foreach(var item in items)
            {
                if(IsGotData(item.Object)) continue;
                if(CanGetRange(item.Object.SendPosition, gps))
                    AddListItem(item.Object);
            }
        }

        private async void SendData(string mainText, string detailText)
        {
            SendCell sendCell = new SendCell();
            var firebase = new FirebaseClient("https://test-da012.firebaseio.com/");

            sendCell.MainText = mainText;
            sendCell.DetailText = detailText;
            sendCell.SendPosition = await GetGPS();

            var item = await firebase.Child("messages").PostAsync(sendCell);

            listItems[listItems.Count-1].SendPosition = sendCell.SendPosition;
            sendButton.IsEnabled = true;
        }

        private bool IsGotData(SendCell cell)
        {
            foreach(var list in listItems)
                if(cell.SendPosition.Timestamp.DateTime == list.SendPosition.Timestamp.DateTime &&
                    cell.SendPosition.Latitude == list.SendPosition.Latitude &&
                    cell.SendPosition.Longitude == list.SendPosition.Longitude) return true;
            return false;
        }

        private bool CanGetRange(Position pos, Position gps)
        {
            setEditor.Text += Sqrt(Pow((pos.Latitude - gps.Latitude) * 2, 2) + Pow(pos.Longitude - gps.Longitude, 2)).ToString() + "\n";
            if(0.0010 < Sqrt(Pow((pos.Latitude - gps.Latitude) * 2, 2) + Pow(pos.Longitude - gps.Longitude, 2))) return false;
            return true;
        }

        private async Task<Position> GetGPS()
        {
            IGeolocator locator = CrossGeolocator.Current;
            locator.DesiredAccuracy = 50;
            Position position = new Position();
            try
            {
                if(locator.IsGeolocationAvailable)
                    if(locator.IsGeolocationEnabled)
                        position = await locator.GetPositionAsync(timeoutMilliseconds: 10000);

                return position;
            }
            catch
            {
                await DisplayAlert("Error", "Could not get GPS data.", "OK");
            }
            return null;
        }

        void AddListItem(string text, string detail)
        {
            listItems.Add(new SendCell { MainText = text, DetailText = detail });
            logListView.ScrollTo(listItems.Last(), ScrollToPosition.End, true);
        }

        void AddListItem(SendCell cell)
        {
            listItems.Add(cell);
            logListView.ScrollTo(listItems.Last(), ScrollToPosition.End, true);
        }
    }
}
