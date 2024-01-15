using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace Database
{
    public partial class Form1 : Form
    {
        Articolo articolo;
        public Form1()
        {
            InitializeComponent();
            articolo = new Articolo();
            BottoneGriglia.Click += visualizzaGriglia;
            BottoneLog.Click += visualizzaLog;
            Aggiungi.Click += AggiungiElemento;
            AggiungiUser.Click += AggiungiUser_Click;
            AggiungiGiacenza.Click += AggiungiGiacenzaFunction;
            DeleteButtonArticolo.Click += DeleteButtonArticoloF;
            DeleteUser.Click += DeleteUser_Click;
            ResetPassword.Click += ResetPassword_Click;
        }

        private void ResetPassword_Click(object sender, EventArgs e)
        {
            //Se errore, togli commento
            //De rallenta attiva compiler
            //SqlConnection mySqlConnection = null;
            {
                try
                {
                    //Stringa connessione
                    string stringaConnessione = "Server=localhost\\SQLEXPRESS; Database=Gestionale; Trusted_Connection=true";

                    //Qui creiamo un comando SQL che connette il file il database
                    using (SqlConnection connessione = new SqlConnection(stringaConnessione))
                    {
                        //Qui apriamo la connessione al db
                        connessione.Open();

                        //Qui creiamo il comando SQL
                        string stringaComando = "SELECT Email FROM Log_in WHERE Email = @Email;";
                        //Crei un nuovo comando SQL che abbia come valori la stringa sql e la connessione creata prima
                        using (SqlCommand comandoSql = new SqlCommand(stringaComando, connessione))
                        {
                            //1 valore: quello che andrà usato nel codice sql
                            //2 valore: quello che prendiamo dal codice (In questo caso viene dalla classe Log_in)
                            comandoSql.Parameters.Add("@Email", SqlDbType.NVarChar);
                            comandoSql.Parameters["@Email"].Value = this.RecuperoEmail.Text;

                            comandoSql.Parameters.Add("@NuovaPassword", SqlDbType.NVarChar);
                            comandoSql.Parameters["@NuovaPassword"].Value = this.NuovaPassword.Text;

                            //SqlDataReader lettore -> Qui crei una variabile SqlDataReader che puoi utilizzare per leggere le colonne e i dati dalla riga corrente dei risultati
                            //comandoSql.ExecuteReader() -> Qui esegui il comando SQL creato prima

                            //Il comando intero salva nella variabile creata il comando SQL creato prima
                            using (SqlDataReader lettore = comandoSql.ExecuteReader())
                            {
                                if (lettore.Read())
                                {
                                    // Leggi il valore dalla colonna specificata (ad esempio, 'NomeColonna')
                                    string valoreLetto = lettore["Email"].ToString();

                                    if (this.NuovaPassword.Text == this.ConfermaPassword.Text)
                                    {
                                        string stringaCambioPassword = "UPDATE Log_in SET Password = @NuovaPassword WHERE Email = @Email;";
                                        using (SqlCommand cambioPassword = new SqlCommand(stringaCambioPassword, connessione))
                                        {
                                            //cambioPassword.Parameters.Add("@NuovaPassword", SqlDbType.NVarChar).Value = this.NuovaPassword.Text;
                                            int righeAggiornate = cambioPassword.ExecuteNonQuery();

                                            if (righeAggiornate > 0)
                                            {
                                                MessageBox.Show("La password è stata cambiata con successo!");
                                            }
                                        }
                                    }
                                    else
                                    {
                                        MessageBox.Show("Attenzione, le password non coincidono!!");
                                    }
                                    //MessageBox.Show("Valore letto: " + valoreLetto);
                                }
                                else
                                {
                                    MessageBox.Show("Nessun dato trovato per la condizione specificata.");
                                }
                                lettore.Close();
                            }
                        }
                        connessione.Close();
                    }


                }
                catch (Exception ex)
                {
                    MessageBox.Show("Eccezione: " + ex.Message);
                }
            }
            /*try
            {
                //Stringa connessione
                string stringaConnessione = "Server=localhost\\SQLEXPRESS; Database=Gestionale; Trusted_Connection=true";

                //Qui creiamo un comando SQL che connette il file il database
                using (SqlConnection connessione = new SqlConnection(stringaConnessione))
                {
                    //Qui apriamo la connessione al db
                    connessione.Open();

                    //Qui creiamo i comando SQL
                    string stringaComando = "SELECT Email FROM Log_in WHERE Email = @Email;";
                    string stringaCambioPassword = "UPDATE Log_in SET Password = @NuovaPassword WHERE Email = @Email;";
                    //Crei un nuovo comando SQL che abbia come valori la stringa sql e la connessione creata prima
                    using (SqlCommand comandoSql = new SqlCommand(stringaComando, connessione))
                    {
                        //1 valore: quello che andrà usato nel codice sql
                        //2 valore: quello che prendiamo dal codice (In questo caso viene dalla classe Log_in)
                        comandoSql.Parameters.Add("@Email", SqlDbType.NVarChar);
                        comandoSql.Parameters["@Email"].Value = this.RecuperoEmail.Text;

                        comandoSql.Parameters.Add("@NuovaPassword", SqlDbType.NVarChar);
                        comandoSql.Parameters["@NuovaPassword"].Value = this.NuovaPassword.Text;

                        //SqlDataReader lettore -> Qui crei una variabile SqlDataReader che puoi utilizzare per leggere le colonne e i dati dalla riga corrente dei risultati
                        //comandoSql.ExecuteReader() -> Qui esegui il comando SQL creato prima

                        //Il comando intero salva nella variabile creata il comando SQL creato prima
                        using (SqlDataReader lettore = comandoSql.ExecuteReader())
                        {
                            if (lettore.Read())
                            {
                                // Leggi il valore dalla colonna specificata (ad esempio, 'NomeColonna')
                                string valoreLetto = lettore["Email"].ToString();
                                lettore.Close();

                                if (this.NuovaPassword.Text == this.ConfermaPassword.Text)
                                {
                                    using (SqlCommand cambioPassword = new SqlCommand(stringaCambioPassword, connessione))
                                    {
                                        //cambioPassword.Parameters.Add("@NuovaPassword", SqlDbType.NVarChar).Value = this.NuovaPassword.Text;
                                        int righeAggiornate = cambioPassword.ExecuteNonQuery();

                                        if (righeAggiornate > 0)
                                        {
                                            MessageBox.Show("La password è stata cambiata con successo!");
                                        }
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("Attenzione, le password non coincidono!!");
                                }
                                //MessageBox.Show("Valore letto: " + valoreLetto);
                            }
                            else
                            {
                                MessageBox.Show("Nessun dato trovato per la condizione specificata.");
                            }
                        }
                    }
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show("Eccezione: " + ex.Message);
            }*/
        }

        private void DeleteUser_Click(object sender, EventArgs e)
        {
            SqlConnection mySqlConnection = null;
            try
            {
                string stringaConnessione = "Server=localhost\\SQLEXPRESS; Database=Gestionale; Trusted_Connection=true";
                mySqlConnection = new SqlConnection(stringaConnessione);
                mySqlConnection.Open();

                string EliminaArticolo = "delete from Log_in where Id = @Id;";

                SqlCommand mySqlCommand = new SqlCommand(stringaConnessione);
                mySqlCommand.Connection = mySqlConnection;
                mySqlCommand.CommandText = EliminaArticolo;

                mySqlCommand.Parameters.Add("@Id", SqlDbType.Int);
                mySqlCommand.Parameters["@Id"].Value = this.IdDelateUser.Text;

                mySqlCommand.ExecuteNonQuery();

                {
                    SqlConnection mySqlConnectionLog = null;
                    List<Log> listaLog = new List<Log>();

                    //Creo l'oggetto che mi permette di estrarre i dati
                    string stringaConnessioneLog = "Server=localhost\\SQLEXPRESS; Database=Gestionale; Trusted_Connection=true";
                    mySqlConnectionLog = new SqlConnection(stringaConnessioneLog);
                    mySqlConnectionLog.Open();
                    SqlCommand mySqlCommandLog = new SqlCommand(stringaConnessioneLog);
                    mySqlCommandLog.Connection = mySqlConnectionLog;
                    mySqlCommandLog.CommandText = "select * from Log_in";
                    IDataReader myReaderLog = mySqlCommandLog.ExecuteReader();
                    while (myReaderLog.Read())
                    {
                        //Legge un record alla volta
                        Log myUser = new Log();
                        myUser.Id = Convert.ToInt32(myReaderLog["Id"]);
                        myUser.Email = Convert.ToString(myReaderLog["Email"]);
                        myUser.Password = Convert.ToString(myReaderLog["Password"]);
                        listaLog.Add(myUser);
                    }
                    //Per vedere i dati collego la lista alla griglia
                    this.GrigliaLog.DataSource = listaLog;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Eccezione: " + ex.Message);
            }
        }

        private void DeleteButtonArticoloF(object sender, EventArgs e)
        {
            SqlConnection mySqlConnection = null;
            try
            {
                string stringaConnessione = "Server=localhost\\SQLEXPRESS; Database=Gestionale; Trusted_Connection=true";
                mySqlConnection = new SqlConnection(stringaConnessione);
                mySqlConnection.Open();

                string EliminaArticolo = "delete from Articoli where Id = @Id";

                SqlCommand mySqlCommand = new SqlCommand(stringaConnessione);
                mySqlCommand.Connection = mySqlConnection;
                mySqlCommand.CommandText = EliminaArticolo;

                mySqlCommand.Parameters.Add("@Id", SqlDbType.Int);
                mySqlCommand.Parameters["@Id"].Value = this.IdDelete.Text;

                mySqlCommand.ExecuteNonQuery();

                {
                    List<Articolo> listaArticoli = new List<Articolo>();


                    mySqlCommand.CommandText = "select * from Articoli";
                    IDataReader myReader = mySqlCommand.ExecuteReader();
                    while (myReader.Read())
                    {
                        //Legge un record alla volta
                        Articolo myArticolo = new Articolo();
                        myArticolo.Id = Convert.ToInt32(myReader["Id"]);
                        myArticolo.Nome = Convert.ToString(myReader["Nome"]);
                        myArticolo.Descrizione = Convert.ToString(myReader["Descrizione"]);
                        myArticolo.Giacenza = Convert.ToInt32(myReader["Giacenza"]);
                        myArticolo.AliquotaIva = Convert.ToInt32(myReader["AliquotaIva"]);
                        myArticolo.Prezzo = Convert.ToInt32(myReader["Prezzo"]);
                        listaArticoli.Add(myArticolo);
                    }
                    //Per vedere i dati collego la lista alla griglia
                    this.GrigliaDb.DataSource = listaArticoli;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Eccezione: " + ex.Message);
            }
        }

        private void AggiungiGiacenzaFunction(object sender, EventArgs e)
        {
            SqlConnection mySqlConnection = null;
            try
            {
                string stringaConnessione = "Server=localhost\\SQLEXPRESS; Database=Gestionale; Trusted_Connection=true";
                mySqlConnection = new SqlConnection(stringaConnessione);
                mySqlConnection.Open();

                string AggiornaGiacenza = "update Articoli set Giacenza = Giacenza + @AddGiacenza - @RemoveGiacenza where Id = @Id";

                SqlCommand mySqlCommand = new SqlCommand(stringaConnessione);
                mySqlCommand.Connection = mySqlConnection;

                SqlCommand UpdateGiacenza = new SqlCommand(string.Empty, mySqlConnection);
                UpdateGiacenza.CommandText = AggiornaGiacenza;

                UpdateGiacenza.Parameters.Add("@AddGiacenza", SqlDbType.NVarChar);
                UpdateGiacenza.Parameters["@AddGiacenza"].Value = this.AddQ.Text;

                UpdateGiacenza.Parameters.Add("@Id", SqlDbType.NVarChar);
                UpdateGiacenza.Parameters["@Id"].Value = this.IdQ.Text;

                UpdateGiacenza.Parameters.Add("@RemoveGiacenza", SqlDbType.NVarChar);
                UpdateGiacenza.Parameters["@RemoveGiacenza"].Value = this.RemoveQ.Text;

                UpdateGiacenza.ExecuteNonQuery();

                {
                    List<Articolo> listaArticoli = new List<Articolo>();


                    mySqlCommand.CommandText = "select * from Articoli";
                    IDataReader myReader = mySqlCommand.ExecuteReader();
                    while (myReader.Read())
                    {
                        //Legge un record alla volta
                        Articolo myArticolo = new Articolo();
                        myArticolo.Id = Convert.ToInt32(myReader["Id"]);
                        myArticolo.Nome = Convert.ToString(myReader["Nome"]);
                        myArticolo.Descrizione = Convert.ToString(myReader["Descrizione"]);
                        myArticolo.Giacenza = Convert.ToInt32(myReader["Giacenza"]);
                        myArticolo.AliquotaIva = Convert.ToInt32(myReader["AliquotaIva"]);
                        myArticolo.Prezzo = Convert.ToInt32(myReader["Prezzo"]);
                        listaArticoli.Add(myArticolo);
                    }
                    //Per vedere i dati collego la lista alla griglia
                    this.GrigliaDb.DataSource = listaArticoli;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Eccezione: " + ex.Message);
            }
        }

        private void AggiungiUser_Click(object sender, EventArgs e)
        {
            /*SqlConnection mySqlConnection = null;
            try
            {
                string stringaConnessione = "Server=localhost\\SQLEXPRESS; Database=Gestionale; Trusted_Connection=true";
                mySqlConnection = new SqlConnection(stringaConnessione);
                mySqlConnection.Open();
                SqlCommand mySqlCommand = new SqlCommand(stringaConnessione);
                mySqlCommand.Connection = mySqlConnection;

                mySqlCommand.Parameters.Add("@Email", SqlDbType.NVarChar);
                mySqlCommand.Parameters["@Email"].Value = this.InputEmail.Text;

                mySqlCommand.Parameters.Add("@Password", SqlDbType.NVarChar);
                mySqlCommand.Parameters["@Password"].Value = this.InputPassword.Text;

                mySqlCommand.CommandText = "INSERT INTO Log_in (Email, Password) VALUES(@Email, @Password)";

                mySqlCommand.ExecuteNonQuery();
            }
            catch(Exception ex)
            {
                MessageBox.Show("Eccezzione: " + ex.Message);
            }*/
            SqlConnection mySqlConnection = null;

            try
            {
                string stringaConnessione = "Server=localhost\\SQLEXPRESS; Database=Gestionale; Trusted_Connection=true";
                mySqlConnection = new SqlConnection(stringaConnessione);
                mySqlConnection.Open();

                // Verifica se la mail è già presente nel database
                string verificaMailQuery = "SELECT COUNT(*) FROM Log_in WHERE Email = @Email";
                using (SqlCommand verificaMailCommand = new SqlCommand(verificaMailQuery, mySqlConnection))
                {
                    verificaMailCommand.Parameters.Add("@Email", SqlDbType.NVarChar);
                    verificaMailCommand.Parameters["@Email"].Value = this.InputEmail.Text;

                    int count = (int)verificaMailCommand.ExecuteScalar();

                    // Se la mail è già presente, mostra un messaggio e non procedere con l'inserimento
                    if (count > 0)
                    {
                        MessageBox.Show("Questa mail è già presente nel database. Inserimento non consentito.");
                    }
                    else
                    {
                        // La mail non è presente nel database, puoi procedere con l'inserimento.
                        SqlCommand inserimentoCommand = new SqlCommand(string.Empty, mySqlConnection);

                        inserimentoCommand.Parameters.Add("@Email", SqlDbType.NVarChar);
                        inserimentoCommand.Parameters["@Email"].Value = this.InputEmail.Text;

                        inserimentoCommand.Parameters.Add("@Password", SqlDbType.NVarChar);
                        inserimentoCommand.Parameters["@Password"].Value = this.InputPassword.Text;

                        inserimentoCommand.CommandText = "INSERT INTO Log_in (Email, Password) VALUES(@Email, @Password)";

                        inserimentoCommand.ExecuteNonQuery();

                        {
                            SqlConnection mySqlConnectionLog = null;
                            List<Log> listaLog = new List<Log>();

                            //Creo l'oggetto che mi permette di estrarre i dati
                            string stringaConnessioneLog = "Server=localhost\\SQLEXPRESS; Database=Gestionale; Trusted_Connection=true";
                            mySqlConnectionLog = new SqlConnection(stringaConnessioneLog);
                            mySqlConnectionLog.Open();
                            SqlCommand mySqlCommandLog = new SqlCommand(stringaConnessioneLog);
                            mySqlCommandLog.Connection = mySqlConnectionLog;
                            mySqlCommandLog.CommandText = "select * from Log_in";
                            IDataReader myReaderLog = mySqlCommandLog.ExecuteReader();
                            while (myReaderLog.Read())
                            {
                                //Legge un record alla volta
                                Log myUser = new Log();
                                myUser.Id = Convert.ToInt32(myReaderLog["Id"]);
                                myUser.Email = Convert.ToString(myReaderLog["Email"]);
                                myUser.Password = Convert.ToString(myReaderLog["Password"]);
                                listaLog.Add(myUser);
                            }
                            //Per vedere i dati collego la lista alla griglia
                            this.GrigliaLog.DataSource = listaLog;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Eccezione: " + ex.Message);
            }
            finally
            {
                // Assicurati di chiudere la connessione quando hai finito
                if (mySqlConnection != null && mySqlConnection.State == ConnectionState.Open)
                {
                    mySqlConnection.Close();
                }
            }
        }

        private void AggiungiElemento(object sender, EventArgs e)
        {
            SqlConnection mySqlConnection = null;
            try
            {
                string stringaConnessione = "Server=localhost\\SQLEXPRESS; Database=Gestionale; Trusted_Connection=true";
                mySqlConnection = new SqlConnection(stringaConnessione);
                mySqlConnection.Open();
                SqlCommand mySqlCommand = new SqlCommand(stringaConnessione);
                mySqlCommand.Connection = mySqlConnection;

                //Con le prossime 10 righe impediamo di aggiungere linquaggio SQL dentro i campi input

                if (InputNome.Text == "" || InputDesc.Text == "" || InputGiacenza.Text == "" || InputAli.Text == "" || InputPrezzo.Text == "")
                {
                    MessageBox.Show("Inserisci tutti i campi del prodotto");
                    return;
                }

                mySqlCommand.Parameters.Add("@Nome", SqlDbType.NVarChar);
                mySqlCommand.Parameters ["@Nome"].Value = this.InputNome.Text;

                mySqlCommand.Parameters.Add("@Descrizione", SqlDbType.NVarChar);
                mySqlCommand.Parameters["@Descrizione"].Value = this.InputDesc.Text;

                mySqlCommand.Parameters.Add("@Giacenza", SqlDbType.NVarChar);
                mySqlCommand.Parameters["@Giacenza"].Value = this.InputGiacenza.Text;

                mySqlCommand.Parameters.Add("@AliquotaIva", SqlDbType.NVarChar);
                mySqlCommand.Parameters["@AliquotaIva"].Value = this.InputAli.Text;

                mySqlCommand.Parameters.Add("@Prezzo", SqlDbType.NVarChar);
                mySqlCommand.Parameters["@Prezzo"].Value = this.InputPrezzo.Text;

                mySqlCommand.CommandText = "INSERT INTO Articoli (Nome, Descrizione, Giacenza, " +
                    "AliquotaIva, Prezzo) VALUES(@Nome, @Descrizione, @Giacenza, @AliquotaIva, @Prezzo)";

                mySqlCommand.ExecuteNonQuery();

                {
                    List<Articolo> listaArticoli = new List<Articolo>();


                    mySqlCommand.CommandText = "select * from Articoli";
                    IDataReader myReader = mySqlCommand.ExecuteReader();
                    while (myReader.Read())
                    {
                        //Legge un record alla volta
                        Articolo myArticolo = new Articolo();
                        myArticolo.Id = Convert.ToInt32(myReader["Id"]);
                        myArticolo.Nome = Convert.ToString(myReader["Nome"]);
                        myArticolo.Descrizione = Convert.ToString(myReader["Descrizione"]);
                        myArticolo.Giacenza = Convert.ToInt32(myReader["Giacenza"]);
                        myArticolo.AliquotaIva = Convert.ToInt32(myReader["AliquotaIva"]);
                        myArticolo.Prezzo = Convert.ToInt32(myReader["Prezzo"]);
                        listaArticoli.Add(myArticolo);
                    }
                    //Per vedere i dati collego la lista alla griglia
                    this.GrigliaDb.DataSource = listaArticoli;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Errore eccezzione:" + ex.Message);
            }
        }

        private void visualizzaLog(object sender, EventArgs e)
        {
            SqlConnection mySqlConnectionLog = null;
            List<Log> listaLog = new List<Log>();

            //Creo l'oggetto che mi permette di estrarre i dati
            string stringaConnessioneLog = "Server=localhost\\SQLEXPRESS; Database=Gestionale; Trusted_Connection=true";
            mySqlConnectionLog = new SqlConnection(stringaConnessioneLog);
            mySqlConnectionLog.Open();
            SqlCommand mySqlCommandLog = new SqlCommand(stringaConnessioneLog);
            mySqlCommandLog.Connection = mySqlConnectionLog;
            mySqlCommandLog.CommandText = "select * from Log_in";
            IDataReader myReaderLog = mySqlCommandLog.ExecuteReader();
            while (myReaderLog.Read())
            {
                //Legge un record alla volta
                Log myUser = new Log();
                myUser.Id = Convert.ToInt32(myReaderLog["Id"]);
                myUser.Email = Convert.ToString(myReaderLog["Email"]);
                myUser.Password = Convert.ToString(myReaderLog["Password"]);
                listaLog.Add(myUser);
            }
            //Per vedere i dati collego la lista alla griglia
            this.GrigliaLog.DataSource = listaLog;
        }

        private void visualizzaGriglia(object sender, EventArgs e)
        {
            SqlConnection mySqlConnection = null;
            List<Articolo> listaArticoli = new List<Articolo>();

            try 
            {
                //Per connettermi al Database mi serve una stringa di connessione
                string stringaConnessione = "Server=localhost\\SQLEXPRESS; Database=Gestionale; Trusted_Connection=true";
                mySqlConnection = new SqlConnection(stringaConnessione);
                mySqlConnection.Open();
                //MessageBox.Show("Operazione riuscita");


                //Creo l'oggetto che mi permette di estrarre i dati
                SqlCommand mySqlCommand = new SqlCommand(stringaConnessione);
                mySqlCommand.Connection = mySqlConnection;
                mySqlCommand.CommandText = "select * from Articoli";
                IDataReader myReader = mySqlCommand.ExecuteReader();
                while (myReader.Read())
                {
                    Articolo myArticolo = new Articolo();
                    myArticolo.Id = Convert.ToInt32(myReader["Id"]);
                    myArticolo.Nome = Convert.ToString(myReader["Nome"]);
                    myArticolo.Descrizione = Convert.ToString(myReader["Descrizione"]);
                    myArticolo.Giacenza = Convert.ToInt32(myReader["Giacenza"]);
                    myArticolo.AliquotaIva = Convert.ToInt32(myReader["AliquotaIva"]);
                    myArticolo.Prezzo = Convert.ToInt32(myReader["Prezzo"]);
                    listaArticoli.Add(myArticolo);
                }
                //Per vedere i dati collego la lista alla griglia
                this.GrigliaDb.DataSource = listaArticoli;
            }
            catch (Exception)
            {
                MessageBox.Show("Impossibile eseguire l'operazione");
            }
            finally 
            { 
                mySqlConnection.Close();
            }
        }
    }
}
//