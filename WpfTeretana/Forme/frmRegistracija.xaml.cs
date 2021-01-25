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
    /// Interaction logic for frmRegistracija.xaml
    /// </summary>
    public partial class frmRegistracija : Window
    {
        SqlConnection konekcija = Konekcija.KreirajKonekciju();
        public frmRegistracija()
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

                string vratiClanstva = @"select ClanstvoID,Cast( ClanstvoID as nvarchar(20)) +' '+tblKorisnik.ImeKorisnika + ' '+ tblKorisnik.PrezimeKorisnika as Clanstvo from tblClanstvo join tblKorisnik on tblKorisnik.KorisnikID=tblClanstvo.KorisnikID";
                DataTable dtClanstva = new DataTable();
                SqlDataAdapter daClanstva = new SqlDataAdapter(vratiClanstva, konekcija);
                daClanstva.Fill(dtClanstva);
                cbClanstvo.ItemsSource = dtClanstva.DefaultView;
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
        }

        private void btnSacuvaj_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                konekcija.Open();
                if (MainWindow.azuriraj)
                {
                    DataRowView red = MainWindow.pomocniRed;

                    DateTime datum = (DateTime)dpDatumRegistracije.SelectedDate;
                    string sdatum = datum.ToString("yyyy-MM-dd HH:mm:ss");

                    string update = @"update tblRegistracija set DatumRegistracije='" + sdatum + "',CijenaRegistracije='" + txtCijenaRegistracije.Text + "'," +
                        "NoviClan='" + Convert.ToInt32(cbxNoviClan.IsChecked) + "',ZaposleniID='" + cbZaposleni.SelectedValue + "'," +
                        "ClanstvoID='" + cbClanstvo.SelectedValue + "' where RegistracijaID="+red["ID"];

                    SqlCommand cmd = new SqlCommand(update, konekcija);
                    cmd.ExecuteNonQuery();
                    MainWindow.pomocniRed = null;
                }
                else {
                    DateTime datum = (DateTime)dpDatumRegistracije.SelectedDate;
                    string sdatum = datum.ToString("yyyy-MM-dd HH:mm:ss");
                    string insert = @"insert into tblRegistracija(DatumRegistracije,CijenaRegistracije,NoviClan,ZaposleniID,ClanstvoID)
                                values('" + sdatum + "','" + txtCijenaRegistracije.Text + "','" + Convert.ToInt32(cbxNoviClan.IsChecked) + "'," +
                                    "'" + cbZaposleni.SelectedValue + "','" + cbClanstvo.SelectedValue + "')";
                    SqlCommand cmd = new SqlCommand(insert, konekcija);
                    cmd.ExecuteNonQuery();
                }
                this.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Unos određenih podataka nije validan! Molimo vas pokušajte ponovo!",
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
