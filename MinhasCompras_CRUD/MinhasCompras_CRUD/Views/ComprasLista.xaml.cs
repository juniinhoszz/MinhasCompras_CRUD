using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MinhasCompras_CRUD.Model;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MinhasCompras_CRUD.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ComprasLista : ContentPage
    {
        ObservableCollection<Produto> lista_produtos = new ObservableCollection<Produto>();
        public ComprasLista()
        {
            InitializeComponent();
            list_produtos.ItemsSource = lista_produtos;

            
        }

        private void btn_somar_Clicked(object sender, EventArgs e)
        {
            double soma = lista_produtos.Sum(i => i.PrecoTotal);

            DisplayAlert("O Total é: ", "R" + soma.ToString("C"), "OK");
        }

        private void btn_add_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new ComprasForm());
        }

        private void txt_search_TextChanged(object sender, TextChangedEventArgs e)
        {
            string q = e.NewTextValue;

            lista_produtos.Clear();

            Task.Run(async () =>
            {
                List<Produto> temp = await App.DB.Search(q);

                foreach (Produto p in temp)
                {
                    lista_produtos.Add(p);
                }
            });
        }

        private async void btn_Remover_Clicked(object sender, EventArgs e)
        {
            MenuItem disparador = sender as MenuItem;

            Produto produto_selecionado = (Produto)disparador.BindingContext;

            string mensagem = "Remover " + produto_selecionado.Nome + " da lista de compras? ";

            bool confirmacao = await DisplayAlert("Tem Certeza?", mensagem, "Sim", "Não");

            if (confirmacao)
            {
                await App.DB.Delete(produto_selecionado.Id);
                lista_produtos.Remove(produto_selecionado);
            }
        }

        private void list_produtos_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Produto produto_selecionado = e.SelectedItem as Produto;

            Navigation.PushAsync(new ComprasForm
            {
                BindingContext = produto_selecionado
            });
        }

        private void lst_produtos_Refreshing(object sender, EventArgs e)
        {
            lista_produtos.Clear();

            Task.Run(async () =>
            {
                List<Produto> temp = await App.DB.GellAll();

                foreach (Produto p in temp)
                {
                    lista_produtos.Add(p);
                }
            });

            ref_carregando.IsRefreshing = false;
        }

        protected override void OnAppearing()
        {
            lista_produtos.Clear();

            if (lista_produtos.Count == 0)
            {
                Task.Run(async () =>
                {
                    List<Produto> temp = await App.DB.GellAll();

                    foreach (Produto p in temp)
                    {
                        lista_produtos.Add(p);
                    }
                });
            }

            ref_carregando.IsRefreshing = false;
            }

        private async void btnFinalizar_Clicked(object sender, EventArgs e)
        {
            string mensagem = "Deseja finalizar e apagar essa lista de compras?";

            bool confirmacao = await DisplayAlert("Tem Certeza?", mensagem, "Sim", "Não");

            if (confirmacao)
            {
                foreach (Produto p in lista_produtos)
                {
                    await App.DB.Delete(p.Id);
                }

                lista_produtos.Clear();
            }
        }
    }
    }