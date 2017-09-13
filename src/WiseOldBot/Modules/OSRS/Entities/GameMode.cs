// Description:
//
// Solution: DiscordBots
// Project: WiseOldBot
//
// Created: 10/17/2016 6:04 PM
// Last Revised: 10/17/2016 6:04 PM
// Last Revised by: Alex Gravely

namespace WiseOldBot.Modules.OSRS.Entities
{
    public enum GameMode
    {
        Regular,
        Ironman,
        Ultimate,
        Deadman,
        DeadmanSeasonal,
        HardcoreIronman,
        R = Regular,
        Reg = Regular,
        Normal = Regular,
        Norm = Regular,
        I = Ironman,
        Iron = Ironman,
        U = Ultimate,
        UI = Ultimate,
        Ulti = Ultimate,
        DM = Deadman,
        DMM = Deadman,
        DMS = DeadmanSeasonal,
        Seasonal = DeadmanSeasonal,
        DMMS = DeadmanSeasonal,
        DMSeasonal = DeadmanSeasonal,
        H = HardcoreIronman,
        HI = HardcoreIronman,
        Hard = HardcoreIronman,
        Hardcore = HardcoreIronman
    }

    public static class GameModeExtensions
    {
        public static string GetGameModeName(this GameMode mode)
        {
            switch ((int)mode)
            {
                case 0:
                    return "Regular";
                case 1:
                    return "Ironman Mode";
                case 2:
                    return "Ultimate Ironman Mode";
                case 3:
                    return "Deadman Mode";
                case 4:
                    return "Deadman Seasonal Mode";
                case 5:
                    return "Hardcore Ironman Mode";
                default:
                    return null;
            }
        }
    }
}