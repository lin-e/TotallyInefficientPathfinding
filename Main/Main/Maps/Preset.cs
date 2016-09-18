﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Main
{
    public class Preset
    {
        public static readonly string[] MAZE_01 =
        {
            "00000000000000000000000000000000000000000000000000000000000000000000000000000000",
            "01111111111111111111111111000001111111111111111111001111111111111111111111111110",
            "00001000000010000000000001000001000000000000000001001000000000100000000001001010",
            "00001000000000000000000000000001000000010000100001001001000000100000000001001010",
            "00001111111111111111111111111111111110010000100001001011111111111111111101001010",
            "00000000000000000000000000000000000000011001100001001001000000000000100101001010",
            "00111111111111111111111111111111111111110001001001001001000011111110100101001010",
            "00101010100100000000000000000000000000000001001001001000000010000010100101001010",
            "00101010100111111111111111111111111111111111001001111111111010000010100111001010",
            "00101010100100000000000000000000000000000001001000000000001010000010100000001010",
            "00100010100100111111111111111111111111111111001000000000001010000010111111111000",
            "00100000100100000000000000000000100000000001111111111111111010000010000000001010",
            "00100000000100000000000000000000100000000000000000000000000010000011111100101010",
            "00111111100111111111111111111110100111111111111001000000000010000000000100101010",
            "00000000100000000000000000000010100100000000001001000111111111111011000100101010",
            "01111100111111111111111111111010100111111100001001010100000000000010000100101010",
            "01000000100000000000000000001010000000000101001001010101111111111110000100101010",
            "01011111111111111111111110001011111111111101001111111101000000000011110100101010",
            "01000000000000000000000010001000000000000001000000000001011111100000010100101010",
            "01001111111111111111110010001111111111111111111111111111000000100000010100101010",
            "01001000000000000000000010001000000000000000000000000000000111111111110100111010",
            "01001111111111111111111111111111111111111111111111010000000000000000000100000010",
            "01000000000000000000000000000000000000000000000001010011111111111111111111111110",
            "01111111111111111111111111111111111111111111111111110000000000000000000000000010",
            "00000000000000000000000000000000000000000000000000000000000000000000000000000000"
        };
        public static readonly string[] EXAMPLE_01 =
        {
            "00000000000000000000000000000000000000000000000000000000000000000000000000000000",
            "11111100000000000000000000000000000000000000000000000000000000000000000000000000",
            "00000110000000000000000000000000000000000000000000000000000000000000000000000000",
            "00000011111111111111111111111111111111111111111111111111111111111111111111111100",
            "00000000000000000011110000000000000000111111111111111111111111111000100000000000",
            "00000000000000000000010000000000000000000001000000000000000000001111111111111111",
            "00000000000000001111111111111111111111111111111111110000000000000000010000000000",
            "00000000000000011000000000000010000000000000001000000000000000000000011100000000",
            "00000000000000001100000000000010000000000000001000000000000000000000000110000000",
            "00000000000000000111000000000010000000000000001000000000111111111111000010000000",
            "00000000000000000001110000000010000000000000001000000000000000001001111110000000",
            "00000000000000000000011100000010000000000000001000000000000000001000000011111111",
            "00000000000000000000000111000010000000000000001000000000000000011000000010000000",
            "00000000001000000000000001111110000000000000001111111111111111110000000010000000",
            "00000000001000000000000001000010000000000000000000000000000000000000000010000000",
            "00000000001111100000000001111111111111111111111111111111000000000001111111111111",
            "00000000001000000000000001000010000000000100000000000000000000000001000000000000",
            "00000000001111111111111111000010000000000111111111111000000000000001000000000000",
            "00000000000000000001000000000010000000000000000000011111111111111111111111100000",
            "00000000000000000001000000000011111111110000000000000000000000000000000000000000",
            "00000000000000000001000000000010000000011111111111111111110000000000000000000000",
            "00000000000000000001000000000010000000000000000000000000010000000000000000000000",
            "00000000000000000001111111111111111111111111111111111111110000000000000000000000",
            "00000000000000000000000000000000000000000000000000000000011111111111111111111111",
            "00000000000000000000000000000000000000000000000000000000000000000000000000000000"
        };
        public static List<int[]> Load(string[] inputGrid)
        {
            List<int[]> newPoints = new List<int[]>();
            for (int y = 0; y < inputGrid.Length; y++)
            {
                for (int x = 0; x < inputGrid[0].Length; x++)
                {
                    if (inputGrid[y][x].ToString() == "1")
                    {
                        newPoints.Add(new int[] { x, y });
                    }
                }
            }
            return newPoints;
        }
    }
}
