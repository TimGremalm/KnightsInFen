using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnightsInFen {
	public class BoardMove {
		BoardMove Parent;
		public List<BoardMove> Children;
		public List<List<Char>> Board = new List<List<char>>();
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
				List<Char> CharList = new List<char>();
				foreach(Char character in line) {
					CharList.Add(character);
				}
				Board.Add(CharList);
			}
		}
		public override string ToString() {
			return string.Join("\n", Board.Select(r => String.Join("", r.Select(c => c.ToString()))));
		}
	}
	class Program {
		static void Main(string[] args) {
			var a = new BoardMove("01011\n110 1\n01110\n01010\n00100");

			Console.WriteLine(a);
			Console.ReadLine();
		}
	}
}
