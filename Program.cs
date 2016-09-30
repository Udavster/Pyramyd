using System;
using System.IO;
using System.Collections.Generic;
using System.Text;


namespace pyramydPath {
    class Program {
        static void Main(string[] args) {
            try {
                Console.WriteLine("Triangle from example:");
                var pyr = Pyramyd.ReadFromFile("pyramyd.txt");
                pyr.Print();
                Console.WriteLine("\nResult:{0}", pyr.findMaxSum());

                Console.WriteLine("\nFirst task:");
                pyr = Pyramyd.ReadFromFile("task.txt");
                pyr.Print(); 
                Console.WriteLine("\nResult:{0}", pyr.findMaxSum());

                Console.WriteLine("\nResult of additional task:");
                pyr = Pyramyd.ReadFromFile("additionalTask.txt");
                //pyr.Print(); //Too fat to be fully seen in console
                Console.WriteLine(pyr.findMaxSum());
            } catch (Exception ex) {
                Console.WriteLine("Unhandled exception. Stopped.");
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
        public static Pyramyd ReadFromFile(string filename, int verbose = 0) { //"pyramyd.txt"
            List<int[]> testPathList = new List<int[]>();
            try {
                using (StreamReader sr = new StreamReader(filename)) {
                    String line;
                    while ((line = sr.ReadLine()) != null) {
                        string[] sNumbersInRow = line.Split(new[] { " " }, System.StringSplitOptions.RemoveEmptyEntries);
                        int[] numbersInRow = new int[sNumbersInRow.Length];
                        for (int i = 0; i < sNumbersInRow.Length; i++) {
                            numbersInRow[i] = Int32.Parse(sNumbersInRow[i]);
                        }
                        testPathList.Add(numbersInRow);
                        if (verbose > 0) {
                            Console.WriteLine(String.Join(" ", numbersInRow));
                            Console.WriteLine(line);
                        }
                    }

                }
            } catch (IOException ex) {
                Console.WriteLine("The file ({0}) could not be read:\n{1}", filename, ex.Message);
                throw;
            } catch (FormatException) {
                Console.WriteLine("Format exception in {0}", filename);
                throw;
            } catch (OverflowException) {
                Console.WriteLine("Using too big numbers in {0} lead to overflow.", filename);
                throw;
            }
            int[][] path = testPathList.ToArray();
            return new Pyramyd(path);
        }
        public void Print() {
            if (this.path == null) {
                Console.WriteLine("Empty pyramyd");
                return;
            }
            for (int i = 0; i < path.Length; i++) {
                for (int j = 0; j < path[i].Length; j++) {
                    Console.Write("{0}\t", path[i][j]);
                }
                Console.WriteLine();
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
