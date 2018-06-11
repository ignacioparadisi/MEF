using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

namespace MEF
{

    public partial class Form1 : Form
    {
        private MainMenu mainMenu1;
        private MenuItem menuItem1;
        private MenuItem mnuSalir;
        private MenuItem menuItem3;
        private MenuItem mnuInicio;
        private MenuItem mnuParo;
        private Timer timer1;

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
        }

        private void mnuParo_Click(object sender, System.EventArgs e)
        {
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

        private void Form1_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            // Creamos la fuente y la brocha para el texto 
            Font fuente = new Font("Arial", 16);
            SolidBrush brocha = new SolidBrush(Color.Black);

            // Dibujamos el robot 
            if (maquina.EstadoM == (int)CMaquina.estados.MUERTO)
                e.Graphics.DrawRectangle(Pens.Black, maquina.CoordX - 4, maquina.CoordY - 4, 20, 20);
            else
                e.Graphics.DrawRectangle(Pens.Green, maquina.CoordX - 4, maquina.CoordY - 4, 20, 20);

            // Dibujamos los objetos 
            for (int n = 0; n < 10; n++)
                if (ListaObjetos[n].activo == true)
                    e.Graphics.DrawRectangle(Pens.Indigo, ListaObjetos[n].x - 4, ListaObjetos[n].y - 4, 20, 20);

            // Dibujamos la bateria 
            e.Graphics.DrawRectangle(Pens.IndianRed, MiBateria.x - 4, MiBateria.y - 4, 20, 20);

            // Indicamos el estado en que se encuentra la maquina 
            e.Graphics.DrawString("Estado -> " + maquina.EstadoM.ToString(), fuente, brocha, 10, 10);

        }
    }
}
