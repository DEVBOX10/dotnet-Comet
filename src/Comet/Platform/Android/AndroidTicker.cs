﻿using System;
using Android.Animation;
using Android.Content;
using Android.OS;
using Microsoft.Maui;
using Microsoft.Maui.Essentials;

namespace Comet.Services
{
	public class NativeTicker : Ticker
	{
		public IMauiContext MauiContext { get; set; }
		ValueAnimator _val;
		bool _systemEnabled;
		public NativeTicker()
		{
			_val = new ValueAnimator();
			_val.SetIntValues(0, 100); // avoid crash
			_val.RepeatCount = ValueAnimator.Infinite;
			_val.Update += (s, e) => Fire?.Invoke(); ;
			CheckPowerSaveModeStatus();
		}

		internal void CheckPowerSaveModeStatus()
		{
			// Android disables animations when it's in power save mode
			// So we need to keep track of whether we're in that mode and handle animations accordingly
			// We can't just check ValueAnimator.AreAnimationsEnabled() because there's no event for that, and it's
			// only supported on API >= 26

			//if (!Forms.IsLollipopOrNewer)
			//{
			//    _systemEnabled = true;
			//    return;
			//}

			var powerManager = (PowerManager)MauiContext.Context.GetSystemService(Context.PowerService);

			var powerSaveOn = powerManager.IsPowerSaveMode;

			// If power saver is active, then animations will not run
			_systemEnabled = !powerSaveOn;

		}

		public override bool IsRunning => _val.IsStarted;
		public override bool SystemEnabled { get => _systemEnabled; }
		public override void Start() => _val.Start();
		public override void Stop()
		{
			ThreadHelper.RunOnMainThread(() => _val?.Cancel());
		}
	}
}