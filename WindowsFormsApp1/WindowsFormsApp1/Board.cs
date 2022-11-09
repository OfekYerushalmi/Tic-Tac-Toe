using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    public enum CellStatus
    {
        Empty,
        X,
        O
    }

    public class Board
    {
        public CellStatus[] cells;
        private int imaginarySteps;
        public Board()
        {
            cells = new CellStatus[16];
            imaginarySteps = 0;
        }
        public Board(Board other)
        {
            cells = new CellStatus[16];
            imaginarySteps = other.imaginarySteps + 1;
            for (int i=0; i<16; i++)
            {
                cells[i] = other.cells[i];
            }
        }

        // משנה מיקומים של איברים ברשימה
        private List<int> getShuffledArray(List<int> array)
        {
            Random r = new Random();
            for (int i = array.Count; i > 0; i--)
            {
                int j = r.Next(i);
                int k = array[j];
                array[j] = array[i - 1];
                array[i - 1] = k;
            }
            return array;
        }

        // מחזיר רשימה של מיקום האיברים הריקים במערך בסדר רנדומלי
        private List<int> getShuffledEmptyCellsIndex()
        {
            List<int> emptyCells = new List<int>();
            for (int i = 0; i < 16; i++)
            {
                if (this.cells[i]== CellStatus.Empty)
                {
                    emptyCells.Add(i);
                }
            }
            return getShuffledArray(emptyCells);
        }

        private List<int> getEmptyCellsIndex(int[] indexes)
        {
            List<int> emptyCells = new List<int>();
            foreach (int i in indexes)
            {
                if (this.cells[i] == CellStatus.Empty)
                {
                    emptyCells.Add(i);
                }
            }
            return emptyCells;
        }

        // בודק ניצחון כללי
        private CellStatus CheckWinner(int i1, int i2, int i3, int i4)
        {
            if (cells[i1] != CellStatus.Empty &&
                cells[i1] == cells[i2] &&
                cells[i2] == cells[i3] &&
                cells[i3] == cells[i4])
                return cells[i1];
            else
                return CellStatus.Empty;
        }

        // בודק  אפשרות לניצחון של שחקן נתון
        private void AddWinningPossibility(int i1, int i2, int i3, int i4, CellStatus player, int[] toIncrease)
        {
            CellStatus otherPlayer;
            if (player == CellStatus.O)
                otherPlayer = CellStatus.X;
            else
                otherPlayer = CellStatus.O;
            if (cells[i1] != otherPlayer &&
                cells[i2] != otherPlayer &&
                cells[i3] != otherPlayer &&
                cells[i3] != otherPlayer)
            {
                toIncrease[i1]--;
                toIncrease[i2]--;
                toIncrease[i3]--;
                toIncrease[i4]--;
            }
        }

        // בודק מספר אפשרויות לניצחון בכל איבר במערך
        public int[] CountWinningPossibilities(CellStatus player)
        {
            int[] counter = new int[16];
            for (int i = 0; i <= 12; i += 4)
            {
                AddWinningPossibility(i, i + 1, i + 2, i + 3, player, counter); //רצף אופקי
            }
            for (int i = 0; i <= 3; i++)
            {
                AddWinningPossibility(i, i + 4, i + 8, i + 12, player, counter); //רצף אנכי
            }
            AddWinningPossibility(0, 5, 10, 15, player, counter); //רצף אלכסון יורד
            AddWinningPossibility(3, 6, 9, 12, player, counter); //רצף אלכסון עולה
            return counter;
        }

        //מכינה מערך חדש מסודר לפי מיקומי האיברים במערך הראשי לפי מספר אפשויות ניצחון
        public int[] GetIndexesSortedByWinningPssibilities(CellStatus player)
        {
            int[] counter = CountWinningPossibilities(player);
            int[] indexes = new int[16];
            for (int i = 0; i < 16; i++)
                indexes[i] = i;
            Array.Sort(counter, indexes);
            return indexes;
        }

        // בודק נצחונות בכל אחת מהאפשרויות
        public CellStatus CheckWinner(out int[] winningIndexes)
        {
            winningIndexes = null;
            CellStatus winner;
            for (int i = 0; i <= 12; i += 4) 
            {
                winner = CheckWinner(i, i + 1, i + 2, i + 3); //רצף אופקי
                if (winner != CellStatus.Empty)
                {
                    winningIndexes = new int[] { i, i + 1, i + 2, i + 3 };
                    return winner;
                }
            }
            for (int i = 0; i <= 3; i++)
            {
                winner = CheckWinner(i, i + 4, i + 8, i + 12); //רצף אנכי
                if (winner != CellStatus.Empty)
                {
                    winningIndexes = new int[] { i, i + 4, i + 8, i + 12 };
                    return winner;
                }
            }

            winner = CheckWinner(0, 5, 10, 15); //רצף אלכסון יורד
            if (winner != CellStatus.Empty)
            {
                winningIndexes = new int[] { 0, 5, 10, 15 };
                return winner;
            }

            winner = CheckWinner(3, 6, 9, 12); //רצף אלכסון עולה
            if (winner != CellStatus.Empty)
            {
                winningIndexes = new int[] { 3, 6, 9, 12 };
                return winner;
            }

            return CellStatus.Empty;
        }

        //בודק אם יש תיקו
        public bool CheckTie()
        {
            for (int i = 0; i < 16; i++)
            {
                if (cells[i] == CellStatus.Empty)
                {
                    return false;
                }
            }
            return true;
        }

        //משחק צעד ומחזיר מי הוא חושב שעומד לנצח
        public CellStatus playOneTurn(CellStatus computer, CellStatus opponent)
        {
            bool played = playOneTurnIfWinner(computer, opponent);
            if (played) //במקרה שהמחשב עומד לנצח עכשיו
            {
                return computer;
            }
            else//במקרה שהמחשב לא עומד לנצח עכשיו
            {
                played = playOneTurnIfBlockingWinner(computer, opponent); //בודק אם השחקן הולך לנצח - אם כן חוסם אותו
                if (played) //בודק אם לשחקן יש אפרות נוספת לנצח מכיוון שונה, ואם כן מחזיר שהשחקן עומד לנצח
                {
                    Board imaginaryBoard = new Board(this);
                    if (playOneTurnIfWinner(opponent, computer))
                        return opponent;
                    else
                        return CellStatus.Empty;//ואם לא מחזיר שלא ברור מי עומד לנצח

                }
                else if (imaginarySteps<4)//כל עוד לא נכנסנו ליותר מדי רמות "דמיוניות" קדימה
                {
                    int[] orderedIndexes = GetIndexesSortedByWinningPssibilities(computer);                
                    foreach (int i in getEmptyCellsIndex(orderedIndexes))//עבור על כל האיברים הריקים שנותרו
                    {
                        Board imaginaryBoard = new Board(this);//לגבי כל מיקום ריק, התחל לוח דמיוני חדש
                        imaginaryBoard.cells[i] = computer;//הנח שהמיקום הזה בלוח הדמיוני החדש מסומן
                        CellStatus winner = imaginaryBoard.playOneTurn(opponent, computer);//מי עומד לנצח עכשיו ? יש כאן קריאת הפונקצייה לעצמה  - קריאה רקורסיבית
                        if (winner == computer)//אם המחשב עומד לנצח אז הפעל על הלוח הנוכחי את מה שסומן בלוח הדמיוני
                        {
                            cells[i] = computer;
                            return winner;//והחזר שהמחשב עומד לנצח
                        }
                    }
                    //אם עדיין לא החלטנו לסמן אף איבר
                    foreach (int i in getEmptyCellsIndex(orderedIndexes))//עבור על כל האיברים הריקים שנותרו
                    {
                        Board imaginaryBoard = new Board(this);//לגבי כל מיקום ריק, התחל לוח דמיוני חדש
                        imaginaryBoard.cells[i] = computer;//הנח שהמיקום הזה בלוח הדמיוני החדש מסומן
                        CellStatus winner = imaginaryBoard.playOneTurn(opponent, computer);//מי עומד לנצח עכשיו ? יש כאן קריאת הפונקצייה לעצמה  - קריאה רקורסיבית
                        if (winner != opponent)//אם השחקן לא עומד לנצח כתוצאה מהמהלך שלי, אז בצע אותו בלוח הנוכחי
                        {
                            cells[i] = computer;
                            return winner;
                        }
                    }
                }
            }
            //אם לא סימנת כלום - בצע סימון רנדומאלי
            playOneTurnDumb(computer, opponent);
            return CellStatus.Empty;
        }

        // תחסום את השחקן אם הוא הולך לנצח - מחזיר אמת אם חסם
        public bool playOneTurnIfBlockingWinner(CellStatus computer, CellStatus opponent)
        {
            foreach (int i in getShuffledEmptyCellsIndex())
            {
                Board imaginaryBoard = new Board(this);
                imaginaryBoard.cells[i] = opponent;
                int[] winningIndexes;
                if (imaginaryBoard.CheckWinner(out winningIndexes) == opponent)
                {
                    cells[i] = computer;
                    return true;
                }
            }
            return false;
        }

        //תשחק צעד אחד אם המחשב הולך לנצח - מחזיר אמת אם שיחק
        public bool playOneTurnIfWinner(CellStatus computer, CellStatus opponent)
        {
            foreach (int i in getShuffledEmptyCellsIndex())
            {
                Board imaginaryBoard = new Board(this);
                imaginaryBoard.cells[i] = computer;
                int[] winningIndexes;
                if (imaginaryBoard.CheckWinner(out winningIndexes) ==computer)
                {
                    cells[i] = computer;
                    return true;
                }
            }
            return false;
        }

        // משחק באופן רנדומלי
        public bool playOneTurnDumb(CellStatus computer, CellStatus opponent)
        {
            int[] orderedIndexes = GetIndexesSortedByWinningPssibilities(computer);
            foreach (int i in getEmptyCellsIndex(orderedIndexes))//עבור על כל האיברים הריקים שנותרו
            {
                cells[i] = computer;
                return true;
            }
            return false;
        }
    }
}
