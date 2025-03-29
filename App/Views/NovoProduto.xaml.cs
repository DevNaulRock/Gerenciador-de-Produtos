using App.Models;

namespace App.Views;

public partial class NovoProduto : ContentPage
{
	public NovoProduto()
	{
		InitializeComponent();
	}

    private async void SalvarButton_Clicked(object sender, EventArgs e)
    {
        try
        {
            Produto P = new Produto
            {
                Descricao = txt_descricao.Text,
                Quantidade = Convert.ToDouble(txt_quantidade.Text),
                Preco = Convert.ToDouble(txt_preco.Text),
            };

            // Aguarde a execução da inserção no banco
            await App.DB.Insert(P);

            await DisplayAlert("Sucesso!", "Registro Inserido!", "OK");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "OK");
        }
    }
}