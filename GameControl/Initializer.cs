using System.Windows.Forms;
using GameUI;
using GameLogic;

namespace GameControl
{
	public class Initializer
	{
		public static void Run()
		{
			string player1Name, player2Name;
			int boardSize;
			bool twoPlayers;
			Game game;
			FormGame formGame;
			Controller controller;
			FormSettings formSettings = new FormSettings();

			formSettings.ShowDialog();
			if (formSettings.DialogResult == DialogResult.OK)
			{
				player1Name = formSettings.Player1Name;
				boardSize = formSettings.BoardSize;
				twoPlayers = formSettings.TwoPlayers;
				if (twoPlayers)
				{
					player2Name = formSettings.Player2Name;
				}
				else
				{
					player2Name = "Computer";
				}

				game = new Game(player1Name, player2Name, boardSize, twoPlayers);
				formGame = new FormGame(boardSize, player1Name, player2Name);
				controller = new Controller(game, formGame);
				controller.Run();
			}
		}
	}
}