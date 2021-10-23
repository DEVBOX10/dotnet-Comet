﻿using System.Collections.Generic;
using CoreLocation;
using Foundation;
using Comet.Samples;
using MapKit;
using UIKit;
using Microsoft.Maui;
using Microsoft.Maui.HotReload;
using Microsoft.Maui.Hosting;

namespace Comet.iOS.Sample
{
	[Register("AppDelegate")]
	public class AppDelegate : MauiUIApplicationDelegate
	{
		protected override MauiApp CreateMauiApp() => MyApp.CreateMauiApp();
	}
}

