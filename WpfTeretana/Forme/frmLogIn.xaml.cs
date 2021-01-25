using System;
using System.Collections.Generic;
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
    /// Interaction logic for frmLogIn.xaml
    /// </summary>
    public partial class frmLogIn : Window
    {
        SqlConnection konekcija = Konekcija.KreirajKonekciju();
        public frmLogIn()
        {
            InitializeComponent();
            txtLogMail.Focus();
        }

        

        private bool postojiEmailZaposlenog(string EmailZaposlenog)
       
        {
            try
            {
                konekcija.Open();
                string upitKorisnickoIme = @"select count(*) from tblZaposleni where EmailZaposlenog='" + txtLogMail.Text + "'";

                SqlCommand cmdKorisnickoIme = new SqlCommand(upitKorisnickoIme, konekcija);

                int rezultatUpita = Convert.ToInt32(cmdKorisnickoIme.ExecuteScalar());              

                if (rezultatUpita > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Greška!");
                return false;
            }
            finally
            {
                if (konekcija != null)
                {
                    konekcija.Close();
                }
            }
        }


        private bool ispravnaLozinka(string EmailZaposlenog, string unetaLozinka)

        {
            try
            {
                konekcija.Open();
                string upitLozinka = @"select Lozinka from tblZaposleni where EmailZaposlenog='" + txtLogMail.Text + "'";
                SqlCommand cmd = new SqlCommand(upitLozinka, konekcija);
                string lozinkaIzBaze = cmd.ExecuteScalar().ToString();

                if (String.Equals(lozinkaIzBaze, unetaLozinka))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                if (konekcija != null)
                {
                    konekcija.Close();
                }
            }
        }


        private void btnUlogujSe_Click(object sender, RoutedEventArgs e)
        {
            try
            {


                if (string.IsNullOrEmpty(txtLogMail.Text))
                {
                    MessageBox.Show("E-mail nije unet!",
                                    "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                    txtLogMail.Focus();
                    return;
                }
                if (string.IsNullOrEmpty(txtLogPass.Password))
                {
                    MessageBox.Show("Lozinka nije uneta!",
                                    "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                    txtLogPass.Focus();
                    return;
                }


                string unetiEmail = txtLogMail.Text;
                string unetaLozinka = txtLogPass.Password;

                if (postojiEmailZaposlenog(unetiEmail) == false)
                {
                    MessageBox.Show("Uneti E-mail ne postoji u bazi podataka! Dodajte novi nalog! ",
                                    "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                    txtLogMail.Focus();
                    return;
                }

                if (ispravnaLozinka(unetiEmail, unetaLozinka) == false)
                {
                    MessageBox.Show("Lozinka nije ispravna! Pokušajte ponovo!",
                    "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                    txtLogPass.Focus();
                    return;
                }


                MainWindow main = new MainWindow();
                main.Show();
                this.Close();

            }
            catch (Exception)
            {
                MessageBox.Show("Doslo je do greške!",
                                "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void btnOtkazi_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


        private void btnNoviNalog_Click(object sender, RoutedEventArgs e)
        {

            Window prozor = new frmZaposleni();
            prozor.ShowDialog();

            
        }
    }
}
