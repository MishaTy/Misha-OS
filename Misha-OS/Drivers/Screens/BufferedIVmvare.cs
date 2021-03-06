﻿using Cosmos.System.Graphics;
using Cosmos.System.Graphics.Fonts;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace MishaOS.Drivers.Screens
{
    public class BufferedIVmvare : IDisplay
    {
        private DoubleBufferedVMWareSVGAII backends = new DoubleBufferedVMWareSVGAII();
        public int ScreenWidth { get; private set; } = 640;

        public int ScreenHeight { get; private set; } = 480;
        public void Clear(Color col)
        {
            backends.DoubleBuffer_Clear(ColorToUint(col));
        }

        private uint ColorToUint(Color col)
        {
            return (uint)col.ToArgb();
        }

        public void Disable()
        {
            backends.Disable();
        }

        public void DisplayD()
        {
            backends.SetMode(640, 480);
        }

        public void DrawRectangle(int x, int y, int Width, int Height, Color col)
        {
            backends.DoubleBuffer_DrawFillRectangle((uint)x, (uint)y, (uint)Width, (uint)Height, ColorToUint(col));
        }
        //Copied from: https://github.com/CosmosOS/Cosmos/blob/master/source/Cosmos.System2/Graphics/Canvas.cs
        public void DrawString(string str, Pen pen, int x, int y)
        {
            Font aFont = PCScreenFont.Default;
            foreach (char c in str)
            {
                DrawChar(c, aFont, pen, x, y); ;
                x += aFont.Width;
            }
        }
        //Copied from: https://github.com/CosmosOS/Cosmos/blob/master/source/Cosmos.System2/Graphics/Canvas.cs
        public void DrawChar(char c, Font aFont, Pen pen, int x, int y)
        {
            int p = aFont.Height * (byte)c;

            for (int cy = 0; cy < aFont.Height; cy++)
            {
                for (byte cx = 0; cx < aFont.Width; cx++)
                {
                    if (aFont.ConvertByteToBitAddres(aFont.Data[p + cy], cx + 1))
                    {
                        setPixel((ushort)((x) + (aFont.Width - cx)), (ushort)((y) + cy),pen.Color);
                    }
                }
            }
        }
        public void Init(Mode mode)
        {
            backends.SetMode((uint)mode.Columns, (uint)mode.Rows);
        }

        public void Render()
        {
            backends.DoubleBuffer_Update();
        }

        public void setPixel(int x, int y, Color c)
        {
            if (Color.FromArgb((int)backends.DoubleBuffer_GetPixel((uint)x, (uint)y)) == c)
                return;
            backends.DoubleBuffer_SetPixel((uint)x, (uint)y, ColorToUint(c));
        }
    }
}
