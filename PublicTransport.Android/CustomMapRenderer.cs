using Android.Content;
using Android.Gms.Maps.Model;
using System;
using PublicTransport;
using PublicTransport.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Maps.Android;

[assembly: ExportRenderer(typeof(CustomMap), typeof(CustomMapRenderer))]
namespace PublicTransport.Droid
{
    class CustomMapRenderer : MapRenderer
    {

        public CustomMapRenderer(Context context) : base(context) {}

        protected override MarkerOptions CreateMarker(Pin pin)
        {
            
            var marker = new MarkerOptions();
            marker.SetPosition(new LatLng(pin.Position.Latitude, pin.Position.Longitude));
            
            marker.SetSnippet(pin.Address);
            if(pin.Type == PinType.Place)
            {
                if(pin.Label[pin.Label.Length-1]==Convert.ToChar("0"))
                {
                    marker.SetIcon(BitmapDescriptorFactory.FromResource(Resource.Drawable.bus));
                }
                else
                {
                    marker.SetIcon(BitmapDescriptorFactory.FromResource(Resource.Drawable.bus_p));
                }
                pin.Label = pin.Label.Remove(pin.Label.Length - 1);
            }
            else marker.SetIcon(BitmapDescriptorFactory.FromResource(Resource.Drawable.station));
            marker.SetTitle(pin.Label);
            return marker;
        }
    }
}
