﻿using System;
using System.Collections.Generic;
using System.Linq;
using Comet.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui;
using Microsoft.Maui.Hosting;

namespace Comet
{

	public class CometApp : View, IApplication, IMauiContextHolder
	{
		public CometApp()
		{
			CurrentApp = this;
		}
		public static CometApp CurrentApp { get; protected set; }
		public static CometWindow CurrentWindow { get; protected set; }
		public static IMauiContext MauiContext => StateManager.CurrentContext ?? CurrentWindow?.MauiContext ?? ((IMauiContextHolder)CurrentApp).MauiContext;

		public static float DisplayScale => CurrentWindow?.DisplayScale ?? 1;
		List<IWindow> windows = new List<IWindow>();
		public IReadOnlyList<IWindow> Windows => windows;


		IWindow IApplication.CreateWindow(IActivationState activationState)
		{
			((IMauiContextHolder)this).MauiContext = activationState.Context;

			windows.Add(CurrentWindow = new CometWindow
			{
				MauiContext = activationState.Context,
				Content = this,
			}) ;
			return CurrentWindow;
		}

		void IApplication.ThemeChanged()
		{
			//TODO: apply new theme
		}

		IMauiContext IMauiContextHolder.MauiContext { get; set; }

		IReadOnlyList<IWindow> IApplication.Windows => windows;
	}
}
