﻿using System;
namespace Comet
{
	public class Gesture<T> : Gesture
	{
		public Gesture(Action<T> action)
		{
			Action = action;
		}

		public Action<T> Action { get; }
		public override void Invoke() => Action?.Invoke((T)Convert.ChangeType(this, typeof(T)));
	}
	public class Gesture
	{
		public const string AddGestureProperty = "AddGesture";
		public const string RemoveGestureProperty = "RemoveGesture";

		public object PlatformGesture { get; set; }

		public virtual void Invoke()
		{

		}

	}
}
