﻿using PixiEditor.Helpers;
using PixiEditorDotNetCore3.Models.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PixiEditorDotNetCore3.Models.Layers
{
    public class Layer : BasicLayer
    {
        private WriteableBitmap _layerBitmap;

        public WriteableBitmap LayerBitmap
        {
            get { return _layerBitmap; }
            set
            {
                _layerBitmap = value;
                RaisePropertyChanged("LayerBitmap");
            }
        }

        public Layer(int width, int height)
        {
            Layer layer = LayerGenerator.Generate(width, height);
            LayerBitmap = layer.LayerBitmap;
            Width = width;
            Height = height;
        }


        public Layer(WriteableBitmap layerBitmap)
        {
            LayerBitmap = layerBitmap;
            Width = (int)layerBitmap.Width;
            Height = (int)layerBitmap.Height;
        }

        public void ApplyPixels(BitmapPixelChanges pixels, Color color)
        {
            LayerBitmap.Lock();
            foreach (var coords in pixels.ChangedCoordinates)
            {
                LayerBitmap.SetPixel(Math.Clamp(coords.X, 0, Width - 1), Math.Clamp(coords.Y, 0, Height - 1), color);
            }
            LayerBitmap.Unlock();
        }
    }
}