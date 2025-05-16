# WordFinderApplication
This project solves a technical challenge involving the detection of the most frequent words in a matrix of letters. The solution is structured using clean architecture principles and is designed to be testable, extendable, and performance-oriented.

## 🧩 Project Structure

- `WordFinderApp` — Class library containing the main logic for word detection.
- `WordFinderApplication.Tests` — xUnit test project with extensive test coverage using FluentAssertions.

## ✅ Features

- Supports matrices up to **64x64** in size.
- Detects words **horizontally** (left to right) and **vertically** (top to bottom).
- Case-insensitive comparison using `ToUpperInvariant`.
- Ignores duplicate entries in the input `wordStream`.
- Returns a ranked list of the **top 10 most frequent words** found in the matrix.
- Clean code, SOLID principles, and TDD applied.

## 🚀 Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- Visual Studio 2022 or later (recommended) or any IDE that supports .NET 8

### Run Tests

**From Visual Studio:**
- Open Test Explorer → Right-click → Run or Debug any test individually.

**From CLI:**
```bash
dotnet test
---------------------------------------------------------------------------------------------------------------------------
📦 How It Works
The class WordFinder receives a square matrix of strings and an enumerable of words (wordStream). It counts how many times each distinct word appears in the matrix (horizontally and vertically), then returns the top 10 results ordered by frequency.

The detection logic compares characters one by one for performance, without unnecessary allocations. Input is normalized to uppercase once for better performance in comparisons.

🛠️ Design Notes
The IWordFinder interface makes the logic easily replaceable or extendable for future words finders (e.g., diagonal search).

Word search methods are private and cleanly separated (horizontal and vertical).

Designed for high performance on large wordStreams and worst-case matrix sizes.

🧪 Test Coverage
The test suite includes:

Valid scenarios with different matrix/wordStream sizes

Edge cases (empty wordStream, very long wordStream, max matrix size)

Stress tests

Exception handling (e.g., for invalid matrix sizes)
