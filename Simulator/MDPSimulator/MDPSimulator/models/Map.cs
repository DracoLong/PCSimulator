using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDPModel
{
    public class Map
    {
        public const int width = 15;
        public const int height = 20;
        private Cell[,] grid;
        private string map1;
        private string map2;
        public Cell[,] Grid
        {
            get { return this.grid; }
        }

        public Map()
        {
            this.grid = new Cell[height, width];
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    this.grid[i, j] = new Cell();
                }
            }
        }
        public Map(int[,] mapData)
        {
            grid = new Cell[height, width];
            for (int i = 0; i < mapData.GetLength(0); i++)
            {
                for (int j = 0; j < mapData.GetLength(1); j++)
                {
                    this.grid[i, j] = new Cell(mapData[i, j]);
                }
            }
        }
        public void print()
        {
            Console.WriteLine("=========Map==========");
            for (int i = 0; i < this.Grid.GetLength(0); i++)
            {
                for (int j = 0; j < this.Grid.GetLength(1); j++)
                {
                    Console.Write(this.Grid[this.Grid.GetLength(0) - i - 1, j].Status);
                    Console.Write(" ");
                }
                Console.WriteLine();
            }
        }

        public bool writeMapToText(string loc)
        {
            try
            {
                convertMap();
                string hex1 = convertToHex(this.map1);
                string hex2 = convertToHex(this.map2);
                File.WriteAllText(@loc + "map1.txt", hex1);
                File.WriteAllText(@loc + "map2.txt", hex2);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
            return true;
        }

        public bool writeMapToTextRealTime(string loc)
        {
            try
            {
                convertMap();
                string hex1 = convertToHex(this.map1);
                string hex2 = convertToHex(this.map2);
                File.WriteAllText(@loc + "realtime1.txt", hex1);
                File.WriteAllText(@loc + "realtime2.txt", hex2);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
            return true;
        }
        public void draw()
        {

        }
        public void convertMap()
        {
            this.map1 = "11";
            this.map2 = "";
            for (int i = 0; i < Map.height; i++)
            {
                for (int j = 0; j < Map.width; j++)
                {
                    if (Grid[i, j].Status != 0)
                    {
                        this.map1 += "1";
                        if (Grid[i, j].Status == 1)
                        {
                            this.map2 += "1";
                        }
                        else
                            this.map2 += "0";
                    }
                    else
                    {
                        this.map1 += "0";
                    }
                }
            }
            map1 += "11";
            while (this.map2.Length % 8 != 0)
            {
                this.map2 += "0";
            }
        }
        public string convertToHex(string s) 
        {
            string hex = "";
            string temp;
            for (int i = 0; i < s.Length; i+=4)
            {
                temp = s.Substring(i, 4);
                switch (temp)
                {
                    case "0000":
                        hex += "0";
                        break;
                    case "0001":
                        hex += "1";
                        break;
                    case "0010":
                        hex += "2";
                        break;
                    case "0011":
                        hex += "3";
                        break;
                    case "0100":
                        hex += "4";
                        break;
                    case "0101":
                        hex += "5";
                        break;
                    case "0110":
                        hex += "6";
                        break;
                    case "0111":
                        hex += "7";
                        break;
                    case "1000":
                        hex += "8";
                        break;
                    case "1001":
                        hex += "9";
                        break;
                    case "1010":
                        hex += "A";
                        break;
                    case "1011":
                        hex += "B";
                        break;
                    case "1100":
                        hex += "C";
                        break;
                    case "1101":
                        hex += "D";
                        break;
                    case "1110":
                        hex += "E";
                        break;
                    case "1111":
                        hex += "F";
                        break;
                    default:
                        break;
                }
            }
            return hex;
        }
    }
}
