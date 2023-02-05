import cv2
import numpy as np # we need this for the array again

# Read the original image
# Read the original image You replace the below line with following
#img = cv2.imread('Test1.bmp')
#img = cv2.imread('Test2.bmp')
#img = cv2.imread('Test3.png')

img = cv2.imread('Test1.bmp')

# Display original image we hide these in most of the photos
cv2.imshow('Original', img)


# Convert to graycsale to be able to detect edges
img_gray = cv2.cvtColor(img, cv2.COLOR_BGR2GRAY)

# doing a Gauusian Blur, which will be done at the end of the document the image for better edge detection
img_blur = cv2.GaussianBlur(img_gray, (3, 3), 0)

sobel_x3 = np.array([[ -1, 0, 1],
                    [ -2, 0, 2],
                    [ -1, 0, 1]])

sobel_y3 = np.array([[ -1, -2, -1],
                    [ 0, 0, 0],
                    [ 1, 2, 1]])

sobel_xy3= np.array([[ 0, -2, 0],
                    [ -2, 0, 2],
                    [ 0, 2, 0]])

# The Box array as a 5*5
sobel_x5  = np.array([  [2, 2, 4, 2, 2],
                        [1, 1, 2, 1, 1],
                        [0, 0, 0, 0, 0],
                        [-1, -1, -2, -1, -1],
                        [-2, -2, -4, -2, -2]])

sobel_y5  = np.array([  [2, 1, 0, -1, -2],
                        [2, 1, 0, -1, -2],
                        [4, 2, 0, -2, -4],
                        [2, 1, 0, -1, -2],
                        [2, 1, 0, -1, -2]])

sobel_xy5  = np.array([ [4, 3, 4, -1, 0],
                        [3, 2, 2, 0, -1],
                        [4, 2, 0, -2, -4],
                        [1, 0, -2, -2, -3],
                        [0, 1, -4, -3, -4]])

# Sobel Edge Detection, fists for 3's then 5's
sobelx3 = cv2.filter2D(img_blur, -1, sobel_x3) # Note: the divide by 8 does change the effect in some intresting ways, we honestly perfer it removed
sobely3 = cv2.filter2D(img_blur, -1, sobel_y3) # without it there a bit more noise in the data but overall brighter and more akin to original
sobelxy3 = cv2.filter2D(img_blur, -1, sobel_xy3)

sobelx5 = cv2.filter2D(img_blur, -1, sobel_x5) # there a total of 36 data points here
sobely5 = cv2.filter2D(img_blur, -1, sobel_y5)
sobelxy5 = cv2.filter2D(img_blur, -1, sobel_xy5) # meanwhile 24+28

# Display Sobel Edge Detection Images of 3 power array
cv2.imshow('Sobel X 3', sobelx3)

cv2.imshow('Sobel Y 3', sobely3)

cv2.imshow('Sobel X Y 3 ', sobelxy3)


# Display Sobel Edge Detection Images that are of 5 power array
cv2.imshow('Sobel X 5', sobelx5)

cv2.imshow('Sobel Y 5', sobely5)

cv2.imshow('Sobel X Y 5', sobelxy5)

cv2.waitKey(0) # 0 holds it in place when needed
cv2.destroyAllWindows()



