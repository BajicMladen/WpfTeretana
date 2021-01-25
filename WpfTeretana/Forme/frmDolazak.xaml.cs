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
    /// Interaction logic for frmDolazak.xaml
    /// </summary>
    public partial class frmDolazak : Window
    {
        SqlConnection konekcija = Konekcija.KreirajKonekciju();
        public frmDolazak()
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

                string vratiKorisnike = @"select KorisnikID,ImeKorisnika +' '+ PrezimeKorisnika +' '+ JMBGKorisnika as Korisnik from tblKorisnik";
                DataTable dtKorisnik = new DataTable();
                SqlDataAdapter daKorisnik = new SqlDataAdapter(vratiKorisnike, konekcija);
                daKorisnik.Fill(dtKorisnik);
                cbKorisnik.ItemsSource = dtKorisnik.DefaultView;
            }
            catch(Exception)
            {
                MessageBox.Show("Padajuće liste nisu popunjene!", "Greška!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                if (konekcija != null)
                {
                    konekcija.Close();
                }
            }

            txtBrojKljuca.Focus();
        }

        private void btnSacuvaj_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                konekcija.Open();
                if (MainWindow.azuriraj)
                {
                    DataRowView red = MainWindow.pomocniRed;
                    string update=@"update tblDolazak set BrojKljuca='"+txtBrojKljuca.Text+"',ZaposleniID='"+cbZaposleni.SelectedValue+"'," +
                        "KorisnikID='"+cbKorisnik.SelectedValue+"' where DolazakID=" + red["ID"];

                    SqlCommand cmd = new SqlCommand(update, konekcija);
                    cmd.ExecuteNonQuery();
                    MainWindow.pomocniRed = null;

                }
                else {

                    string insert = @"insert into tblDolazak(BrojKljuca,ZaposleniID,KorisnikID)
                                values('" + txtBrojKljuca.Text + "','" + cbZaposleni.SelectedValue + "','" + cbKorisnik.SelectedValue + "')";
                    SqlCommand cmd = new SqlCommand(insert, konekcija);
                    cmd.ExecuteNonQuery();
                }
                this.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Unos određenih podataka nije validan! Molimo Vas pokušajte ponovo!","Greska!",MessageBoxButton.OK,MessageBoxImage.Error);
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
