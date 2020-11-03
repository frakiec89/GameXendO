using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Configuration;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    class Program
    {
        static void Main(string[] args)
        {
            int x = 3; int y = 3;
            try
            {
                Console.WriteLine("Введите ширину поля");
                x = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("Введите высоту поля");
                y = Convert.ToInt32(Console.ReadLine());
            }
            catch
            {
                Console.WriteLine("Неверный ввод");
                Console.WriteLine("пока");
                Console.Read();
                return;
            }

            while (true)
            {
               

                StartGame( x , y);
                Console.WriteLine("Продолжить  дальше? * для выхода ");

                if (Console.ReadLine()=="*")
                {
                    break;
                }
            }
            Console.WriteLine("пока");
            
            Console.Read();
        }

        /// <summary>
        /// Запуск  игры
        /// </summary>
        private static void StartGame(int x , int  y)
        {
            Console.WriteLine("Игра крестики  нолики \"СГК\" ");

            bool gamer = true;  // первые  ходят крестики

            string message = ""; // сообщение  о  победе или    нечье 

            int[,] mas = new int[x, y]; //  если  0 - пусто  если 1 - крестик  - если 2 нолик 

            PrintBord(mas); // отпечать  доску 
            while (true)
            {
                if ( IsWin(mas, out message ) ) // если победа
                {
                    Console.WriteLine("Игра окончена" + " " +  message);
                    break;
                }
               
                mas = NextTurn(mas, gamer); // совершили  ход 
                PrintBord(mas); // отпечать  доску занова 
                gamer = !gamer;
            }
        }

        /// <summary>
        /// Метод  меняет  массив   в  зависимости  от хода 
        /// </summary>
        /// <param name="mas">текущий  массив</param>
        /// <param name="gamer">текущий игрок</param>
        /// <returns></returns>
        private static int[,] NextTurn(int[,] mas, bool gamer)
        {
           if (gamer==true)
           {
                Console.WriteLine("Ходят Крестики");
           }

            if (gamer == false)
            {
                Console.WriteLine("Ходят Нолики");
            }

            Console.WriteLine("Укажите координату  хода (например 2 1  - это  второкая  строка  первый столбец)");
            string newTurn = Console.ReadLine();

            int i; int j; // наши координаты 

            if (IsTryTurn (newTurn , mas.GetLongLength(0) , mas.GetLongLength(1) , out i , out j ) ) // проверяем  есть  ли такая  кордината
            {
                if (IsTurnIsBusy(mas , i , j ) ) // если ячейка  свободна 
                {
                    if (gamer==true)
                    {
                        mas[i, j] = 1; // если  крестики
                    }

                    if (gamer == false)
                    {
                        mas[i, j] = 2; // если нолики 
                    }
                }
                else
                {
                    Console.WriteLine("коордитана занята - пофторите  ход");
                    NextTurn(mas, gamer);
                }

            }
            else
            {
                Console.WriteLine("коордитана неверная - введите  верные");
                NextTurn(mas, gamer);
            }

            return mas;
        }
        private static bool IsTurnIsBusy(int[,] mas, int i, int j)
        {
           if (mas[i,j]==0)
            {
                return true;

            }
            return false;
        }
        private static bool IsTryTurn(string newTurn, long v1, long v2, out int i, out int j)
        {
            try
            {
                i = Convert.ToInt32( newTurn[0].ToString()); // забираем коодинату  x
                j = Convert.ToInt32(newTurn[2].ToString()); // забираем коодинату y
                i--; j--;
            }
            catch
            {
                i = 0; j = 0;
                return false;
            }

            if ( i< v1 && j<v2) // проверяем  на  разоешенный  диапазон
            {
              
                return true;
            }
            
            return false;
        }

        /// <summary>
        /// отрисовывает  нашу доску 
        /// </summary>
        /// <param name="mas"></param>
        private static void PrintBord(int[,] mas)
        {
            Console.Clear(); // очистить  старый борд

            for ( int  i = 0; i <= mas.GetLength(1); i++) // печатает шапку 
            {
                if (i == 0)
                {
                    Console.Write("   "); continue;
                }

                Console.Write(" " +i +"|" ); 
            }

            Console.WriteLine();

            for ( int  i = 0; i < mas.GetLength(0); i++)  // это  печатает  сторки  в  игровом  поле 
            {
                for ( int j = 0; j < mas.GetLength(1); j++)
                {
                    if (j==0)
                    {
                        Console.Write(" " + (i+1) + "|");
                    }

                    if (mas[i,j] == 0)
                    {
                        Console.Write(" - ");
                    }

                    if (mas[i, j] == 1)
                    {
                        Console.Write(" X ");
                    }

                    if (mas[i, j] == 2)
                    {
                        Console.Write(" O ");
                    }
                }
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Определеят  победу 
        /// </summary>
        /// <param name="mas"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        private static bool IsWin(int[,] mas, out string message)
        {
            message = "";

            if(ColumWin(mas,out message))
            {
                return true;
            }

            if (RowWin(mas, out message))
            {
                return true;
            }

            if (mas.GetLength(0) == mas.GetLength(1))
            {
                if (DiagonalWin(mas, out message))
                {
                    return true;
                }

                if (DiagonalWinRevers(mas, out message))
                {
                    return true;
                }
            }

            if ( DrawGame(mas, out message))
            {
                return true;
            }
            return false;
        }

        private static bool DiagonalWin(int[,] mas, out string message)
        {
             message = "";
             int sum = 0;
             for (int i = 0; i < mas.GetLength(0); i++)
             {
                if (mas[i, i] != 0)
                {
                    sum += mas[i, i];
                }
                else
                {
                    return false;
                }
             }

            if ((double) sum / (double) mas.GetLength(0) == 1.0)
            {      
                message = "Крестики  победили по  диагонале";
                return true;
            }

            if ((double)sum / (double)mas.GetLength(0) == 2.0)
            {
                message = "Нолики победили по  диагонале";
                return true;
            }

            return false;
        }

        private static bool DiagonalWinRevers(int[,] mas, out string message)
        {
            message = "";
            int sum = 0;
            for (int i = 0; i < mas.GetLength(0); i++)
            {
                if (mas[mas.GetLength(0)-i-1, i] != 0)
                {
                    sum += mas[mas.GetLength(0) - i - 1, i];
                }
                else
                {
                    return false;
                }
            }

            if ((double)sum / (double)mas.GetLength(0) == 1.0)
            {
                message = "Крестики  победили по  диагонале";
                return true;
            }

            if ((double)sum / (double)mas.GetLength(0) == 2.0)
            {
                message = "Нолики победили по  диагонале";
                return true;
            }

            return false;
        }


        private static bool RowWin(int[,] mas, out string message)
        {
            message = "";
            for (int i = 0; i < mas.GetLength(0); i++)
            {
                int sum = 0;
                bool flag = true;
                for (int j = 0; j < mas.GetLength(1); j++)
                {
                    if (mas[i, j] != 0)
                    {
                        sum += mas[i, j];
                    }
                    else
                    {
                        flag = false;
                        break;
                    }
                }
                if (flag == false)
                {
                    continue;
                }

                if ((double)sum / mas.GetLength(1) == 1)
                {
                    message = "Крестики  победили";
                    return true;
                }
                if ((double)sum / mas.GetLength(1) == 2)
                {
                    message = " победили";
                    return true;
                }
            }
            return false;
        }

        private static bool ColumWin(int[,] mas, out string message)
        {
            message = "";
            for (int i = 0; i < mas.GetLength(1); i++)
            {
                int sum = 0;
                bool flag = true;

                for (int j = 0; j < mas.GetLength(0); j++)
                {
                    if (mas[j, i] != 0)
                    {
                        sum += mas[j, i];
                    }
                    else
                    {
                        flag = false;
                        break;
                    }
                }
                if (flag == false)
                {
                    continue;
                }

                if ((double)sum / mas.GetLength(0) == 1)
                {
                    message = "Крестики  победили";
                    return true;
                }
                if ((double)sum / mas.GetLength(0) == 2)
                {
                    message = "Нолики победили";
                    return true;
                }
            }

            return false;
        }

        private static bool DrawGame(int[,] mas, out string message)
        {
            message = "";

            for (int i = 0; i < mas.GetLength(0); i++)
            {
                for (int j = 0; j < mas.GetLength(1); j++)
                {
                    if (mas[i, j] == 0)
                    {
                        return false;
                    }
                }
            }

            message = "Ничья";
            return true;
        }
    }
}
