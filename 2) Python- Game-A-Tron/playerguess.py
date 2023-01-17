# Returns the letter the player has entered
def getGuess(playerGuess):
    while True:
        guess = input("\nPlease enter a letter. ")
        guess = guess.lower()

        # A check to make sure that the player has only entered a single charater and not anything else like a number.
        if len(guess) != 1:
            print("Please only enter a single letter.")

        elif guess in playerGuess:
            print(f"You have already guessed {guess}. Please choose another letter.")

        elif guess not in 'abcdefghijklmnopqrstuvwxyz':
            print("Sorry you can only enter a letter from the alphabet.")

        else: return guess