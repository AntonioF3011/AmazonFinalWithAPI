using CRM.Library.Services;
using CRM.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using CRM.Library.DTO;

namespace CRM.MAUI.ViewModels
{
    public class ProductManagementViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public List<ProductViewModel> Products
        {
            get
            {
                return ProductServiceProxy.Current.Products?.Where(p => p != null)
                    .Select(p => new ProductViewModel(p)).ToList()
                    ?? new List<ProductViewModel>();
            }
        }

        public ProductViewModel SelectedProduct { get; set; }

        public ICommand ImportCsvCommand { get; private set; }

        public ProductManagementViewModel()
        {
            ImportCsvCommand = new Command(async () => await ImportCsv());
        }

        public async void RefreshProducts()
        {
            await ProductServiceProxy.Current.Get();
            NotifyPropertyChanged(nameof(Products));
        }

        public void UpdateProduct()
        {
            if (SelectedProduct?.Model == null)
            {
                return;
            }
            Shell.Current.GoToAsync($"//Product?productId={SelectedProduct.Model.Id}"); //this is sending the product Id 
            ProductServiceProxy.Current.AddOrUpdate(SelectedProduct.Model);
            RefreshProducts();

        }

        public async void DeleteProducts()
        {
            if (SelectedProduct?.Model == null)
            {
                return;
            }
            await ProductServiceProxy.Current.Delete(SelectedProduct.Model.Id);
            RefreshProducts();
        }

        private async Task ImportCsv()
        {
            try
            {
                var customFileType = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
                {
                    { DevicePlatform.WinUI, new[] { ".csv" } },
                });

                var options = new PickOptions
                {
                    PickerTitle = "Please select a CSV file",
                    FileTypes = customFileType
                };

                var result = await FilePicker.Default.PickAsync(options);
                if (result != null)
                {
                    using var stream = await result.OpenReadAsync();
                    using var reader = new StreamReader(stream);
                    var content = await reader.ReadToEndAsync();
                    ProcessCsv(content);
                }
            }
            catch (Exception ex)
            {
                
                await Application.Current.MainPage.DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
            }
        }
        


        private void ProcessCsv(string content)
        {
            var lines = content.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var line in lines.Skip(1)) // Skip header row
            {
                var fields = line.Split(',');


                var product = new ProductDTO
                {
                    Id = int.Parse(fields[0]),
                    Name = fields[1],
                    Description = fields[2],
                    Price = decimal.Parse(fields[3]),
                    Quantity = int.Parse(fields[4]),
                    Markdown = decimal.Parse(fields[5]),
                    BuyoneGetone = bool.Parse(fields[6]),
                    ImageName = fields[7]
                };

                // Añadir el producto al servicio
                ProductServiceProxy.Current.AddOrUpdate(product);
            }
            RefreshProducts();
        }
    }
}
