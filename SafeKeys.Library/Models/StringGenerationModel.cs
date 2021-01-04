using System;
using System.Collections.Generic;
using System.Text;

namespace SafeKeys.Library.Models
{
    public class StringGenerationModel
    {

        public StringGenerationModel()
        {

        }
        public StringGenerationModel(int adj, int name, int word, int bigWord, int regChar, int irregChar, int letter, int number, bool randomCaps, int fakeWord)
        {
            Adj = adj;
            Name = name;
            Word = word;
            BigWord = bigWord;
            RegChar = regChar;
            IrregChar = irregChar;
            Letter = letter;
            Number = number;
            RandomCaps = randomCaps;
            FakeWord = fakeWord;
        }

        public int Adj { get; set; } = 0;
        public int Name { get; set; } = 0;
        public int Word { get; set; } = 0;
        public int BigWord { get; set; } = 0;
        public int RegChar { get; set; } = 0;
        public int IrregChar { get; set; } = 0;
        public int Letter { get; set; } = 0;
        public int Number { get; set; } = 0;
        public bool RandomCaps { get; set; } = true;
        public int FakeWord { get; set; } = 1;
    }
}
