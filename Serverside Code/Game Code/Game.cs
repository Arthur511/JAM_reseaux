using System;
using System.Collections.Generic;
using PlayerIO.GameLibrary;

namespace MushroomsUnity3DExample {
	public class Player : BasePlayer {
		public float posx = 0;
		public float posz = 0;
		public int toadspicked = 0;
	}

	public class Toad {
		public int id = 0;
		public float posx = 0;
		public float posz = 0;
	}

	[RoomType("UnityMushrooms")]
	public class GameCode : Game<Player> {
		private int last_toad_id = 0;
		private List<Toad> Toads = new List<Toad>(); 

		// This method is called when an instance of your the game is created
		public override void GameStarted() {
			// anything you write to the Console will show up in the 
			// output window of the development server
			Console.WriteLine("Game is started: " + RoomId);



		}

		private void resetgame() {
			// scoring system
			Player winner = new Player();
			int maxscore = -1;
			foreach(Player pl in Players) {
				if(pl.toadspicked > maxscore) {
					winner = pl;
					maxscore = pl.toadspicked;
				}
			}

			
		}


		// This method is called when the last player leaves the room, and it's closed down.
		public override void GameClosed() {
			Console.WriteLine("RoomId: " + RoomId);
		}

		// This method is called whenever a player joins the game
		public override void UserJoined(Player player) {
			foreach(Player pl in Players) {
				if(pl.ConnectUserId != player.ConnectUserId) {
					pl.Send("PlayerJoined", player.ConnectUserId, 0, 0);
					player.Send("PlayerJoined", pl.ConnectUserId, pl.posx, pl.posz);
				}
			}

			// send current toadstool info to the player
			foreach(Toad t in Toads) {
				player.Send("Toad", t.id, t.posx, t.posz);
			}
		}

		// This method is called when a player leaves the game
		public override void UserLeft(Player player) {
			Broadcast("PlayerLeft", player.ConnectUserId);
		}

		// This method is called when a player sends a message into the server code
		public override void GotMessage(Player player, Message message) {
			switch(message.Type) {
				// called when a player clicks on the ground
				case "Chat":
					foreach(Player pl in Players) {
						if(pl.ConnectUserId != player.ConnectUserId) {
							pl.Send("Chat", player.ConnectUserId, message.GetString(0));
						}
					}
					break;
			}
		}
	}
}