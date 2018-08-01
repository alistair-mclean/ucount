import cv2
import numpy as np
import matplotlib
import matplotlib.pyplot as plt

class ImageProcessor(): 
	original = []

	def processImage(self, img):
		self.original = img
		channels = self.splitChannels(img)
		indx = 0
		for channel in channels:
			channel = self.smooth(channel, 3)
			cv2.imshow('smooth', channel)
			cv2.waitKey(0)
			cv2.destroyAllWindows()
			
			channel = self.improveContrast(channel)
			cv2.imshow('clahe', channel)
			cv2.waitKey(0)
			cv2.destroyAllWindows()
			channel = self.detectEdges(channel)
			cv2.imshow('laplacian', channel)
			cv2.waitKey(0)
			cv2.destroyAllWindows()
			
			channel = self.thresholdImage(channel, [20, 250])
			cv2.imshow('threshold', channel)
			cv2.waitKey(0)
			cv2.destroyAllWindows()

			channel = self.normalizeImage(channel)
			cv2.imshow('normalize', channel)
			cv2.waitKey(0)
			cv2.destroyAllWindows()

			channels[indx] = channel
			indx += 1
		return channels


	# Split the channels of the image into
	def splitChannels(self, img):
		channels = []
		channels = cv2.split(img)
		return channels

	def channelsToGrayScale(self, channels):
		# This method does nothing .....
		grays = []
		for channel in channels:
			img = cv2.bitwise_and(self.original, self.original, mask=channel)
			grays.append(cv2.cvtColor(img, cv2.COLOR_BGR2GRAY))
		return grays

	# Apply a laplacian convolution 
	def detectEdges(self, img):		
		try:
			edges = cv2.Laplacian(img, cv2.CV_8UC1)
		except:
			print("Could't detect the edges!")
			return None
		'''
		cv2.imshow('edges 64F ', edges)
		cv2.waitKey(0)
		cv2.destroyAllWindows()
		cv2.imshow('edges uint8 ', np.uint8(edges))
		cv2.waitKey(0)
		cv2.destroyAllWindows()
		'''
		return edges

	def improveContrast(self, img):
		try:
			clahe = cv2.createCLAHE(clipLimit=2.0, tileGridSize=(5,5))
			temp = clahe.apply(img)
			#cv2.imshow('CLAHE',clahe.apply(img))
			#cv2.waitKey(0)
			#cv2.destroyAllWindows()

		except:
			print("Couldn't improve the contrast.")
			return None
		return temp
		
	def thresholdImage(self, image, valRange):
		ret, thet = cv2.threshold(image, valRange[0], valRange[1], cv2.THRESH_OTSU)
		#cv2.imshow('Ret',ret)
		#cv2.imshow('Thet', thet)
		#cv2.waitKey(0)
		#cv2.destroyAllWindows()
		return thet
		

	def normalizeImage(self, img):
#		cv2.normalize(img, img, 0, 255, norm_type=cv2.NORM_MINMAX, dtype=cv2.CV_8UC1)
		cv2.normalize(img, img, 0, 255, norm_type=cv2.NORM_MINMAX)
		return img

	# Apply a gaussian blur
	def smooth(self, img, ksize):
		return cv2.blur(img, (ksize, ksize))



