#include <iostream>
#include <vector>
#include <cstdlib>
#include <ctime>

// Define symbols
const std::vector<std::string> symbols = { "$", "?", "7" };

// Function to spin the reels
std::vector<std::string> spinReels() {
    std::vector<std::string> result(15);
    for (int i = 0; i < 15; ++i) {
        result[i] = symbols[rand() % symbols.size()];
    }
    return result;
}

// Function to calculate payout
int calculatePayout(const std::vector<std::string>& result) {
    std::vector<std::string> winPatterns = { "$$$", "???", "777", "$$$$", "????", "7777", "$$$$$", "?????" };
    std::vector<int> payouts = { 100, 60, 150, 200, 300, 320, 350, 500 };
    std::string rowStr;

    // Convert result to string pattern
    for (int i = 0; i < 15; ++i) {
        rowStr += result[i];
    }

    int payout = 0;
    for (size_t i = 0; i < winPatterns.size(); ++i) {
        if (rowStr.find(winPatterns[i]) != std::string::npos) {
            payout = payouts[i];
        }
    }
    return payout;
}

// Function to roll dice
int rollDice(const std::string& questionId) {
    int sides = 6; // Default to 6 sides
    if (questionId == "Q1") sides = 6;
    else if (questionId == "Q2") sides = 12;
    else if (questionId == "Q3") sides = 32;

    return rand() % sides + 1;
}

int main() {
    srand(static_cast<unsigned>(time(0))); // Seed for randomness

    // Example spin
    std::vector<std::string> result = spinReels();
    int payout = calculatePayout(result);

    std::cout << "Spin Result: ";
    for (const auto& symbol : result) {
        std::cout << symbol << " ";
    }
    std::cout << "\nPayout: " << payout << " cents" << std::endl;

    // Example dice roll
    std::string questionId = "Q1"; // Example question ID
    int diceRoll = rollDice(questionId);
    std::cout << "Dice Roll: " << diceRoll << std::endl;

    return 0;
}
