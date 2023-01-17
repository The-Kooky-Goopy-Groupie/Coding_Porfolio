#Tic Tac Toe Game   Nicholas Del Gigante

import sys
import random

GAMEBOARD_SIZE= 3

class GameBoard:
    board =[]
    #generates empty board
    def __init__(self):
        self.board = [['_','_','_'],['_','_','_'],['_','_','_']]

    #the space on the game board corresponding to the row and col changes to the player character
    def take_turn(self, row, col, player):
        if(self.board[row][col]=='_'):
            self.board[row][col]= player
            return True
        else: 
            return False

    #prints the board
    def display_board(self):
        print("Here is the game state:")
        for x in self.board:
            print(*x, sep='|')
        print()

    #returns true if a win from the inputted player is detected returns false if they haven't won yet, saves on a win
    def detect_win(self,player)->bool:
        #check horizontal
        for i in range(len(self.board)):
            win = True
            for j in range(len(self.board[i])):
                if not(player==self.board[i][j]):
                    win = False
                    break
            if win:
                return win
        #check vertical
        for i in range(len(self.board[0])):
            win = True
            for j in range(len(self.board)):
                if not(player==self.board[j][i]):
                    win = False
                    break
            if win:
                self.save_on_win(player)
                return win
        #check diagonals
        win = True
        for i in range(len(self.board)):
            if not(player==self.board[i][i]):
                win= False
                break
        if win:
            self.save_on_win(player)
            return win
        
        win = True
        for i in range(len(self.board)):
            if not(player==self.board[i][len(self.board)-i-1]):
                win= False
                break
        if win:
            self.save_on_win(player)
            return win 
        #returns a loss
        return win

    #prints the winning game state to a file
    def save_on_win(self,player):
        original_stdout = sys.stdout
        with open('TicTacToeData.txt', 'a') as f:
            sys.stdout = f # Change the standard output to the file created.
            print(f'Player {player} has WON!!')
            self.display_board()
            sys.stdout = original_stdout
    
    # prints the drawing game state to a file
    def save_on_draw(self):
        original_stdout = sys.stdout
        with open('TicTacToeData.txt', 'a') as f:
            sys.stdout = f # Change the standard output to the file created.
            print('The game ended in a draw!!')
            self.display_board()
            sys.stdout = original_stdout


class TicTacToe:
    
    #checks to see if the input is valid for a player turn and adds it to the board
    def player_turn(player, board:GameBoard):
        print(f"Player {player} take your turn:")
            #checking the coordinate input's validity 
        while True:
            try:
                row, col = list(map(int, input("Enter row and column numbers 0-2 (example: ""0 0""): ").split()))
                if not(board.take_turn(row, col, player)):
                    raise Exception
            except:
                print("Invalid input, try again:  ")
            else:
                board.display_board()
                break
    def main_tictactoe(self):
        play_again=True
        while play_again:
            
            #checks to see if the input is valid
            while True:
                gamemode=input("Welcome To TicTacToe, do you wish to play with 2 players or against the computer? (enter 'P' for 2 player, enter 'C' for computer) ").upper()
                if gamemode == 'C' or gamemode == 'P':
                    break
                else:
                    print("Invalid input please try again")
            
            #PVP gamemode
            if (gamemode=='P'):
                turn = 0
                board=GameBoard()
                while turn < 9:
                    #player X's turn
                    TicTacToe.player_turn("X",board)
                    turn+=1

                    #sees if player X wins
                    if(board.detect_win("X")):
                        print("PLAYER X WINS!!!!")
                        break
                    #recognizes the draw state
                    if turn == 9:
                        print("THE GAME ENDS IN A DRAW")
                        board.save_on_draw()
                        break
                    
                    #player O's turn
                    TicTacToe.player_turn("O",board)
                    turn+=1

                    if(board.detect_win("O")):
                        print("PLAYER O WINS!!!!")
                        break

                
            if gamemode=="C":
                turn = 0
                board=GameBoard()
                print("You are player X:")
                while turn < 9:
                    #Check input's validaty
                    TicTacToe.player_turn("X",board)
                    turn+=1
                    
                    #checks to see if the player wins
                    if(board.detect_win("X")):
                        print("PLAYER X WINS!!!!")
                        break
                    #check the draw condition
                    if turn == 9:
                        print("THE GAME ENDS IN A DRAW")
                        board.save_on_draw()
                        break

                    #computer turn
                    print("The computer(O) will now take their turn")
                    #randomly selects a space for the computer play on if it is already occupied
                    while True:
                        row = random.randint(0,2)
                        col = random.randint(0,2)
                        if board.take_turn(row, col, "O"):
                            break
                    turn += 1
                    
                    #Computer player win check
                    if(board.detect_win("O")):
                        print("THE COMPUTER WINS!!!!")
                        break
                    board.display_board()

            #play again prompt
            while True:
                again=input("do you want to play again? (y or n)").upper()
                if again=="N":
                    play_again=False
                    break
                elif again=="Y":
                    break
                else:
                    print("invalid input, please try again")
                        
