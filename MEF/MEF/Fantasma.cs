using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MEF
{
    class Fantasma
    {
        // Enumeracion de los diferentes estados 
        public enum estados
        {
            BUSQUEDA,
            IRBATERIA,
            RECARGAR,
            MUERTO,
            ALEATORIO,

        };

        // Esta variable representa el estado actual de la maquina 
        private int Estado;

        // Estas variables son las coordenadas del robot 
        private int x, y;

        // Arreglo para guardar una copia de los objetos 
        private CMaquina pacman = new CMaquina();
        private S_objeto bateria;

        // Variable del indice del objeto que buscamos 
        private int indice;

        // Variable para la energia; 
        private int energia;

        // Creamos las propiedades necesarias 
        public int CoordX
        {
            get { return x; }
        }

        public int CoordY
        {
            get { return y; }
        }

        public int EstadoM
        {
            get { return Estado; }
        }

        public Fantasma()
        {
            // Este es el contructor de la clase 

            // Inicializamos las variables 

            Estado = (int)estados.BUSQUEDA;    // Colocamos el estado de inicio. 
            Random random = new Random();

            int nx = random.Next(0, 649);
            int ny = random.Next(0, 479);
            x = nx;        // Coordenada X 
            y = ny;        // Coordenada Y 
            indice = 0;    // Empezamos como si no hubiera objeto a buscar 
            energia = 800;
        }

        public void Inicializa(ref CMaquina Pmaquina, S_objeto Pbateria)
        {
            // Colocamos una copia de los objetos y la bateria 
            // para poder trabajar internamente con la informacion 

            pacman = Pmaquina;
            bateria = Pbateria;

        }

        public void Control()
        {
            // Esta funcion controla la logica principal de la maquina de estados 

            switch (Estado)
            {
                case (int)estados.BUSQUEDA:
                    // Llevamos a cabo la accion del estado 
                    Busqueda();

                    // Verificamos por transicion 
                    if (x == pacman.CoordX && y == pacman.CoordY)
                    {
                        // Desactivamos el objeto encontrado 
                        pacman.Estado = (int)CMaquina.estados.MUERTO;

                        // Cambiamos de estado 
                        Estado = (int)estados.ALEATORIO;

                    }
                    else if (energia < 400) // Checamos condicion de transicion 
                        Estado = (int)estados.IRBATERIA;

                    break;

                case (int)estados.IRBATERIA:
                    // Llevamos a cabo la accion del estado 
                    IrBateria();

                    // Verificamos por transicion 
                    if (x == bateria.x && y == bateria.y)
                        Estado = (int)estados.RECARGAR;

                    if (energia == 0)
                        Estado = (int)estados.MUERTO;

                    break;

                case (int)estados.RECARGAR:
                    // Llevamos a cabo la accion del estado 
                    Recargar();

                    // Hacemos la transicion 
                    Estado = (int)estados.BUSQUEDA;

                    break;

                case (int)estados.MUERTO:
                    // Llevamos a cabo la accion del estado 
                    Muerto();

                    // No hay condicion de transicion 

                    break;

                case (int)estados.ALEATORIO:
                    // Llevamos a cabo la accion del estado 
                    Aleatorio();

                    // Verificamos por transicion 
                    if (energia == 0)
                        Estado = (int)estados.MUERTO;

                    break;
            }
        }

        public void Busqueda()
        {
            // En esta funcion colocamos la logica del estado Busqueda 

            // Nos dirigimos hacia el objeto actual 
            if (x < pacman.CoordX)
                x++;
            else if (x > pacman.CoordX)
                x--;

            if (y < pacman.CoordY)
                y++;
            else if (y > pacman.CoordY) 
                y--;

            // Disminuimos la energia 
            energia--;

        }

        public void Aleatorio()
        {
            // En esta funcion colocamos la logica del estado Aleatorio 
            // Se mueve al azar 

            // Cremos un objeto para tener valores aleatorios 
            Random random = new Random();

            int nx = random.Next(0, 3);
            int ny = random.Next(0, 3);

            // Modificamos la posicion al azar 
            x += nx - 1;
            y += ny - 1;

            energia--;

        }

        public void IrBateria()
        {
            // En esta funcion colocamos la logica del estado Ir Bateria 

            // Nos dirigimos hacia la bateria 
            if (x < bateria.x)
                x++;
            else if (x > bateria.x)
                x--;

            if (y < bateria.y)
                y++;
            else if (y > bateria.y)
                y--;

            // Disminuimos la energia 
            energia--;

        }

        public void Recargar()
        {
            // En esta funcion colocamos la logica del estado Recargar 
            energia = 1000;

        }

        public void Muerto()
        {
            // En esta funcion colocamos la logica del estado Muerto 

            // Sonamos un beep de la computadora 
        }
    }
}
