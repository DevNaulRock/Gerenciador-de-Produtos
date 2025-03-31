using App.Models;

namespace App.Views;

public partial class EditarProduto : ContentPage
    {
        public Produto Produto { get; set; }

        public EditarProduto(Produto produto)
        {
            InitializeComponent();
            Produto = produto;
            BindingContext = this;
        }

        private async void SalvarButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                // Atualizando o produto no banco de dados
                await App.DB.UpdateProdutoAsync(Produto);

                // Retornando para a página anterior
                await Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", $"Erro ao salvar o produto: {ex.Message}", "OK");
            }
        }
    }
