﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;
using PixiEditor.Exceptions;
using PixiEditor.Models.DataHolders;
using PixiEditor.Models.Position;
using PixiEditor.Models.Tools;
using Xunit;

namespace PixiEditorTests.ModelsTests.DataHoldersTests
{
    public class BitmapPixelChangesTests
    {

        [Fact]
        public void TestThatFromSingleColoredArrayCreatesCorrectArray()
        {
            var color = Colors.Chocolate;
            Coordinates[] cords = {new Coordinates(0, 0), new Coordinates(1, 0), new Coordinates(3, 2)};
            var bmpChanges = BitmapPixelChanges.FromSingleColoredArray(cords, color);

            Assert.All(bmpChanges.ChangedPixels.Values, changeColor => Assert.Equal(color, changeColor));
            Assert.True(bmpChanges.WasBuiltAsSingleColored);
        }

        [Fact]
        public void TestThatCombineCombineOverrideCombinesValues()
        {
            Coordinates[] cords1 = {new Coordinates(0, 0), new Coordinates(1, 0), new Coordinates(3, 2)};
            Coordinates[] cords2 = {new Coordinates(3, 2), new Coordinates(0, 0), new Coordinates(5, 5)};
            BitmapPixelChanges changes = BitmapPixelChanges.FromSingleColoredArray(cords1, Colors.Green);
            BitmapPixelChanges changes2 = BitmapPixelChanges.FromSingleColoredArray(cords2, Colors.Red);

            var output = BitmapPixelChanges.CombineOverride(new[] {changes, changes2});
            Assert.Equal(4,output.ChangedPixels.Count);
            Assert.Equal(Colors.Red, output.ChangedPixels[new Coordinates(3,2)]);
            Assert.Equal(Colors.Red, output.ChangedPixels[new Coordinates(0,0)]);
            Assert.Equal(Colors.Green, output.ChangedPixels[new Coordinates(1,0)]);
        }

        [Fact]
        public void TestThatFromArraysThrowsError()
        {
            Assert.Throws<ArrayLengthMismatchException>
                (() => BitmapPixelChanges.FromArrays(new[] {new Coordinates(0, 0)}, new[] {Colors.Red, Colors.Green}));
        }

        [Fact]
        public void TestThatFormArraysWorks()
        {
            Coordinates[] coordinatesArray = {new Coordinates(0, 0), new Coordinates(2, 3), new Coordinates(5, 5)};
            Color[] colorsArray = {Colors.Red, Colors.Green, Colors.Blue};
            var result = BitmapPixelChanges.FromArrays(coordinatesArray, colorsArray);
            for (int i = 0; i < coordinatesArray.Length; i++)
            {
                var cords = coordinatesArray[i];
                Assert.Equal(colorsArray[i], result.ChangedPixels[cords]);
            }
            Assert.False(result.WasBuiltAsSingleColored);
        }

    }
}
