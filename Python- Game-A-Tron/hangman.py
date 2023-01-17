import random
import playerguess
import again
import select

HANGMAN = [
    '''
    +---+
        |
        |
        |
       ===
    ''',
    '''
    +---+
    o   |
        |
        |
       ===
    ''',
    '''
    +---+
    o   |
    |   |
        |
       ===
    ''',
    '''
    +---+
    o   |
   /|   |
        |
       ===
    ''',
    '''
    +---+
    o   |
   /|\  |
        |
       ===
    ''',
    '''
    +---+
    o   |
   /|\  |
   /    |
       ===
    ''',
    '''
    +---+
    o   |
   /|\  |
   / \  |
       ===
    '''
]

#List of car brand names
cars = 'lamborghini ferrari porsche mclaren bugatti maserati lotus astonmartin hennessey koenigsegg ford chevrolet jeep subaru dodge toyota volkswagen honda kia nissan landrover rollsroyce mercedesbenz audi acura jaguar cadillac bmw'.split()

# Prints the hangman game
def showHangman(wrongLetters, correctLetters, secretWord):
    print(HANGMAN[len(wrongLetters)])
    print("Guess the car brand\n")

    print('Wrong letters you have already entered:', end=' ')

    for letter in wrongLetters:
        print(letter, end=' ')
    print()

    spaces = '_' * len(secretWord)

    # Fills in the blank spaces with the correct letter guess
    for guess in range(len(secretWord)):
        if secretWord[guess] in correctLetters:
            spaces = spaces[:guess] + secretWord[guess] + spaces[guess + 1:]

    # Shows each letter in the secret word
    for letter in spaces:
        print(letter, end=' ')
    print()

def TestHangup():
    wrongLetters = ''
    correctLetters = ''
    secretWord = select.getRandomCar(cars)
    endOfGame = False

    while True:
        showHangman(wrongLetters, correctLetters, secretWord)

        guess = playerguess.getGuess(wrongLetters + correctLetters)

        if guess in secretWord:
            correctLetters = correctLetters + guess

            #A check to see if the player has won
            foundWord = True
            for letter in range(len(secretWord)):
                if secretWord[letter] not in correctLetters:
                    foundWord = False
                    break

            if foundWord:
                print(f"\nCorrect!!! The car brand name is {secretWord}. Congratulations you have won!\nYou only got " + str(len(wrongLetters)) + " letters wrong.\n")
                endOfGame = True

        else:
            wrongLetters = wrongLetters + guess

            # A check to stop asking the player when they have run out of guesses and lost the game.
            if len(wrongLetters) == len(HANGMAN) - 1:
                showHangman(wrongLetters, correctLetters, secretWord)
                print("\nSorry you have ran out of guesses,\nYou got " + str(len(wrongLetters)) + " wrong guesses and " + str(len(correctLetters)) + ' correct guesses,\nThe car brand was "' + secretWord + '"\n')
                endOfGame = True

        # After the game is over the player is asked if they want to play again
        if endOfGame:
            if again.playAgain():
                wrongLetters = ''
                correctLetters = ''
                endOfGame = False
                secretWord = select.getRandomCar(cars)
            else:
                break

try:
    file = open("hangman.txt", "at")
    try:
        file.write("You got " + str(len(wrongLetters)) + " letters wrong and those letters were " + wrongLetters + " and you got " + str(len(correctLetters)) + " letters correct and those letters were " + correctLetters + ".\nThe car brand was " + secretWord + ".\n")
    except:
        print("Oh no!!! something went wrong")
    finally:
        file.close
except:
    print("Oh no! the file could not be opened")

class HangmanGame(object):
	def __init__(self, playgame):
		self.playgame = playgame
	
	def play(self):
		secretWord = self.playgame.select.getRandomCar(cars)
		while True:
			self.playgame.playerguess.getGuess(secretWord)
			if self.playgame.endOfGame:
				if self.playgame.playagain():
					secretWord = self.playgame.getRandomWord(cars)
					self.playgame.wrongLetters = ''
					self.playgame.correctLetters = ''
					self.playgame.endOfGame = False
				else:
					break