using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnightsInFen {
	public class BoardMove {
		BoardMove Parent = null;
		public List<BoardMove> AvailableMoves = new List<BoardMove>();
		public List<BoardMove> ValidMoves = new List<BoardMove>();
		public List<List<char>> Board = new List<List<char>>();	//Stores the position of pieces here
		public int Generation;
		public bool Finished;
		public int ShortestPathItemNo = -1;             //Init as -1 (out of range), this will be set when we find new valid board moves
		public int ShortestPathSteps = int.MaxValue;    //Init as huge, this will decrease when we find new valid board moves

		private const string FinishedLook =	"11111\n" +
											"01111\n" +
											"00 11\n" +
											"00001\n" +
											"00000";
		private const int MaxNumberOfMoves = 10;

		public BoardMove (string Layout) {
			InitBoardMove(null, 0, Layout);
		}
		public BoardMove(BoardMove parent, int generation, string Layout) {
			InitBoardMove(parent, generation, Layout);
		}
		private void InitBoardMove(BoardMove parent, int generation, string Layout) {
			Parent = parent;
			Generation = generation;
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
			BoardMove newMove = new BoardMove(this, Generation+1, TextRepresentationOfBoard(Board));
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
					//A board piece can be anything but a space
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
									//In that case it's a successful move and should be stored
									MakeAMoveAndStore(i, j, iToRow, iToCol);
								}
							}
						}
					}
				}
			}
		}

		public void CalculateValidBoardMoves() {
			foreach(BoardMove move in AvailableMoves) {
				//Todo: Check previous BoardMoves so we don't get any duplicates

				//Check that max generation isn't reached
				if (move.Generation < MaxNumberOfMoves) {
					ValidMoves.Add(move);
					string a = TextRepresentationOfBoard(move.Board);
					if (TextRepresentationOfBoard(move.Board).Equals(FinishedLook)) {
						move.Finished = true;
					}
					if (move.Finished) {
						move.SetShortestPath();
					}
				}
			}

		}
		private void SetShortestPath() {
			//Walk up through parents and check if this generation is shorter then previous
			//If shorter, set ShortestPath ItemNo and Steps for parent
			BoardMove parentBoardMove = this;
			BoardMove previousBoardMove = this;
			while (parentBoardMove.Generation != 0) {
				parentBoardMove = parentBoardMove.Parent;

				if (this.Generation < parentBoardMove.ShortestPathSteps) {
					parentBoardMove.ShortestPathSteps = this.Generation;
					parentBoardMove.ShortestPathItemNo = parentBoardMove.ValidMoves.IndexOf(previousBoardMove);
				}

				previousBoardMove = parentBoardMove;	//Save for the next iteration
			}
		}
	}
	class Program {
		static void Main(string[] args) {
			var a = new BoardMove(	"01011\n" +
									"110 1\n" +
									"01110\n" +
									"01010\n" +
									"00100");
			a = new BoardMove(		"11111\n" +
									"0111 \n" +
									"00111\n" +
									"00001\n" +
									"00000");
			a.CalculateAvailableBoardMoves();
			a.CalculateValidBoardMoves();

			foreach (BoardMove move in a.ValidMoves) {
				move.CalculateAvailableBoardMoves();
				move.CalculateValidBoardMoves();
			}

			Console.WriteLine(a);
			Console.ReadLine();
		}
	}
}
