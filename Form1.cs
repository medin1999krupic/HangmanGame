using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace HangMan
{
    public partial class Form1 : Form
    {
        public char[] gesuchtesWortarray;

        public TextBox[] Buchstaben;

        public Panel[] Panels;

        public string gesuchtesWort;

        public int Fehler = 0;

        public int NochOffen = 0;

        public bool verschieben = false;

        public bool GameStarted = false;

        List<string> wordsdictionary = new List<string>();

        public string currentword = string.Empty;
        private User currentUser;

        public Users Users { get; set; }

        public bool GuestUser = true;


        public Form1()
        {
            InitializeComponent();
        }

        private void Go(object sender, EventArgs e)
        {
            if (!GameStarted)
            {
                MessageBox.Show("Starte das Spiel.");
                return;
            }


            if(string.IsNullOrEmpty(textBox1.Text))
                {
                if (NochOffen == 0)
                {
                    MessageBox.Show("Sie Haben Gewonnen");
                    GameStarted = false;
                    return;
                }
                else if (Fehler == 10)
                {
                    MessageBox.Show("Sie Haben Verloren");
                    GameStarted = false;
                    return;
                }

                string getesteterBuchstabe = textBox6.Text.ToUpper();

                bool istEnthalten = false;

                istEnthalten = gesuchtesWort.Contains(getesteterBuchstabe);

                if (istEnthalten)
                {
                    for (int i = 0; i < gesuchtesWortarray.Length; i++)
                    {
                        if (gesuchtesWortarray[i].ToString() == getesteterBuchstabe)
                        {
                            if (Buchstaben[i].Text == "")
                            {
                                Buchstaben[i].Text = getesteterBuchstabe;
                                NochOffen--;
                            }
                            else
                            {
                                Panels[Fehler].Visible = true;
                                Fehler++;
                                if (!listBox1.Items.Cast<string>().Contains(getesteterBuchstabe))
                                    listBox1.Items.Add(getesteterBuchstabe);

                            }
                        }

                    }
                }
                else
                {
                    Panels[Fehler].Visible = true;
                    Fehler++;
                    if (!listBox1.Items.Cast<string>().Contains(getesteterBuchstabe))
                        listBox1.Items.Add(getesteterBuchstabe);

                }
                if (NochOffen == 0)
                {
                    Gewonnen();
                    ShowWord();
                }
                else if (Fehler == 10)
                {
                    Verloren();
                    ShowWord();
                }
                
                return;
                
            }

            else
            {
                if (textBox1.Text.ToUpper() == gesuchtesWort)
                {
                    Gewonnen();
                }
                else
                {
                    Verloren();
                }
                ShowWord();
            }

            
            textBox6.Text = string.Empty;
        }

        public void ShowWord()
        {
            for (int i = 0; i < gesuchtesWortarray.Length; i++)
            {
                Buchstaben[i].Text = gesuchtesWortarray[i].ToString();
                NochOffen--;
            }

            foreach (var panel in Panels)
            {
                panel.Visible = true;
            }

        }

        private void Verloren()
        {
            MessageBox.Show("Sie Haben Verloren");

            label4.Visible = true;
            label5.Visible = true;

            if(currentUser != null)
            {
                this.currentUser.Score.Matches++;
                this.currentUser.Score.Losts++;
                this.currentUser.Score.Points -= 15;
                Save();
            }
            
            GameStarted = false;
        }

        private void Gewonnen()
        {
            MessageBox.Show("Sie Haben Gewonnen");
            verschieben = true;
            label4.Visible = false;
            label5.Visible = false;

            panel9.Location = new Point(panel9.Location.X + 0, panel9.Location.Y + 40);
            panel13.Location = new Point(panel13.Location.X + 0, panel13.Location.Y + 40);
            panel18.Location = new Point(panel18.Location.X + 0, panel18.Location.Y + 40);
            panel15.Location = new Point(panel15.Location.X + 0, panel15.Location.Y + 40);
            panel17.Location = new Point(panel17.Location.X + 0, panel17.Location.Y + 40);
            panel19.Location = new Point(panel19.Location.X + 0, panel19.Location.Y + 40);

            if(currentUser != null)
            {
                this.currentUser.Score.Matches++;
                this.currentUser.Score.Wins++;
                this.currentUser.Score.Points += 10;
                Save();
            }
            
            GameStarted = false;
        }

        private void SpielStarten(object sender, EventArgs e)
        {
            if (currentUser == null && GuestUser == false)
            {
                MessageBox.Show("Bitte anmelden oder als Gast spielen.");
                return;
            }

            if (verschieben)
            {
                panel9.Location = new Point(panel9.Location.X + 0, panel9.Location.Y - 40);
                panel13.Location = new Point(panel13.Location.X + 0, panel13.Location.Y - 40);
                panel18.Location = new Point(panel18.Location.X + 0, panel18.Location.Y - 40);
                panel15.Location = new Point(panel15.Location.X + 0, panel15.Location.Y - 40);
                panel17.Location = new Point(panel17.Location.X + 0, panel17.Location.Y - 40);
                panel19.Location = new Point(panel19.Location.X + 0, panel19.Location.Y - 40);
                label4.Visible = false;
                label5.Visible = false;
                verschieben = false;
            }
            string schwierigkeitsgrad = comboBox1.Text;

            int wordcount = wordsdictionary.Count;
            int selected = new Random().Next(0, wordcount);
            currentword = wordsdictionary[selected];

            GameStarted = true;

            listBox1.Items.Clear();
                Fehler = 0;
                NochOffen = currentword.Length;
                gesuchtesWort = currentword.ToUpper();
                gesuchtesWortarray = gesuchtesWort.ToCharArray();

                if(Buchstaben!= null)
                {
                    foreach(var t in Buchstaben)
                {
                    t.Dispose();
                }
                }

                Buchstaben = new TextBox[currentword.Length];

            for(int i = 0; i< currentword.Length; i++)
            {
                TextBox textBox = new TextBox();
                this.Controls.Add(textBox);
                textBox.Width = 50;
                
                textBox.ReadOnly = true;
                Point point = new Point(55 * (i+1), 457);
                textBox.Location = point;
                Buchstaben[i] = textBox;
            }

                if(Panels != null)
                {
                foreach (var panel in Panels)
                {
                    panel.Visible = false;
                }
            }

                Panels = new Panel[10];
                Panels[0] = panel1;
                Panels[1] = panel2;
                Panels[2] = panel5;
                Panels[3] = panel8;
                Panels[4] = panel9;
                Panels[5] = panel13;
                Panels[6] = panel18;
                Panels[7] = panel15;
                Panels[8] = panel17;
                Panels[9] = panel19;

                label4.Visible = false;
                label5.Visible = false;

            switch (schwierigkeitsgrad)
            {
                case "Leicht":
                    Console.WriteLine("Leicht");
                    {

                        int lenght = currentword.Length;

                        //1
                        int random = new Random().Next(0, lenght);


                        string gesuchterBuchstabe = gesuchtesWortarray[random].ToString();
                        FillCharacter(Char.Parse(gesuchterBuchstabe));

                        int random2;
                        while((random2 = new Random().Next(0, lenght)) == random || gesuchtesWortarray[random].ToString() == gesuchtesWortarray[random2].ToString())
                        {

                        }
                        //2
                        
                        random = random2;

                        gesuchterBuchstabe = gesuchtesWortarray[random].ToString(); 
                        FillCharacter(Char.Parse(gesuchterBuchstabe));

                        if (NochOffen == 0)
                        {
                            Gewonnen();
                            ShowWord();
                            return;
                        }
                        break;
                    }
                    
                case "Mittel":
                    {
                        Console.WriteLine("Mittel");
                        int lenght = currentword.Length;
                        int random = new Random().Next(0, lenght);
                        string gesuchterBuchstabe = gesuchtesWortarray[random].ToString();
                        FillCharacter(Char.Parse(gesuchterBuchstabe));
                        

                        break;
                    }
                    
                case "Schwer":
                    Console.WriteLine("Schwer");
                    break;
                default:
                    Console.WriteLine("Default case");
                    break;
            }

        }


        public void FillCharacter(char character)
        {
            for(int i = 0; i< gesuchtesWortarray.Length;i++)
            {
                if(gesuchtesWortarray[i] == character)
                {
                    if (Buchstaben[i].Text == "")
                    {
                        Buchstaben[i].Text = character.ToString();
                        NochOffen--;
                    }
                }
            }
        }


        public void UpdateUI()
        {
            Updatelistbox();
            ShowUser();
            
        }

        private void ShowUser()
        {
            if (currentUser != null)
            {
                labelBenutzerAnzeige.Text = currentUser.Name;
                ShowLiga();
            }
                
            else if (currentUser == null && GuestUser == true)
            {
                labelBenutzerAnzeige.Text = "Gast";
            }
            else
            {
                labelBenutzerAnzeige.Text = string.Empty;
            }


        }

        private void ShowLiga()
        {

            if (currentUser.Score.Points >= 0 && currentUser.Score.Points <= 99)
            {
                label7.Text = 100 - currentUser.Score.Points + " bis Liga Bronze";
                label6.Text = "keine Liga";
            }

            else if (currentUser.Score.Points >= 100 && currentUser.Score.Points <= 249)
            {
                label7.Text = 250 - currentUser.Score.Points + " bis Liga Silber";
                label6.Text = "Bronze";
            }
            else if (currentUser.Score.Points >= 250 && currentUser.Score.Points <= 499)
            {
                label7.Text = 500 - currentUser.Score.Points + " bis Liga Gold";
                label6.Text = "Silber";
            }
            else if (currentUser.Score.Points >= 500 && currentUser.Score.Points <= 749)
            {
                label7.Text = 750 - currentUser.Score.Points + " bis Liga Platin";
                label6.Text = "Gold";
            }
            else if (currentUser.Score.Points >= 750 && currentUser.Score.Points <= 999)
            {
                label7.Text = 1000 - currentUser.Score.Points + " bis Liga Diamant";
                label6.Text = "Platin";
            }
            else if(currentUser.Score.Points >= 1000)
            {
                label7.Text = "Du hast die höhste Liga erreicht.";
                label6.Text = "Diamant";
            }
        }

        private void Updatelistbox()
        {
            listBox2.Items.Clear();

            foreach (User u in Users.User)
            {
                listBox2.Items.Add($"{u.Name} Matches:{u.Score.Matches} Wins:{u.Score.Wins} Losts:{u.Score.Losts} Points:{u.Score.Points}");
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            XmlSerializer deserializer = new XmlSerializer(typeof(Users));
            TextReader reader = new StreamReader(@"C:\Users\medin.krupic\Desktop\UserDB");
            object obj = deserializer.Deserialize(reader);
            Users = (Users)obj;
            reader.Close();



            XmlDocument doc = new XmlDocument();
            doc.Load(@"C:\Users\medin.krupic\Desktop\Dictionary.xml");

            XmlNodeList words = doc.SelectNodes("//Wort");

            foreach (XmlNode xmlnode in words)
            {
                wordsdictionary.Add(xmlnode.InnerText);
            }
            comboBox1.Items.Add("Leicht");
            comboBox1.Items.Add("Mittel");
            comboBox1.Items.Add("Schwer");


            UpdateUI();
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            string name = Microsoft.VisualBasic.Interaction.InputBox("Wie heißt du", "Registrieren", "", 0, 0);
            string pwd = Microsoft.VisualBasic.Interaction.InputBox("Passwort festlegen", "Passwort", "", 0, 0);

            if (name == "" || pwd == "")
            {
                MessageBox.Show("Bitte gültigen text eingeben.");
                return;
            }

            User newuser = new User();
            newuser.Name = name;
            newuser.Password = pwd;

            Users.User.Add(newuser);

            Save();
        }

        private void Save()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Users));
            using (TextWriter writer = new StreamWriter(@"C:\Users\medin.krupic\Desktop\UserDB"))
            {
                serializer.Serialize(writer, Users);
            }

            UpdateUI();
        }

        private void Anmelden(object sender, EventArgs e)
        {

            if (currentUser != null)
            {
                MessageBox.Show("Du bist bereits angemeldet " + currentUser.Name + ".");
                return;
            }

            string name = Microsoft.VisualBasic.Interaction.InputBox("Name", "Login", "", 0, 0);
            string pwd = Microsoft.VisualBasic.Interaction.InputBox("Passwort", "Login", "", 0, 0);

            if (name == "" || pwd == "")
            {
                MessageBox.Show("Bitte gültigen text eingeben.");
                return;
            }


            User foundUser = Users.User.FirstOrDefault(ee => ee.Name == name && ee.Password == pwd);

            if(foundUser != null)
            {
                this.currentUser = foundUser;
                MessageBox.Show(name+ " erfolgreich angemeldet");
            }
            else
            {
                MessageBox.Show("Benutzername oder Passwort sind falsch.");
            }

            UpdateUI();
        }

        private void Button5_Click(object sender, EventArgs e)
        {
            //Auslogen
            currentUser = null;
            UpdateUI();
        }

        private void AlsGastAnmelden(object sender, EventArgs e)
        {
            currentUser = null;
            GuestUser = true;
            MessageBox.Show("Du Spielst als Gast.");
            ShowUser();  
        }

        private void Label6_Click(object sender, EventArgs e)
        {
            
        }
    }
}
