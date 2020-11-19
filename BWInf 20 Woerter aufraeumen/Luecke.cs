using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BWInf_20_Woerter_aufraeumen {
    class Luecke {
        public char[] luecke;
        public int passt;
        public List<int> passtList;
        public int index;
        

        public Luecke(char[] luecke, List<int> passtList, int index) {
            this.luecke = luecke;
            this.passtList = passtList;
            this.index = index;
        }

        public static int compareListLength(Luecke l1, Luecke l2) {
            return l1.passtList.Count.CompareTo(l2.passtList.Count);
        }

        public static int compareLuecke(Luecke l1, Luecke l2) {
            int l1Num = 0;
            for (int i = 0; i < l1.luecke.Length; i++) {
                if (l1.luecke[i] != '_')
                    l1Num++;
            }
            int l2Num = 0;
            for (int i = 0; i < l2.luecke.Length; i++) {
                if (l2.luecke[i] != '_')
                    l2Num++;
            }

            return l1Num.CompareTo(l2Num);
        }
    }
}
