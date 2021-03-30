using System;
using System.Collections.Generic;
using System.Text;

namespace HappyBudget.Models
{
    public class ColorPicker
    {
        public string HexCode { get; set; }

        public ColorPicker(string hexCode)
        {
            HexCode = hexCode;
        }
        
        public static List<ColorPicker> GetHexCodes()
        {
            return new List<ColorPicker>
            {
                new ColorPicker("#990000"),//
                new ColorPicker("#FF0000"),//
                new ColorPicker("#FF6666"),//
                new ColorPicker("#993D00"),//
                new ColorPicker("#FF6600"),//
                new ColorPicker("#FFA366"),//
                new ColorPicker("#DEB887"),//
                new ColorPicker("#CC0066"),//
                new ColorPicker("#FF3399"),//
                new ColorPicker("#FF99CC"),//
                new ColorPicker("#7A0099"),//
                new ColorPicker("#CC00FF"),//
                new ColorPicker("#E066FF"),//
                new ColorPicker("#191970"),//
                new ColorPicker("#003D99"),//
                new ColorPicker("#0000FF"),//
                new ColorPicker("#0066FF"),//
                new ColorPicker("#66A3FF"),//
               
               
                new ColorPicker("#87CEEB"),//
                new ColorPicker("#008080"),//
                new ColorPicker("#00997A"),//
                new ColorPicker("#00CED1"),//
                new ColorPicker("#00FFCC"),//
                new ColorPicker("#66FFE0"),//

                new ColorPicker("#708090"),//
                new ColorPicker("#808080"),//
               
            };
        }

    }
}
