using System;
using System.Collections.Generic;

namespace PilotsSafe
{
    internal class PlayField
    {
        //Playing field
        public int[,] Field { get; set; }
        //field size (size x size)
        public int Size { get; set; }

        public PlayField(int size)
        {
            this.Field = new int[size, size];
            this.Size = size;
            GenerateField(size);
        }

        public void Motion(KeyValuePair<int, int> coordinatesField)
        {
            for (int i = 0; i < Size; i++)
            {
                Field[coordinatesField.Key, i] = InvertCell(Field[coordinatesField.Key, i]);
                if(!coordinatesField.Key.Equals(i))
                {
                    Field[i, coordinatesField.Value] = InvertCell(Field[i, coordinatesField.Value]);
                }
            }
        }

        private static int InvertCell(int valueCell)
        {
            return Math.Abs(valueCell - 1);
        }

        private void GenerateField(int size)
        {
            Random rnd = new();
            int quantityMotion = 100;
            for(int i = 0; i < quantityMotion; i++)
            {
                int row = rnd.Next(size);
                int column = rnd.Next(size);
                Field[row, column] = InvertCell(Field[row, column]);
            }
            if(VictoryCheck())
            {
                GenerateField(size);
            }
        }

        public bool VictoryCheck()
        {
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    if (Field[0, 0] != Field[i, j])
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
