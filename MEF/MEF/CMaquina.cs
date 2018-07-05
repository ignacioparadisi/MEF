using System;

namespace MEF
{
    // Estructura usada para los objetos y la bateria 
    public struct S_objeto
    {
        public bool activo;    // Indica si el objeto es visible o no 
        public int x, y;        // Coordenadas del objeto 
    }


    /// <summary> 
    /// Esta clase representa a nuestra maquina de estados finitos. 
    /// </summary> 
    public class CMaquina
    {
        // Enumeracion de los diferentes estados 
        public enum estados
        {
            BUSQUEDA,
            NBUSQUEDA,
            IRBATERIA,
            RECARGAR,
            MUERTO,
            ALEATORIO,

        };

        // Esta variable representa el estado actual de la maquina 
        private int Estado;

        // Estas variables son las coordenadas del robot 
        private int x, y, lastx, lasty;

        // Arreglo para guardar una copia de los objetos 
        private S_objeto[] objetos = new S_objeto[10];
        private S_objeto bateria;

        private Fantasma[] fantasmas = new Fantasma[4];

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

        public void setEstado(int estado)
        {
            Estado = estado;
        }

        public CMaquina()
        {
            // Este es el contructor de la clase 

            // Inicializamos las variables 

            Estado = (int)estados.NBUSQUEDA;    // Colocamos el estado de inicio. 
            x = 320;        // Coordenada X 
            y = 240;        // Coordenada Y 
            lastx = x;
            lasty = y;
            indice = -1;    // Empezamos como si no hubiera objeto a buscar 
            energia = 800;
        }

        public void Inicializa(ref S_objeto[] Pobjetos, ref Fantasma[] fantasmas, S_objeto Pbateria)
        {
            // Colocamos una copia de los objetos y la bateria 
            // para poder trabajar internamente con la informacion 

            objetos = Pobjetos;
            bateria = Pbateria;
            this.fantasmas = fantasmas;

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
                    if (x == objetos[indice].x && y == objetos[indice].y)
                    {
                        // Desactivamos el objeto encontrado 
                        objetos[indice].activo = false;

                        // Cambiamos de estado 
                        Estado = (int)estados.NBUSQUEDA;

                    }
                    else if (energia < 400) // Checamos condicion de transicion 
                        Estado = (int)estados.IRBATERIA;

                    break;

                case (int)estados.NBUSQUEDA:
                    // Llevamos a cabo la accion del estado 
                    NuevaBusqueda();

                    // Verificamos por transicion 
                    if (indice == -1)    // Si ya no hay objetos, entonces aleatorio 
                        Estado = (int)estados.ALEATORIO;
                    else
                        Estado = (int)estados.BUSQUEDA;

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
            bool moveX = false;
            bool moveY = false;
            int fantasmaXIndex = 0;
            int fantasmaYIndex = 0;
            for (int i = 0; i < fantasmas.Length; i++)
            {
                if ((fantasmas[i].CoordX == x) && ((fantasmas[fantasmaYIndex].CoordY - y == 1) || (fantasmas[fantasmaYIndex].CoordY - y == -1)))
                {
                    moveX = true;
                    fantasmaXIndex = i;
                }

                if ((fantasmas[i].CoordY == y) && ((fantasmas[fantasmaXIndex].CoordX - x == 1) || (fantasmas[fantasmaXIndex].CoordX - x == -1)))
                {
                    moveY = true;
                    fantasmaYIndex = i;
                }
            }

            if (moveX)
            {
                if (lasty > y)
                {
                    y--;
                } else
                {
                    y++;
                }
            } else if (moveY) {
                if (lastx > x)
                {
                    x--;
                }
                else
                {
                    x++;
                }
            } else
            {
                if (x < objetos[indice].x)
                {
                    if (lastx != x)
                    {
                        lastx = x;
                    }
                    x++;
                }
                else if (x > objetos[indice].x)
                {
                    if (lastx != x)
                    {
                        lastx = x;
                    }
                    x--;
                }

                if (y < objetos[indice].y)
                {
                    if (lasty != y)
                    {
                        lasty = y;
                    }
                    y++;
                }
                else if (y > objetos[indice].y)
                {
                    if (lasty != y)
                    {
                        lasty = y;
                    }
                    y--;
                }
            }

            // Nos dirigimos hacia el objeto actual 
            

            // Disminuimos la energia 
            moveX = false;
            moveY = false;
            energia--;

        }

        public void NuevaBusqueda()
        {
            // En esta funcion colocamos la logica del estado Nueva Busqueda 
            // Verificamos que haya otro objeto a buscar 
            indice = -1;

            // Recorremos el arreglo buscando algun objeto activo 
            for (int n = 0; n < objetos.Length; n++)
            {
                if (objetos[n].activo == true)
                    indice = n;
            }
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
            bool moveX = false;
            bool moveY = false;
            int fantasmaXIndex = 0;
            int fantasmaYIndex = 0;
            for (int i = 0; i < fantasmas.Length; i++)
            {
                if ((fantasmas[i].CoordX == x) && ((fantasmas[fantasmaYIndex].CoordY - y == 1) || (fantasmas[fantasmaYIndex].CoordY - y == -1)))
                {
                    moveX = true;
                    fantasmaXIndex = i;
                }

                if ((fantasmas[i].CoordY == y) && ((fantasmas[fantasmaXIndex].CoordX - x == 1) || (fantasmas[fantasmaXIndex].CoordX - x == -1)))
                {
                    moveY = true;
                    fantasmaYIndex = i;
                }
            }

            if (moveX)
            {
                if (lasty > y)
                {
                    y--;
                }
                else
                {
                    y++;
                }
            }
            else if (moveY)
            {
                if (lastx > x)
                {
                    x--;
                }
                else
                {
                    x++;
                }
            }
            else
            {
                if (x < bateria.x)
                    x++;
                else if (x > bateria.x)
                    x--;

                if (y < bateria.y)
                    y++;
                else if (y > bateria.y)
                    y--;
            }

            moveX = false;
            moveY = false;
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
