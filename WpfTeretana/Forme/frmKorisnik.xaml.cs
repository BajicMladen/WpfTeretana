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
    /// Interaction logic for frmKorisnik.xaml
    /// </summary>
    public partial class frmKorisnik : Window
    {
        SqlConnection konekcija = Konekcija.KreirajKonekciju();
        public frmKorisnik()
        {
            
            InitializeComponent();

            txtImeKorsinika.Focus();
        }

        private void btnSacuvaj_Click(object sender, RoutedEventArgs e)
        {
            try {

                konekcija.Open();

                if (MainWindow.azuriraj)
                {
                    DataRowView red = MainWindow.pomocniRed;
                    string update = @"update tblKorisnik
                                            set ImeKorisnika='" + txtImeKorsinika.Text + "', PrezimeKorisnika='" + txtPrezimeKorisnika.Text + "',JMBGKorisnika='" + txtJMBGKorisnika.Text + "'," +
                                            "TelefonKorisnika='" + txtTelefonKorisnika.Text + "',AdresaKorisnika='" + txtAdresaKorisnika.Text + "',PolKorisnika='" + txtPolKorisnika.Text + "',EmailKorisnika='" + txtEmailKorisnika.Text + "'" +
                                            "where KorisnikID="+red["ID"];
                    SqlCommand cmd = new SqlCommand(update, konekcija);
                    cmd.ExecuteNonQuery();
                    MainWindow.pomocniRed = null;
                }
                else
                {

                    
                    string insert = @"insert into tblKorisnik(ImeKorisnika,PrezimeKorisnika,JMBGKorisnika,TelefonKorisnika,AdresaKorisnika,PolKorisnika,EmailKorisnika) 
                                 values('" + txtImeKorsinika.Text + "','" + txtPrezimeKorisnika.Text + "','" + txtJMBGKorisnika.Text + "','" + txtTelefonKorisnika.Text + "'," +
                                     "'" + txtAdresaKorisnika.Text + "','" + txtPolKorisnika.Text + "','" + txtEmailKorisnika.Text + "');";

                    SqlCommand cmd = new SqlCommand(insert, konekcija);
                    cmd.ExecuteNonQuery();
                }
                this.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Unos određenih podataka nije validan! Molimo Vas pokušajte ponovo!",
                    "Greska", MessageBoxButton.OK, MessageBoxImage.Error);
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
