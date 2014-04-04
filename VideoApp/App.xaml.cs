using System;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;

namespace VideoApp
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		protected override void OnStartup(StartupEventArgs e)
		{	
			Thread.CurrentThread.CurrentCulture = CultureInfo.CurrentCulture;
			FrameworkElement.LanguageProperty.OverrideMetadata(typeof(FrameworkElement),
		new FrameworkPropertyMetadata(XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag)));

			EventManager.RegisterClassHandler(typeof(TextBox),
		 TextBox.GotFocusEvent,
		 new RoutedEventHandler(TextBox_SelectAll));

			EventManager.RegisterClassHandler(typeof(TextBox),
   TextBox.GotMouseCaptureEvent,
   new RoutedEventHandler(TextBox_SelectAll));

			base.OnStartup(e);
		}

		void TextBox_SelectAll(object sender, RoutedEventArgs e)
		{
			(sender as TextBox).SelectAll();
		}



	}
}
