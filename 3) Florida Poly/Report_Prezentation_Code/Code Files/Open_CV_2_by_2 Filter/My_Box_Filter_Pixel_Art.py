import matplotlib.pyplot as plt
import matplotlib.image as mpimg
import cv2
import numpy as np
#%matplotlib inline

# Input the Test Pixel art Images
img1 = mpimg.imread('Pixel1.png')
img2 = mpimg.imread('Pixel2.png')
img3 = mpimg.imread('Pixel3.png')
img4 = mpimg.imread('Pixel4.png')

img5 = mpimg.imread('Pixel3.png')

# Apply the color conversion to the image due to open ccv
img1 = cv2.cvtColor(img1, cv2.COLOR_BGR2RGB)
img2 = cv2.cvtColor(img2, cv2.COLOR_BGR2RGB)
img3 = cv2.cvtColor(img3, cv2.COLOR_BGR2RGB) 
img4 = cv2.cvtColor(img4, cv2.COLOR_BGR2RGB)

img5 = cv2.cvtColor(img5, cv2.COLOR_BGR2RGB)

# Display Original images
cv2.imshow('Image Original 1', img1)
cv2.imshow('Image Original 2', img2)
cv2.imshow('Image Original 3', img3)
cv2.imshow('Image Original 4', img4)

# The Box Array with only 2*2 rather then the default test
box2 = np.array([[ 1, 1],
                [ 1, 1]])

box3 = np.array([[ 1, 1, 1],
                [ 1, 1, 1],
                [ 1, 1, 1]])

#Apply the box filters to the images
filtered_image_1_2 = cv2.filter2D(img1, -1, box2/4)
filtered_image_2_2 = cv2.filter2D(img3, -1, box2/4)
filtered_image_3_2 = cv2.filter2D(img2, -1, box2/4)
filtered_image_4_2 = cv2.filter2D(img4, -1, box2/4)

filtered_image_5_3 = cv2.filter2D(img5, -1, box3/9)

#Output the images

plt.imshow(filtered_image_1_2, cmap='gray')
cv2.imshow('Box Filtered Image 1-2', filtered_image_1_2)
cv2.imwrite("pixelblur1.png", filtered_image_1_2)

plt.imshow(filtered_image_2_2, cmap='gray')
cv2.imshow('Box Filtered Image 2-2', filtered_image_2_2)

cv2.imwrite("pixelblur2.png", filtered_image_2_2)

plt.imshow(filtered_image_3_2, cmap='gray')
cv2.imshow('Box Filtered Image 3-2', filtered_image_3_2)
cv2.imwrite("pixelblur3.png", filtered_image_3_2)

plt.imshow(filtered_image_4_2, cmap='gray')
cv2.imshow('Box Filtered Image 4-2', filtered_image_4_2)
cv2.imwrite("pixelblur4.png", filtered_image_4_2)


plt.imshow(filtered_image_5_3, cmap='gray')
cv2.imshow('Box Filtered Image 5-3', filtered_image_5_3)
cv2.imwrite("pixelblur4.png", filtered_image_5_3)

cv2.waitKey(0)