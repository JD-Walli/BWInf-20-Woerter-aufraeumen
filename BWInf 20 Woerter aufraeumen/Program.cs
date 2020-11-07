using System;
using System.Collections.Generic;
using QA_Classification;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BWInf_20_Woerter_aufraeumen {
    class Program {
        static void Main(string[] args) {
            Console.WriteLine("\nLösung:");
            Console.WriteLine("\n" + findSolution(readFile(@"C:\Users\Jakov\Desktop\git\BWInf 20\BWInf 20 Woerter aufraeumen\raetsel3.txt")));
            Console.ReadKey();
        }

        public static string findSolution(Tuple<string[], List<Luecke>, string> input) {
            string[] woerter = input.Item1;
            List<Luecke> lueckenPaare = input.Item2;
            List<Luecke> lueckenPaareLeer = new List<Luecke>();
            //lueckenPaare.Sort(Luecke.compareLuecke);
            //fügt alle indices der wörter, die in lueckenpaare.key möglich sind, zu lueckenpaare.value hinzu
            for (int i = 0; i < lueckenPaare.Count; i++) {
                for (int j = 0; j < woerter.Length; j++) {
                    if (lueckenPaare[i].luecke.Length == woerter[j].Length) {
                        int equal = 0;//null=0  true=1 false=2
                        for (int k = 0; k < woerter[j].Length; k++) {
                            if (lueckenPaare[i].luecke[k] != '_') {
                                if (lueckenPaare[i].luecke[k] == woerter[j][k]) {
                                    equal = 1;
                                }
                                else {
                                    equal = 2;
                                    break;
                                }
                            }
                        }
                        if (equal == 1) {
                            //if (lueckenPaare[i].passtList.Count > 0) {
                            //jedes wort nur einmal, gleiche wörter missachten
                            //   bool check = true;
                            //    for(int l=0;l< lueckenPaare[i].passtList.Count; l++) {
                            //        if (woerter[j] == woerter[lueckenPaare[i].passtList[l]])
                            //            check = false;
                            //    }
                            //   if (check) {
                            //    if (woerter[lueckenPaare[i].passtList[lueckenPaare[i].passtList.Count - 1]] != woerter[j]) {
                            lueckenPaare[i].passtList.Add(j);
                            Console.WriteLine("+norm {0} beim Wort {1}", woerter[j], i);
                            //    }
                            //}
                            // else {
                            ////    lueckenPaare[i].passtList.Add(j);
                            //    Console.WriteLine("+norm {0} beim Wort {1}", woerter[j], i);
                            // }
                        }
                        else if (equal == 0) {
                            try {
                                lueckenPaareLeer[lueckenPaareLeer.FindIndex(ind => ind.index.Equals(lueckenPaare[i].index))].passtList.Add(j);
                                Console.WriteLine("+leer {0} beim Wort {1}", woerter[j], i);
                            }
                            catch {
                                lueckenPaareLeer.Add(new Luecke(lueckenPaare[i].luecke, new List<int> { j }, lueckenPaare[i].index));
                                Console.WriteLine("+leer {0} beim Wort {1}", woerter[j], i);
                            }
                        }
                    }
                }
            }

            //lueckenPaare.Sort(Luecke.compareListLength);
            //klar feststehende (nur eine möglichkeit) einfüllen
            string[] unbenutzteWoerter = new string[woerter.Length];
            woerter.CopyTo(unbenutzteWoerter, 0);
            for (int i = 0; i < lueckenPaare.Count; i++) {
                for (int j = 0; j < woerter.Length; j++) {
                    if (lueckenPaare[i].passtList.Contains(j) && lueckenPaare[i].passtList.Count == 1) {
                        lueckenPaare[i].passt = lueckenPaare[i].passtList[0];
                        unbenutzteWoerter[j] = "";
                    }
                }
            }

            //Optimierungsproblem
            //alle Luecken wo mehrere Wörter möglich sind werden in Matrix eingefüllt und am QA berechnet

            //matrixlength berechnen
            int matrixlength = 0;
            for (int i = 0; i < lueckenPaare.Count; i++) {
                if (lueckenPaare[i].passtList.Count > 1) {
                    for (int j = 0; j < lueckenPaare[i].passtList.Count; j++) {
                        if (unbenutzteWoerter[lueckenPaare[i].passtList[j]] != "") {
                            matrixlength++;
                        }
                    }
                }
            }

            if (matrixlength > 0) {
                float[,] matrix = new float[matrixlength, matrixlength];
                int x = 0, y = 0;
                for (int wort1 = 0; wort1 < lueckenPaare.Count; wort1++) {
                    if (lueckenPaare[wort1].passtList.Count > 1) {
                        for (int passt1ind = 0; passt1ind < lueckenPaare[wort1].passtList.Count; passt1ind++) {
                            if (unbenutzteWoerter[lueckenPaare[wort1].passtList[passt1ind]] != "") {
                                for (int wort2 = 0; wort2 < lueckenPaare.Count; wort2++) {
                                    if (lueckenPaare[wort2].passtList.Count > 1) {
                                        for (int passt2ind = 0; passt2ind < lueckenPaare[wort2].passtList.Count; passt2ind++) {
                                            if (unbenutzteWoerter[lueckenPaare[wort2].passtList[passt2ind]] != "") {
                                                int passt1 = lueckenPaare[wort1].passtList[passt1ind];
                                                int passt2 = lueckenPaare[wort2].passtList[passt2ind];

                                                if (wort1 == wort2) {
                                                    if (passt1 == passt2) { matrix[x, y] = -2; } //grundbelohnung
                                                    else { matrix[x, y] = 2; } //bestrafung: nur ein passt pro wort
                                                }

                                                if (passt1 == passt2 && wort1 != wort2) {
                                                    matrix[x, y] = 2; //bestrafung jedes passt nur einmal
                                                }

                                                y++;
                                            }
                                        }
                                    }
                                }
                                Console.WriteLine(x + " = ({0} , {1})", wort1, lueckenPaare[wort1].passtList[passt1ind]);
                                x++;
                                y = 0;
                            }
                        }
                    }
                }


                Matrix.printMatrix(matrix);
                qaConstellation constellation;
                try {
                    Dictionary<string, string> qaArguments = new Dictionary<string, string>() {
                {"annealing_time","30"},
                {"num_reads","1000"}, //max 10000 (limitation by dwave)
                {"chain_strength","1.7" }
                };
                    Dictionary<string, string> pyParams = new Dictionary<string, string>() {
                {"problem_type","qubo"}, //qubo //ising
                {"dwave_solver", "DW_2000Q_6"}, //DW_2000Q_6 //Advantage_system1.1
                {"dwave_inspector","false" }
                };
                    Task<qaConstellation> constellationTask = QA_Classification.Program.qaCommunication(matrix, qaArguments, pyParams);
                    constellation = constellationTask.Result;
                    constellation.printConstellation();
                    QA_Classification.Program.getUserInput(constellation, matrix);

                    int[] solution = constellation.results[constellation.getLowest(1)].Item4;
                    int c = 0;
                    for (int wort1 = 0; wort1 < lueckenPaare.Count; wort1++) {
                        if (lueckenPaare[wort1].passtList.Count > 1) {
                            for (int passt1ind = 0; passt1ind < lueckenPaare[wort1].passtList.Count; passt1ind++) {
                                if (unbenutzteWoerter[lueckenPaare[wort1].passtList[passt1ind]] != "") {
                                    if (solution[c] == 1) {
                                        lueckenPaare[wort1].passt = lueckenPaare[wort1].passtList[passt1ind];
                                    }
                                    c++;
                                }
                            }
                        }
                    }
                }
                catch (Exception e) {
                    Console.WriteLine("\nERROR occured:");
                    Console.WriteLine(e.Message);
                    Console.WriteLine(e.StackTrace);
                }
            }
            //leere auffüllen
            for (int i = 0; i < lueckenPaareLeer.Count; i++) {
                for (int k = 0; k < lueckenPaareLeer[i].passtList.Count; k++) {
                    if (unbenutzteWoerter[lueckenPaareLeer[i].passtList[k]] != "") {
                        lueckenPaare[lueckenPaare.FindIndex(ind => ind.index.Equals(lueckenPaareLeer[i].index))].passt = lueckenPaareLeer[i].passtList[k];
                        break;
                    }
                }
            }

            string[] originalText = input.Item3.Split(' ');
            for (int i = 0; i < lueckenPaare.Count; i++) {
                Luecke aktuelleLuecke = lueckenPaare[lueckenPaare.FindIndex(ind => ind.index.Equals(i))];
                string oldstring = originalText[i].Substring(0, woerter[aktuelleLuecke.passt].Length);
                //Console.WriteLine(string.Join(" ", originalText) + "   \"" + oldstring + "\" -> \"" + woerter[aktuelleLuecke.passt] + "\"");
                originalText[i] = originalText[i].Replace(oldstring, woerter[aktuelleLuecke.passt]);
            }
            return string.Join(" ", originalText);
        }

        public static Tuple<string[], List<Luecke>, string> readFile(string pfad = @"C:\Users\Jakov\Desktop\git\BWInf 20\BWInf 20 Woerter aufraeumen\raetsel1txt") {
            string[] lines = new string[2];
            string line = "";
            System.IO.StreamReader file = new System.IO.StreamReader(pfad);
            for (int i = 0; (line = file.ReadLine()) != null; i++) {
                lines[i] = line;
            }
            file.Close();

            string lines0out = lines[0];
            string[] woerter = lines[1].Split(' ');
            Console.WriteLine("Wörter: ");
            Console.WriteLine(string.Join(", ", woerter));
            Console.WriteLine("Aufgabe:");
            Console.WriteLine(lines0out);
            List<Luecke> lueckenPaare = new List<Luecke>();
            while (lines[0].Contains(",")) {
                lines[0] = lines[0].Remove(lines[0].IndexOf(","), 1);
            }
            while (lines[0].Contains(".")) {
                lines[0] = lines[0].Remove(lines[0].IndexOf("."), 1);
            }
            while (lines[0].Contains("!")) {
                lines[0] = lines[0].Remove(lines[0].IndexOf("!"), 1);
            }
            while (lines[0].Contains("?")) {
                lines[0] = lines[0].Remove(lines[0].IndexOf("?"), 1);
            }
            string[] luecken = lines[0].Split(' ');
            for (int i = 0; i < luecken.Length; i++) {
                lueckenPaare.Add(new Luecke(luecken[i].ToCharArray(), new List<int>(), i));
            }

            return new Tuple<string[], List<Luecke>, string>(woerter, lueckenPaare, lines0out);
        }
    }
}
