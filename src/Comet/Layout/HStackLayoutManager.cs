﻿using System;
using Microsoft.Maui.Primitives;

namespace Comet.Layout
{
	public class HStackLayoutManager : Microsoft.Maui.Layouts.ILayoutManager
	{
		private readonly VerticalAlignment _defaultAlignment;
		private readonly float _spacing;

		public HStackLayoutManager(ContainerView layout,
			VerticalAlignment alignment = VerticalAlignment.Center,
			float? spacing = null)
		{
			this.layout = layout;
			_defaultAlignment = alignment;
			_spacing = spacing ?? 4;
		}

		ContainerView layout;
		public Size ArrangeChildren(Rectangle rect)
		{
			var measured = rect.Size;
			double height = 0;

			var index = 0;
			double nonSpacerWidth = 0;
			var spacerCount = 0;
			var sizes = new List<Size>();
			var lastWasSpacer = false;

			foreach (var view in layout)
			{
				var isSpacer = false;

				if (view is Spacer)
				{
					spacerCount++;
					isSpacer = true;
					sizes.Add(new Size());
				}
				else
				{
					var size = view.MeasuredSize;
					var constraints = view.GetFrameConstraints();
					var margin = view.GetMargin();
					var sizing = view.GetVerticalLayoutAlignment(layout);

					// todo: this should never be needed.  Need to investigate this.
					if (!view.MeasurementValid)
					{
						view.MeasuredSize = size = view.Measure(measured.Width,measured.Height);
						view.MeasurementValid = true;
					}

					if (constraints?.Width != null)
						size.Width = Math.Min((float)constraints.Width, measured.Width);

					if (constraints?.Height != null)
						size.Height = Math.Min((float)constraints.Height, measured.Height);

					if (sizing == LayoutAlignment.Fill && constraints?.Height == null)
						size.Height = measured.Height - margin.VerticalThickness;

					sizes.Add(size);
					height = Math.Max(size.Height, height);
					nonSpacerWidth += size.Width + margin.HorizontalThickness;
				}

				if (index > 0 && !lastWasSpacer && !isSpacer)
					nonSpacerWidth += _spacing;

				lastWasSpacer = isSpacer;
				index++;
			}

			nonSpacerWidth = Math.Min(nonSpacerWidth, measured.Width);

			double spacerWidth = 0;
			if (spacerCount > 0)
			{
				var availableWidth = measured.Width - nonSpacerWidth;
				spacerWidth = availableWidth / spacerCount;
			}

			var x = rect.X;
			var y = rect.Y;
			index = 0;
			foreach (var view in layout)
			{
				var isSpacer = false;

				Size size;
				if (view is Spacer)
				{
					isSpacer = true;
					size = new Size(spacerWidth, height);
				}
				else
				{
					size = sizes[index];
				}

				var constraints = view.GetFrameConstraints();
				var alignment = constraints?.Alignment?.Vertical ?? _defaultAlignment;
				var alignedY = y;

				var margin = view.GetMargin();

				switch (alignment)
				{
					case VerticalAlignment.Center:
						alignedY += (measured.Height - size.Height - margin.Bottom + margin.Top) / 2;
						break;
					case VerticalAlignment.Bottom:
						alignedY += measured.Height - size.Height - margin.Bottom;
						break;
					case VerticalAlignment.Top:
						alignedY = margin.Top;
						break;
					case VerticalAlignment.FirstTextBaseline:
						throw new NotSupportedException(VerticalAlignment.FirstTextBaseline.ToString());
					case VerticalAlignment.LastTextBaseline:
						throw new NotSupportedException(VerticalAlignment.LastTextBaseline.ToString());
					default:
						throw new ArgumentOutOfRangeException();
				}

				if (index > 0 && !lastWasSpacer && !isSpacer)
					x += _spacing;

				x += margin.Left;

				var sizing = view.GetVerticalLayoutAlignment(layout);
				if (sizing == LayoutAlignment.Fill && constraints?.Height == null)
				{
					alignedY = margin.Top;
					size.Height = measured.Height - margin.VerticalThickness;
				}

				view.Frame = new Rectangle(x, alignedY, size.Width, size.Height);

				x += size.Width;
				x += margin.Right;

				lastWasSpacer = isSpacer;
				index++;
			}
			return new Size(x, height);
		}
		public Size Measure(double widthConstraint, double heightConstraint)
		{
			var index = 0;
			double width = 0;
			double height = 0;
			var spacerCount = 0;
			var lastWasSpacer = false;


			foreach (var view in layout)
			{
				var isSpacer = false;

				if (view is Spacer)
				{
					spacerCount++;
					isSpacer = true;

					if (!view.MeasurementValid)
					{
						view.MeasuredSize = new Size(-1, -1);
						view.MeasurementValid = true;
					}
				}
				else
				{
					var size = view.MeasuredSize;
					if (!view.MeasurementValid)
					{
						view.MeasuredSize = size = view.Measure(widthConstraint,heightConstraint);
						view.MeasurementValid = true;
					}

					var finalHeight = size.Height;
					var finalWidth = size.Width;

					var margin = view.GetMargin();
					finalHeight += margin.VerticalThickness;
					finalWidth += margin.HorizontalThickness;

					var constraints = view.GetFrameConstraints();
					var verticalSizing = view.GetVerticalLayoutAlignment(layout);
					if (verticalSizing == LayoutAlignment.Fill && constraints?.Height == null)
						height = heightConstraint;

					height = Math.Max(finalHeight, height);
					width += finalWidth;
				}

				if (index > 0 && !lastWasSpacer && !isSpacer)
					width += _spacing;

				lastWasSpacer = isSpacer;
				index++;
			}

			if (spacerCount > 0)
				width = widthConstraint;

			var layoutVerticalSizing = layout.GetVerticalLayoutAlignment(layout);
			if (layoutVerticalSizing == LayoutAlignment.Fill)
				height = heightConstraint;

			var layoutHorizontalSizing = layout.GetHorizontalLayoutAlignment(layout);
			if (layoutHorizontalSizing == LayoutAlignment.Fill)
				width = widthConstraint;

			return new Size(width, height);
		}
	}
}

