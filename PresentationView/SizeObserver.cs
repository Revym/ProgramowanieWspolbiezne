using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace PresentationView
{
    public static class SizeObserver
    {
        public static readonly DependencyProperty ObservedWidthProperty =
            DependencyProperty.RegisterAttached("ObservedWidth", typeof(double), typeof(SizeObserver), new FrameworkPropertyMetadata(0.0));

        public static readonly DependencyProperty ObservedHeightProperty =
            DependencyProperty.RegisterAttached("ObservedHeight", typeof(double), typeof(SizeObserver), new FrameworkPropertyMetadata(0.0));

        public static readonly DependencyProperty ObserveProperty =
            DependencyProperty.RegisterAttached("Observe", typeof(bool), typeof(SizeObserver), new FrameworkPropertyMetadata(false, OnObserveChanged));

        public static double GetObservedWidth(DependencyObject obj) => (double)obj.GetValue(ObservedWidthProperty);
        public static void SetObservedWidth(DependencyObject obj, double value) => obj.SetValue(ObservedWidthProperty, value);

        public static double GetObservedHeight(DependencyObject obj) => (double)obj.GetValue(ObservedHeightProperty);
        public static void SetObservedHeight(DependencyObject obj, double value) => obj.SetValue(ObservedHeightProperty, value);

        public static bool GetObserve(DependencyObject obj) => (bool)obj.GetValue(ObserveProperty);
        public static void SetObserve(DependencyObject obj, bool value) => obj.SetValue(ObserveProperty, value);

        private static void OnObserveChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is FrameworkElement fe && (bool)e.NewValue)
            {
                fe.SizeChanged += (s, args) =>
                {
                    SetObservedWidth(fe, fe.ActualWidth);
                    SetObservedHeight(fe, fe.ActualHeight);
                };
            }
        }
    }
}
