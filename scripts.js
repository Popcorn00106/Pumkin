let currentQuestion = '';
let answers = ['Answer 1', 'Answer 2', 'Answer 3'];

document.addEventListener('DOMContentLoaded', () => {
    selectRandomQuestion();
});

async function selectRandomQuestion() {
    const response = await fetch('https://your-web-server.com/api/questions/random');
    const data = await response.json();

    currentQuestion = data.QuestionId;
    document.getElementById('question').textContent = `Question: ${data.QuestionText}`;
    updateAnswerButtons(data.Answers);
}

function updateAnswerButtons(answers) {
    document.getElementById('answerOne').textContent = answers[0];
    document.getElementById('answerTwo').textContent = answers[1];
    document.getElementById('answerThree').textContent = answers[2];
}

async function answerQuestion(answerId) {
    const bet = parseInt(document.getElementById('bet').value);

    if (bet < 1 || bet > 100) {
        alert('Please enter a bet between 1 and 100 cents.');
        return;
    }

    const response = await fetch(`https://your-web-server.com/api/spin?bet=${bet}&question=${currentQuestion}&answer=${answerId}`);
    const data = await response.json();

    const result = data.Result;
    const payout = data.Payout;
    const bankAmount = data.BankAmount;
    const freeSpins = data.FreeSpins;
    const diceRoll = data.DiceRoll;

    updateReels(result);
    document.getElementById('payout').textContent = `Payout: ${payout} cents`;
    document.getElementById('bankAmount').textContent = `Bank Amount: ${bankAmount} cents`;
    document.getElementById('freeSpins').textContent = `Free Spins Awarded: ${freeSpins}`;
    document.getElementById('diceRoll').textContent = `Dice Roll: ${diceRoll}`;
    document.getElementById('questionText').textContent = `Question: ${data.QuestionText}`;

    if (freeSpins > 0) {
        document.getElementById('results').style.backgroundColor = '#d4edda'; // Green for free spins
    }
}

function updateReels(result) {
    for (let i = 0; i < 15; i++) {
        document.getElementById(`reel${i + 1}`).textContent = result[i];
    }
}
