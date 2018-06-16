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
        //private MenuItem menuItem1 = new MenuItem();
        private MenuItem mnuSalir = new MenuItem();
        //private MenuItem menuItem3 = new MenuItem();
        private MenuItem mnuInicio = new MenuItem();
        private MenuItem mnuParo = new MenuItem();
        private Timer timer1 = new Timer();

        // Creamos un objeto para la maquina de estados finitos 
        private CMaquina maquina = new CMaquina();

        // Objetos necesarios 
        public S_objeto[] ListaObjetos = new S_objeto[10];
        public S_objeto MiBateria;

        public Form1()
        {
            // 
            // Necesario para admitir el Diseñador de Windows Forms 
            // 
            InitializeComponent();

            // 

            // Inicializamos los objetos 
            mnuInicio.Text = "Inicio";
            mnuInicio.Click += new System.EventHandler(this.mnuInicio_Click);
            mnuParo.Text = "Paro";
            mnuParo.Click += new System.EventHandler(this.mnuParo_Click);
            mnuSalir.Text = "Salir";
            mnuSalir.Click += new System.EventHandler(this.mnuSalir_Click);

            mainMenu1.MenuItems.Add(mnuInicio);
            mainMenu1.MenuItems.Add(mnuParo);
            mainMenu1.MenuItems.Add(mnuSalir);

            timer1.Tick += new System.EventHandler(this.timer1_Tick);
            timer1.Interval = 10;

            Menu = mainMenu1;

            // Cremos un objeto para tener valores aleatorios 
            Random random = new Random();

            // Recorremos todos los objetos 
            for (int n = 0; n < 10; n++)
            {
                // Colocamos las coordenadas 
                ListaObjetos[n].x = random.Next(0, 639);
                ListaObjetos[n].y = random.Next(0, 479);

                // Lo indicamos activo 
                ListaObjetos[n].activo = true;
            }

            // Colocamos la bateria 
            MiBateria.x = random.Next(0, 639);
            MiBateria.y = random.Next(0, 479);
            MiBateria.activo = true;

            maquina.Inicializa(ref ListaObjetos, MiBateria);

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
            //timer1.Start();
        }

        private void mnuParo_Click(object sender, System.EventArgs e)
        {
            //timer1.Stop();
            timer1.Enabled = false;
        }

        private void timer1_Tick(object sender, System.EventArgs e)
        {
            // Esta funcion es el handler del timer 
            // Aqui tendremos la logica para actualizar nuestra maquina de estados 

            // Actualizamos a la maquina 
            maquina.Control();

            // Mandamos a redibujar la pantalla 
            this.Invalidate();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            PictureBox pb1 = new PictureBox();
            pb1.ImageLocation = "./MEF/images/pacman.png";
            pb1.SizeMode = PictureBoxSizeMode.AutoSize;
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
                Image image = Image.FromFile("../../images/pacman.png");
                e.Graphics.DrawImage(image, maquina.CoordX - 4, maquina.CoordY - 4, 20, 20);
            }

            // Dibujamos los objetos 
            for (int n = 0; n < 10; n++)
            {
                if (ListaObjetos[n].activo == true)
                {
                    e.Graphics.DrawEllipse(Pens.Purple, ListaObjetos[n].x, ListaObjetos[n].y, 10, 10);
                    //e.Graphics.DrawRectangle(Pens.Indigo, ListaObjetos[n].x - 4, ListaObjetos[n].y - 4, 20, 20);
                }
            }

            // Dibujamos la bateria 
            e.Graphics.DrawRectangle(Pens.Red, MiBateria.x - 4, MiBateria.y - 4, 20, 20);

            // Indicamos el estado en que se encuentra la maquina 
            e.Graphics.DrawString("Estado -> " + maquina.EstadoM.ToString(), fuente, brocha, 10, 10);

        }
    }
}
