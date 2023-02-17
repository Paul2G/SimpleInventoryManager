using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;

namespace ProyectoFinal
{
    public class Program
    {
        //Lista en que se almacenan los articulos
        private static List<Item> items;

        //Variables multiusos
        private static string itemSel, name;
        private static char opt;
        private static int index, cant;
        
        public static void Main(string[] args)
        {
            //Intento de recuperar estado anterior del inventario
            try {
                items = DataFiles.toDesearialize();
            } catch
            {
                items = new List<Item>();
                Console.WriteLine("No se pudo leer los datos almacenados");
                Console.Write("Presione cualquier tecla para continuar...");
                Console.ReadKey();
            }

            //Impresion del menu principal e inicio del programa inventario
            mainMenu();

            //Intento de guardar el estado actual del inventario
            try{
                DataFiles.toSerialize(items);
            } catch
            {
                Console.WriteLine("\nNo se pudo guardar el inventario");
                Console.Write("Presione cualquier tecla para salir...");
                Console.ReadKey();
            }
        }
        
        //Funcion de impresion y funcionamiento de Menu Principal
        public static void mainMenu ()
        {   
            do{
                Console.Clear();
                Console.WriteLine("\tINVENTARIO");
                Console.WriteLine("Menu de opciones");
                Console.WriteLine("a) Informacion de articulos");
                Console.WriteLine("b) Agregar unidades");
                Console.WriteLine("c) Retirar unidades");
                Console.WriteLine("d) Añadir nuevo articulo");
                Console.WriteLine("e) Retirar articulo del inventario");
                Console.WriteLine("x) Salir");
                Console.Write("\nSeleccione: ");

                //Primera validacion de seleccion
                try{
                    opt = char.Parse(Console.ReadLine());
                }catch{ opt = '\0'; }

                //Revalidacion y solicitud de nueva seleccion
                while(!"abcdex".Contains(opt))
                {
                    Console.Clear();
                    //Reinpresion del menu principal
                    Console.WriteLine("\tINVENTARIO");
                    Console.WriteLine("Menu de opciones");
                    Console.WriteLine("a) Informacion de articulos");
                    Console.WriteLine("b) Agregar unidades");
                    Console.WriteLine("c) Retirar unidades");
                    Console.WriteLine("d) Añadir nuevo articulo");
                    Console.WriteLine("e) Retirar articulo del inventario");
                    Console.WriteLine("x) Salir");
                    Console.Write("\nOpcion invalida\nSeleccione de nuevo: ");

                    //Validacion de seleccion
                    try{
                        opt = char.Parse(Console.ReadLine());
                    }catch{ opt = '\0'; }
                }

                switch (opt)
                {
                    case 'a':
                        itemsMenu();
                        break;
                    case 'b':
                        addUnitsUI();
                        break;
                    case 'c':
                        remUnitsUI();
                        break;
                    case 'd':
                        addNewItemUI();
                        break;
                    case 'e':
                        delItemUI();
                        break;
                    case 'x':
                        //Saliendo
                        break;
                    default:
                        //Nunca se pasa por aqui xd
                        break;
                }

                //Repetir programa si no se seleccino salir
            }while(!"xX".Contains(opt));
        }//Termina mainMenu

        //Impresion y funcionamiento de menu de articulos
        public static void itemsMenu()
        {
            do{
                Console.Clear();
                //Impresion del menu de articulos
                Console.WriteLine("\tMOSTRADOR DE ARTICULOS");
                Console.WriteLine("Mostrar historial de articulo: ");

                //Impresion de inscisos con articulos
                printItems();

                Console.WriteLine("0) Regresar");
                Console.Write("\nSeleccione: ");

                //Validacion de seleccion
                try {
                    index = int.Parse(Console.ReadLine());
                } catch{ index = -1; }

                //Revalidacion y solicitud de reingreso de seleccion
                while (index < 0 || index > items.Count)
                {
                    Console.Clear();
                    //Impresion del menu de articulos
                    Console.WriteLine("\tMOSTRADOR DE ARTICULOS");
                    Console.WriteLine("Mostrar historial de articulo: ");

                    printItems();

                    Console.WriteLine("0) Regresar");
                    Console.Write("\nEntrada invalida \nSeleccione de nuevo: ");

                    //Validacion de seleccion
                    try {
                        index = int.Parse(Console.ReadLine());
                    } catch{ index = -1; }
                }

                //Si no se selecciono 0 (Salir), se imprime el historial
                if (index != 0)
                {
                    Console.Clear();
                    printHistory(items[index-1]);
                    Console.Write("\nPresione cualquier tecla para continuar...");
                    Console.ReadKey();
                }
                //Si se selecciono salir, pa tras
            }while(index != 0);
        }//Termina itemsMenu

