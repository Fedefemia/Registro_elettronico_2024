using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Newtonsoft.Json;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace Registroelettronico
{
    public partial class Form1 : Form
    {
        private Label ChangePass;
        private Button myButton;
        private TextBox UserBox;
        private TextBox PasswordBox;
        private Button PasswordButton;
        private Label InfoLabel;
        private bool userb;
        private bool passwordb;
        private AuthenticationService authService;
        string Insegnantifilepath = Path.GetFullPath("Users.txt");
        private List<Student> students;
        private bool[] classes;
        private string[] classestud;
        private int i;
        private int j;
        private string nomeinsegnante;
        private int position;
        private string classeora;
        private string matricolaora;
        private Panel scrollPanel;
        Button Backto;
        Button Hometo;
        string Studentifilepath;
        string userinsegnante;
        public Form1()
        {
            InitializeComponent();
            CreateLogin();
        }

        private void RemovePlaceholderText(object sender, EventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null)
            {
                if (textBox.Text == "Username" || textBox.Text == "Password")
                {
                    textBox.Text = "";
                    textBox.ForeColor = Color.Black;
                    if (textBox == PasswordBox)
                    {
                        textBox.UseSystemPasswordChar = true;
                    }
                }
            }
        }

        private void SetPlaceholderText(object sender, EventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null)
            {
                if (string.IsNullOrWhiteSpace(textBox.Text))
                {
                    if (textBox == UserBox)
                    {
                        textBox.Text = "Username";
                    }
                    else if (textBox == PasswordBox)
                    {
                        textBox.Text = "Password";
                        textBox.UseSystemPasswordChar = false;
                    }
                    textBox.ForeColor = Color.Gray;
                }
            }
        }

        private void ShowHide(object sender, EventArgs e)
        {
            if (PasswordBox.Text != "Password")
            {
                if (PasswordBox.UseSystemPasswordChar == false)
                {
                    PasswordBox.UseSystemPasswordChar = true;
                }
                else
                {
                    PasswordBox.UseSystemPasswordChar = false;
                }
            }
        }
        private void CreateLogin()
        {
            authService = new AuthenticationService(Insegnantifilepath);
            Studentifilepath = "students.json";
            classes = new bool[] { false, false, false, false, false };
            this.AutoScroll = false;
            if (File.Exists(Studentifilepath))
            {
                string jsonData = File.ReadAllText(Studentifilepath);
                students = JsonConvert.DeserializeObject<List<Student>>(jsonData);
            }
            else
            {
                MessageBox.Show("File non trovato!");
            }
            position = 0;
            Console.WriteLine("new position= " + position);
            for (int i = this.Controls.Count - 1; i >= 0; i--)
            {
                Control control = this.Controls[i];
                this.Controls.Remove(control);
                control.Dispose();
            }

            this.Size = new System.Drawing.Size(400, 600); // Larghezza, Altezza
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            ChangePass = new Label();
            ChangePass.Text = "Non ricordo la mia password";
            ChangePass.ForeColor = Color.FromArgb(30, 144, 255);
            ChangePass.Location = new System.Drawing.Point(210, 260);
            ChangePass.Size = new System.Drawing.Size(140, 30);
            this.Controls.Add(ChangePass);
            ChangePass.Click += delegate {MessageBox.Show("Studente: La tua password é il numero di matricola, chiedi ad un Insegnante di riceverla di nuovo. \n \n Insegnante: Contatta l'amministratore per modificare la password");};
            ChangePass.MouseEnter += delegate { ChangePass.ForeColor = Color.FromArgb(0, 0, 128); };
            ChangePass.MouseLeave += delegate { ChangePass.ForeColor = Color.FromArgb(30, 144, 255); };

            myButton = new Button();
            myButton.Text = "Login";
            myButton.Location = new System.Drawing.Point(150, 350);
            myButton.Size = new System.Drawing.Size(100, 30);
            myButton.Click += new EventHandler(LoginTry);
            this.Controls.Add(myButton);

            UserBox = new TextBox();
            UserBox.ForeColor = Color.Gray;
            UserBox.Location = new System.Drawing.Point(50, 200);
            UserBox.Size = new System.Drawing.Size(300, 25);
            this.Controls.Add(UserBox);
            UserBox.Text = "Username";
            UserBox.Enter += RemovePlaceholderText;
            UserBox.Leave += SetPlaceholderText;


            PasswordBox = new TextBox();
            PasswordBox.ForeColor = Color.Gray;
            PasswordBox.Location = new System.Drawing.Point(50, 240);
            PasswordBox.Size = new System.Drawing.Size(300, 25);
            this.Controls.Add(PasswordBox);
            PasswordBox.Text = "Password";
            PasswordBox.Enter += RemovePlaceholderText;
            PasswordBox.Leave += SetPlaceholderText;

            PasswordButton = new Button();
            PasswordButton.Text = "X";
            PasswordButton.Location = new System.Drawing.Point(330, 240);
            PasswordButton.Size = new System.Drawing.Size(20, 20);
            PasswordButton.Click += new EventHandler(ShowHide);
            this.Controls.Add(PasswordButton);
            PasswordButton.BringToFront();

        }

        private void LoginTry(object sender, EventArgs e)
        {
            string username = UserBox.Text;
            string password = PasswordBox.Text;

            var user = authService.Authenticate(username, password);
            if (UserBox.Text == "Username" || PasswordBox.Text == "Password")
            {
                InfoLabel = new Label();
                InfoLabel.ForeColor = Color.Red;
                InfoLabel.Text = "Compila tutti i campi";
                InfoLabel.Location = new System.Drawing.Point(50, 260);
                InfoLabel.Size = new Size(400, 100);
                this.Controls.Add(InfoLabel);
            }
            else
            {
                this.Controls.Remove(InfoLabel);
                userb = false;
                for (int i = 0; i < students.Count; i++)
                {
                    var student = students[i];
                    if (UserBox.Text == $"{student.nome}.{student.cognome}" && PasswordBox.Text == $"{student.matricola}")
                    {
                        userb = true;
                        break;
                    }
                }

                if (user == null && userb == false)
                {
                    InfoLabel = new Label();
                    InfoLabel.ForeColor = Color.Red;
                    InfoLabel.Text = "Nome utente o Password errati";
                    InfoLabel.Location = new System.Drawing.Point(50, 260);
                    InfoLabel.Size = new Size(150, 100);
                    this.Controls.Add(InfoLabel);
                    UserBox.Clear();
                    PasswordBox.Clear();
                    UserBox.ForeColor = Color.Gray;
                    PasswordBox.ForeColor = Color.Gray;
                    UserBox.Text = "Username";
                    PasswordBox.Text = "Password";
                    PasswordBox.UseSystemPasswordChar = false;
                }
                else
                {
                    if (userb == false)
                    {
                        nomeinsegnante = UserBox.Text;
                        userinsegnante = nomeinsegnante;
                        nomeinsegnante = nomeinsegnante.Replace('.', ' ');
                        pulisciGui();
                        AdminGui();
                    }
                    else
                    {
                        nomeinsegnante = PasswordBox.Text;
                        Console.WriteLine("nome insegnante " + nomeinsegnante);
                        pulisciGui();
                        UserGui();
                    }

                }
            }
        }
        private void pulisciGui()
        {
            if (this.Controls.Contains(Backto))
            {
                Backto.Enabled = true;
                Hometo.Enabled = true;
            }
            
            this.Size = new System.Drawing.Size(1200, 600); // Larghezza Altezza
            for (int i = this.Controls.Count - 1; i >= 0; i--)
            {
                Control control = this.Controls[i];
                if (control.Name != "AdminLabel")
                {
                    this.Controls.Remove(control);
                    control.Dispose();
                }
            }
        }
        private Image ResizeImage(Image image, int width, int height)
        {
            Bitmap resizedBitmap = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(resizedBitmap))
            {
                g.DrawImage(image, 0, 0, width, height);
            }
            return resizedBitmap;
        }
        private void UserGui()
        {
            position = 1;
            Console.WriteLine("new position= " + position);
            Label AdminLabel2 = new Label();
            AdminLabel2.Text = "Studente";
            AdminLabel2.Font = new Font(AdminLabel2.Font.FontFamily, 7);
            AdminLabel2.Location = new System.Drawing.Point(1125, 15);
            AdminLabel2.Size = new Size(50, 30);

            Button Backto = new Button();
            string imagePath = "backto.png";
            Image originalImage = Image.FromFile(imagePath);
            Image resizedImage = ResizeImage(originalImage, 25, 25);
            Backto.Image = resizedImage;
            Backto.Location = new System.Drawing.Point(5, 5);
            Backto.Size = new Size(40, 40);
            Backto.Click += (sender, e) => Back_Click(0);

            this.Controls.Add(AdminLabel2);
            this.Controls.Add(Backto);
            AdminLabel2.Name = "AdminLabel";
            Backto.Name = "AdminLabel";
            List<Student> filteredStudents = new List<Student>();
            User_Click(nomeinsegnante);
        }
        private void AdminGui()
        {
            string pass = "";
            if (File.Exists(Insegnantifilepath))
            {
                var lines = File.ReadAllLines(Insegnantifilepath);
                for (int i = 0; i < lines.Length; i++)
                {
                    var parts = lines[i].Split(';');
                    pass = parts[1];
                }
            }
            else
            {
                MessageBox.Show("User file not found");
            }

            Button Backto = new Button();
            string imagePath = "backto.png";
            Image originalImage = Image.FromFile(imagePath);
            Image resizedImage = ResizeImage(originalImage, 25, 25);
            Backto.Image = resizedImage;
            Button Hometo = new Button();
            string imageHomePath = "hometo.png";
            Image originalHomeImage = Image.FromFile(imageHomePath);
            Image resizedHomeImage = ResizeImage(originalHomeImage, 25, 25);
            Hometo.Image = resizedHomeImage;
            position = 1;
            Console.WriteLine("new position= " + position);
            Backto.Enabled = true;
            Hometo.Enabled = true;
            Label AdminLabel = new Label();
            AdminLabel.Text = nomeinsegnante;
            AdminLabel.Font = new Font(AdminLabel.Font.FontFamily, 9);
            AdminLabel.Location = new System.Drawing.Point(85, 15);
            AdminLabel.Size = new Size(80, 30);

            Label AdminLabel2 = new Label();
            AdminLabel2.Text = "Admin";
            AdminLabel2.Font = new Font(AdminLabel2.Font.FontFamily, 7);
            AdminLabel2.Location = new System.Drawing.Point(1145, 15);
            AdminLabel2.Size = new Size(37, 30);


            Backto.Location = new System.Drawing.Point(5, 5);
            Backto.Size = new Size(40, 40);
            Backto.Click += (sender, e) => Back_Click(position);


            Hometo.Location = new System.Drawing.Point(43, 5);
            Hometo.Size = new Size(40, 40);
            Hometo.Click += (sender, e) => Back_Click(1);

            Label Bentornatolabel = new Label();
            Bentornatolabel.Text = "Bentornato " + nomeinsegnante + "!";
            Bentornatolabel.Font = new Font(AdminLabel.Font.FontFamily, 20);
            Bentornatolabel.Location = new System.Drawing.Point(450, 10);
            Bentornatolabel.Size = new Size(400, 30);

            Label Classilabel = new Label();
            Classilabel.Text = "Le tue classi:";
            Classilabel.Font = new Font(AdminLabel.Font.FontFamily, 14);
            Classilabel.Location = new System.Drawing.Point(500, 120);
            Classilabel.Size = new Size(200, 30);

            Button Editprof = new Button();
            Editprof.Text = "Modifica la password dell'account";
            Editprof.Location = new System.Drawing.Point(915, 500);
            Editprof.Size = new Size(250, 45);
            Editprof.Click += (sender, e) => {
                for (int ias = this.Controls.Count - 1; ias >= 0; ias--)
                {
                    Control control = this.Controls[ias];
                    control.Enabled = false;
                }
                Panel aggiungivotopan = new Panel
                {
                    Size = new Size(300, 500),
                    Location = new Point(463, 50),
                    BorderStyle = BorderStyle.FixedSingle,

                };
                this.Controls.Add(aggiungivotopan);
                aggiungivotopan.BringToFront();
                Button layoutpannello = new Button
                {
                    Size = new Size(300, 22),
                    Location = new Point(0, 0),
                    FlatStyle = FlatStyle.Flat
                };
                Button confirmpanel = new Button
                {
                    Size = new Size(80, 30),
                    Location = new Point(212, 462),
                    Text = "Applica",
                    FlatStyle = FlatStyle.System
                };
                Button annullapanel = new Button
                {
                    Size = new Size(80, 30),
                    Location = new Point(128, 462),
                    Text = "Cancella",
                    FlatStyle = FlatStyle.System
                };
                Label Infoclass = new Label
                {
                    Size = new Size(120, 15),
                    Location = new Point(1, 4),
                    Text = "Modifica Password",
                    FlatStyle = FlatStyle.System,
                    TextAlign = ContentAlignment.MiddleCenter,

                };
                TextBox confirmpass = new TextBox
                {
                    Size = new Size(250, 15),
                    Location = new Point(25, 120),
                };
                Label confirmpasslabel = new Label
                {
                    Size = new Size(100, 15),
                    Location = new Point(22, 105),
                    Text = "Password Attuale:"
                };
                TextBox newwpass = new TextBox
                {
                    Size = new Size(250, 15),
                    Location = new Point(25, 200),
                };
                Label newpasslabel = new Label
                {
                    Size = new Size(100, 15),
                    Location = new Point(22, 185),
                    Text = "Nuova Password:"
                };
                TextBox confirmnewwpass = new TextBox
                {
                    Size = new Size(250, 15),
                    Location = new Point(25, 250),
                };
                Label confirmnewpasslabel = new Label
                {
                    Size = new Size(100, 15),
                    Location = new Point(22, 235),
                    Text = "Conferma Nuova Password:"
                };
                confirmpanel.Click += (s, args) =>{

                    if (confirmpass.Text == pass && newwpass.Text == confirmnewwpass.Text) { 
                    string password = newwpass.Text;
                    UpdatePasswordInFile(userinsegnante, password);
                    this.Controls.Remove(aggiungivotopan);
                    for (int ias = this.Controls.Count - 1; ias >= 0; ias--)
                    {
                        Control control = this.Controls[ias];
                        control.Enabled = true;
                    }
                }

                };
                annullapanel.Click += (s, args) => {
                    this.Controls.Remove(aggiungivotopan);
                    for (int ias = this.Controls.Count - 1; ias >= 0; ias--)
                    {
                        Control control = this.Controls[ias];
                        control.Enabled = true;
                    }
                };
                aggiungivotopan.Controls.Add(layoutpannello);
                aggiungivotopan.Controls.Add(Infoclass);
                aggiungivotopan.Controls.Add(confirmpanel);
                aggiungivotopan.Controls.Add(annullapanel);
                aggiungivotopan.Controls.Add(confirmpass);
                aggiungivotopan.Controls.Add(newwpass);
                aggiungivotopan.Controls.Add(confirmnewwpass);
                aggiungivotopan.Controls.Add(confirmpasslabel);
                aggiungivotopan.Controls.Add(newpasslabel);
                aggiungivotopan.Controls.Add(confirmnewpasslabel);
                
                Infoclass.BringToFront();
                layoutpannello.Enabled = false;
            };

            this.Controls.Add(AdminLabel);
            this.Controls.Add(AdminLabel2);
            this.Controls.Add(Bentornatolabel);
            this.Controls.Add(Classilabel);
            this.Controls.Add(Editprof);
            this.Controls.Add(Backto);
            this.Controls.Add(Hometo);
            AdminLabel.Name = "AdminLabel";
            AdminLabel2.Name = "AdminLabel";
            Backto.Name = "AdminLabel";
            Hometo.Name = "AdminLabel";
            Ordinaclassi();
            CreateClassButtons();
        }
        public void UpdatePasswordInFile(string username, string newPassword)
        {
            if (File.Exists(Insegnantifilepath))
            {
                var lines = File.ReadAllLines(Insegnantifilepath);
                bool userFound = false;
                for (int i = 0; i < lines.Length; i++)
                {
                    var parts = lines[i].Split(';');
                    if (parts.Length == 2 && parts[0] == username)
                    {
                        lines[i] = $"{username};{newPassword}";
                        userFound = true;
                        MessageBox.Show("new pass " + newPassword);
                        break;
                    }
                }

                if (userFound)
                {
                    File.WriteAllLines(Insegnantifilepath, lines);
                }
                else
                {
                    MessageBox.Show("Username not found");
                }
            }
            else
            {
                MessageBox.Show("User file not found");
            }
        }

        private void Back_Click(int pos)
        {
            Console.WriteLine("pos " + pos + " position" + position);
            if (this.Controls.Contains(Backto))
            {
                Backto.Enabled = false;
                Hometo.Enabled = false;
            }
            
            this.AutoScroll = false;
            pulisciGui();
            if (userb)
            {
                CreateLogin();
            }
            else
            {
            switch (pos)
            {
                case 1:
                        for (int i = this.Controls.Count - 1; i >= 0; i--)
                        {
                            Control control = this.Controls[i];
                            if (control.Name == "AdminLabel")
                            {
                                this.Controls.Remove(control);
                                control.Dispose();
                            }
                        }
                    CreateLogin();
                    break;

                case 2:
                        AdminGui();
                        break;

                case 3:
                        ClassButton_Click(classeora);
                        break;

                default:
                    MessageBox.Show("Errore imprevisto position " + pos.ToString());
                    break;
            };
            }
        }
        private void CreateClassButtons()
        {
            int buttonTop = 175;
            int buttonLeft = 500;
            foreach (var classe in classestud)
            {
                Button classButton = new Button();
                classButton.Text = classe;
                classButton.Left = buttonLeft;
                classButton.Top = buttonTop;
                classButton.Width = 200;
                classButton.Height = 30;
                classeora = classe;
                classButton.Click += (sender, e) => ClassButton_Click(classe);
                this.Controls.Add(classButton);
                buttonTop += 40;
            }
        }

        private void ClassButton_Click(string classe)
        {
            position = 2;
            Console.WriteLine("new position= " + position);
            pulisciGui();
            classeora = classe;
            int buttonTop = 115;
            int buttonLeft = 40;
            int countst = 0;

            Label infoz = new Label();
            infoz.Text = classe;
            infoz.Font = new Font(infoz.Font.FontFamily, 16);
            infoz.Left = 40;
            infoz.Top = 75;
            infoz.Width = 80;
            infoz.Height = 30;
            this.Controls.Add(infoz);

            Button aggiungiclasse = new Button();
            aggiungiclasse.Text = "Nuova materia";
            aggiungiclasse.Left = 120;
            aggiungiclasse.Top = 75;
            aggiungiclasse.Width = 120;
            aggiungiclasse.Height = 30;
            this.Controls.Add(aggiungiclasse);
            aggiungiclasse.Click += (s, args) =>
            {
                this.Controls.Remove(aggiungiclasse);
                TextBox Materianew = new TextBox();
                Materianew.Left = 120;
                Materianew.Top = 80;
                Materianew.Width = 90;

                Button Materianewbu = new Button();
                Materianewbu.Left = 210;
                Materianewbu.Top = 80;
                Materianewbu.Width = 30;
                Materianewbu.Height = 20;
                this.Controls.Add(Materianew);
                this.Controls.Add(Materianewbu);
                Materianewbu.Click += (d, idk) =>
                {
                    string NuovaMat = Materianew.Text;
                    if (NuovaMat != "")
                    {
                        this.Controls.Add(aggiungiclasse);
                        this.Controls.Remove(Materianew);
                        this.Controls.Remove(Materianewbu);

                        foreach (var student in students)
                        {
                            if (student.classe == classe)
                            {
                                if (!student.voti.ContainsKey(NuovaMat))
                                {
                                    student.voti.Add(NuovaMat, new List<int>());
                                }
                            }
                        }

                        string updatedJsonData = JsonConvert.SerializeObject(students, Formatting.Indented);
                        File.WriteAllText(Studentifilepath, updatedJsonData);
                        ClassButton_Click(classe);
                        MessageBox.Show("Aggiunta la materia: \"" + NuovaMat + "\" dalla classe " + classe);
                    }
                };
            };


            Button rimuoviclasse = new Button();
            rimuoviclasse.Text = "Rimuovi materia";
            rimuoviclasse.Left = 250;
            rimuoviclasse.Top = 75;
            rimuoviclasse.Width = 120;
            rimuoviclasse.Height = 30;
            this.Controls.Add(rimuoviclasse);
            rimuoviclasse.Click += (s, args) =>
            {
                this.Controls.Remove(rimuoviclasse);
                ComboBox Materiadel = new ComboBox();
                Materiadel.Left = 250;
                Materiadel.Top = 80;
                Materiadel.Width = 90;
                string[] materie = { };
                foreach (var student in students)
                {
                    if (student.classe == classe)
                    {
                        materie = student.voti.Keys.ToArray();
                        break;
                    }
                }
                Materiadel.Items.Clear();
                Materiadel.Items.AddRange(materie);
                Button Materiadelbu = new Button();
                Materiadelbu.Left = 340;
                Materiadelbu.Top = 80;
                Materiadelbu.Width = 30;
                Materiadelbu.Height = 20;
                this.Controls.Add(Materiadel);
                this.Controls.Add(Materiadelbu);
                Materiadelbu.Click += (d, idk) =>
                {
                    string NuovaMat = Materiadel.Text;
                    if (NuovaMat != "")
                    {
                        this.Controls.Add(rimuoviclasse);
                        this.Controls.Remove(Materiadel);
                        this.Controls.Remove(Materiadelbu);

                        foreach (var student in students)
                        {
                            if (student.classe == classe)
                            {
                                if (student.voti.ContainsKey(NuovaMat))
                                {
                                    student.voti.Remove(NuovaMat);
                                }
                            }
                        }
                        string updatedJsonData = JsonConvert.SerializeObject(students, Formatting.Indented);
                        File.WriteAllText(Studentifilepath, updatedJsonData);
                        ClassButton_Click(classe);
                        MessageBox.Show("Rimossa la materia: \"" + NuovaMat + "\" dalla classe " + classe);
                    }
                };
            };


            List<Student> filteredStudents = new List<Student>();
            foreach (var student in students)
            {
                if (student.classe == classe)
                {
                    filteredStudents.Add(student);
                }
            }

            for (int i = 1; i < filteredStudents.Count; i++)
            {
                var currentStudent = filteredStudents[i];
                int j = i - 1;

                while (j >= 0 && string.Compare(filteredStudents[j].cognome, currentStudent.cognome) > 0)
                {
                    filteredStudents[j + 1] = filteredStudents[j];
                    j--;
                }
                filteredStudents[j + 1] = currentStudent;
            }

            for (int i = 0; i < filteredStudents.Count; i++)
            {
                if (countst == 11)
                {
                    countst = 0;
                    buttonTop = 115;
                    buttonLeft += 200;
                }

                var student = filteredStudents[i];

                Button classButton = new Button();
                classButton.Text = (i + 1).ToString() + " " + student.cognome + " " + student.nome;
                classButton.Left = buttonLeft;
                classButton.Top = buttonTop;
                classButton.Width = 180;
                classButton.Height = 30;
                string matricola = student.matricola.ToString();
                matricolaora = matricola;
                classButton.Click += (s, args) => User_Click(matricola);

                this.Controls.Add(classButton);
                buttonTop += 40;
                countst++;
            }
        }

        private void User_Click(string matricola)
        {
            List<Student> filteredStudents = new List<Student>();
            position = 3;
            Console.WriteLine("new position= " + position);
            var student = students[0];
            string[] materie = { };
            for (int i = 0; i < students.Count; i++)
            {
                student = students[i];
                if (matricola == student.matricola.ToString())
                {
                    materie = student.voti.Keys.ToArray();
                    break;
                }
            }
            pulisciGui();
            scrollPanel = new Panel();
            scrollPanel.Location = new Point(0, 45);
            scrollPanel.Size = new Size(1200, 555);
            scrollPanel.AutoScroll = true;
            this.Controls.Add(scrollPanel);
            Label InfoStud = new Label();

            InfoStud.Text = student.classe + " - " + student.cognome.ToUpper() + " " + student.nome.ToUpper();
            InfoStud.Width = 400;
            InfoStud.Height = 60;
            InfoStud.Left = 40;
            InfoStud.Top = 30;
            InfoStud.Font = new Font(InfoStud.Font.FontFamily, 16);

            scrollPanel.Controls.Add(InfoStud);

            int buttonTop = 90;
            int buttonLeft = 40;
            for (int i = 0; i < materie.Length; i++)
            {
                Button classButton = new Button();
                classButton.Text = materie[i];
                classButton.Left = buttonLeft;
                classButton.Top = buttonTop;
                classButton.Width = 120;
                classButton.Height = 30;
                scrollPanel.Controls.Add(classButton);

                if (!student.voti.ContainsKey(materie[i]))
                {
                    student.voti[materie[i]] = new List<int>(); // Crea una lista vuota se non esiste
                }

                List<int> votiList = student.voti[materie[i]];
                int labelleft = 185;
                float votomedio = 0;
                float cont = 0;
                string materiabut = materie[i];

                foreach (int voto in votiList)
                {
                    Label votilab = new Label();
                    votilab.Text = voto.ToString();
                    votilab.Font = new Font(votilab.Font.FontFamily, 9);
                    votilab.Left = labelleft;
                    votilab.Top = buttonTop + 3;
                    votilab.Width = 25;
                    votilab.Height = 25;
                    votilab.Name = "VotoLabel";

                    if (voto >= 6)
                        votilab.BackColor = Color.LightGreen;
                    else if (voto == 5)
                        votilab.BackColor = Color.Orange;
                    else
                        votilab.BackColor = Color.Red;

                    votilab.TextAlign = ContentAlignment.MiddleCenter;

                    scrollPanel.Controls.Add(votilab);

                    votilab.Click += (sender, e) =>
                    {
                        student.voti[materiabut].Remove(voto);
                        if (student.voti[materiabut].Count == 0)
                        {
                            student.voti.Remove(materiabut);
                        }
                        string updatedJsonData = JsonConvert.SerializeObject(students, Formatting.Indented);
                        File.WriteAllText(Studentifilepath, updatedJsonData);
                        User_Click(matricola);
                    };

                    labelleft += 30;
                    cont++;
                    votomedio += voto;
                }

                if (!userb)
                {
                    Button aggiungivoto = new Button();
                    aggiungivoto.Text = "+";
                    aggiungivoto.Left = labelleft;
                    aggiungivoto.Top = buttonTop + 3;
                    aggiungivoto.Width = 25;
                    aggiungivoto.Height = 25;
                    aggiungivoto.Name = "VotoLabel";
                    labelleft += 30;
                    scrollPanel.Controls.Add(aggiungivoto);
                    aggiungivoto.Click += (s, args) =>
                    {
                        for (int ias = this.Controls.Count - 1; ias >= 0; ias--)
                        {
                            Control control = this.Controls[ias];
                            control.Enabled = false;
                        }
                        Aggiungi_voto(matricola, materiabut);
                    };
                }

                if (cont != 0)
                {
                    votomedio /= cont;
                    Label votomed = new Label();
                    votomed.Text = votomedio.ToString("0.0");
                    votomed.Font = new Font(votomed.Font.FontFamily, 9);
                    votomed.Left = labelleft + 20;
                    votomed.Top = buttonTop + 3;
                    votomed.Width = 25;
                    votomed.Height = 25;
                    votomed.Name = "VotoLabel";
                    if (votomedio >= 6)
                        votomed.BackColor = Color.LightGreen;
                    else if (votomedio < 5)
                        votomed.BackColor = Color.Red;
                    else
                        votomed.BackColor = Color.Orange;

                    votomed.TextAlign = ContentAlignment.MiddleCenter;

                    scrollPanel.Controls.Add(votomed);
                }
                buttonTop += 40;
            }
        }


        private void Aggiungi_voto(string matricola, string materia)
        {
            var student = students[0];
            for (int glu = 0; glu < students.Count; glu++)
            {
                student = students[glu];
                if(matricola == student.matricola.ToString())
                {
                    break;
                }
            }
            Panel aggiungivotopan = new Panel
            {
                Size = new Size(800, 300),
                Location = new Point(200, 150),
                BorderStyle = BorderStyle.FixedSingle,
                
            };
            this.Controls.Add(aggiungivotopan);
            aggiungivotopan.BringToFront();
            Button layoutpannello = new Button
            {
                Size = new Size(800, 22),
                Location = new Point(0, 0),
                FlatStyle = FlatStyle.Flat
            };
            Button closepanel = new Button
            {
                Size = new Size(30, 20),
                Location = new Point(768, 1),
                Text = "x",
                FlatStyle = FlatStyle.System
            };
            Button confirmpanel = new Button
            {
                Size = new Size(80, 30),
                Location = new Point(715, 265),
                Text = "Applica",
                FlatStyle = FlatStyle.System
            };
            Button annullapanel = new Button
            {
                Size = new Size(80, 30),
                Location = new Point(635, 265),
                Text = "Cancella",
                FlatStyle = FlatStyle.System
            };
            Label Infoclass = new Label
            {
                Size = new Size(80, 15),
                Location = new Point(1, 4),
                Text = "Aggiungi Voto",
                FlatStyle = FlatStyle.System,
                TextAlign = ContentAlignment.MiddleCenter,
                
            };
            Label Infoclass2 = new Label
            {
                Size = new Size(300, 60),
                Location = new Point(100, 50),
                Text = "Studente: " + student.nome + " " + student.cognome + " (" + student.matricola + ")"  + "\nMateria: " +  materia,
                FlatStyle = FlatStyle.System,
                TextAlign = ContentAlignment.MiddleCenter,

            };
            NumericUpDown Votobox = new NumericUpDown
            {
                Size = new Size(100, 40),
                Location = new Point(100, 150),
                Value = 6,
                Maximum = 10,
                Minimum = 0,
            };
            float votomedio = 0;
            float cont = 0;
            float mediarr = votomedio;
            mediarr /= cont;
            Label Mediac = new Label
            {
                Location = new Point(270, 150),
                Text = mediarr.ToString("0.0"),
                TextAlign = ContentAlignment.MiddleCenter,
                FlatStyle = FlatStyle.System,
                Size = new Size(20, 20)
            };
            Mediac.BackColor = Color.LightBlue;
            List<int> votiList = student.voti[materia];
            votomedio = 0;
            cont = 0;
            foreach (int voto in votiList)
            {
                cont++;
                votomedio += voto;
            }
            float mediard = votomedio;
            mediard += (float)Votobox.Value;
            mediard /= cont + 1;
            Mediac.Text = mediard.ToString("0.0");

            Infoclass.Font = new Font(Infoclass.Font.FontFamily, 8);
            Infoclass2.Font = new Font(Infoclass2.Font.FontFamily, 11);
            Infoclass2.TextAlign = ContentAlignment.MiddleLeft;
            closepanel.Click += (s, args) => { this.Controls.Remove(aggiungivotopan);
                for (int ias = this.Controls.Count - 1; ias >= 0; ias--)
                {
                    Control control = this.Controls[ias];
                    control.Enabled = true;
                }
            };
            annullapanel.Click += (s, args) => { this.Controls.Remove(aggiungivotopan);
                for (int ias = this.Controls.Count - 1; ias >= 0; ias--)
                {
                    Control control = this.Controls[ias];
                    control.Enabled = true;
                }
            };
            votomedio = 0;
            cont = 0;
            foreach (int voto in votiList)
            {
                cont++;
                votomedio += voto;
            }
            mediarr = votomedio;
            mediarr /= cont;
            Label Mediar = new Label
            {
                Location = new Point(250, 150),
                Text = mediarr.ToString("0.0"),
                TextAlign = ContentAlignment.MiddleCenter,
                FlatStyle = FlatStyle.System,
                Size = new Size(20, 20)
            };
            Mediar.Font = new Font(Mediar.Font.FontFamily, Mediar.Font.Size);
            if (mediarr >= 6)
                Mediar.BackColor = Color.LightGreen;
            else
                   if (mediarr < 5)

                Mediar.BackColor = Color.Red;
            else
                Mediar.BackColor = Color.Orange;

           
            
            Votobox.ValueChanged += (s, args) => {
                mediard = votomedio;
                mediard += (float)Votobox.Value;
                mediard /= cont + 1;
                Mediac.Text = mediard.ToString("0.0");
            };
            confirmpanel.Click += (s, args) => {

                for (int ias = this.Controls.Count - 1; ias >= 0; ias--)
                {
                    Control control = this.Controls[ias];
                    control.Enabled = true;
                }
                int voto = (int)Votobox.Value;
                    if (student.voti.ContainsKey(materia))
                    {
                        student.voti[materia].Add(voto);
                        string updatedJsonData = JsonConvert.SerializeObject(students, Formatting.Indented);
                        File.WriteAllText(Studentifilepath, updatedJsonData);
                        User_Click(matricola);
                    }
                    else
                    {
                        MessageBox.Show("No");
                    }             
                         
            };
            aggiungivotopan.Controls.Add(layoutpannello);
            aggiungivotopan.Controls.Add(closepanel);
            aggiungivotopan.Controls.Add(Infoclass);
            aggiungivotopan.Controls.Add(Infoclass2);
            aggiungivotopan.Controls.Add(confirmpanel);
            aggiungivotopan.Controls.Add(annullapanel);
            aggiungivotopan.Controls.Add(Votobox);
            aggiungivotopan.Controls.Add(Mediar);
            aggiungivotopan.Controls.Add(Mediac);
            closepanel.BringToFront();
            Infoclass.BringToFront();
            Infoclass2.BringToFront();
            Votobox.BringToFront();
            Mediar.BringToFront();
            Mediac.BringToFront();
            layoutpannello.Enabled = false;
        }
        private void Ordinaclassi()
        {
            var classList = new List<string>();

            for (i = 0; i < students.Count; i++)
            {
                var student = students[i];
                if (!classList.Contains(student.classe))
                {
                    classList.Add(student.classe);
                }
            }
            classestud = classList.ToArray();
            i = 0;

            while (i < classestud.Length - 1)
            {
                j = i + 1;
                Checkclasses();
            }

            foreach (var classe in classestud)
            {
              //  Console.WriteLine(classe);
            }
        }
        private void Checkclasses()
        {
            if (i < 0 || i >= classestud.Length || j < 0 || j >= classestud.Length)
            {
                return;
            }
            int firstCharacterI = classestud[i][0] - '0';
            int firstCharacterJ = classestud[j][0] - '0';
            if (firstCharacterI > firstCharacterJ)
            {
                string temp = classestud[i];
                classestud[i] = classestud[j];
                classestud[j] = temp;
                i = 0;
            }
            else
            {
                i++;
            }
        }
    }

    public class AuthenticationService
    {
        private List<Insegnante> users = new List<Insegnante>();

        public AuthenticationService(string filePath)
        {
            LoadUsers(filePath);
        }

        private void LoadUsers(string filePath)
        {
            if (File.Exists(filePath))
            {
                var lines = File.ReadAllLines(filePath);
                foreach (var line in lines)
                {
                    var parts = line.Split(';');
                    if (parts.Length == 2)
                    {
                        users.Add(new Insegnante
                        {
                            Username = parts[0],
                            Password = parts[1],
                        });
                    }
                }
            }
            else
            {
                MessageBox.Show("User file not found");
            }
        }
        public Insegnante Authenticate(string username, string password)
        {
            foreach (var user in users)
            {
                if (username == user.Username && user.Password == password)
                {
                    return user;
                }
            }
            return null;
        }
    }
    public class Insegnante
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
    public class Student
    {
        public int matricola { get; set; }
        public string nome { get; set; }
        public string cognome { get; set; }
        public DateTime data_nascita { get; set; }
        public string luogo_nascita { get; set; }
        public string classe { get; set; }
        public Dictionary<string, List<int>> voti { get; set; }
    }
}