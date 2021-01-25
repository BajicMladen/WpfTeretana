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
    /// Interaction logic for frmClanstvo.xaml
    /// </summary>
    public partial class frmClanstvo : Window
    {
        SqlConnection konekcija = Konekcija.KreirajKonekciju();

        public frmClanstvo()
        {
            InitializeComponent();

            try
            {
                konekcija.Open();


                string vratiVrsteClanstva = @"select VrstaClanstvaID,NazivVrsteClanstva from tblVrstaClanstva";
                DataTable dtVrstaClanstva = new DataTable();
                SqlDataAdapter daVrstaClanstva = new SqlDataAdapter(vratiVrsteClanstva, konekcija);
                daVrstaClanstva.Fill(dtVrstaClanstva);
                cbVrstaClanstva.ItemsSource = dtVrstaClanstva.DefaultView;

                string vratiKorisnike = @"select KorisnikID,ImeKorisnika + ' ' + PrezimeKorisnika +' '+JMBGKorisnika as Korisnik from tblKorisnik";
                DataTable dtKorisnik = new DataTable();
                SqlDataAdapter daKorisnik = new SqlDataAdapter(vratiKorisnike, konekcija);
                daKorisnik.Fill(dtKorisnik);
                cbKorisnik.ItemsSource = dtKorisnik.DefaultView;

          
            }
            catch (Exception) {

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

                    DateTime datum1 = (DateTime)dpDatumIsteka.SelectedDate;
                    DateTime datum2 = (DateTime)dpDatumPocetka.SelectedDate;
                    string sdatum1 = datum2.ToString("yyyy-MM-dd HH:mm:ss");
                    string sdatum2 = datum1.ToString("yyyy-MM-dd HH:mm:ss");

                    string update=@"update tblClanstvo set DatumPocetka='"+sdatum1+"',DatumIsteka='"+sdatum2+"'," +
                        "KorisnikID='"+cbKorisnik.SelectedValue+"',VrstaClanstvaID='"+cbVrstaClanstva.SelectedValue+"' where ClanstvoID="+red["ID"];

                    SqlCommand cmd = new SqlCommand(update, konekcija);
                    cmd.ExecuteNonQuery();
                    MainWindow.pomocniRed = null;
                }
                else {

                    DateTime datum1 = (DateTime)dpDatumIsteka.SelectedDate;
                    DateTime datum2 = (DateTime)dpDatumPocetka.SelectedDate;
                    string sdatum1 = datum2.ToString("yyyy-MM-dd HH:mm:ss");
                    string sdatum2 = datum1.ToString("yyyy-MM-dd HH:mm:ss");

                    string insert = @"insert into tblClanstvo(DatumPocetka,DatumIsteka,KorisnikID,VrstaClanstvaID)
                                values('" + sdatum1 + "','" + sdatum2 + "','" + cbKorisnik.SelectedValue + "','" + cbVrstaClanstva.SelectedValue + "')";
                    SqlCommand cmd = new SqlCommand(insert, konekcija);
                    cmd.ExecuteNonQuery();
                }       
                this.Close();

            }
            catch (Exception)
            {
                MessageBox.Show("Unos određenih podataka nije validan!", "Greška!", MessageBoxButton.OK, MessageBoxImage.Error);
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
