using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace SlotMachineApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SpinController : ControllerBase
    {
        private static readonly string[] Symbols = { "$", "?", "7" };
        private static readonly Dictionary<string, string> Questions = new Dictionary<string, string>
        {
            { "Q1", "On a sunny day, you find a pumpkin plant with a single ripe pumpkin. Do you..." },
            { "Q2", "While tending your pumpkin patch, you spot a bug on one of your pumpkins. Do you..." },
            { "Q3", "Your friend offers you a unique pumpkin seed. Do you..." },
            { "Q4", "You see a pumpkin festival in town. Do you..." },
            { "Q5", "A neighbor asks for help with their pumpkin harvest. Do you..." },
            { "Q6", "You discover a mysterious pumpkin in your garden. Do you..." },
            { "Q7", "Your family asks if you want to make pumpkin pie or pumpkin soup. Do you..." },
            { "Q8", "You find a rare pumpkin variety at the market. Do you..." },
            { "Q9", "Your pumpkin patch is overrun with weeds. Do you..." },
            { "Q10", "A stranger offers you a special pumpkin fertilizer. Do you..." }
        };

        [HttpGet("random")]
        public IActionResult GetRandomQuestion()
        {
            Random rand = new Random();
            var questionId = $"Q{rand.Next(1, 11)}";
            var questionText = Questions[questionId];
            var answers = new List<string>
            {
                "Plant it immediately",
                "Consult a gardening book",
                "Give it to a friend"
            };
            return Ok(new
            {
                QuestionId = questionId,
                QuestionText = questionText,
                Answers = answers
            });
        }

        [HttpGet("spin")]
        public IActionResult Spin(int bet, string question, string answer)
        {
            if (bet < 1 || bet > 100)
                return BadRequest("Invalid bet amount.");

            var result = SpinReels();
            var payout = CalculatePayout(result);
            var bankAmount = 100 - bet + payout; // Example logic
            var freeSpins = new Random().Next(0, 2); // Randomly award 0 or 1 free spins
            var diceRoll = RollDice(question);

            return Ok(new
            {
                Result = result,
                Payout = payout,
                BankAmount = bankAmount,
                FreeSpins = freeSpins,
                DiceRoll = diceRoll,
                QuestionText = GetQuestionText(question)
            });
        }

        private string[] SpinReels()
        {
            var rand = new Random();
            var result = new string[15];
            for (int i = 0; i < 15; i++)
            {
                result[i] = Symbols[rand.Next(Symbols.Length)];
            }
            return result;
        }

        private int CalculatePayout(string[] result)
        {
            var flatPayouts = new Dictionary<string, int>
            {
                { "$$$", 100 },
                { "???", 60 },
                { "777", 150 },
                { "$$$$", 200 },
                { "????", 300 },
                { "7777", 320 },
                { "$$$$$", 350 },
                { "?????", 500 }
            };

            var payout = 0;
            var rowStr = string.Join("", result);
            if (flatPayouts.ContainsKey(rowStr))
            {
                payout = flatPayouts[rowStr];
            }

            return payout;
        }

        private int RollDice(string question)
        {
            var rand = new Random();
            var sides = GetDiceSides(question);
            return rand.Next(1, sides + 1);
        }

        private int GetDiceSides(string question)
        {
            return question switch
            {
                "Q1" => 6,
                "Q2" => 12,
                "Q3" => 32,
                _ => 6
            };
        }

        private string GetQuestionText(string questionId)
        {
            return Questions.ContainsKey(questionId) ? Questions[questionId] : "No question selected.";
        }
    }
}
