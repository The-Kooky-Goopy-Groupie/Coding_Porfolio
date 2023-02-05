import cv2

# Read the original image
# Read the original image You replace the below line with following
#img = cv2.imread('Test1.bmp')
#img = cv2.imread('Test2.bmp')
#img = cv2.imread('Test3.png')

img = cv2.imread('testkirby.png')

# Display original image we hide these in most of the photos
cv2.imshow('Original', img)


# Convert to graycsale to be able to detect edges
img_gray = cv2.cvtColor(img, cv2.COLOR_BGR2GRAY)

# doing a Gauusian Blur, which will be done at the end of the document the image for better edge detection
img_blur = cv2.GaussianBlur(img_gray, (3, 3), 0)

# Sobel Edge Detection, fists for 3's then 5's
sobelx3 = cv2.Sobel(src=img_blur, ddepth=cv2.CV_64F, dx=1, dy=0, ksize=3)  # Sobel Edge Detection on the X axis 3
sobely3 = cv2.Sobel(src=img_blur, ddepth=cv2.CV_64F, dx=0, dy=1, ksize=3)  # Sobel Edge Detection on the Y axis 3
sobelxy3 = cv2.Sobel(src=img_blur, ddepth=cv2.CV_64F, dx=1, dy=1, ksize=3)  # Combined X and Y Sobel Edge Detection 3

sobelx5 = cv2.Sobel(src=img_blur, ddepth=cv2.CV_64F, dx=1, dy=0, ksize=5)  # Sobel Edge Detection on the X axis 5
sobely5 = cv2.Sobel(src=img_blur, ddepth=cv2.CV_64F, dx=0, dy=1, ksize=5)  # Sobel Edge Detection on the Y axis 5
sobelxy5 = cv2.Sobel(src=img_blur, ddepth=cv2.CV_64F, dx=1, dy=1, ksize=5)  # Combined X and Y Sobel Edge Detection 5

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
