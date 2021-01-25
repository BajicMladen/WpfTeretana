using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
using System.Windows.Shapes;

namespace WpfTeretana.Forme
{
    /// <summary>
    /// Interaction logic for frmOprema.xaml
    /// </summary>
    public partial class frmOprema : Window
    {
        SqlConnection konekcija = Konekcija.KreirajKonekciju();
        public frmOprema()
        {
            InitializeComponent();

            try
            {
                konekcija.Open();

                string vratiZaposlene = @"select ZaposleniID,ImeZaposlenog +' '+ PrezimeZaposlenog as Zaposleni from tblZaposleni";
                DataTable dtZaposleni = new DataTable();
                SqlDataAdapter daZaposleni = new SqlDataAdapter(vratiZaposlene, konekcija);
                daZaposleni.Fill(dtZaposleni);
                cbZaposleni.ItemsSource = dtZaposleni.DefaultView;
            }
            catch(Exception)
            {
                MessageBox.Show("Padajuća lista nije popunjena!", "Greška!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                if (konekcija != null)
                {
                    konekcija.Close();
                }
            }


            txtCijenaOpreme.Focus();
        }

        private void btnSacuvaj_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                konekcija.Open();
                if (MainWindow.azuriraj)
                {
                    DataRowView red = MainWindow.pomocniRed;

                    string update=@"update tblOprema set CijenaOpreme='"+txtCijenaOpreme.Text+"',VrstaOpreme='"+txtVrstaOpreme.Text+"'," +
                        "NazivOpreme='"+txtNazivOpreme.Text+"',ZaposleniID='"+cbZaposleni.SelectedValue+"' where OpremaID=" + red["ID"];
                    SqlCommand cmd = new SqlCommand(update, konekcija);
                    cmd.ExecuteNonQuery();
                    MainWindow.pomocniRed = null;

                }
                else {
                    string insert = @" insert into tblOprema(CijenaOpreme,VrstaOpreme,NazivOpreme,ZaposleniID)
                                values('" + txtCijenaOpreme.Text + "','" + txtVrstaOpreme.Text + "','"
                                    + txtNazivOpreme.Text + "','" + cbZaposleni.SelectedValue + "')";
                    SqlCommand cmd = new SqlCommand(insert, konekcija);
                    cmd.ExecuteNonQuery();
                    }
                this.Close();

            }
            catch (Exception)
            {
                MessageBox.Show("Unos određenih podataka nije validan! Molimo vas pokušajte ponovo",
                   "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                if (konekcija != null)
                {
                    konekcija.Close();
                }
            }
        }

        private void btnOtkazi_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
