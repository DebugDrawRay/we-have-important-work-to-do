using System;
using System.Collections.Generic;
using UnityEngine;

namespace FSS
{
	public static class Settings
	{
        //Ram
        public const float WindowCost = 1;
        public const float StartRam = 32;
        public const string RamUnit = "mb";
        public const float PurchasableRam = 5;
        
        public const float AntiAdRamCost = 5;
        public const float ResizeRamCost = 5;
        public const float PredictionRamCost = 5;
        public const float ConsolidatorRamCost = 5;
        public const float TimeRamCost = 5;

        //Time 
        public const float StartPopupInterval = 1f;

        public static DateTime StartClockTime = new DateTime(1986, 1, 1, 9, 0, 0);
        public static DateTime EasyClockEnd = new DateTime(1986, 1, 1, 12, 0, 0);
        public static DateTime HardClockEnd = new DateTime(1986, 1, 1, 17, 0, 0);

        public const float EasyModeTime = 300;
        public const float HardModeTime = 600;

        public const float ClockUpdateInterval = 1f;

        public const float AntiAdLength = 15;
        public const float ResizeLength = 15;
        public const float PredictionLength = 15;
        public const float ConsolidatorLength = 15;
        public const float TimeLength = 15;

        public const float AntiAdDelay = 15;
        public const float ResizeDelay = 15;
        public const float PredictionDelay = 15;
        public const float ConsolidatorDelay = 15;
        public const float TimeDelay = 15;

        //Money
        public const int CurrencyOnClose = 1000;
        public const int AntiAdCost = 10;
        public const int ResizeCost = 10;
        public const int PredictionCost = 10;
        public const int ConsolidatorCost = 10;
        public const int TimeCost = 10;
        public const int RamCost = 10;

        //Penalties
        public const float PenaltyTime = 15f;
        public const float PenaltySlowFactor = 2f;
        public const float PenaltySpeedFactor = 4f;
        public const float EmoteRate = .05f;
        public const float PetPositionRate = 1f;

        //Programs
        public const float AntiAdCloseInterval = 4f;
        public const float TimeSpeedMulti = 4f;
        public static Vector2 SmallWindowSize = new Vector2(128, 128);
        public static Vector2 ConsolodatePosition = new Vector2(0, 0);

    }
}
