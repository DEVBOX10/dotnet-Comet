﻿using System;
using System.Collections.Generic;
using System.Linq;
using Comet.Reflection;
using Microsoft.Maui;
using Microsoft.Maui.HotReload;

namespace Comet.Internal
{
	public static class Extensions
	{
		public static T GetValueOfType<T>(this object obj)
		{
			if (obj == null)
				return default;

			if (obj is T t)
				return t;
			if (obj is Binding<T> bt)
				return bt.CurrentValue;
			if (obj is Binding b && b.Value is T bv)
				return bv;
			try
			{
				return (T)Convert.ChangeType(obj, typeof(T));
			}
			catch
			{
				return default;
			}
		}

		public static View FindViewById(this View view, string id)
		{
			if(view == null)
				return MauiHotReloadHelper.ActiveViews.OfType<View>().Select(x=> x.FindViewById(id)).FirstOrDefault();
			if (view.Id == id)
				return view;
			if(view is IContainerView ic)
				return ic.GetChildren().Select(x => x.FindViewById(id)).FirstOrDefault();
			return null;
		}

		public static Func<View> GetBody(this View view)
		{
			var bodyMethod = view.GetType().GetDeepMethodInfo(typeof(BodyAttribute));
			if (bodyMethod != null)
				return (Func<View>)Delegate.CreateDelegate(typeof(Func<View>), view, bodyMethod.Name);
			return null;
		}
		public static BindingState InternalGetState(this View view) => view.GetState();
		public static void ResetGlobalEnvironment(this View view) => View.Environment.Clear();

		//public static void DisposeAllViews(this View view) => View.ActiveViews.Clear();

		public static View GetView(this View view) => view.GetView();

		//public static Dictionary<Type, Type> GetAllRenderers(this Registrar<IFrameworkElement, IViewHandler> registar) => registar._handler;

		public static T SetParent<T>(this T view, View parent) where T : View
		{
			if (view != null)
				view.Parent = parent;
			return view;
		}

		public static T FindParentOfType<T>(this View view)
		{
			if (view == null)
				return default;
			if (view.BuiltView is T bt)
			{
				return bt;
			}
			if (view is T t)
				return t;
			return view.Parent.FindParentOfType<T>() ?? default;
		}
		public static NavigationView FindNavigation (this View view)
		{
			if (view == null)
				return default;
			var v = view.GetView();
			if(v.Navigation != null)
				return v.Navigation;

			if (v is ContentView cv)
				return cv.Content?.FindNavigation();

			return null;
		}
	}
}
