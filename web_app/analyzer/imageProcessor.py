import cv2
import numpy as np
import matplotlib.pyplot as plt

class ImageProcessor(): 
	def __init__(self, mode=None, original=None):
		self.mdoe = mode
		self.original = []
		if original:
			self.original = cv2.imread(original)
			return self.process_image(self.original)

	def process_image(self, img):
		self.original = img
		try:
			channels = self.split_channels(img)
		except Exception as e:
			print('[ERROR] ImageProcessor: failed to split_channels on img: ', img)
		indx = 0
		for channel in channels[1:]:
			channel = self.smooth(channel, 3)
			# cv2.imshow('smooth', channel)
			# cv2.waitKey(0)
			# cv2.destroyAllWindows()
			
			channel = self.improve_contrast(channel)
			# cv2.imshow('clahe', channel)
			# cv2.waitKey(0)
			# cv2.destroyAllWindows()

			# channel = self.normalize_image(channel)
			# cv2.imshow('normalize', channel)
			# cv2.waitKey(0)
			# cv2.destroyAllWindows()
			
			channel = self.threshold_image(channel, [35, 255])
			cv2.imshow('threshold', channel)
			cv2.waitKey(0)
			cv2.destroyAllWindows()

			# channel = self.highlight_edges(channel)
			# cv2.imshow('laplacian', channel)
			# cv2.waitKey(0)
			# cv2.destroyAllWindows()
			channels[indx] = channel
			indx += 1
		return channels


	# Split the channels of the image into
	def split_channels(self, img):
		channels = []
		try:
			channels = cv2.split(img)
		except Exception as e:
			print('[ERROR] ImageProcessor.split_channels: Exception occurred: ', e)
			raise
		return channels

	# Apply a laplacian convolution 
	def highlight_edges(self, img):		
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

	def improve_contrast(self, img):
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
		
	def threshold_image(self, image, valRange):
		ret, thet = cv2.threshold(image, valRange[0], valRange[1], cv2.THRESH_OTSU)
		#cv2.imshow('Ret',ret)
		#cv2.imshow('Thet', thet)
		#cv2.waitKey(0)
		#cv2.destroyAllWindows()
		return thet
		

	def normalize_image(self, img):
#		cv2.normalize(img, img, 0, 255, norm_type=cv2.NORM_MINMAX, dtype=cv2.CV_8UC1)
		cv2.normalize(img, img, 0, 255, norm_type=cv2.NORM_MINMAX)
		return img

	# Apply a gaussian blur
	def smooth(self, img, ksize):
		return cv2.blur(img, (ksize, ksize))



