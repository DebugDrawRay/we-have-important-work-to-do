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
        //Time 
        public const float StartPopupInterval = 5f;

        public static DateTime StartClockTime = new DateTime(1986, 1, 1, 9, 0, 0);
        public static DateTime EasyClockEnd = new DateTime(1986, 1, 1, 12, 0, 0);

        public const float EasyModeTime = 300;
        public const float MediumModeTime = 600;
        public const float HardModeTime = 900;

        public const float ClockUpdateInterval = 1f;
        //Money
        public const int CurrencyOnClose = 1;
    }
}
