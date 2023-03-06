#Importing the random pyton package
import random

#create a dictionary of key value pairs to symbolize the board. This makes it easier to keep track of the positions on the board.
board = {1: ' ', 2: ' ', 3: ' ',
         4: ' ', 5: ' ', 6: ' ',
         7: ' ', 8: ' ', 9: ' '}

player = 'O'
computer = 'X'

#Function responsible for printing the board to the screen.
def printBoard(board):
    print(board[1] + "|" + board[2] + "|" + board[3])
    print("-*-*-")
    print(board[4] + "|" + board[5] + "|" + board[6])
    print("-*-*-")
    print(board[7] + "|" + board[8] + "|" + board[9])
    print("\n")

# check the dictionary to see if any space is free or occupied and returns a boolean value of true or false
def spaceIsFree(position):
    if board[position] == ' ':
        return True 
    return False 

#inserts letter at the given key position. 
def insertLetter(letter, position):
    #calls the check space free function and if it is true it will insert a value at the key pair
    if spaceIsFree(position):
        board[position] = letter 
        #prints the updated board
        printBoard(board)
        if checkDraw():
            print("Draw!")
            exit() 
        if checkWin():
            if letter == 'X':
                print("Bot wins!")
                exit()
            else:
                print("Player wins!")
                exit()
        return 
    else:
        print("Invalid position")
        position = int(input("Please enter a new position: "))
        insertLetter(letter, position)
        return    
#This function checks the win conditions for Horizontal vertical and diagonal combinations
def checkWin():
    if (board[1] == board[2] and board[1] == board[3] and board[1] != ' '):
        return True
    elif (board[4] == board[5] and board[4] == board[6] and board[4] != ' '):
        return True
    elif (board[7] == board[8] and board[7] == board[9] and board[7] != ' '):
        return True
    elif (board[1] == board[4] and board[1] == board[7] and board[1] != ' '):
        return True
    elif (board[2] == board[5] and board[2] == board[8] and board[2] != ' '):
        return True
    elif (board[3] == board[6] and board[3] == board[9] and board[3] != ' '):
        return True
    elif (board[1] == board[5] and board[1] == board[9] and board[1] != ' '):
        return True
    elif (board[7] == board[5] and board[7] == board[3] and board[7] != ' '):
        return True
    else:
        return False

#checks which player has won by comparing O or X
def checkWhichPlayerWon(mark):
    if (board[1] == board[2] and board[1] == board[3] and board[1] == mark):
        return True
    elif (board[4] == board[5] and board[4] == board[6] and board[4] == mark):
        return True
    elif (board[7] == board[8] and board[7] == board[9] and board[7] == mark):
        return True
    elif (board[1] == board[4] and board[1] == board[7] and board[1] == mark):
        return True
    elif (board[2] == board[5] and board[2] == board[8] and board[2] == mark):
        return True
    elif (board[3] == board[6] and board[3] == board[9] and board[3] == mark):
        return True
    elif (board[1] == board[5] and board[1] == board[9] and board[1] == mark):
        return True
    elif (board[7] == board[5] and board[7] == board[3] and board[7] == mark):
        return True
    else:
        return False

def checkDraw():
    for key in board.keys():
        if board[key] == ' ':
            return False 
    return True 

#Responsible for the players turn
def playerMove():
    position = int(input("Enter a position for 'O': "))
    insertLetter(player, position)
    return 

def RandomCompMove():

    randomPos = random.randrange(0,len(board.keys()))
    if board[randomPos] == ' ':
        insertLetter(computer,randomPos)
        return
    else:
        RandomCompMove()


def HeuristicCHoice():
    if board[1] == 'O' :
        insertLetter(computer,board[3])
    elif board[1] == ' ':
        insertLetter(computer,board[1])
    else:
        randomPos = random.randrange(0,len(board.keys()))
        insertLetter(computer,randomPos)

    return

def compMove():
    
    bestScore = -800
    bestMove = 0
    for key in board.keys():
        if board[key] == ' ':
            board[key] = computer
            score = minimax(board, False)
            board[key] = ' '
            if score > bestScore:
                bestScore = score 
                bestMove = key
    insertLetter(computer, bestMove)
    return 

#Minimax function. 
def minimax(board, isMaximizing):
    if checkWhichPlayerWon(computer):
        return 1 
    elif checkWhichPlayerWon(player):
        return -1 
    elif checkDraw():
        return 0
    #Checks if the AI is maximizing or the player is minimizing   
    if isMaximizing:
        bestScore = -100
        for key in board.keys():
            if board[key] == ' ':
                board[key] = computer 
                score = minimax(board, False)
                board[key] = ' '
                if score > bestScore:
                    bestScore = score
        return bestScore 
    else:
        bestScore = 100 
        for key in board.keys():
            if board[key] == ' ':
                board[key] = player 
                score = minimax(board, True)
                board[key] = ' '
                if score < bestScore:
                    bestScore = score 
        return bestScore


while not checkWin():
    compMove()
    playerMove()
    #RandomCompMove()
