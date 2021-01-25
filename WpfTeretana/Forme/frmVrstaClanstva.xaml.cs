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
    /// Interaction logic for frmVrstaClanstva.xaml
    /// </summary>
    public partial class frmVrstaClanstva : Window
    {
        SqlConnection konekcija = Konekcija.KreirajKonekciju();

        public frmVrstaClanstva()
        {
            InitializeComponent();
            txtNazivVrsteClanstva.Focus();
        }

        private void btnSacuvaj_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                konekcija.Open();
                if (MainWindow.azuriraj)
                {
                    DataRowView red = MainWindow.pomocniRed;

                    string update = @"update tblVrstaClanstva set NazivVrsteClanstva='" + txtNazivVrsteClanstva.Text + "',MuskaClanarina='" + Convert.ToInt32(cbxMuskaClanarina.IsChecked) + "'," +
                        "ZenskaClanarina='" + Convert.ToInt32(cbxZenskaClanarina.IsChecked) + "',PenzionerskaClanarina='" + Convert.ToInt32(cbxPenzClanarina.IsChecked) + "'," +
                        "StudentskaClanarina='" + Convert.ToInt32(cbxStudClanarina.IsChecked) + "' where VrstaClanstvaID=" + red["ID"];
                    SqlCommand cmd = new SqlCommand(update, konekcija);
                    cmd.ExecuteNonQuery();
                    MainWindow.pomocniRed = null;

                }
                else
                {

                    string insert = @"insert into tblVrstaClanstva(NazivVrsteClanstva,MuskaClanarina,ZenskaClanarina,PenzionerskaClanarina,StudentskaClanarina) 
                                  values('" + txtNazivVrsteClanstva.Text + "','" + Convert.ToInt32(cbxMuskaClanarina.IsChecked) + "','"
                                      + Convert.ToInt32(cbxZenskaClanarina.IsChecked) + "','"
                                      + Convert.ToInt32(cbxPenzClanarina.IsChecked) + "'," +
                                      "'" + Convert.ToInt32(cbxStudClanarina.IsChecked) + "') ";
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
