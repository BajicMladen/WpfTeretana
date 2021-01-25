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
    /// Interaction logic for frmZaposleni.xaml
    /// </summary>
    public partial class frmZaposleni : Window
    {
        SqlConnection konekcija = Konekcija.KreirajKonekciju();
        
        public frmZaposleni()
        {
            InitializeComponent();
            txtImeZaposlenog.Focus();
        }

        private void btnSacuvaj_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                konekcija.Open();
                if (MainWindow.azuriraj)
                {
                    DataRowView red = MainWindow.pomocniRed;

                    string update = @"update tblZaposleni set ImeZaposlenog='" + txtImeZaposlenog.Text + "',PrezimeZaposlenog='" + txtPrezimeZaposlenog.Text + "'," +
                        "Trener='" + Convert.ToInt32(cbxTrener.IsChecked) + "',AdresaZaposlenog='" + txtAdresaZaposlenog.Text + "',TelefonZaposlenog='" + txtTelefonZaposlenog.Text + "'," +
                        "EmailZaposlenog='" + txtEmailZaposlenog.Text + "',Lozinka='" + txtLozinkaZaposlenog.Text + "' where ZaposleniID=" + red["ID"];

                    SqlCommand cmd = new SqlCommand(update, konekcija);
                    cmd.ExecuteNonQuery();
                    MainWindow.pomocniRed = null;
                }
                else {
                    string insert = @"insert into tblZaposleni(ImeZaposlenog,PrezimeZaposlenog,Trener,AdresaZaposlenog,TelefonZaposlenog,EmailZaposlenog,Lozinka)
                                  values('" + txtImeZaposlenog.Text + "','" + txtPrezimeZaposlenog.Text + "','" + Convert.ToInt32(cbxTrener.IsChecked) + "'," +
                                      "'" + txtAdresaZaposlenog.Text + "','" + txtTelefonZaposlenog.Text + "','" + txtEmailZaposlenog.Text + "','" + txtLozinkaZaposlenog.Text + "');";
                    SqlCommand cmd = new SqlCommand(insert, konekcija);
                    cmd.ExecuteNonQuery();
                }
                this.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Određeni podaci nisu uneti!", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
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
