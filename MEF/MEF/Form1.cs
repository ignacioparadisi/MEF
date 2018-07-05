using System;
using System.IO;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

namespace MEF
{

    public partial class Form1 : Form
    {
        private MainMenu mainMenu1 = new MainMenu();
        private MenuItem menuReset = new MenuItem();
        private MenuItem mnuSalir = new MenuItem();
        //private MenuItem menuItem3 = new MenuItem();
        private MenuItem mnuInicio = new MenuItem();
        private MenuItem mnuParo = new MenuItem();
        private Timer timer1 = new Timer();
        private Timer timer2 = new Timer();

        string pacmanImage = "pacman";
        int timeCounter = 0;

        // Creamos un objeto para la maquina de estados finitos 
        private CMaquina maquina = new CMaquina();

        private Fantasma[] fantasmas = new Fantasma[4];
        public S_objeto bateria2;

        // Objetos necesarios 
        public S_objeto[] ListaObjetos = new S_objeto[20];
        public S_objeto MiBateria;

        public Form1()
        {
            // 
            // Necesario para admitir el Diseñador de Windows Forms 
            // 
            InitializeComponent();
            this.DoubleBuffered = true;
            // 

            // Inicializamos los objetos 
            mnuInicio.Text = "Inicio";
            mnuInicio.Click += new System.EventHandler(this.mnuInicio_Click);
            mnuParo.Text = "Parar";
            mnuParo.Click += new System.EventHandler(this.mnuParo_Click);
            mnuSalir.Text = "Salir";
            mnuSalir.Click += new System.EventHandler(this.mnuSalir_Click);
            menuReset.Text = "Reset";
            menuReset.Click += new EventHandler(this.menuReset_Click);

            mainMenu1.MenuItems.Add(mnuInicio);
            mainMenu1.MenuItems.Add(mnuParo);
            mainMenu1.MenuItems.Add(mnuSalir);
            mainMenu1.MenuItems.Add(menuReset);

            timer1.Tick += new System.EventHandler(this.timer1_Tick);
            timer1.Interval = 10;

            timer2.Tick += new System.EventHandler(this.timer2_Tick);
            timer2.Interval = 50;

            Menu = mainMenu1;

            // Cremos un objeto para tener valores aleatorios 
            Random random = new Random();

            // Recorremos todos los objetos 
            for (int n = 0; n < ListaObjetos.Length; n++)
            {
                // Colocamos las coordenadas 
                ListaObjetos[n].x = random.Next(0, 639);
                ListaObjetos[n].y = random.Next(0, 479);

                // Lo indicamos activo 
                ListaObjetos[n].activo = true;
            }

            for (int n = 0; n < 4; n++)
            {
                fantasmas[n] = new Fantasma();

                //Random random2 = new Random();
                int nx = random.Next(0, 649);
                int ny = random.Next(0, 479);
                int energia = random.Next(600, 799);
                fantasmas[n].setCoordX(nx);        // Coordenada X 
                fantasmas[n].setCoordY(ny);        // Coordenada Y 
                fantasmas[n].SetEnergia(energia);
            }

                // Colocamos la bateria 
                MiBateria.x = random.Next(0, 639);
            MiBateria.y = random.Next(0, 479);
            MiBateria.activo = true;

            bateria2.x = random.Next(0, 639);
            bateria2.y = random.Next(0, 479);
            bateria2.activo = true;

            maquina.Inicializa(ref ListaObjetos, ref fantasmas, MiBateria);
            for (int i = 0; i < 4; i ++)
            {
                fantasmas[i].Inicializa(ref maquina, bateria2);
            }

            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Form1_Paint);
        }

        // static void Main()
        // {
        //     Application.Run(new Form1());
        // }

        private void mnuSalir_Click(object sender, System.EventArgs e)
        {
            // Cerramos la ventana y finalizamos la aplicacion 
            this.Close();
        }

        private void mnuInicio_Click(object sender, System.EventArgs e)
        {
            timer1.Enabled = true;
            timer2.Enabled = true;
            //timer1.Start();
        }

        private void mnuParo_Click(object sender, System.EventArgs e)
        {
            //timer1.Stop();
            timer1.Enabled = false;
            timer2.Enabled = false;
        }

