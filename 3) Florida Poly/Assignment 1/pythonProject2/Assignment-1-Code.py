# Imports that are needed for the assignment.

import cv2
import numpy as np
from matplotlib import pyplot as plt

def BrightnessContrast(brightness=0):
    # getTrackbarPos returns the current, position of the specified trackbar. this then allows them to be put on the base item
    brightness = cv2.getTrackbarPos('Brightness', 'BASE')
    contrast = cv2.getTrackbarPos('Contrast', 'BASE')
    effect = controller(img, brightness, contrast) # this tells which of the ones it should affect for the image

    # The function imshow displays an image, in the specified window, and then puts out this window
    cv2.imshow('EFFECT_OUTPUT', effect)

# this is how the controlers work, first put up the ranges for the brightness and contrast,
def controller(img, brightness=255,
               contrast=127):
    brightness = int((brightness - 0) * (255 - (-255)) / (510 - 0) + (-255))

    contrast = int((contrast - 0) * (127 - (-127)) / (254 - 0) + (-127))
# check if it is not wqual to 0 if it is over 0 make brighter up to max of 255, else make it darker
    if brightness != 0:

        if brightness > 0:

            shadow = brightness

            max = 255

        else:

            shadow = 0
            max = 255 + brightness

        al_pha = (max - shadow) / 255
        ga_mma = shadow

        # The function addWeighted calculates
        # the weighted sum of two arrays
        cal = cv2.addWeighted(img, al_pha,
                              img, 0, ga_mma)

    else:
        cal = img
# same thing with contrast just with a different range and also application of different formula in order to raise contrast instead of brightness
    if contrast != 0:
        Alpha = float(131 * (contrast + 127)) / (127 * (131 - contrast))
        Gamma = 127 * (1 - Alpha)

        # The function addWeighted calculates
        # the weighted sum of two arrays
        cal = cv2.addWeighted(cal, Alpha,
                              cal, 0, Gamma)

    # putText renders the specified text string in the image.
    cv2.putText(cal, 'B:{},C:{}'.format(brightness,
                                        contrast), (10, 30),
                cv2.FONT_HERSHEY_SIMPLEX, 1, (0, 0, 255), 2)

    return cal

# this gets the data needed for rendering the images
if __name__ == '__main__':
    # The function imread loads an image
    # from the specified file and returns it.
    original = cv2.imread("test.png")

    # Making another copy of an image.
    img = original.copy()

    # The function namedWindow creates a
    # window that can be used as a placeholder
    # for images.
    cv2.namedWindow('BASE')

    # The function imshow displays an
    # image in the specified window.
    cv2.imshow('BASE', original)

    # createTrackbar(trackbarName,
    # windowName, value, count, onChange)
    # Brightness range -255 to 255
    cv2.createTrackbar('Brightness',
                       'BASE', 255, 2 * 255,
                       BrightnessContrast)

    # Contrast range -127 to 127
    cv2.createTrackbar('Contrast', 'BASE',
                       127, 2 * 127,
                       BrightnessContrast)

#  this is mainly where new code comes into play to get the Histogram for base image
    histogram_base = cv2.calcHist(img, [0], None, [256], [0, 256])
    plt.hist(img.ravel(), 256, [0, 256])
    plt.title('Histogram for the Images')
    plt.show()

    # should save a copy of the edited image, it sort of works but didnt
    cv2.imwrite('Contrast_Adjusted_Image.jpg', img)

    BrightnessContrast(0)


# The function waitKey waits for
# a key event infinitely or for delay
# milliseconds, when it is positive.
cv2.waitKey(0)







