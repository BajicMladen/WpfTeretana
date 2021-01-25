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
    /// Interaction logic for frmProgram.xaml
    /// </summary>
    public partial class frmProgram : Window
    {
        SqlConnection konekcija = Konekcija.KreirajKonekciju();

        public frmProgram()
        {
            InitializeComponent();

            try
            {
                string vratiKorisnike = @"select KorisnikID,ImeKorisnika +' '+ PrezimeKorisnika +' '+JMBGKorisnika as Korisnik from tblKorisnik";
                DataTable dtKorisnik = new DataTable();
                SqlDataAdapter daKorisnik = new SqlDataAdapter(vratiKorisnike, konekcija);
                daKorisnik.Fill(dtKorisnik);
                cbKorisnik.ItemsSource = dtKorisnik.DefaultView;

                string vratiZaposlene = @"select ZaposleniID,ImeZaposlenog +' '+ PrezimeZaposlenog as Zaposleni from tblZaposleni where Trener='1'";
                DataTable dtZaposleni = new DataTable();
                SqlDataAdapter daZaposleni = new SqlDataAdapter(vratiZaposlene, konekcija);
                daZaposleni.Fill(dtZaposleni);
                cbZaposleni.ItemsSource = dtZaposleni.DefaultView;

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

            txtVrstaPrograma.Focus();

        }

        private void btnSacuvaj_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                konekcija.Open();
                if (MainWindow.azuriraj)
                {
                    DataRowView red = MainWindow.pomocniRed;

                    string update=@"update tblProgram set VrstaPrograma='"+txtVrstaPrograma.Text+"',CijenaPrograma='"+txtCijenaPrograma.Text+"'," +
                        "KorisnikID='"+cbKorisnik.SelectedValue+"',ZaposleniID='"+cbZaposleni.SelectedValue+"' where ProgramID=" +red["ID"];

                    SqlCommand cmd = new SqlCommand(update, konekcija);
                    cmd.ExecuteNonQuery();
                    MainWindow.pomocniRed = null;
                }
                else
                {
                    string insert = @"insert into tblProgram(VrstaPrograma,CijenaPrograma,KorisnikID,ZaposleniID)
                                values('" + txtVrstaPrograma.Text + "','" + txtCijenaPrograma.Text + "'," +
                               "'" + cbKorisnik.SelectedValue + "','" + cbZaposleni.SelectedValue + "')";
                    SqlCommand cmd = new SqlCommand(insert, konekcija);
                    cmd.ExecuteNonQuery();
                }
                this.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Unos određenih podataka nije validan! Molimo Vas poušajte ponovo!",
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
