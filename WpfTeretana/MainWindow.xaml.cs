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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using WpfTeretana.Forme;

namespace WpfTeretana
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public static string ucitanaTabela;
        public static bool azuriraj;
        public static DataRowView pomocniRed;
        static SqlConnection konekcija = Konekcija.KreirajKonekciju();
        public static string upit;

        #region Select Upiti
        static string clanstvoSelect = @"select ClanstvoID as ID, Convert(nvarchar(10),DatumPocetka,103) as 'Datum početka', Convert(nvarchar(10),DatumIsteka,103) as 'Datum isteka',ImeKorisnika+' '+PrezimeKorisnika
                                         as 'Korisnik',NazivVrsteClanstva as 'VrstaClanstva'
                                                from tblClanstvo join tblKorisnik on tblKorisnik.KorisnikID=tblClanstvo.KorisnikID
												join tblVrstaClanstva on tblVrstaClanstva.VrstaClanstvaID=tblClanstvo.VrstaClanstvaID ";

        static string dolazakSelect = @"select DolazakID as ID, BrojKljuca as 'Broj ključa', ImeKorisnika+' '+PrezimeKorisnika as 'Korisnik',
                                               ImeZaposlenog+' '+PrezimeZaposlenog as 'Zaposleni'
                                                        from tblDolazak join tblKorisnik on tblKorisnik.KorisnikID=tblDolazak.KorisnikID
                                                                        join tblZaposleni on tblZaposleni.ZaposleniID=tblDolazak.ZaposleniID ";

        static string zaposleniSelect = @"select ZaposleniID as ID, ImeZaposlenog as 'Ime',PrezimeZaposlenog as 'Prezime', Trener,
                                            AdresaZaposlenog as 'Adresa',TelefonZaposlenog as 'Telefon', EmailZaposlenog as 'E-mail'
                                                from  tblZaposleni ";

        static string korisnikSelect = @"select KorisnikID as ID,ImeKorisnika as 'Ime',PrezimeKorisnika as 'Prezime',JMBGKorisnika as 'JMBG',
                                                TelefonKorisnika as 'Telefon',AdresaKorisnika  as 'Adresa',PolKorisnika as 'Pol',
                                                EmailKorisnika as 'E-mail'
                                                    from tblKorisnik ";

        static string opremaSelect = @"select OpremaID as ID, NazivOpreme as 'Naziv',VrstaOpreme as 'Vrsta',CijenaOpreme as 'Cijena', ImeZaposlenog+' '+PrezimeZaposlenog as 'Prodao' 
                                              from tblOprema join tblZaposleni  on tblZaposleni.ZaposleniID=tblOprema.ZaposleniID ";

        static string programSelect = @"select ProgramID as ID, ImeKorisnika+' '+ PrezimeKorisnika as 'Polaznik',VrstaPrograma as 'Program',
                                               ImeZaposlenog+' '+PrezimeZaposlenog as 'Trener',CijenaPrograma as 'Cijena'
                                               from tblProgram join tblKorisnik on tblKorisnik.KorisnikID=tblProgram.KorisnikID
                                                               join tblZaposleni on tblZaposleni.ZaposleniID=tblProgram.ZaposleniID ";

        static string registracijaSelect = @"select RegistracijaID as ID, Cast(tblClanstvo.ClanstvoID as nvarchar(20)) +'-'+ tblKorisnik.ImeKorisnika +' '+ tblKorisnik.PrezimeKorisnika as 'Članstvo', Convert(nvarchar(10),DatumRegistracije,103) as 'Datum registracije',NoviClan as	'Novi član',
					CijenaRegistracije as 'Cijena', ImeZaposlenog+' '+PrezimeZaposlenog as 'Izvršio registraciju'
					from tblRegistracija join tblClanstvo on tblClanstvo.ClanstvoID=tblRegistracija.ClanstvoID
										join tblZaposleni on tblZaposleni.ZaposleniID=tblRegistracija.ZaposleniID
										join tblKorisnik on tblKorisnik.KorisnikID=tblClanstvo.KorisnikID ";

        static string vrstaClanstvaSelect = @"select VrstaClanstvaID as ID, NazivVrsteClanstva as 'Vrsta članstva',MuskaClanarina as 'Muška',ZenskaClanarina as 'Zenska',
		                                    PenzionerskaClanarina as 'Penzionerska',StudentskaClanarina as 'Studentska'
				                                    from tblVrstaClanstva ";

        #endregion

        #region Select sa uslovom

        string selectUslovKorisnik = @"select * from tblKorisnik where KorisnikID=";

        string selectUslovClanstvo = @"select * from tblClanstvo where ClanstvoID=";

        string selectUslovDolazak = @"select * from tblDolazak where DolazakID=";

        string selectUslovOprema = @"select * from tblOprema where OpremaID=";

        string selectuslovProgram = @"select * from tblProgram where ProgramID=";

        string selectUslovRegistracija = @"select * from tblRegistracija where RegistracijaID=";

        string selectUslovVrstaClanstva = @"select * from tblVrstaClanstva where VrstaClanstvaID=";

        string selectUslovZaposleni = @"select * from tblZaposleni where ZaposleniID=";

        #endregion

        #region Delete Upiti
        string deleteClanstvo = @"delete from tblClanstvo where ClanstvoID=";
        string deleteDolazak = @"delete from tblDolazak where DolazakID=";
        string deleteKorisnik = @"delete from tblKorisnik where KorisnikID=";
        string deleteOprema = @"delete from tblOprema where OpremaID=";
        string deleteProgram = @"delete from tblProgram where ProgramID=";
        string deleteRegistracija = @"delete from tblRegistracija where RegistracijaID=";
        string deleteVrstaClanstva = @"delete from tblVrstaClanstva where VrstaClanstvaID=";
        string deleteZaposleni = @"delete from tblZaposleni where ZaposleniID=";

        #endregion

        #region Select search


        string clanstvoSearch = @"Where DatumPocetka like ";
        string dolazakSearch = @"Where BrojKljuca like ";
        string ZaposleniSearch = @"Where ImeZaposlenog like ";
        string korisnikSearch = @"Where ImeKorisnika like ";
        string opremaSearch = @"Where NazivOpreme like ";
        string programSearch = @"Where VrstaPrograma like ";
        string registracijaSearch = @"Where DatumRegistracije like ";
        string vrstaClanstvaSearch = @"Where NazivVrsteClanstva like ";

        #endregion



        public MainWindow()
        {
            InitializeComponent();
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += timer_Tick;
            timer.Start();

            UcitajPodatke(DataGridCentralni, dolazakSelect);
        }

        void timer_Tick(object sender, EventArgs e)
        {
           
            lblDatum.Content = DateTime.Now.ToString();
        }

        public static void UcitajPodatke(DataGrid grid,string selectUpit)
        {
            try
            {
                konekcija.Open();
                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(selectUpit, konekcija);
                da.Fill(dt);
                grid.ItemsSource = dt.DefaultView;
                ucitanaTabela = selectUpit;
            }
            catch (Exception)
            {
                MessageBox.Show("Neuspešno učitani podaci!", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally {
                if (konekcija != null)
                {
                    konekcija.Close();
                }
            }

            
        }

        private void btnDolazak_Click(object sender, RoutedEventArgs e)
        {
            UcitajPodatke(DataGridCentralni, dolazakSelect);
        }

        private void btnZaposleni_Click(object sender, RoutedEventArgs e)
        {
            UcitajPodatke(DataGridCentralni, zaposleniSelect);
        }

        private void btnKorisnik_Click(object sender, RoutedEventArgs e)
        {
            UcitajPodatke(DataGridCentralni, korisnikSelect);
        }

        private void btnClanstvo_Click(object sender, RoutedEventArgs e)
        {
            UcitajPodatke(DataGridCentralni, clanstvoSelect);
        }

        private void btnOprema_Click(object sender, RoutedEventArgs e)
        {
            UcitajPodatke(DataGridCentralni, opremaSelect);
        }

        private void btnProgram_Click(object sender, RoutedEventArgs e)
        {
            UcitajPodatke(DataGridCentralni, programSelect);
        }

        private void btnRegistracija_Click(object sender, RoutedEventArgs e)
        {
            UcitajPodatke(DataGridCentralni, registracijaSelect);
        }

        private void btnVrstaClanstva_Click(object sender, RoutedEventArgs e)
        {
            UcitajPodatke(DataGridCentralni, vrstaClanstvaSelect);
        }

        private void btnDodaj_Click(object sender, RoutedEventArgs e)
        {
            Window prozor;

            if (ucitanaTabela.Equals(clanstvoSelect)){

                prozor = new frmClanstvo();
                prozor.ShowDialog();

                UcitajPodatke(DataGridCentralni, clanstvoSelect);
            }
            else if (ucitanaTabela.Equals(dolazakSelect))
            {
                prozor = new frmDolazak();
                prozor.ShowDialog();

                UcitajPodatke(DataGridCentralni, dolazakSelect);
            }
            else if (ucitanaTabela.Equals(korisnikSelect))
            {
                prozor = new frmKorisnik();
                prozor.ShowDialog();

                UcitajPodatke(DataGridCentralni, korisnikSelect);
            }
            else if (ucitanaTabela.Equals(opremaSelect))
            {
                prozor = new frmOprema();
                prozor.ShowDialog();

                UcitajPodatke(DataGridCentralni, opremaSelect);
            }
            else if (ucitanaTabela.Equals(programSelect))
            {
                prozor = new frmProgram();
                prozor.ShowDialog();

                UcitajPodatke(DataGridCentralni, programSelect);
            }
            else if (ucitanaTabela.Equals(registracijaSelect))
            {
                prozor = new frmRegistracija();
                prozor.ShowDialog();

                UcitajPodatke(DataGridCentralni, registracijaSelect);
            }
            else if (ucitanaTabela.Equals(vrstaClanstvaSelect))
            {
                prozor = new frmVrstaClanstva();
                prozor.ShowDialog();

                UcitajPodatke(DataGridCentralni, vrstaClanstvaSelect);
            }
            else if (ucitanaTabela.Equals(zaposleniSelect))
            {
                prozor = new frmZaposleni();
                prozor.ShowDialog();

                UcitajPodatke(DataGridCentralni, zaposleniSelect);
            }
        }

        private void btnIzmjeni_Click(object sender, RoutedEventArgs e)
        {
            if (ucitanaTabela.Equals(korisnikSelect))
            {
                PopuniFormu(DataGridCentralni, selectUslovKorisnik);
                UcitajPodatke(DataGridCentralni, korisnikSelect);

            }else if (ucitanaTabela.Equals(clanstvoSelect))
            {
                PopuniFormu(DataGridCentralni, selectUslovClanstvo);
                UcitajPodatke(DataGridCentralni, clanstvoSelect);

            }else if (ucitanaTabela.Equals(dolazakSelect))
            {
                PopuniFormu(DataGridCentralni,selectUslovDolazak);
                UcitajPodatke(DataGridCentralni, dolazakSelect);
                  
            }else if (ucitanaTabela.Equals(opremaSelect))
            {
                PopuniFormu(DataGridCentralni, selectUslovOprema);
                UcitajPodatke(DataGridCentralni, opremaSelect);

            }else if (ucitanaTabela.Equals(programSelect))
            {
                PopuniFormu(DataGridCentralni, selectuslovProgram);
                UcitajPodatke(DataGridCentralni, programSelect);

            }else if (ucitanaTabela.Equals(registracijaSelect))
            {
                PopuniFormu(DataGridCentralni, selectUslovRegistracija);
                UcitajPodatke(DataGridCentralni, registracijaSelect);

            }else if (ucitanaTabela.Equals(vrstaClanstvaSelect))
            {
                PopuniFormu(DataGridCentralni, selectUslovVrstaClanstva);
                UcitajPodatke(DataGridCentralni, vrstaClanstvaSelect);

            }else if (ucitanaTabela.Equals(zaposleniSelect))
            {
                PopuniFormu(DataGridCentralni, selectUslovZaposleni);
                UcitajPodatke(DataGridCentralni, zaposleniSelect);
            }
        }


        static void PopuniFormu(DataGrid grid,string selectUslov)
        {
            try
            {
                konekcija.Open();
                azuriraj = true;
                DataRowView red = (DataRowView)grid.SelectedItems[0];
                pomocniRed = red;
                string upit = selectUslov + red["ID"];
                SqlCommand cmd = new SqlCommand(upit, konekcija);
                SqlDataReader citac = cmd.ExecuteReader();
                while (citac.Read())
                {
                    if (ucitanaTabela.Equals(korisnikSelect))
                    {
                        frmKorisnik prozorKorisnik = new frmKorisnik();

                        prozorKorisnik.txtImeKorsinika.Text = citac["ImeKorisnika"].ToString();
                        prozorKorisnik.txtPrezimeKorisnika.Text = citac["PrezimeKorisnika"].ToString();
                        prozorKorisnik.txtJMBGKorisnika.Text = citac["JMBGKorisnika"].ToString();
                        prozorKorisnik.txtTelefonKorisnika.Text = citac["TelefonKorisnika"].ToString();
                        prozorKorisnik.txtAdresaKorisnika.Text = citac["AdresaKorisnika"].ToString();
                        prozorKorisnik.txtPolKorisnika.Text = citac["PolKorisnika"].ToString();
                        prozorKorisnik.txtEmailKorisnika.Text = citac["EmailKorisnika"].ToString();

                        prozorKorisnik.ShowDialog();

                    }else if (ucitanaTabela.Equals(clanstvoSelect))
                    {
                        frmClanstvo prozorClanstvo = new frmClanstvo();

                        prozorClanstvo.dpDatumPocetka.SelectedDate =(DateTime)citac["DatumPocetka"];
                        prozorClanstvo.dpDatumIsteka.SelectedDate = (DateTime)citac["DatumIsteka"];
                        prozorClanstvo.cbKorisnik.SelectedValue = citac["KorisnikID"];
                        prozorClanstvo.cbVrstaClanstva.SelectedValue = citac["VrstaClanstvaID"];

                        prozorClanstvo.ShowDialog();
                           

                    }else if (ucitanaTabela.Equals(dolazakSelect))
                    {
                        frmDolazak prozorDolazak = new frmDolazak();

                        prozorDolazak.txtBrojKljuca.Text = citac["BrojKljuca"].ToString();
                        prozorDolazak.cbKorisnik.SelectedValue = citac["KorisnikID"];
                        prozorDolazak.cbZaposleni.SelectedValue = citac["zaposleniID"];

                        prozorDolazak.ShowDialog();

                    }else if (ucitanaTabela.Equals(opremaSelect))
                    {
                        frmOprema prozorOprema = new frmOprema();

                        prozorOprema.txtCijenaOpreme.Text = citac["CijenaOpreme"].ToString();
                        prozorOprema.txtNazivOpreme.Text = citac["NazivOpreme"].ToString();
                        prozorOprema.txtVrstaOpreme.Text = citac["VrstaOpreme"].ToString();
                        prozorOprema.cbZaposleni.SelectedValue = citac["ZaposleniID"];

                        prozorOprema.ShowDialog();

                    }else if (ucitanaTabela.Equals(programSelect))
                    {
                        frmProgram prozorProgram = new frmProgram();

                        prozorProgram.txtVrstaPrograma.Text = citac["VrstaPrograma"].ToString();
                        prozorProgram.txtCijenaPrograma.Text = citac["CijenaPrograma"].ToString();
                        prozorProgram.cbKorisnik.SelectedValue = citac["KorisnikID"];
                        prozorProgram.cbZaposleni.SelectedValue = citac["KorisnikID"];

                        prozorProgram.ShowDialog();

                    }else if (ucitanaTabela.Equals(registracijaSelect))
                    {
                        frmRegistracija prozorRegitracija = new frmRegistracija();

                        prozorRegitracija.dpDatumRegistracije.SelectedDate =(DateTime)citac["DatumRegistracije"];
                        prozorRegitracija.txtCijenaRegistracije.Text = citac["CijenaRegistracije"].ToString();
                        prozorRegitracija.cbxNoviClan.IsChecked = Convert.ToBoolean(citac["NoviClan"]);
                        prozorRegitracija.cbZaposleni.SelectedValue = citac["ZaposleniID"];
                        prozorRegitracija.cbClanstvo.SelectedValue = citac["ClanstvoID"];

                        prozorRegitracija.ShowDialog();

                    }else if (ucitanaTabela.Equals(vrstaClanstvaSelect))
                    {
                        frmVrstaClanstva prozorVrste = new frmVrstaClanstva();

                        prozorVrste.txtNazivVrsteClanstva.Text = citac["NazivVrsteClanstva"].ToString();
                        prozorVrste.cbxMuskaClanarina.IsChecked = Convert.ToBoolean(citac["MuskaClanarina"]);
                        prozorVrste.cbxZenskaClanarina.IsChecked = Convert.ToBoolean(citac["ZenskaClanarina"]);
                        prozorVrste.cbxStudClanarina.IsChecked = Convert.ToBoolean(citac["StudentskaClanarina"]);
                        prozorVrste.cbxPenzClanarina.IsChecked = Convert.ToBoolean(citac["PenzionerskaClanarina"]);

                        prozorVrste.ShowDialog();

                    }else if (ucitanaTabela.Equals(zaposleniSelect))
                    {
                        frmZaposleni prozorZaposleni = new frmZaposleni();

                        prozorZaposleni.txtImeZaposlenog.Text = citac["ImeZaposlenog"].ToString();
                        prozorZaposleni.txtPrezimeZaposlenog.Text = citac["PrezimeZaposlenog"].ToString();
                        prozorZaposleni.cbxTrener.IsChecked = Convert.ToBoolean(citac["Trener"]);
                        prozorZaposleni.txtAdresaZaposlenog.Text = citac["AdresaZaposlenog"].ToString();
                        prozorZaposleni.txtTelefonZaposlenog.Text = citac["TelefonZaposlenog"].ToString();
                        prozorZaposleni.txtEmailZaposlenog.Text = citac["EmailZaposlenog"].ToString();
                        prozorZaposleni.txtLozinkaZaposlenog.Text = citac["Lozinka"].ToString();

                        prozorZaposleni.ShowDialog();

                    }
                }
            }
            catch(ArgumentOutOfRangeException)
            {
                MessageBox.Show("Niste selektovali red!", "Greška!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception)
            {
                MessageBox.Show("Greška, moliko kontaktirajte administratora!", "Greška!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                if (konekcija != null)
                {
                    konekcija.Close();
                }
            }
        }


        static void ObrisiZapis(DataGrid grid, string deleteUpit)
        {
            try
            {
                konekcija.Open();
                DataRowView red = (DataRowView)grid.SelectedItems[0];
                string upit = deleteUpit + red["ID"];
                MessageBoxResult rezultat = MessageBox.Show("Da li ste sigurni?", "Upozorenje!", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (rezultat == MessageBoxResult.Yes)
                {
                    SqlCommand cmd = new SqlCommand(upit, konekcija);
                    cmd.ExecuteNonQuery();
                }

                
            }
            catch(ArgumentOutOfRangeException)
            {
                MessageBox.Show("Niste selektovali red!", "Greška",
               MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (SqlException)
            {
                MessageBox.Show("Postoje povezani podaci u drugim tabelama!Molimo Vas da ih provjerite!", "Greška",
                MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                if (konekcija != null)
                {
                    konekcija.Close();
                }
            }
        }

            private void btnObrisi_Click(object sender, RoutedEventArgs e)
            {
                if (ucitanaTabela.Equals(clanstvoSelect))
                {
                    ObrisiZapis(DataGridCentralni, deleteClanstvo);
                    UcitajPodatke(DataGridCentralni, clanstvoSelect);

                }else if (ucitanaTabela.Equals(dolazakSelect))
                {
                    ObrisiZapis(DataGridCentralni, deleteDolazak);
                    UcitajPodatke(DataGridCentralni, dolazakSelect);

                }else if (ucitanaTabela.Equals(korisnikSelect))
                {
                    ObrisiZapis(DataGridCentralni, deleteKorisnik);
                    UcitajPodatke(DataGridCentralni, korisnikSelect);

            }else if (ucitanaTabela.Equals(opremaSelect))
            {
                ObrisiZapis(DataGridCentralni, deleteOprema);
                UcitajPodatke(DataGridCentralni, opremaSelect);

            }else if (ucitanaTabela.Equals(programSelect))
            {
                ObrisiZapis(DataGridCentralni, deleteProgram);
                UcitajPodatke(DataGridCentralni, programSelect);

            }else if (ucitanaTabela.Equals(registracijaSelect))
            {
                ObrisiZapis(DataGridCentralni, deleteRegistracija);
                UcitajPodatke(DataGridCentralni, registracijaSelect);

            }else if (ucitanaTabela.Equals(vrstaClanstvaSelect))
            {
                ObrisiZapis(DataGridCentralni, deleteVrstaClanstva);
                UcitajPodatke(DataGridCentralni, vrstaClanstvaSelect);

            }else if (ucitanaTabela.Equals(zaposleniSelect))
            {
                ObrisiZapis(DataGridCentralni, deleteZaposleni);
                UcitajPodatke(DataGridCentralni, zaposleniSelect);
            }

        }

        private void txtPretraga_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (ucitanaTabela.Equals(korisnikSelect))
                {

                    upit = ucitanaTabela + "Where ImeKorisnika like " + "'%" + txtPretraga.Text + "%'"
                         + " or PrezimeKorisnika like " + "'%"+ txtPretraga.Text + "%'"
                         + " or JMBGKorisnika like " + "'%" + txtPretraga.Text + "%'"
                         + " or TelefonKorisnika like " + "'%" + txtPretraga.Text + "%'"
                         + " or AdresaKorisnika like " + "'%" + txtPretraga.Text + "%'"
                         + " or PolKorisnika like " + "'%" + txtPretraga.Text + "%'"
                         + " or EmailKorisnika like " + "'%" + txtPretraga.Text + "%'"; 

                }
                else if (ucitanaTabela.Equals(clanstvoSelect))
                {

                    upit = ucitanaTabela + "Where DatumPocetka like " + "'%" + txtPretraga.Text + "%'"
                     + " or DatumIsteka like " + "'%" + txtPretraga.Text + "%'"
                     +" or tblKorisnik.ImeKorisnika like " + "'%" + txtPretraga.Text + "%'"
                     + " or tblKorisnik.PrezimeKorisnika like " + "'%" + txtPretraga.Text + "%'"
                    + " or tblVrstaClanstva.NazivVrsteClanstva like " + "'%" + txtPretraga.Text + "%'";

                }
                else if (ucitanaTabela.Equals(dolazakSelect))
                {
                    upit = ucitanaTabela + "Where BrojKljuca like " + "'%" + txtPretraga.Text + "%'"
                    + " or tblZaposleni.ImeZaposlenog like " + "'%" + txtPretraga.Text + "%'"
                    + " or tblZaposleni.PrezimeZaposlenog like " + "'%" + txtPretraga.Text + "%'"
                    + " or tblKorisnik.ImeKorisnika like " + "'%" + txtPretraga.Text + "%'"
                   + " or   tblKorisnik.PrezimeKorisnika like " + "'%" + txtPretraga.Text + "%'";

                }
                else if (ucitanaTabela.Equals(opremaSelect))
                {

                    upit = ucitanaTabela + "Where CijenaOpreme like " + "'%" + txtPretraga.Text + "%'"
                   + " or tblZaposleni.ImeZaposlenog like " + "'%" + txtPretraga.Text + "%'"
                   + " or tblZaposleni.PrezimeZaposlenog like " + "'%" + txtPretraga.Text + "%'"
                   + " or NazivOpreme like " + "'%" + txtPretraga.Text + "%'"
                  + " or  VrstaOpreme like " + "'%" + txtPretraga.Text + "%'";
                }
                else if (ucitanaTabela.Equals(programSelect))
                {
                    upit = ucitanaTabela + "Where VrstaPrograma like " + "'%" + txtPretraga.Text + "%'"
                  + " or tblZaposleni.ImeZaposlenog like " + "'%" + txtPretraga.Text + "%'"
                  + " or tblZaposleni.PrezimeZaposlenog like " + "'%" + txtPretraga.Text + "%'"
                  + " or tblKorisnik.ImeKorisnika like " + "'%" + txtPretraga.Text + "%'"
                 + " or   tblKorisnik.PrezimeKorisnika like " + "'%" + txtPretraga.Text + "%'"
                 + " or   CijenaPrograma like " + "'%" + txtPretraga.Text + "%'";

                }
                else if (ucitanaTabela.Equals(registracijaSelect))
                {
                    upit = ucitanaTabela + "Where DatumRegistracije like " + "'%" + txtPretraga.Text + "%'"
                 + " or tblZaposleni.ImeZaposlenog like " + "'%" + txtPretraga.Text + "%'"
                 + " or tblZaposleni.PrezimeZaposlenog like " + "'%" + txtPretraga.Text + "%'"
                 + " or tblKorisnik.ImeKorisnika like " + "'%" + txtPretraga.Text + "%'"
                + " or   tblKorisnik.PrezimeKorisnika like " + "'%" + txtPretraga.Text + "%'"
                + " or   CijenaRegistracije like " + "'%" + txtPretraga.Text + "%'"
                + " or   RegistracijaID like " + "'%" + txtPretraga.Text + "%'";

                }
                else if (ucitanaTabela.Equals(vrstaClanstvaSelect))
                {

                    upit = ucitanaTabela + "Where NazivVrsteClanstva like " + "'%" + txtPretraga.Text + "%'";
                }
                else if (ucitanaTabela.Equals(zaposleniSelect))
                {
                    upit = ucitanaTabela + "Where ImeZaposlenog like " + "'%" + txtPretraga.Text + "%'"
                       + " or PrezimeZaposlenog like " + "'%" + txtPretraga.Text + "%'"
                       + " or TelefonZaposlenog like " + "'%" + txtPretraga.Text + "%'"
                       + " or AdresaZaposlenog like " + "'%" + txtPretraga.Text + "%'"                      
                       + " or EmailZaposlenog like " + "'%" + txtPretraga.Text + "%'";
                }

                if (txtPretraga.Text == "")
                {
                    UcitajPodatke(DataGridCentralni, ucitanaTabela);
                }
                else
                {
                   
                    
                    konekcija.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(upit, konekcija);
                    DataTable tabela = new DataTable();
                    adapter.Fill(tabela);
                    DataGridCentralni.ItemsSource = tabela.DefaultView;
                }
            }
            catch (SqlException ex)
            {

                MessageBox.Show(ex.ToString());
            }
            finally
            {
                if (konekcija != null)
                {
                    konekcija.Close();
                }
            }
        }

        
    }
        
    
}