        //Algoritmo de adicion de unidades
        private static void addUnitsUI()
        {
            //Impresion de titulo
            Console.Clear();
            Console.WriteLine("\tADICION DE UNIDADES");
            Console.Write("Ingrese el articulo: ");
            //Peticion de articulo con el cual trabajar
            itemSel = Console.ReadLine();

            //Obtencion de indice del articulo por medio de su nombre
            index = searchItemByName(itemSel);
            if(index != -1)
            {
                Console.WriteLine("Stock actual: {0}", items[index].Stock);
                Console.Write("\nCantidad a añadir: ");

                //Validacion de solo entradas numericas enteras
                try {
                    cant = int.Parse(Console.ReadLine());
                } catch{ cant = -1; }

                //Validacion de numeros positivos y peticion de reingreso de cantidad en caso de que no
                while (cant < 0)
                {
                    Console.Clear();
                    Console.WriteLine("\tADICION DE UNIDADES");
                    Console.WriteLine("Articulo seleccionado: {0}", items[index].Name);
                    Console.WriteLine("Stock actual: {0}", items[index].Stock);
                    Console.Write("\nSolo numeros positivos \nCantidad a añadir: ");

                    //Revalidacion de solo entradas de numeros enteros
                    try {
                        cant = int.Parse(Console.ReadLine());
                    } catch{ cant = -1; }
                }

                //Peticion del nombre de usuario al que se asignara el ingreso de unidades
                Console.Write("Nombre de usuario: ");
                name = Console.ReadLine();

                //Añadiendo la cantidad al stock del articulo seleccionado
                items[index].addStock(cant);
                //Añadiendo en el historial de entradas el cambio realizado y por quien
                items[index].History.Add(new InOutPut(name, DateTime.Now, cant));

                //Impresion de mensaje amigable con el usuario
                Console.WriteLine("\nSe han añadido {0} unidades al articulo {1}", cant, items[index].Name);
                Console.Write("Pulse cualquier tecla para continuar...");
                Console.ReadKey();
            } 
            else {
                //Pues lo que dice el mensaje
                Console.WriteLine("\nEl articulo no esta en el inventario");
                Console.Write("Presione cualquier tecla para continuar...");
                Console.ReadKey();
            }
        }

        //Algoritmo de retirada de unidades de articulos
        private static void remUnitsUI()
        {
            //Impreison de de titulo
            Console.Clear();
            Console.WriteLine("\tRETIRADA DE UNIDADES");
            //Peticion de articulo por nombre
            Console.Write("Ingrese el articulo: ");
            itemSel = Console.ReadLine();

            //Obtencion de indice del articulo en la lista
            index = searchItemByName(itemSel);
            if(index != -1)
            {
                Console.WriteLine("Stock actual: {0}", items[index].Stock);
                //Peticion de cantidad a retirar
                Console.Write("\nCantidad a retirar: ");

                //Validacion de solo numeros enteros
                try {
                    cant = int.Parse(Console.ReadLine());
                } catch{ cant = -1; }

                //Validacion de numeros positivos y menores al stock actual en el articulo a retirar
                while (cant < 0 || cant > items[index].Stock)
                {
                    //Reimprecion del encabezado por que si
                    Console.Clear();
                    Console.WriteLine("\tADICION DE UNIDADES");
                    Console.WriteLine("Articulo seleccionado: {0}", items[index].Name);
                    Console.WriteLine("Stock actual: {0}", items[index].Stock);
                    Console.Write("\nSolo numeros positivos \nLa cantidad debe ser menor al stock actual \nCantidad a retirar: ");

                    //Revalidacion de numeros enteros
                    try {
                        cant = int.Parse(Console.ReadLine());
                    } catch{ cant = -1; }
                }

                //Solicitud de nombre de usuario que realiza el cambio
                Console.Write("Nombre de usuario: ");
                name = Console.ReadLine();

                //Se retira la cantidad al stock del articulo seleccionado por medio de su indice
                items[index].addStock(-cant);
                //Se escribe en el historial el cambio realizado
                items[index].History.Add(new InOutPut(name, DateTime.Now, -cant));

                //Impresion de mensaje de cambios hechos al usuario
                Console.WriteLine("\nSe han retirado {0} unidades al articulo {1}", cant, items[index].Name);
                Console.Write("Pulse cualquier tecla para continuar...");
                Console.ReadKey();
            } 
            else {
                //Lo que se ve no se pregunta
                Console.WriteLine("\nEl articulo no esta en el inventario");
                Console.Write("Presione cualquier tecla para continuar...");
                Console.ReadKey();
            }
        }

