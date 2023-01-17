# The Game-A-Tron (Game Module Handler) - 
import math # honestly is just here to test imports work and nothing else
import Py_High_Num_Fun  # bring in the game function 
import TicTacToe # Tic Tac Toe game
import hangman# Hangman game
# import your functions here guys

def playgames(WishtoGame):   # Call a function  - P.S - function calls should be lower case, Also changes to your function called name make it different from what inherits too
    while True: # forces you to pass this check to continue
        try: # try the int input
            WishtoGame = str(input("Would you like to play a game? Type in 'y' or 'n' for yes and no:  ")) # Choose a number to be the highest boundry block
            print("Choice: ", WishtoGame) # used to let the user know what output they did
            if WishtoGame == "y" : # check wish to game is either y or n
                print ("Got it Booting up Game Choice Sequence:") # output that it going to start
                break # exit
            elif WishtoGame == "n" :  # if told no
                print ("Understood we are shutting down then!") # display shutdown message
                break #exit
            else: # any other value will give us an exception 
                raise Exception # if so go to the exception
        except Exception as e: # these next three lines handle the error
            print ("I can only accept either 'y' or 'n'  as responses. You did something else which caused an error")
            print(e)
    return WishtoGame # returns back the wishtogame value

def selectgames(Int_based_value, Output_Msg): #Works somewhat like the previous one
    while True: # this forces you to pass this check to continue 
        try: # try the int input
            Int_based_value = int(input(Output_Msg))  # Choose a Number to be the lower_Bound bounding Box of the game.
            if Int_based_value == 1 : # make sure is not a bigger then lower bound one
                print("Starting Game 1: ")
                Py_High_Num_Fun.HighestNum().main_highest_num() # YOU NEED TO CALL THE FUNCTION OF IT NOT THE CLASS
                break
            elif Int_based_value == 2 : # make sure is not a bigger then lower bound one
                print("Starting Game 2: ")
                # Your Game Goes Here eric for hangman
                hangman.TestHangup()
                break
            elif Int_based_value == 3 : # make sure is not a bigger then lower bound one
                print("Starting Game 3: ")
                TicTacToe.TicTacToe().main_tictactoe()
                break     
        except Exception as e: # get any value errors 
            print("The game your try to access seems to not exist yet...") # error msg for failure
            print(e)
    return Int_based_value # gets back the int based value 


class GameTron:
    UserChoice = 'x' # sets the users game choice to x by default
    UserChoice = playgames(UserChoice) # choose yes or no for the user choice
    while UserChoice == 'y':  # user choice 
        print("-*"*50) # style
        print("Choose a game to call forth by picking the corsponding list: ") # output instructions 
        print("-*"*50) # style
        Default_Game_Value = 0 # a default value needed for an input so game selected can be changed
        GameSelected = selectgames(Default_Game_Value,"The games you can select from are:\n \t '1' for Highest Number Game \n \t '2' for Hangman\n \t '3' for Tic Tac Toe\n \t Put your input here:  ") # Game output list codes
        print(GameSelected) # test print to make sure the game selected has updated properly 
        UserChoice = 'x' # reset the bool back to default after the game is done
        print(" Wahoo, It's a me, Mario - you have played a game! Congratulations!") # announce that a game is finished
        UserChoice = playgames(UserChoice)  # check to continue the while loop
         
UltimateGamer = GameTron  # invoking of the gamer class - this is the only one of the three classes that should be invoked as it's the hub  
