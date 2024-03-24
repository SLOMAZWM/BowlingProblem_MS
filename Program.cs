using System;

namespace BowlingProblem_MS
{
    class Program
    {
        static void Main()
        {
            Game game = new();
            game.StartGame();
        }
    }

    class Game
    {
        private const int MaxFrame = 10;
        private const int MaxRoll = 21;
        private readonly int[] rolls = new int[MaxRoll];
        private int currentRoll = 0;

        public void Roll(int pins)
        {
            rolls[currentRoll++] = pins;
        }

        public int GetScore()
        {
            int score = 0;
            int rollIndex = 0;
            for (int frame = 0; frame < MaxFrame; frame++)
            {
                if (IsStrike(rollIndex))
                {
                    score += 10 + StrikeBonus(rollIndex);
                    rollIndex++;
                }
                else if (IsSpare(rollIndex))
                {
                    score += 10 + SpareBonus(rollIndex);
                    rollIndex += 2;
                }
                else
                {
                    score += SumOfPinsInFrame(rollIndex);
                    rollIndex += 2;
                }
            }
            return score;
        }

        public void StartGame()
        {
            Console.WriteLine("Gra w kręgle rozpoczęta!");
            for (int frame = 1; frame <= MaxFrame; frame++)
            {
                Console.WriteLine();
                Console.WriteLine($"Ramka {frame}:");
                if (frame == MaxFrame)
                {
                    HandleLastFrame();
                }
                else
                {
                    HandleFrame();
                }
            }
            Console.WriteLine();
            Console.WriteLine($"Koniec gry! Twój końcowy wynik to: {GetScore()}");
        }

        private void HandleFrame()
        {
            Console.WriteLine("Rzut 1:");
            int pins = GetPinsFromUser();
            Roll(pins);
            Console.WriteLine($"Aktualny wynik: {GetScore()}");

            if (pins != 10)
            {
                Console.WriteLine("Rzut 2:");
                int remainingPins = 10 - pins;
                pins = GetPinsFromUser(remainingPins);
                Roll(pins);
                Console.WriteLine($"Aktualny wynik: {GetScore()}");
                if (pins == remainingPins)
                {
                    Console.WriteLine("Spare!");
                }
            }
        }

        private void HandleLastFrame()
        {
            int firstRollPins = GetPinsFromUser();
            Console.WriteLine($"Rzut 1: Zbitych kręgli: {firstRollPins}");
            Roll(firstRollPins);
            Console.WriteLine($"Aktualny wynik: {GetScore()}");

            int secondRollPins = GetPinsFromUser(firstRollPins == 10 ? 10 : 10 - firstRollPins);
            Console.WriteLine($"Rzut 2: Zbitych kręgli: {secondRollPins}");
            Roll(secondRollPins);
            Console.WriteLine($"Aktualny wynik: {GetScore()}");

            if (firstRollPins == 10 || firstRollPins + secondRollPins == 10)
            {
                int thirdRollPins = GetPinsFromUser();
                Console.WriteLine($"Rzut 3: Zbitych kręgli: {thirdRollPins}");
                Roll(thirdRollPins);
                Console.WriteLine($"Aktualny wynik: {GetScore()}");
            }
        }

        private static int GetPinsFromUser(int maxPins = 10)
        {
            int pins = -1;
            while (pins < 0 || pins > maxPins)
            {
                Console.Write($"Podaj liczbę strąconych kręgli (0-{maxPins}): ");
                if (!int.TryParse(Console.ReadLine(), out pins) || pins < 0 || pins > maxPins)
                {
                    Console.WriteLine($"Nieprawidłowa wartość, podaj liczbę od 0 do {maxPins}!");
                    pins = -1;
                }
            }
            return pins;
        }

        private bool IsStrike(int rollIndex)
        {
            return rolls[rollIndex] == 10;
        }

        private bool IsSpare(int rollIndex)
        {
            return rolls[rollIndex] + rolls[rollIndex + 1] == 10;
        }

        private int StrikeBonus(int rollIndex)
        {
            return rolls[rollIndex + 1] + rolls[rollIndex + 2];
        }

        private int SpareBonus(int rollIndex)
        {
            return rolls[rollIndex + 2];
        }

        private int SumOfPinsInFrame(int rollIndex)
        {
            return rolls[rollIndex] + rolls[rollIndex + 1];
        }
    }
}
