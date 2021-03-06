﻿using System;
using System.ComponentModel;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace FamilyMenu
{
	public class ChefListViewModel : INotifyPropertyChanged
	{
		private INavigation navigation;

		public ChefListViewModel (INavigation navigation)
		{
			this.navigation = navigation;

			MessagingCenter.Subscribe<INetworkFunctions> (this, "UpdateChefList", (sender) => {

				LoadChefsCommand.Execute(null);
			});

			LoadChefsCommand.Execute(null);
		}

		#region INotifyPropertyChanged implementation
		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChanged(string name)
		{
			if (PropertyChanged == null)
				return;

			PropertyChanged (this, new PropertyChangedEventArgs(name));
		}
		#endregion

		private ObservableCollection<ChefDetailViewModel> _Chefs = new ObservableCollection<ChefDetailViewModel>();
		public ObservableCollection<ChefDetailViewModel> Chefs {
			get { return _Chefs; } 
			set { 
				if (_Chefs == value)
					return;

				_Chefs = value;

				OnPropertyChanged ("Chefs");
			}
		}

		private Chef _selectedItem;
		public Chef SelectedItem
		{
			get { return _selectedItem; }
			set {
				if (_selectedItem != value) {
					_selectedItem = value;

					OnPropertyChanged ("SelectedItem");
				}
			}
		}

		#region Commands
		private Command loadChefsCommand;
		public Command LoadChefsCommand {
			get {
				return loadChefsCommand ?? (loadChefsCommand = new Command(ExecuteLoadChefsCommand));
			}
		}

		private async void ExecuteLoadChefsCommand()
		{
            Chefs.Clear();

            var chefs = await App.Database.GetChefs ();
            			
			foreach (var chef in chefs) {
				Chefs.Add(new ChefDetailViewModel(chef, navigation));
			}
		}

		private Command addCommand;
		public Command AddCommand {
			get {
				return addCommand ?? (addCommand = new Command(ExecuteAddCommand));
			}
		}

		private void ExecuteAddCommand()
		{
			navigation.PushAsync(new ChefDetailView());
		}

		#endregion Commands

	}
}

