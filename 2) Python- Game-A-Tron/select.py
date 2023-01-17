import random

# Returns a car string from a list
def getRandomCar(carList):
    selectCar = random.randint(0, len(carList) - 1)
    return carList[selectCar]