# If you want to play again returns a boolean
def playAgain():
    play = input("Do you want to play again (yes or no)? ")

    try: 
        if play == "yes" or play == 'y':
            return True
    
        elif play == "no" or play == 'n':
            return False
    except:
            print("Something went wrong.")

    else:
        print("Thanks for playing")
