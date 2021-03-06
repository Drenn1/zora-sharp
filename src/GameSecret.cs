﻿/*
 *  Copyright © 2013-2015, Andrew Nagle.
 *  All rights reserved.
 *
 *  This file is part of ZoraSharp.
 *
 *  ZoraSharp is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU Lesser General Public License as
 *  published by the Free Software Foundation, either version 3 of the
 *  License, or (at your option) any later version.
 *
 *  ZoraSharp is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU Lesser General Public License for more details.
 *
 *  You should have received a copy of the GNU Lesser General Public
 *  License along with ZoraSharp. If not, see <http://www.gnu.org/licenses/>.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zyrenth.Zora
{
	/// <summary>
	/// Represents a secret used to start a new game in the Zelda Oracle series
	/// </summary>
	public class GameSecret : Secret
	{
		// The PAL version checks that names consist of only these characters.
		private static readonly byte[] validPALCharacters =
		{
			0x41, 0x42, 0x43, 0x44, 0x45, 0x46, 0x47, 0x48,
			0x49, 0x4a, 0x4b, 0x4c, 0x4d, 0x4e, 0x4f, 0x50,
			0x51, 0x52, 0x53, 0x54, 0x55, 0x56, 0x57, 0x58,
			0x59, 0x5a, 0x20, 0x2e, 0x2c, 0x5f, 0x80, 0x81,
			0x82, 0x83, 0x84, 0x20, 0x85, 0x86, 0x87, 0x88,
			0x89, 0x8a, 0x8b, 0x8c, 0x8d, 0x8e, 0x8f, 0x90,
			0x21, 0x27, 0x2d, 0x3a, 0x3b, 0x3d, 0x11, 0x12,
			0xbd, 0x13, 0x28, 0x29, 0x00, 0x61, 0x62, 0x63,
			0x64, 0x65, 0x66, 0x67, 0x68, 0x69, 0x6a, 0x6b,
			0x6c, 0x6d, 0x6e, 0x6f, 0x70, 0x71, 0x72, 0x73,
			0x74, 0x75, 0x76, 0x77, 0x78, 0x79, 0x7a, 0x20,
			0x2e, 0x2c, 0x5f, 0xa0, 0xa1, 0xa2, 0xa3, 0xa4,
			0x20, 0xa5, 0xa6, 0xa7, 0xa8, 0xa9, 0xaa, 0xab,
			0xac, 0xad, 0xae, 0xaf, 0xb0, 0x21, 0x27, 0x2d,
			0x3a, 0x3b, 0x3d, 0x11, 0x12, 0xbd, 0x13, 0x28,
			0x29, 0x00
		};

		string _hero = "\0\0\0\0\0";
		string _child = "\0\0\0\0\0";
		byte _behavior = 0;
		byte _animal = 0;
		Game _targetGame = 0;
		bool _isHeroQuest = false;
		bool _isLinkedGame = false;
		bool _wasGivenFreeRing = false;

		bool _unknown58 = false;
		bool _unknown59 = false;

		/// <summary>
		/// Gets the required length of the secret
		/// </summary>
		public override int Length
		{
			get { return 20; }
		}

		/// <summary>
		/// Gets or sets the Game used for this user data
		/// </summary>
		public Game TargetGame
		{
			get { return _targetGame; }
			set
			{
				_targetGame = value;
				NotifyPropertyChanged("Game");
			}
		}

		/// <summary>
		/// Gets or sets the Quest type used for this user data
		/// </summary>
		public bool IsHeroQuest
		{
			get { return _isHeroQuest; }
			set
			{
				_isHeroQuest = value;
				NotifyPropertyChanged("IsHeroQuest");
			}
		}

		/// <summary>
		/// Gets or sets the Quest type used for this user data
		/// </summary>
		public bool IsLinkedGame
		{
			get { return _isLinkedGame; }
			set
			{
				_isLinkedGame = value;
				NotifyPropertyChanged("IsLinkedGame");
			}
		}

		/// <summary>
		/// Gets or sets the hero's name
		/// </summary>
		public string Hero
		{
			get { return _hero.Trim(' ', '\0'); }
			set
			{
				if (string.IsNullOrWhiteSpace(value))
					_hero = "\0\0\0\0\0";
				else
					_hero = value.TrimEnd().PadRight(5, '\0');
				NotifyPropertyChanged("Hero");
			}
		}

		/// <summary>
		/// Gets or sets the child's name
		/// </summary>
		public string Child
		{
			get { return _child.Trim(' ', '\0'); }
			set
			{
				if (string.IsNullOrWhiteSpace(value))
					_child = "\0\0\0\0\0";
				else
					_child = value.TrimEnd().PadRight(5, '\0');
				NotifyPropertyChanged("Child");
			}
		}

		/// <summary>
		/// Gets or sets the animal friend
		/// </summary>
		public Animal Animal
		{
			get { return (Animal)_animal; }
			set
			{
				_animal = (byte)value;
				NotifyPropertyChanged("Animal");
			}
		}

		/// <summary>
		/// Gets or set the behavior of the child
		/// </summary>
		public ChildBehavior Behavior
		{
			get { return (ChildBehavior)_behavior; }
			set
			{
				_behavior = (byte)value;
				NotifyPropertyChanged("Behavior");
			}
		}

		/// <summary>
		/// Gets or sets the value indicating if Vasu has given the player a free ring
		/// </summary>
		public bool WasGivenFreeRing
		{
			get { return _wasGivenFreeRing; }
			set
			{
				_wasGivenFreeRing = value;
				NotifyPropertyChanged("WasGivenFreeRing");
			}
		}

		/// <summary>
		/// Gets or sets the unknown flag at offset 58
		/// </summary>
		public bool Unknown58
		{
			get { return _unknown58; }
			set
			{
				_unknown58 = value;
				NotifyPropertyChanged("Unknown58");
			}
		}

		/// <summary>
		/// Gets or sets the unknown flag at offset 59
		/// </summary>
		public bool Unknown59
		{
			get { return _unknown59; }
			set
			{
				_unknown59 = value;
				NotifyPropertyChanged("Unknown59");
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="GameSecret"/> class.
		/// </summary>
		public GameSecret(GameRegion r)
		{
			Region = r;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="GameSecret"/> class from the
		/// specified game <paramref name="info"/>.
		/// </summary>
		/// <param name="info">The game information.</param>
		public GameSecret(GameInfo info) : base()
		{
			Load(info);
		}

		/// <summary>
		/// Loads in data from the specified game info
		/// </summary>
		/// <param name="info">The game info</param>
		/// <example>
		/// <code language="C#">
		/// GameInfo info = new GameInfo()
		/// {
		///     Game = Game.Ages,
		///     GameID = 14129,
		///     Hero = "Link",
		///     Child = "Pip",
		///     Animal = Animal.Dimitri,
		///     Behavior = ChildBehavior.BouncyD,
		///     IsLinkedGame = true,
		///     IsHeroQuest = false,
		///     WasGivenFreeRing = true
		/// };
		/// GameSecret secret = new GameSecret();
		/// secret.Load(info);
		/// </code>
		/// </example>
		public override void Load(GameInfo info)
		{
			Region = info.Region;
			GameID = info.GameID;
			TargetGame = info.Game;
			Hero = info.Hero;
			Child = info.Child;
			Animal = info.Animal;
			Behavior = info.Behavior;
			IsLinkedGame = info.IsLinkedGame;
			IsHeroQuest = info.IsHeroQuest;
			WasGivenFreeRing = info.WasGivenFreeRing;
			Unknown58 = info.Unknown58;
			Unknown59 = info.Unknown59;
		}

		/// <summary>
		/// Loads in data from the raw secret data provided
		/// </summary>
		/// <param name="secret">The raw secret data</param>
		/// <example>
		/// This example demonstrates loading a <see cref="GameSecret"/> from a
		/// a byte array containing an encoded secret.
		/// <code language="C#">
		/// // H~2:@ ←2♦yq GB3●( 6♥?↑6
		/// byte[] rawSecret = new byte[]
		/// {
		///      4, 37, 51, 36, 63,
		///     61, 51, 10, 44, 39,
		///      3,  0, 52, 21, 48,
		///     55,  9, 45, 59, 55
		/// };
		/// Secret secret = new GameSecret();
		/// secret.Load(rawSecret);
		/// </code>
		/// </example>
		public override void Load(byte[] secret)
		{
			if (secret == null || secret.Length != Length)
				throw new InvalidSecretException("Secret must contatin exactly 20 bytes");

			byte[] decodedBytes = DecodeBytes(secret);
			string decodedSecret = ByteArrayToBinaryString(decodedBytes);

			byte[] clonedBytes = (byte[])decodedBytes.Clone();
			clonedBytes[19] = 0;
			var checksum = CalculateChecksum(clonedBytes);
			if ((decodedBytes[19] & 7) != (checksum & 7))
				throw new InvalidSecretException("Checksum does not match expected value");

			GameID = Convert.ToInt16(decodedSecret.ReversedSubstring(5, 15), 2);

			if (decodedSecret[3] != '0' && decodedSecret[4] != '0')
				throw new ArgumentException("The specified data is not a game code", "secret");
			
			TargetGame = (Game)(byte)(decodedSecret[21] == '1' ? 1 : 0);
			IsHeroQuest = decodedSecret[20] == '1';
			IsLinkedGame = decodedSecret[105] == '1';


			Encoding encoding;
			if (Region == GameRegion.US)
				encoding = new USEncoding();
			else
				encoding = new JapaneseEncoding();

			Hero = encoding.GetString(new byte[] {
				Convert.ToByte(decodedSecret.ReversedSubstring(22, 8), 2),
				Convert.ToByte(decodedSecret.ReversedSubstring(38, 8), 2),
				Convert.ToByte(decodedSecret.ReversedSubstring(60, 8), 2),
				Convert.ToByte(decodedSecret.ReversedSubstring(77, 8), 2),
				Convert.ToByte(decodedSecret.ReversedSubstring(89, 8), 2)
			});

			Child = encoding.GetString(new byte[] {
				Convert.ToByte(decodedSecret.ReversedSubstring(30, 8), 2),
				Convert.ToByte(decodedSecret.ReversedSubstring(46, 8), 2),
				Convert.ToByte(decodedSecret.ReversedSubstring(68, 8), 2),
				Convert.ToByte(decodedSecret.ReversedSubstring(97, 8), 2),
				Convert.ToByte(decodedSecret.ReversedSubstring(106, 8), 2)
			});

			Animal = (Animal)Convert.ToByte(decodedSecret.ReversedSubstring(85, 4), 2);
			Behavior = (ChildBehavior)Convert.ToByte(decodedSecret.ReversedSubstring(54, 4), 2);
			WasGivenFreeRing = decodedSecret[76] == '1';


			// TODO: Figure out what all the unknown values are for.
			Unknown58 = decodedSecret[58] == '1';
			Unknown59 = decodedSecret[59] == '1';
		}

		/// <summary>
		/// Gets the raw secret data as a byte array
		/// </summary>
		/// <returns>A byte array containing the secret</returns>
		/// <example>
		/// <code language="C#">
		/// GameSecret secret = new GameSecret()
		/// {
		///     TargetGame = Game.Ages,
		///     GameID = 14129,
		///     Hero = "Link",
		///     Child = "Pip",
		///     Animal = Animal.Dimitri,
		///     Behavior = ChildBehavior.BouncyD,
		///     IsLinkedGame = true,
		///     IsHeroQuest = false,
		///     WasGivenFreeRing = true
		/// };
		/// byte[] data = secret.ToBytes();
		/// </code>
		/// </example>
		public override byte[] ToBytes()
		{
			Encoding encoding;
			if (Region == GameRegion.US)
				encoding = new USEncoding();
			else
				encoding = new JapaneseEncoding();

			byte[] heroBytes = encoding.GetBytes(_hero);
			byte[] childBytes = encoding.GetBytes(_child);

			int cipherKey = ((GameID >> 8) + (GameID & 255)) & 7;
			string unencodedSecret = Convert.ToString(cipherKey, 2).PadLeft(3, '0').Reverse();

			unencodedSecret += "00"; // game = 0

			unencodedSecret += Convert.ToString(GameID, 2).PadLeft(15, '0').Reverse();
			unencodedSecret += _isHeroQuest ? "1" : "0";
			unencodedSecret += TargetGame == Game.Ages ? "0" : "1";
			unencodedSecret += Convert.ToString(heroBytes[0], 2).PadLeft(8, '0').Reverse();
			unencodedSecret += Convert.ToString(childBytes[0], 2).PadLeft(8, '0').Reverse();
			unencodedSecret += Convert.ToString(heroBytes[1], 2).PadLeft(8, '0').Reverse();
			unencodedSecret += Convert.ToString(childBytes[1], 2).PadLeft(8, '0').Reverse();
			unencodedSecret += Convert.ToString(_behavior, 2).PadLeft(8, '0').Reverse().Substring(0, 4);
			unencodedSecret += _unknown58 ? "1" : "0"; // TODO: This
			unencodedSecret += _unknown59 ? "1" : "0"; // TODO: This
			unencodedSecret += Convert.ToString(heroBytes[2], 2).PadLeft(8, '0').Reverse();
			unencodedSecret += Convert.ToString(childBytes[2], 2).PadLeft(8, '0').Reverse();
			unencodedSecret += _wasGivenFreeRing ? "1" : "0";
			unencodedSecret += Convert.ToString(heroBytes[3], 2).PadLeft(8, '0').Reverse();
			unencodedSecret += Convert.ToString(_animal, 2).PadLeft(4, '0').Reverse();
			unencodedSecret += Convert.ToString(heroBytes[4], 2).PadLeft(8, '0').Reverse();
			unencodedSecret += Convert.ToString(childBytes[3], 2).PadLeft(8, '0').Reverse();
			unencodedSecret += _isLinkedGame ? "1" : "0";
			unencodedSecret += Convert.ToString(childBytes[4], 2).PadLeft(8, '0').Reverse();

			byte[] unencodedBytes = BinaryStringToByteArray(unencodedSecret);
			unencodedBytes[19] = CalculateChecksum(unencodedBytes);
			byte[] secret = EncodeBytes(unencodedBytes);

			return secret;
		}

		/// <summary>
		/// Updates the game information.
		/// </summary>
		/// <param name="info">The information.</param>
		/// <example>
		/// <code language="C#">
		/// GameSecret secret = new GameSecret()
		/// {
		///     TargetGame = Game.Ages,
		///     GameID = 14129,
		///     Hero = "Link",
		///     Child = "Pip",
		///     Animal = Animal.Dimitri,
		///     Behavior = ChildBehavior.BouncyD,
		///     IsLinkedGame = true,
		///     IsHeroQuest = false,
		///     WasGivenFreeRing = true
		/// };
		/// GameInfo info = new GameInfo();
		/// secret.UpdateGameInfo(info);
		/// </code>
		/// </example>
		public void UpdateGameInfo(GameInfo info)
		{
			info.Region = Region;
			info.GameID = GameID;
			info.Game = TargetGame;
			info.Hero = Hero;
			info.Child = Child;
			info.Animal = Animal;
			info.Behavior = Behavior;
			info.IsLinkedGame = IsLinkedGame;
			info.IsHeroQuest = IsHeroQuest;
			info.WasGivenFreeRing = WasGivenFreeRing;
			info.Unknown58 = Unknown58;
			info.Unknown59 = Unknown59;
		}

		/// <summary>
		/// Checks if the secret is valid for PAL, since it has extra sanity checks.
		/// </summary>
		public bool IsValidForPAL()
		{
			if (_animal != 0x0b && _animal != 0x0c && _animal != 0x0d)
				return false;

			Encoding encoding = new USEncoding();
			byte[] heroBytes = encoding.GetBytes(_hero);
			byte[] childBytes = encoding.GetBytes(_child);

			for (int i=0; i<5; i++)
			{
				if (!validPALCharacters.Contains(heroBytes[i]))
					return false;
				if (!validPALCharacters.Contains(childBytes[i]))
					return false;
			}

			return true;
		}

		/// <summary>
		/// Determines whether the specified <see cref="System.Object" />, is equal to this instance.
		/// </summary>
		/// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
		/// <returns>
		///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
		/// </returns>
		public override bool Equals(object obj)
		{
			if (obj == null || GetType() != obj.GetType())
				return false;

			GameSecret g = (GameSecret)obj;

			return
				(base.Equals(g)) &&
				(_targetGame == g._targetGame) &&
				(_hero == g._hero) &&
				(_child == g._child) &&
				(_behavior == g._behavior) &&
				(_animal == g._animal) &&
				(_isHeroQuest == g._isHeroQuest) &&
				(_isLinkedGame == g._isLinkedGame) &&
				(_wasGivenFreeRing == g._wasGivenFreeRing) &&
				(_unknown58 == g._unknown58) &&
				(_unknown59 == g._unknown59);

		}

		/// <summary>
		/// Returns a hash code for this instance.
		/// </summary>
		/// <returns>
		/// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
		/// </returns>
		public override int GetHashCode()
		{
			return base.GetHashCode() ^
				_targetGame.GetHashCode() ^
				_hero.GetHashCode() ^
				_child.GetHashCode() ^
				_behavior.GetHashCode() ^
				_animal.GetHashCode() ^
				_isHeroQuest.GetHashCode() ^
				_isLinkedGame.GetHashCode() ^
				_wasGivenFreeRing.GetHashCode() ^
				_unknown58.GetHashCode() ^
				_unknown59.GetHashCode();
		}
	}
}
