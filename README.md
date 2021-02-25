# BwInf-39.1.1-Woerter-aufraeumen

Lösung der Aufgabe "Wörter aufräumen" vom Bundeswettbewerb Informatik 2020.

## Aufgabe

Opa Jürgen blättert in einer Zeitschrift aus der
Apotheke und findet ein Rätsel. Es ist eine Liste
von Wörtern gegeben, die in die richtige Reihenfolge
gebracht werden sollen, so dass sie eine
lustige Geschichte ergeben. Leerzeichen und Satzzeichen
sowie einige Buchstaben sind schon vorgegeben.
„Oh je, was für eine Arbeit“, sagt Opa Jürgen.
„Kein Problem“ erwidert seine Enkelin Lotta.
„Der Computer kann es doch für uns machen.“

Hilf den beiden und schreibe ein Programm, das
einen Lückentext und eine Liste von Wörtern
einliest und anschließend den vervollständigten
Text ausgibt. Du kannst davon ausgehen, dass es
nur eine richtige Lösung gibt.
Wende dein Programm auf die Beispiele an, die
du auf den BWINF-Webseiten findest, und dokumentiere
die Ergebnisse.

## Lösungsansatz
Um zur Lösung zu kommen, bin ich in mehreren Schritten vorgegangen:
1.	Für jede Lücke berechnen, welche Wörter „hineinpassen“
2.	Alle Lücken mit nur einer Möglichkeit befüllen
3.	Verteilung der Wörter auf die Lücken mit mehreren passenden Wörtern als „QUBO“ Optimierungsproblem formulieren und mit einem adiabatischen Quanten-Annealer lösen
4.	Lücken ohne vorgegebenen Buchstaben mit übrigen Wörtern auffüllen

Ausführlicheres in Doku.docx
