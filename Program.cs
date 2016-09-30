using System;
using System.IO;
using System.Collections.Generic;
using System.Text;


namespace pyramydPath {
    class Program {
        static void Main(string[] args) {
            //int[][] testPath = new int[4][];
            //testPath[0] = new int[] { 1 };
            //testPath[1] = new int[] { 2, 3 };
            //testPath[2] = new int[] { 4, 5, 6 };
            //testPath[3] = new int[] { 10, 9, 8, 7 };
            //Pyramyd pyr = new Pyramyd(testPath);
            //Console.WriteLine(pyr.findMaxSum());
            //Console.ReadLine();
            List<int[]> testPathList = new List<int[]>();
            try {
                using (StreamReader sr = new StreamReader("pyramyd.txt")) {
                    String line;
                    while ((line = sr.ReadLine()) != null) {
                        string[] sNumbersInRow = line.Split(new []{" "},System.StringSplitOptions.RemoveEmptyEntries);
                        
                        int[] numbersInRow = new int[sNumbersInRow.Length];
                        Console.WriteLine(sNumbersInRow.Length);
                        Console.WriteLine(line);
                    }
                    
                }
            } catch (Exception e) {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }
            Console.ReadLine();
        }
    }
    class Pyramyd {
        private int[][] path;
        private int[][] sums;
        public Pyramyd(int[][] path) {
            this.path = new int[path.Length][];
            this.sums = new int[path.Length][];
            for (int i = 0; i < path.Length; i++) {
                this.path[i] = new int[i];
                if (path[i].Length > i + 1) {
                    string exceptionDesc = String.Format(
                        "Path should be not wider than pyramyd. (Row {0} is {1} elements wide, should be {2})",
                        i, path.Length, (i + 1));
                    throw new ArgumentException(exceptionDesc);
                }
                this.path[i] = new int[path[i].Length];
                this.sums[i] = new int[path[i].Length];
                Array.Copy(path[i], this.path[i], path[i].Length); //TODO: Add elements if it is smaller
            }
        }
        public int findMaxSum() {
            this.sums[0][0] = this.path[0][0];
            for (int i = 1; i < this.path.Length; i++) {
                for (int j = 0; j < this.path[i].Length; j++) {
                    int sumPathToElement = 0;
                    if (j > 0) {
                        sumPathToElement = this.sums[i - 1][j - 1];
                    }
                    if (j <= i - 1) {
                        sumPathToElement = (this.sums[i - 1][j] > sumPathToElement) ? this.sums[i - 1][j] : sumPathToElement;
                    }
                    if (j < i - 1) {
                        sumPathToElement = (this.sums[i - 1][j + 1] > sumPathToElement) ? this.sums[i - 1][j + 1] : sumPathToElement;
                    }
                    this.sums[i][j] = sumPathToElement + this.path[i][j];
                }
            }
            int sum = 0;
            for (int i = 0; i < this.sums.Length; i++) {
                sum = (sum > this.sums[this.sums.Length - 1][i]) ? sum : this.sums[this.sums.Length - 1][i];
            }
            return sum;
        }


    }
}
