using System.Collections.ObjectModel;
using App.Models;

namespace App.Views;

public partial class ListaProduto : ContentPage
{
    // Declarando as ObservableCollections para armazenar e filtrar os produtos
    public ObservableCollection<Produto> Produtos { get; set; }
    public ObservableCollection<Produto> ProdutosFiltrados { get; set; } // Coleção filtrada

    public ListaProduto()
    {
        InitializeComponent();

        // Inicializando as ObservableCollections
        Produtos = new ObservableCollection<Produto>();
        ProdutosFiltrados = new ObservableCollection<Produto>();

        // Configurando o BindingContext para conectar ao XAML
        BindingContext = this;

        // Adicionando exemplo de produto inicial
        Produtos.Add(new Produto { Descricao = "Arroz", Quantidade = 1, Preco = 20.0 });
        Produtos.Add(new Produto { Descricao = "Feijão", Quantidade = 2, Preco = 10.0 });

        // Atualizando os produtos filtrados inicialmente
        AtualizarProdutosFiltrados();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        try
        {
            // Carregando produtos do banco de dados
            var produtosDoBanco = await App.DB.Table<Produto>().ToListAsync();

            // Limpando e adicionando os produtos à ObservableCollection
            Produtos.Clear();
            foreach (var produto in produtosDoBanco)
            {
                Produtos.Add(produto);
            }

            // Reaplicando o filtro após carregar os dados
            AtualizarProdutosFiltrados();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "OK");
        }
    }

    private void AdicionarProdutoButton_Clicked(object sender, EventArgs e)
    {
        try
        {
            Navigation.PushAsync(new Views.NovoProduto());
        }
        catch (Exception ex)
        {
            DisplayAlert("Ops", ex.Message, "OK");
        }
    }

    private void EditarButton_Clicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.CommandParameter is Produto produto)
        {
            Navigation.PushAsync(new EditarProduto(produto));
        }
    }

    private async void RemoverButton_Clicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.CommandParameter is Produto produto)
        {
            await RemoverProduto(produto);
        }
    }

    private async Task RemoverProduto(Produto produto)
    {
        bool confirmarRemocao = await DisplayAlert("Confirmação",
                                                   $"Deseja remover o produto {produto.Descricao}?",
                                                   "Sim", "Não");
        if (confirmarRemocao)
        {
            try
            {
                // Removendo o produto da base de dados
                await App.DB.DeleteProdutoAsync(produto);

                // Removendo o produto da coleção
                Produtos.Remove(produto);

                // Atualizando a coleção filtrada
                AtualizarProdutosFiltrados();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Ops", ex.Message, "OK");
            }
        }
    }

    private void AtualizarProdutosFiltrados(string termoPesquisa = "")
    {
        // Limpa os produtos filtrados e adiciona os que correspondem ao termo
        ProdutosFiltrados.Clear();
        foreach (var produto in Produtos)
        {
            if (string.IsNullOrWhiteSpace(termoPesquisa) ||
                produto.Descricao.Contains(termoPesquisa, StringComparison.OrdinalIgnoreCase))
            {
                ProdutosFiltrados.Add(produto);
            }
        }
    }

    private void OnSearchBarTextChanged(object sender, TextChangedEventArgs e)
    {
        // Atualiza os produtos filtrados com o texto da barra de pesquisa
        AtualizarProdutosFiltrados(e.NewTextValue);
    }
}