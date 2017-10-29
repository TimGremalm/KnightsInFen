using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnightsInFen {
	public class BoardMove {
		BoardMove Parent;
		public List<BoardMove> AvailableMoves = new List<BoardMove>();
		public List<BoardMove> ValidMoves = new List<BoardMove>();
		public List<List<char>> Board = new List<List<char>>();
		public int Generation;

		public BoardMove (string Layout) {
			Generation = 0;
			ReadLayout(Layout);
		}
		public BoardMove (int Gen, string Layout) {
			Generation = Gen;
			ReadLayout(Layout);
		}
		private void ReadLayout(string Layout) {
			var lines = Layout.Split(new string[] { "\n" }, StringSplitOptions.None);
			foreach (string line in lines) {
				List<char> CharList = new List<char>();
				foreach(char character in line) {
					CharList.Add(character);
				}
				Board.Add(CharList);
			}
		}
		private string TextRepresentationOfBoard(List<List<char>> BoardRepresent) {
			return string.Join("\n", BoardRepresent.Select(r => String.Join("", r.Select(c => c.ToString()))));
		}
		public override string ToString() {
			return TextRepresentationOfBoard(Board);
		}
		private void MakeAMoveAndStore(int pieceToMoveRow, int pieceToMoveCol, int moveToRow, int moveToCol) {
			char typeOfCharacterToMove = Board[pieceToMoveRow][pieceToMoveCol];
			BoardMove newMove = new BoardMove(Generation+1, TextRepresentationOfBoard(Board));
			newMove.Board[pieceToMoveRow][pieceToMoveCol] = " ".ToCharArray()[0];
			newMove.Board[moveToRow][moveToCol] = typeOfCharacterToMove;
			AvailableMoves.Add(newMove);
		}
		public void CalculateAvailableBoardMoves() {
			List<Tuple<int, int>> legalMoves = new List < Tuple < int, int>>{	new Tuple<int,int>(-2, -1),
																				new Tuple<int,int>(-2, 1),
																				new Tuple<int,int>(-1, -2),
																				new Tuple<int,int>(-1, 2),
																				new Tuple<int,int>(1, -2),
																				new Tuple<int,int>(1, 2),
																				new Tuple<int,int>(2, -1),
																				new Tuple<int,int>(2, 1),
			};

			//Walk through every board-position
			for (int i=0; i<Board.Count; i++) {
				List<char> row = Board[i];
				for (int j = 0; j < row.Count; j++) {
					char col = row[j];
					if (!col.Equals(" ")) {
						//Check which positions the knight can move
						foreach (Tuple<int, int> legal in legalMoves) {
							int iToRow = i + legal.Item1;
							int iToCol = j + legal.Item2;

							//Is move inside range of the board?
							if (iToRow >= 0 && iToRow < Board.Count &&
								iToCol >= 0 && iToCol < Board[0].Count) {
								//Is move going to be a space?
								if (Board[iToRow][iToCol] == 32) {
									MakeAMoveAndStore(i, j, iToRow, iToCol);
								}
							}
						}
					}
				}
			}
		}
	}
	class Program {
		static void Main(string[] args) {
			var a = new BoardMove("01011\n110 1\n01110\n01010\n00100");
			a.CalculateAvailableBoardMoves();

			Console.WriteLine(a);
			Console.ReadLine();
		}
	}
}
