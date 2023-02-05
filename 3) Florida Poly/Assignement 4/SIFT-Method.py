# other half of the sift method code we have
import cv2

# input the images
image1 = cv2.imread('Original_Blocks.png')
image2 = cv2.imread('Edited_Blocks.png')
# convert images to grayscale versions of the image
image1 = cv2.cvtColor(image1, cv2.COLOR_BGR2GRAY)
image2 = cv2.cvtColor(image2, cv2.COLOR_BGR2GRAY)

# create the basline sift algoritim
sift = cv2.SIFT_create()

# detect SIFT features in both images
keypoints_1, descriptors_1 = sift.detectAndCompute(image1,None)
keypoints_2, descriptors_2 = sift.detectAndCompute(image2,None)

# generate the feature matcher to use on the image feature matcher
bf = cv2.BFMatcher(cv2.NORM_L1, crossCheck=True)
# use this to detect the matches
matches = bf.match(descriptors_1,descriptors_2)

# sort the matches to make it a bit easier to tell
matches = sorted(matches, key = lambda x:x.distance)
# draw the matches for the image
matched_img = cv2.drawMatches(image1, keypoints_1, image2, keypoints_2, matches[:50], image2, flags=2)

# show the image that shows the sift points
cv2.imshow('The Sift algorithim', matched_img)
# save that image
cv2.imwrite("Sift.png", matched_img)
cv2.waitKey(0)
cv2.destroyAllWindows()