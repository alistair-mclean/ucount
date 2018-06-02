import cv2
import numpy as np
import matplotlib
import matplotlib.pyplot as plt

class ImageProcessor(): 
	original = []
	def test(self, img):
		self.original = img
		gray = cv2.cvtColor(img, cv2.COLOR_BGR2GRAY)
		laplacian = cv2.Laplacian(img, cv2.CV_64F)
		
		sobelx = cv2.Sobel(img, cv2.CV_64F, 1, 0, ksize=5)
		sobely = cv2.Sobel(img, cv2.CV_64F, 0, 1, ksize=5)

		plt.subplot(2,2,1),plt.imshow(img,cmap='gray')
		plt.title('Original'), plt.xticks([]), plt.yticks([])
		plt.subplot(2,2,2),plt.imshow(laplacian,cmap = 'gray')
		plt.title('Laplacian'), plt.xticks([]), plt.yticks([])
		plt.subplot(2,2,3),plt.imshow(sobelx,cmap = 'gray')
		plt.title('Sobel X'), plt.xticks([]), plt.yticks([])
		plt.subplot(2,2,4),plt.imshow(sobely,cmap = 'gray')
		plt.title('Sobel Y'), plt.xticks([]), plt.yticks([])
		plt.show()

	def preProcess(self, img):
		self.original = img
		grays = self.splitChannels(img)
		#grays = self.channelsToGrayScale(channels)
		indx = 0
		clahe = cv2.createCLAHE(clipLimit=3.0, tileGridSize=(8,8))
		for channel in grays:
			channel = self.smooth(channel, 3)
			channel = clahe.apply(channel)
			grays[indx] = channel
			indx += 1
		return grays


	# Split the channels of the image into
	def splitChannels(self, img):
		channels = []
		channels = cv2.split(img)
		cv2.waitKey(0)
		cv2.destroyAllWindows()
		return channels

	def channelsToGrayScale(self, channels):
		# This method does nothing .....
		grays = []
		for channel in channels:
			img = cv2.bitwise_and(self.original, self.original, mask=channel)
			grays.append(cv2.cvtColor(img, cv2.COLOR_BGR2GRAY))
		return grays

	# Apply a laplacian convolution 
	def sharpenEdges(self, img):
		return cv2.Laplacian(img, cv2.CV_64F)

	# Apply a gaussian blur
	def smooth(self, img, ksize):
		return cv2.blur(img, (ksize, ksize))



