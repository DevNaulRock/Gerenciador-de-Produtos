using App.Helpers;
using App.Models;

namespace App
{
    public partial class App : Application
    {
        static SQLiteDatabaseHelper? _db;

        // Propriedade estática para acessar o banco de dados
        public static SQLiteDatabaseHelper DB
        {
            get
            {
                if (_db == null)
                {
                    string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "banco_sqlite_compras.db3");
                    _db = new SQLiteDatabaseHelper(path);

                    // Garantindo a criação da tabela de Produto
                    _db.CreateTable<Produto>();
                }
                return _db;
            }
        }

        public App()
        {
            InitializeComponent();
        }

        // Sobrescrevendo o método CreateWindow
        protected override Window CreateWindow(IActivationState? activationState)
        {
            var window = new Window(new NavigationPage(new Views.ListaProduto()));
            return window;
        }
    }
}