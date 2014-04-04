using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using VideoCommon;

namespace VideoApp
{
	public partial class Main : Window
	{
		private CollectionViewSource filmViewSource = new CollectionViewSource();
		private CollectionViewSource genreViewSource = new CollectionViewSource();
		private ObservableCollection<Film> filmsOb = new ObservableCollection<Film>();

		public Main()
		{
			InitializeComponent();
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			VulDeListBox();
			VulDeComboBox();
			titelTextBox.Text = AppDomain.CurrentDomain.GetData("DataDirectory").ToString();
		}

		private void VulDeListBox()
		{
			filmViewSource = ((CollectionViewSource)(this.FindResource("filmViewSource")));
			var manager = new FilmManager();
			filmsOb = manager.GetAllFilms();
			filmViewSource.Source = filmsOb;
			//filmsOb.CollectionChanged += this.OnCollectionChanged;
		}

		private void VulDeComboBox()
		{
			genreViewSource = ((CollectionViewSource)(this.FindResource("genreViewSource")));
			var manager = new GenreManager();
			genreViewSource.Source = manager.GetAllGenres();
		}

		private void prijsTextBox_GotFocus(object sender, RoutedEventArgs e)
		{
			Decimal tempPrijs = new Decimal();
			if (Decimal.TryParse(prijsTextBox.Text, NumberStyles.Currency, CultureInfo.CurrentCulture, out tempPrijs))
				prijsTextBox.Text = tempPrijs.ToString();
			prijsTextBox.SelectAll();
		}

		private void prijsTextBox_LostFocus(object sender, RoutedEventArgs e)
		{
			Decimal tempPrijs = new Decimal();
			if (Decimal.TryParse(prijsTextBox.Text.Replace(",", CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator).Replace(".", CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator), NumberStyles.Currency, CultureInfo.CurrentCulture, out tempPrijs))
				prijsTextBox.Text = String.Format("{0:c}", tempPrijs);
		}

		private Film LeesInput()
		{
			Int32 eenBandNr = new Int32();
			if (bandNrTextBox.Text == String.Empty)
				eenBandNr = 0;
			else if (!Int32.TryParse(bandNrTextBox.Text, out eenBandNr))
				throw new Exception("Gelieve een geldig bandnummer in te geven.");

			String eenTitel = String.Empty;
			if (titelTextBox.Text != String.Empty)
				eenTitel = titelTextBox.Text.ToUpper();
			else
				throw new Exception("Gelieve een titel in te vullen.");

			var eenGenreNr = new Int32();
			if (genreNrComboBox.SelectedIndex != -1)
				eenGenreNr = (Int32)genreNrComboBox.SelectedValue;
			else
				throw new Exception("Gelieve een genre te selecteren.");

			Int32 eenInVoorraad = new Int32();
			if (!Int32.TryParse(inVoorraadTextBox.Text, out eenInVoorraad))
				throw new Exception("Gelieve een geldig aantal In Voorraad in te vullen.");

			Int32 eenUitVoorraad = new Int32();
			if (!Int32.TryParse(uitVoorraadTextBox.Text, out eenUitVoorraad))
				throw new Exception("Gelieve een geldig aantal Uit Voorraad in te vullen.");

			Decimal eenPrijs = new Decimal();
			if (!Decimal.TryParse(prijsTextBox.Text, NumberStyles.Currency, CultureInfo.CurrentCulture, out eenPrijs))
				throw new Exception("Gelieve een geldige prijs in te vullen.");

			Int32 eenTotaalVerhuurd = new Int32();
			if (!Int32.TryParse(totaalVerhuurdTextBox.Text, out eenTotaalVerhuurd))
				throw new Exception("Gelieve een geldig aantal Totaal Verhuurd in te vullen.");

			Film eenFilm = new Film(eenBandNr, eenTitel, eenGenreNr, eenInVoorraad, eenUitVoorraad, eenPrijs, eenTotaalVerhuurd);
			return eenFilm;
		}

		private void SwitchEditMode()
		{
			if (buttonToevoegen.Content.ToString() == "Toevoegen annuleren")
			{
				buttonToevoegen.Content = "Toevoegen";
				buttonVerwijderen.IsEnabled = true;
				buttonOpslaan.Content = "Wijzigingen opslaan";
				listBoxFilms.Focus();
				listBoxFilms.IsEnabled = true;
				bandNrTextBox.IsEnabled = true;
			}
			else
			{
				buttonToevoegen.Content = "Toevoegen annuleren";
				buttonVerwijderen.IsEnabled = false;
				buttonOpslaan.Content = "Nieuwe film opslaan";
				listBoxFilms.SelectedIndex = -1;
				listBoxFilms.IsEnabled = false;
				bandNrTextBox.IsEnabled = false;
				titelTextBox.Focus();
			}
		}

		private void ButtonToevoegen_Click(object sender, RoutedEventArgs e)
		{
			SwitchEditMode();
		}

		private void ButtonVerwijderen_Click(object sender, RoutedEventArgs e)
		{
			if (MessageBox.Show("Weet u zeker dat u deze film wilt verwijderen?", "Verwijderen", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes)
			{
				Film eenFilm = LeesInput();
				var manager = new FilmManager();
				manager.SchrijfVerwijdering(eenFilm.BandNr);
				if (listBoxFilms.SelectedIndex == listBoxFilms.Items.Count - 1)
					SelectFilm(listBoxFilms.SelectedIndex - 1);
				else
					SelectFilm(listBoxFilms.SelectedIndex);
			}
		}

		private void ButtonOpslaan_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				Film eenFilm = LeesInput();
				var manager = new FilmManager();
				if (buttonToevoegen.Content.ToString() == "Toevoegen annuleren")
				{
					eenFilm.BandNr = manager.SchrijfToevoeging(eenFilm);
					SwitchEditMode();
				}
				else
				{
					manager.SchrijfWijziging(eenFilm);
				}
				SelectFilm(eenFilm);


			}
			catch (Exception ex)
			{
				MessageBox.Show("Fout:\n" + ex.Message, "Foutmelding", MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.OK);
			}
		}

		private void SelectFilm(Film eenFilm)
		{
			VulDeListBox();
			listBoxFilms.SelectedValue = eenFilm.BandNr;
			listBoxFilms.ScrollIntoView(listBoxFilms.SelectedItem);
			ListBoxItem selectedFilm = (ListBoxItem)listBoxFilms.ItemContainerGenerator.ContainerFromIndex(listBoxFilms.SelectedIndex);
			selectedFilm.Focus();
		}
		private void SelectFilm(int eenIndex)
		{
			VulDeListBox();
			listBoxFilms.SelectedIndex = eenIndex;
			listBoxFilms.ScrollIntoView(listBoxFilms.SelectedItem);
			ListBoxItem selectedFilm = (ListBoxItem)listBoxFilms.ItemContainerGenerator.ContainerFromIndex(eenIndex);
			selectedFilm.Focus();
		}

	}
}
