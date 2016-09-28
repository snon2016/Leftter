using System;
using Plugin.Geolocator.Abstractions;
using Xamarin.Forms;

namespace Leftter
{
    class SendCell
    {
        public string MainText { get; set; }
        public string DetailText { get; set; }
        public Position SendPosition { get; set; }
    }
}