        private void menuReset_Click(object sender, EventArgs e)
        {
            
        }

        private void timer1_Tick(object sender, System.EventArgs e)
        {
            // Esta funcion es el handler del timer 
            // Aqui tendremos la logica para actualizar nuestra maquina de estados 

            // Actualizamos a la maquina 

            if ((timeCounter % 9) == 0)
            {
                if (pacmanImage == "pacman")
                {
                    pacmanImage = "pacman2";
                }
                else
                {
                    pacmanImage = "pacman";
                }
            }

            timeCounter++;

            maquina.Control();

            // Mandamos a redibujar la pantalla 
            this.Invalidate();
        }

        private void timer2_Tick(object sender, System.EventArgs e)
        {
            // Esta funcion es el handler del timer 
            // Aqui tendremos la logica para actualizar nuestra maquina de estados 

            // Actualizamos a la maquina
            for (int i = 0; i < 4; i++)
            {
                fantasmas[i].Control();
            }

            // Mandamos a redibujar la pantalla 
            this.Invalidate();
        }

        private void Form1_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            // Creamos la fuente y la brocha para el texto 
            Font fuente = new Font("Arial", 11);
            SolidBrush brocha = new SolidBrush(Color.White);

            // Dibujamos el robot 
            if (maquina.EstadoM == (int)CMaquina.estados.MUERTO)
            {
                e.Graphics.DrawRectangle(Pens.Black, maquina.CoordX - 4, maquina.CoordY - 4, 20, 20);
            }
            else
            {
                Image image = Image.FromFile("../../images/"+ pacmanImage + ".png");
                e.Graphics.DrawImage(image, maquina.CoordX - 4, maquina.CoordY - 4, 20, 20);
            }

            // Dibujamos los objetos 
            for (int n = 0; n < ListaObjetos.Length; n++)
            {
                if (ListaObjetos[n].activo == true)
                {
                    //e.Graphics.DrawEllipse(Pens.YellowGreen, ListaObjetos[n].x, ListaObjetos[n].y, 10, 10);
                    Brush brush = new SolidBrush(Color.GreenYellow);
                    e.Graphics.FillEllipse(brush, ListaObjetos[n].x, ListaObjetos[n].y, 10, 10);
                    //e.Graphics.DrawRectangle(Pens.Indigo, ListaObjetos[n].x - 4, ListaObjetos[n].y - 4, 20, 20);
                }
            }

            string[] images = new string[] {"blinky", "inky", "sue", "pinky"};

            for (int n = 0; n < 4; n++)
            {
                if (fantasmas[n].EstadoM == (int)Fantasma.estados.MUERTO)
                {
                    e.Graphics.DrawRectangle(Pens.Black, fantasmas[n].CoordX - 4, fantasmas[n].CoordY - 4, 20, 20);
                }
                else
                {
                    Image ghostImage = Image.FromFile("../../images/" + images[n] + ".png");
                    e.Graphics.DrawImage(ghostImage, fantasmas[n].CoordX - 4, fantasmas[n].CoordY - 4, 20, 20);
                    //e.Graphics.DrawRectangle(Pens.Green, fantasmas[n].CoordX - 4, fantasmas[n].CoordY - 4, 20, 20);
                }
            }

            // Dibujamos la bateria 
            Image lifeImage = Image.FromFile("../../images/life.png");
            e.Graphics.DrawImage(lifeImage, MiBateria.x - 4, MiBateria.y - 4, 30, 30);
            //e.Graphics.DrawRectangle(Pens.Red, MiBateria.x - 4, MiBateria.y - 4, 20, 20);

            // Dibujamos la bateria 
            e.Graphics.DrawRectangle(Pens.Blue, bateria2.x - 4, bateria2.y - 4, 20, 20);

            // Indicamos el estado en que se encuentra la maquina 
            e.Graphics.DrawString("Estado Pacman -> " + maquina.EstadoM.ToString(), fuente, brocha, 10, 10);
            int estadoY = 25;
            for (int i = 0; i < 4; i++)
            {
                e.Graphics.DrawString("Estado Fantasma " + i + " -> " + fantasmas[i].EstadoM.ToString(), fuente, brocha, 10, estadoY);
                estadoY += 15;
            }

        }
    }
}
