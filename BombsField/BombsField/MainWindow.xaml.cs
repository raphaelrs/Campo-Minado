using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BombsField
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string[,] MatrizDeValores { get; set; }
        public Button[,] Campo { get; set; }
        public int DimensaoDaLinha { get; set; }
        public int DimensaoDaColuna { get; set; }
        public int EspacoSemBombas { get; set; }
        public int BombasMarcadas { get; set; }
        public bool Fim { get; set; }
        public int BombaPrincipalx { get; set; }
        public int BombaPrincipaly { get; set; }
        public Image[] Imagens { get; set; }
        public string[,] Valores { get; set; }
        public bool PrimeiraJogada { get; set; }

        Image imgTmp;

        public MainWindow()
        {
            this.PrimeiraJogada = true;
            this.Campo = new Button[5, 10];
            this.MatrizDeValores = new string[5, 10];
            this.DimensaoDaLinha = 5;
            this.DimensaoDaColuna = 10;
            this.EspacoSemBombas = 0;
            this.BombasMarcadas = 0;
            this.BombaPrincipaly = 0;
            this.BombaPrincipalx = 0;
            this.Fim = false;
            this.Imagens = new Image[50];
            this.Valores = new string[5, 10];
            InitializeComponent();

            imgTmp = (Image)this.FindName("img3");

            InserirBombas();
            FinalizarCampo();
            FecharCampo();

            //InitializeComponent();
        }

        public void FecharCampo()
        {
            int cont = 0;

            for (int i = 0; i < DimensaoDaLinha; i++)
            {
                for (int j = 0; j < DimensaoDaColuna; j++)
                {
                    this.Campo[i, j] = this.FindName("btn" + cont) as Button;
                    string codigo = Campo[i, j].Name.Substring(3);
                    this.Valores[i, j] = "";
                    //Image img = (Image)this.FindName("img" + codigo);
                    //img.Source = new BitmapImage(new Uri("Images/CampoFechado.png", UriKind.Relative));
                    //this.Imagens[cont] = img;
                    this.Campo[i, j].Background = new SolidColorBrush(Colors.Gray);

                    cont++;
                }
            }
        }

        public void NovoJogo()
        {
            //int cont = 0;
            for (int i = 0; i < DimensaoDaLinha; i++)
            {
                for (int j = 0; j < DimensaoDaColuna; j++)
                {
                    this.Valores[i, j] = "";
                    Campo[i, j].Content = string.Empty;
                    Campo[i, j].IsEnabled = true;
                    //Campo[i, j].Background = new SolidColorBrush(Colors.Gray);
                    //string codigo = Campo[i, j].Name.Substring(3);
                    //this.Imagens[cont].Source = new BitmapImage(new Uri("Images/CampoFechado.png", UriKind.Relative));
                    //cont++;
                    this.Campo[i, j].Background = new SolidColorBrush(Colors.Gray);
                }
            }
        }

        public void BtnClickButton(object sender, RoutedEventArgs e)
        {
            int qtdBombasMarcadas = 0;
            var btn = sender as Button;
            int i = int.Parse(btn.GetValue(Grid.RowProperty).ToString());
            int j = int.Parse(btn.GetValue(Grid.ColumnProperty).ToString());


            if (!btn.IsEnabled && !this.Valores[i, j].Equals("*") && !this.Valores[i, j].Equals("@") && !Fim)
            {
                qtdBombasMarcadas = VerificaQuantidadeDeBombasMarcadas(i, j);
                if (qtdBombasMarcadas == int.Parse(this.Valores[i, j]))
                {
                    //campo[i][j].setText("a");
                    BotaoDoMeio(i, j);
                }
            }

            else if (btn.Name.Equals("btnNovo"))
            {
                //MessageBox.Show("Jogo reiniciado!");
                NovoJogo();
                InserirBombas();
                FinalizarCampo();
                BombasMarcadas = 0;
                EspacoSemBombas = 40;
                Fim = false;
                this.PrimeiraJogada = true;
            }
            else if (!Fim && !this.Valores[i, j].Equals("@"))
            {

                if (!MatrizDeValores[i, j].Equals("*"))
                {
                    if (int.Parse(MatrizDeValores[i, j]) > 0)
                    {
                        btn.Content = "" + MatrizDeValores[i, j];
                        Valores[i, j] = "" + MatrizDeValores[i, j];
                        btn.IsEnabled = false;
                        EspacoSemBombas -= 1;
                        if (EspacoSemBombas == 0)
                        {
                            MessageBox.Show("Parabéns Você Ganhou!");
                            Fim = true;
                        }

                    }
                    else
                    {
                        if (this.Valores[i, j].Equals("@"))
                        {
                            BombasMarcadas--;
                            //string codigo = btn.Name.Substring(3);
                            //Image imagem = this.Imagens[int.Parse(codigo)];
                            //imagem.Source = new BitmapImage(new Uri("Images/CampoFechado.png", UriKind.Relative));
                            this.Campo[i, j].Background = new SolidColorBrush(Colors.Gray);
                        }

                        this.Valores[i, j] = "";
                        Campo[i, j].Content = "";
                        btn.IsEnabled = false;

                        EspacoSemBombas -= 1;
                        if (EspacoSemBombas == 0)
                        {
                            MessageBox.Show("Parabéns Você Ganhou!");
                            Fim = true;
                        }
                        Explodir(i, j);
                    }
                    return;
                }
                else
                {
                    if (BombasMarcadas < 10)
                    {
                        //btn.Content = "*";
                        //string codigo = btn.Name.Substring(3);
                        //Image img = this.Imagens[int.Parse(codigo)];
                        //img.Source = new BitmapImage(new Uri("Images/bombavermelha.png", UriKind.Relative));
                        btn.Background = new SolidColorBrush(Colors.Red);

                        if (!Fim)
                        {
                            BombaPrincipalx = i;
                            BombaPrincipaly = j;
                            MessageBox.Show("OPS! Você perdeu!");
                            Fim = true;
                            AbreJogo();
                            return;
                        }
                    }
                }
            }
        }

        public void Hold(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            int i = int.Parse(btn.GetValue(Grid.RowProperty).ToString());
            int j = int.Parse(btn.GetValue(Grid.ColumnProperty).ToString());

            if (btn.IsEnabled && !Fim)
            {
                if (this.Valores[i, j].Equals("@"))
                {
                    this.Valores[i, j] = "";
                    Campo[i, j].Content = "";
                    //string codigo = btn.Name.Substring(3);
                    //Image imagem = this.Imagens[int.Parse(codigo)];
                    //imagem.Source = new BitmapImage(new Uri("Images/CampoFechado.png", UriKind.Relative));
                    Campo[i, j].Background = new SolidColorBrush(Colors.Gray);
                    BombasMarcadas--;
                }
                else
                {
                    //string codigo = btn.Name.Substring(3);
                    //Image imagem = this.Imagens[int.Parse(codigo)];
                    //imagem.Source = new BitmapImage(new Uri("Images/bandeira.png", UriKind.Relative));
                    Campo[i, j].Background = new SolidColorBrush(Colors.Green);
                    this.Valores[i, j] = "@";
                    BombasMarcadas++;
                }
            }
        }

        public void DoClick(int x, int y)
        {
            Button btn = Campo[x, y];
            int qtdBombasMarcadas = 0;
            int i = x;
            int j = y;

            if (!btn.IsEnabled && !this.Valores[i, j].Equals("*") && !this.Valores[i, j].Equals("@") && !Fim)
            {
                qtdBombasMarcadas = VerificaQuantidadeDeBombasMarcadas(i, j);
                if (qtdBombasMarcadas == int.Parse(this.Valores[i, j]))
                {
                    BotaoDoMeio(i, j);
                }   
            }

            else if (btn.Name.Equals(btnNovo))
            {
                //MessageBox.Show("Jogo reiniciado!");
                NovoJogo();
                InserirBombas();
                FinalizarCampo();
                BombasMarcadas = 0;
                EspacoSemBombas = 40;
                Fim = false;
            }
            else if (!Fim && !this.Valores[i, j].Equals("@"))
            {

                if (!MatrizDeValores[i, j].Equals("*"))
                {
                    if (int.Parse(MatrizDeValores[i, j]) > 0)
                    {
                        btn.Content = "" + MatrizDeValores[i, j];
                        this.Valores[i, j] = "" + MatrizDeValores[i, j];
                        btn.IsEnabled = false;
                        EspacoSemBombas -= 1;
                        if (EspacoSemBombas == 0)
                        {
                            MessageBox.Show("Parabéns Você Ganhou!");
                            Fim = true;
                        }

                    }
                    else
                    {
                        if (this.Valores[i, j].Equals("@"))
                        {
                            BombasMarcadas--;
                            //string codigo = btn.Name.Substring(3);
                            //Image imagem = this.Imagens[int.Parse(codigo)];
                            //imagem.Source = new BitmapImage(new Uri("Images/CampoFechado.png", UriKind.Relative));
                            this.Campo[i, j].Background = new SolidColorBrush(Colors.Gray);
                        }

                        this.Valores[i, j] = "";
                        Campo[i, j].Content = "";
                        btn.IsEnabled = false;

                        EspacoSemBombas -= 1;
                        if (EspacoSemBombas == 0)
                        {
                            MessageBox.Show("Parabéns Você Ganhou!");
                            Fim = true;
                        }
                        Explodir(i, j);
                    }
                    return;
                }
                else
                {
                    if (BombasMarcadas < 10)
                    {
                        this.Valores[i, j] = "*";
                        //string codigo = btn.Name.Substring(3);
                        //Image imagem = this.Imagens[int.Parse(codigo)];
                        //imagem.Source = new BitmapImage(new Uri("Images/bombavermelha.png", UriKind.Relative));

                        this.Campo[i, j].Background = new SolidColorBrush(Colors.Red);

                        if (!Fim)
                        {
                            BombaPrincipalx = i;
                            BombaPrincipaly = j;
                            MessageBox.Show("OPS! Você perdeu!");
                            Fim = true;
                            AbreJogo();
                            return;
                        }
                    }
                }
            }
        }

        public void BotaoDoMeio(int x, int y)
        {
            if (!Fim)
            {
                for (int i = (x - 1); i < (x + 2); i++)
                {
                    for (int j = (y - 1); j < (y + 2); j++)
                    {
                        if ((i >= 0) && (j >= 0) && (i < DimensaoDaLinha) && (j < DimensaoDaColuna))
                        {
                            if ((i != x) || (j != y))
                            {
                                if (Campo[i, j].IsEnabled)
                                {

                                    if (this.Valores[i, j].Equals("@") && MatrizDeValores[i, j].Equals("*"))
                                    {
                                    }
                                    else if (this.Valores[i, j].Equals("@") && !MatrizDeValores[i, j].Equals("*"))
                                    {

                                    }
                                    else if (!this.Valores[i, j].Equals("@") && !MatrizDeValores[i, j].Equals("*"))
                                    {
                                        if (int.Parse(MatrizDeValores[i, j]) == 0)
                                        {
                                            Campo[i, j].IsEnabled = false;
                                            EspacoSemBombas -= 1;
                                            if (EspacoSemBombas == 0)
                                            {
                                                MessageBox.Show("Parabéns Você Ganhou!");
                                                Fim = true;
                                            }
                                            Explodir(i, j);
                                        }
                                        else
                                        {
                                            Campo[i, j].Content = MatrizDeValores[i, j];
                                            this.Valores[i, j] = MatrizDeValores[i, j];
                                            Campo[i, j].IsEnabled = false;
                                            EspacoSemBombas -= 1;
                                            if (EspacoSemBombas == 0)
                                            {
                                                MessageBox.Show("Parabéns Você Ganhou!");
                                                Fim = true;
                                            }
                                        }
                                    }
                                    else if (!this.Valores[i, j].Equals("@") && MatrizDeValores[i, j].Equals("*"))
                                    {
                                        this.Valores[i, j] = "*";
                                        //string codigo = Campo[i, j].Name.Substring(3);
                                        //Image imagem = this.Imagens[int.Parse(codigo)];
                                        //imagem.Source = new BitmapImage(new Uri("Images/bombavermelha.png", UriKind.Relative));
                                        this.Campo[i, j].Background = new SolidColorBrush(Colors.Red);
                                        DoClick(i, j);
                                        //campo[i][j].setEnabled(false);
                                        if (!Fim)
                                        {
                                            MessageBox.Show("OPS! Você perdeu!");
                                            BombaPrincipalx = i;
                                            BombaPrincipaly = j;
                                            Fim = true;
                                            AbreJogo();
                                            return;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        public void InserirBombas()
        {
            int i = 0;
            int j = 0;
            int qtdBombas = 0;
            Random rnd = new Random();

            for (int x = 0; x < DimensaoDaLinha; x++)
            {
                for (int y = 0; y < DimensaoDaColuna; y++)
                {
                    MatrizDeValores[x, y] = ".";
                    this.EspacoSemBombas += 1;
                }
            }

            while (true)
            {
                i = rnd.Next(5);
                j = rnd.Next(10);

                if (!MatrizDeValores[i, j].Equals("*"))
                {
                    if (qtdBombas == 10)
                    {
                        break;
                    }
                    MatrizDeValores[i, j] = "*";
                    EspacoSemBombas -= 1;
                    qtdBombas++;
                }
            }
        }

        public void FinalizarCampo()
        {
            for (int i = 0; i < DimensaoDaLinha; i++)
            {
                for (int j = 0; j < DimensaoDaColuna; j++)
                {
                    if (!MatrizDeValores[i, j].Equals("*"))
                    {
                        MatrizDeValores[i, j] = (VerificaQuantidadeDeBombas(i, j).ToString());
                    }
                }
            }
        }

        public void Explodir(int x, int y)
        {
            for (int i = (x - 1); i < (x + 2); i++)
            {
                for (int j = (y - 1); j < (y + 2); j++)
                {
                    if ((i >= 0) && (j >= 0) && (i < DimensaoDaLinha) && (j < DimensaoDaColuna))
                    {
                        if (((i != x) || (j != y)) && Campo[i, j].IsEnabled)      //)&& !MatrizDeValores[i][j].equals("0")
                        {
                            if (MatrizDeValores[i, j].Equals("0"))
                            {
                                if (this.Valores[i, j].Equals("@"))
                                {
                                    BombasMarcadas--;
                                    //string codigo = Campo[i, j].Name.Substring(3);
                                    //Image imagem = this.Imagens[int.Parse(codigo)];
                                    //imagem.Source = new BitmapImage(new Uri("Images/CampoFechado.png", UriKind.Relative));
                                    this.Campo[i, j].Background = new SolidColorBrush(Colors.Gray);
                                }
                                this.Valores[i, j] = "";
                                Campo[i, j].Content = "";
                                Campo[i, j].IsEnabled = false;
                                EspacoSemBombas -= 1;
                                if (EspacoSemBombas == 0)
                                {
                                    MessageBox.Show("Parabéns Você Ganhou!");
                                    Fim = true;
                                }
                                Explodir(i, j);

                            }
                            else
                            {
                                this.Valores[i, j] = MatrizDeValores[i, j];
                                Campo[i, j].Content = MatrizDeValores[i, j];
                                Campo[i, j].IsEnabled = false;
                                EspacoSemBombas -= 1;
                                if (EspacoSemBombas == 0)
                                {
                                    MessageBox.Show("Parabéns Você Ganhou!");
                                    Fim = true;
                                }

                            }
                        }
                    }
                }
            }
        }

        public void AbreJogo()
        {
            for (int e = 0; e < DimensaoDaLinha; e++)
            {
                for (int d = 0; d < DimensaoDaColuna; d++)
                {
                    if (e == BombaPrincipalx && d == BombaPrincipaly)
                    {

                    }
                    else if (MatrizDeValores[e, d].Equals("*"))
                    {

                        if (!this.Valores[e, d].Equals("@"))
                        {
                            //string codigo = Campo[e, d].Name.Substring(3);
                            //Image imagem = this.Imagens[int.Parse(codigo)];
                            //imagem.Source = new BitmapImage(new Uri("Images/bomba.png", UriKind.Relative));
                            this.Campo[e, d].Background = new SolidColorBrush(Colors.Orange);
                            this.Valores[e, d] = "*";
                            DoClick(e, d);
                        }
                        else
                        {
                            //string codigo = Campo[e, d].Name.Substring(3);
                            //Image imagem = this.Imagens[int.Parse(codigo)];
                            //imagem.Source = new BitmapImage(new Uri("Images/bandeiracombomba.png", UriKind.Relative));
                            this.Campo[e, d].Background = new SolidColorBrush(Colors.Orange);
                        }
                    }
                    else if (this.Valores[e, d].Equals("@"))
                    {
                        //campo[e][d].setEnabled(false);	
                        //string codigo = Campo[e, d].Name.Substring(3);
                        //Image imagem = this.Imagens[int.Parse(codigo)];
                        //imagem.Source = new BitmapImage(new Uri("Images/bombaErrada.png", UriKind.Relative));
                        this.Campo[e, d].Background = new SolidColorBrush(Colors.Purple);
                    }
                }
            }
        }

        public int VerificaQuantidadeDeBombas(int a, int b)
        {
            int auxa = a;
            int auxb = b;
            int qtdBombas = 0;

            for (int i = (a - 1); i < (auxa + 2); i++)
            {
                for (int j = (b - 1); j < (auxb + 2); j++)
                {
                    if ((i >= 0) && (j >= 0) && (i < DimensaoDaLinha) && (j < DimensaoDaColuna))
                    {
                        if ((i == auxa) && (j == auxb))
                        {

                        }
                        else if (MatrizDeValores[i, j].Equals("*"))
                        {
                            qtdBombas++;
                        }
                    }
                }
            }
            return qtdBombas;
        }

        public int VerificaQuantidadeDeBombasMarcadas(int a, int b)
        {
            int bombas = 0;

            for (int i = (a - 1); i < (a + 2); i++)
            {
                for (int j = (b - 1); j < (b + 2); j++)
                {
                    if ((i >= 0) && (j >= 0) && (i < DimensaoDaLinha) && (j < DimensaoDaColuna))
                    {
                        if ((i == a) && (j == b))
                        {

                        }
                        else
                        {
                            if (this.Valores[i, j].Equals("@"))
                            {
                                bombas++;
                            }
                        }
                    }
                }
            }
            //campo[a][b].setText(""+bombas);
            return bombas;
        }

        private void btn0_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var btn = sender as Button;
            int i = int.Parse(btn.GetValue(Grid.RowProperty).ToString());
            int j = int.Parse(btn.GetValue(Grid.ColumnProperty).ToString());

            if (e.MiddleButton == MouseButtonState.Pressed)
            {
                this.BtnClickButton(e, new RoutedEventArgs());
            }
            else if (e.RightButton == MouseButtonState.Pressed)
            {
                Hold(sender, e);
            }
            else
            {
                BotaoDoMeio(i, j);
            }
        }
    }
}
