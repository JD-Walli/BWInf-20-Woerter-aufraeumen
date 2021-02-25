using System;
using System.Collections.Generic;
using QA_Communication;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BwInf_39_1_1_Woerter_aufraeumen {
    class Program {
        static void Main(string[] args) {
            Console.WriteLine("\nLösung:");
            Console.WriteLine("\n" + findSolution(readFile(System.IO.Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName+@"\raetsel4.txt")));
            Console.ReadKey();
        }

        public static string findSolution(Tuple<string[], List<Luecke>, string> input) {
            string[] woerter = input.Item1;
            List<Luecke> luecken = input.Item2;
            List<Luecke> lueckenLeer = new List<Luecke>();
            //lueckenPaare.Sort(Luecke.compareLuecke);
            //fügt alle indices der wörter, die in lueckenpaare.key möglich sind, zu lueckenpaare.value hinzu
            for (int i = 0; i < luecken.Count; i++) {
                for (int j = 0; j < woerter.Length; j++) {
                    if (luecken[i].luecke.Length == woerter[j].Length) {
                        int equal = 0;//null=0  true=1 false=2
                        for (int k = 0; k < woerter[j].Length; k++) {
                            if (luecken[i].luecke[k] != '_') {

                                if (luecken[i].luecke[k] == woerter[j][k]) {

                                    equal = 1;
                                }
                                else {
                                    equal = 2;
                                    break;
                                }
                            }
                        }

                        if (equal == 1 || equal == 0) {
                            luecken[i].passtList.Add(j);
                            Console.WriteLine("+norm {0} bei Lücke {1}", woerter[j], i);
                        }
                        else if (equal == 0) {
                            try {
                                lueckenLeer[lueckenLeer.FindIndex(ind => ind.index.Equals(luecken[i].index))].passtList.Add(j);
                                Console.WriteLine("+leer {0} bei Lücke {1}", woerter[j], i);
                            }
                            catch {
                                lueckenLeer.Add(new Luecke(luecken[i].luecke, new List<int> { j }, luecken[i].index));
                                Console.WriteLine("+leer {0} bei Lücke {1}", woerter[j], i);
                            }
                        }
                    }
                }
            }

            //klar feststehende (nur eine möglichkeit) einfüllen
            string[] unbenutzteWoerter = new string[woerter.Length];
            woerter.CopyTo(unbenutzteWoerter, 0);
            //feststellen wie groß die größte passtList ist
            int meistePasst = 0;
            for (int i = 0; i < luecken.Count; i++) {
                if (luecken[i].passtList.Count > meistePasst) {
                    meistePasst = luecken[i].passtList.Count;
                }
            }

            for (int j = 0; j < meistePasst; j++) {
                //alle Lücken mit nur einem möglichen Wort befüllen
                for (int i = 0; i < luecken.Count; i++) {
                    if (luecken[i].passtList.Count == 1 && unbenutzteWoerter[luecken[i].passtList[0]] != "") {
                        luecken[i].passt = luecken[i].passtList[0];
                        unbenutzteWoerter[luecken[i].passt] = "";
                    }
                }
                //neu gesetzte Wörter aus allen passtList entfernen 
                for (int l = 0; l < luecken.Count; l++) {
                    for (int w = 0; w < luecken[l].passtList.Count; w++) {
                        if (unbenutzteWoerter[luecken[l].passtList[w]] == "") {
                            luecken[l].passtList.RemoveAt(w);
                            break;
                        }
                    }
                }
            }

            //Optimierungsproblem
            //alle Luecken wo mehrere Wörter möglich sind werden in Matrix eingefüllt und am QA berechnet

            //matrixlength berechnen
            int matrixlength = 0;
            for (int i = 0; i < luecken.Count; i++) {
                if (luecken[i].passtList.Count > 1) {
                    for (int j = 0; j < luecken[i].passtList.Count; j++) {
                        if (unbenutzteWoerter[luecken[i].passtList[j]] != "") {
                            matrixlength++;
                        }
                    }
                }
            }

            //matrix befüllen
            if (matrixlength > 0) {
                float[,] matrix = new float[matrixlength, matrixlength];
                int x = 0, y = 0;
                for (int luecke1 = 0; luecke1 < luecken.Count; luecke1++) {
                    if (luecken[luecke1].passtList.Count > 1) {
                        for (int wort1ind = 0; wort1ind < luecken[luecke1].passtList.Count; wort1ind++) {
                            if (unbenutzteWoerter[luecken[luecke1].passtList[wort1ind]] != "") {
                                for (int luecke2 = 0; luecke2 < luecken.Count; luecke2++) {
                                    if (luecken[luecke2].passtList.Count > 1) {
                                        for (int wort2ind = 0; wort2ind < luecken[luecke2].passtList.Count; wort2ind++) {
                                            if (unbenutzteWoerter[luecken[luecke2].passtList[wort2ind]] != "") {
                                                int wort1 = luecken[luecke1].passtList[wort1ind];
                                                int wort2 = luecken[luecke2].passtList[wort2ind];

                                                if (luecke1 == luecke2) {
                                                    if (wort1 == wort2) { matrix[x, y] = -2; } //grundbelohnung
                                                    else { matrix[x, y] = 2; } //bestrafung: nur ein "passt" pro wort
                                                }

                                                if (wort1 == wort2 && luecke1 != luecke2) {
                                                    matrix[x, y] = 2; //bestrafung jedes passt nur einmal
                                                }

                                                y++;
                                            }
                                        }
                                    }
                                }
                                x++;
                                y = 0;
                            }
                        }
                    }
                }


                Matrix.printMatrix(matrix);
                //infos an Quantencomputer senden
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
                    Task<qaConstellation> constellationTask = QA_Communication.Program.qaCommunication(matrix, qaArguments, pyParams);
                    constellation = constellationTask.Result;
                    constellation.printConstellation();
                    QA_Communication.Program.getUserInput(constellation, matrix);

                    int[] solution = constellation.results[constellation.getLowest(1,new List<int>())[0]].Item4;
                    int c = 0;
                    for (int lücke = 0; lücke < luecken.Count; lücke++) {
                        if (luecken[lücke].passtList.Count > 1) {
                            for (int wort = 0; wort < luecken[lücke].passtList.Count; wort++) {
                                if (unbenutzteWoerter[luecken[lücke].passtList[wort]] != "") {
                                    if (solution[c] == 1) {
                                        luecken[lücke].passt = luecken[lücke].passtList[wort];
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

            string[] originalText = input.Item3.Split(' ');
            for (int i = 0; i < luecken.Count; i++) {
                Luecke aktuelleLuecke = luecken[luecken.FindIndex(ind => ind.index.Equals(i))];
                string oldstring = originalText[i].Substring(0, woerter[aktuelleLuecke.passt].Length);
                //Console.WriteLine(string.Join(" ", originalText) + "   \"" + oldstring + "\" -> \"" + woerter[aktuelleLuecke.passt] + "\"");
                originalText[i] = originalText[i].Replace(oldstring, woerter[aktuelleLuecke.passt]);
            }
            return string.Join(" ", originalText);
        }

        public static Tuple<string[], List<Luecke>, string> readFile(string pfad = @"C:\Users\Jakov\Desktop\git\BWInf 20\BwInf 39.1.1 Woerter aufraeumen\raetsel4.txt") {
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
