using HappyBudget.Models;
using HappyBudget.Services.Dialog;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace HappyBudget.ViewModels
{
    public class SelectColorViewModel : BaseViewModel
    {
        private IDialogService _dialogService;

        private ObservableCollection<ColorPicker> _colors;
        public ObservableCollection<ColorPicker> Colors
        {
            get { return _colors; }
            set { SetProperty(ref _colors, value); }
        }

        private ColorPicker _selectedColor;
        public ColorPicker SelectedColor
        {
            get { return _selectedColor; }
            set { SetProperty(ref _selectedColor, value); }
        }

        public SelectColorViewModel(IDialogService dialogService)
        {
            _dialogService = dialogService;
            Colors = new ObservableCollection<ColorPicker>(ColorPicker.GetHexCodes());
            SelectedColor = null;
        }

        public ICommand SelectColorCommand { get => new Command(() => SelectColor()); }
        private async void SelectColor()
        {
            if (SelectedColor == null)
            {
                await _dialogService.DisplayAlert("Error", "Choose a color!", "Ok");
                return;
            }

            MessagingCenter.Send(this, "_color");
            await PopupNavigation.Instance.PopAsync(true);
        }

        //public ICommand SelectCategoryColorCommand { get => new Command(() => SelectCategoryColor()); }
        //private async void SelectCategoryColor()
        //{
        //    if (SelectedColor == null)
        //    {
        //        await _dialogService.DisplayAlert("Error", "Choose a color for the category!", "Ok");
        //        return;
        //    }

        //    MessagingCenter.Send(this, "_categoryColor");
        //    await PopupNavigation.Instance.PopAsync(true);
        //}
    }

}