        //Algoritmo de adicion de nuevos articulos al inventario
        private static void addNewItemUI()
        {
            //Creacion de un Item temporal
            Item tempItem;
            //Impresion del encabezado
            Console.Clear();
            Console.WriteLine("\tADICION DE NUEVO ARTICULO");
            //Peticion de nombre para el nuevo articulo
            Console.Write("Ingrese articulo nuevo: ");
            itemSel = Console.ReadLine();

            //Busqueda del nombre dentro de la lista
            index = searchItemByName(itemSel);
            //Si el articulo no existe en el inventario actual procede
            if(index == -1)
            {
                //Peticion de unidades para inicializar stock
                Console.WriteLine("Sin stock");
                Console.Write("\nCantidad a añadir: ");
                //Validacion de numeros enteros
                try {
                    cant = int.Parse(Console.ReadLine());
                } catch{ cant = -1; }

                //Validacion de numeros positivos
                while (cant < 0)
                {
                    //Reimpresion de encabezado
                    Console.Clear();
                    Console.WriteLine("\tADICION DE NUEVO ARTICULO");
                    Console.Write("Ingrese articulo nuevo: {0}", itemSel);
                    Console.WriteLine("Sin stock");
                    Console.Write("\nSolo numeros positivos \nCantidad a añadir: ");
                    //Validacion de numeros enteros
                    try {
                        cant = int.Parse(Console.ReadLine());
                    } catch{ cant = -1; }
                }

                //Peticion de usuario que realiza la adicion
                Console.Write("Nombre de usuario: ");
                name = Console.ReadLine();

                //Geneeracion del nuevo articulo
                tempItem = new Item(itemSel, cant);
                //Generacion entrada en el historial del nuevo articulo
                tempItem.History.Add(new InOutPut(name, DateTime.Now, cant));
                //Adicion del articulo a la lista de articulos del inventario
                items.Add(tempItem);

                //Impresion de mensaje de cambios realizados
                Console.WriteLine("\nSe ha añadido el articulo {0}", tempItem.Name);
                Console.WriteLine("Con el stock incial de {0}", tempItem.Stock);
                Console.Write("Pulse cualquier tecla para continuar...");
                Console.ReadKey();
            } 
            else {
                //Si no, pues no procede
                Console.WriteLine("\nEl articulo ya esta en el inventario");
                Console.Write("Presione cualquier tecla para continuar...");
                Console.ReadKey();
            }
        }

        //Algoritmo de eliminacion de articulos del inventario
        private static void delItemUI()
        {
            //Variable de confirmacion de eliminacion
            char confirm = '\0';

            //Impresion de encabezado
            Console.Clear();
            Console.WriteLine("\tELIMINAR ARTICULO DE INVENTARIO");
            Console.Write("Ingrese el articulo: ");
            //Peticion de nombre de articulo a eliminar
            itemSel = Console.ReadLine();

            //Busqueda del articulo en la lista de articulos del inventario
            index = searchItemByName(itemSel);
            //Si el articulo existe en el inventario procede a la eliminacion
            if(index != -1)
            {
                Console.WriteLine("Stock actual: {0}", items[index].Stock);
                Console.Write("\n\u00bfSeguro que desea eliminar este arcitulo?[y/*] ");

                //Solicidud de confirmacion de eliminacion
                // y validacion de caracter char
                try{
                    confirm = char.Parse(Console.ReadLine());
                }catch{ confirm = '\0'; }

                //Si la respuesta fue 'y' o 'Y' procede
                if("yY".Contains(confirm))
                {
                    Console.WriteLine("\nSe ha eliminado el articulo {0} del inventario", items[index].Name);
                    items.RemoveAt(index);
                } else {
                    //Si no, te despides
                    Console.WriteLine("\nOperacion cancelada");
                }
            } 
            else {
                //Si no existe pues no
                Console.WriteLine("\nEl articulo no esta en el inventario");
            }

            //Espera pulsacion en el teclado para continuar
            Console.Write("Pulse cualquier tecla para continuar...");
            Console.ReadKey();
        }

        //Algoritmo de impresion de los articulos del inventario en forma de lista
        private static void printItems()
        {
            //Inicializion de contador
            int i = 1;
            //Instruccion para recorrer todad la lista de articulos elemento por elemento
            foreach(Item item in items)
            {
                Console.WriteLine("{0}) {1, -20} Stock: {2}", i, item.Name, item.Stock);
                i++;
            }
        } //Termina printNames

        //Algoritmo de impresion de interfaz de historial
        private static void printHistory(Item item)
        {
            //Impresion de encabezado
            Console.WriteLine("\tHISTORIAL DE ENTRADAS SALIDAS");
            Console.WriteLine("Articulo: " + item.Name);
            Console.WriteLine("Stock: " + item.Stock);

            //Impresion de encabezado de tabla
            Console.WriteLine("{0, -23}{1, -16}{2, -14}", "        Usuario", "     Fecha", "    Cambio");

            //Ciclo que recorre la lista de entradas y salidas del final al principio elemento por elemento
            for(int i = item.History.Count - 1 ; i >= 0 ; i--)
            {
                Console.WriteLine("{0, -23}{1, -16}{2, 14}",
                    item.History[i].User,
                    item.History[i].Date.ToString("g"),
                    item.History[i].Change);
            }
        }

        //Algoritmo de busqueda de articulos en el inventario por medio del nombre
        //Retorna el indice en el que se encuentra, en caso de no existir retorna -1
        private static int searchItemByName(string name)
        {
            //Inicializacion de indice
            int index = -1;
            int i = 0;
            //Ciclo que recorre la lista de articulos elemento por elemento
            foreach (Item item in items)
            {
                //Comparacion del nombre del articulo actual con el nombre recibido
                //No respeta mayusculas o minusculas
                if (item.Name.ToLower() == name.ToLower())
                {
                    index = i;
                    break;
                }
                i++;
            }
            return index;
        }
    }//Termina program
}//Termina namespace