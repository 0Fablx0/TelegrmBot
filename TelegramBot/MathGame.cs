using System;
using System.Collections.Generic;
using System.Text;

namespace TelegramBot
{
    public class MathGame
    {
        public int? answer = null;
        public int right = 0;
        public int wrong = 0;

        public string getQuestion()
        {
            Random rnd = new Random();
            int switchParam = rnd.Next(0, 3);
            switch (switchParam)
            {
                case 0: { return multiplication(); };
                case 1: { return division(); };
                case 2: { return addition(); };
                case 3: { return subtraction(); };
                default: { return multiplication(); };
            }
        }

        private string multiplication()
        {
            Random rnd = new Random();
            int paramX = rnd.Next(1, 100);
            int paramY = rnd.Next(0, 10);
            int resParam = paramX * paramY;
            answer = paramY;
            return $"{paramX} * x = {resParam}";
        }

        private string division()
        {
            Random rnd = new Random();
            int paramX = rnd.Next(1, 100);
            int paramY = rnd.Next(1, 15);
            int resParam = paramX * paramY;
            answer = paramY;
            return $"{resParam} / {paramX} = x";
        }

        private string addition()
        {
            Random rnd = new Random();
            int paramX = rnd.Next(1, 1000);
            int paramY = rnd.Next(0, 1000);
            int resParam = paramX + paramY;
            answer = paramY;
            return $"{paramX} + x = {resParam}";
        }

        private string subtraction()
        {
            Random rnd = new Random();
            int paramX = rnd.Next(1, 1500);
            int paramY = rnd.Next(0, 3000);
            int resParam = paramX + paramY;
            answer = paramY;
            return $"{resParam} - {paramX} = x";
        }
    }
}
